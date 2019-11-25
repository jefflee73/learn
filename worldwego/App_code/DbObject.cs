using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Globalization;

public class DbObject
{
    protected string ConnectionString;
    protected MySqlConnection Connection;

    public DbObject()
    {
        ConnectionString = ConfigurationManager.ConnectionStrings["GeoBee"].ConnectionString;
        Connection = new MySqlConnection(ConnectionString);
    }

    public DbObject(string newConnectionString)
    {
        ConnectionString = newConnectionString;
        Connection = new MySqlConnection(ConnectionString);
    }

    public static DbObject FromConnectionStringName(string connectionStringName)
    {
        string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        return new DbObject(connectionString);
    }

    public void Close()
    {
        Connection.Close();
    }

    protected DataTable RunProcedureDataTable(string storedProcName)
    {
        string key = CacheHelper.Key(storedProcName);

        if (!CacheHelper.Exists(key))
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            MySqlDataAdapter da = new MySqlDataAdapter(storedProcName, Connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.Fill(dt);

            CacheHelper.Add<DataTable>(dt, key);

            return dt;
        }
        else
        {
            return CacheHelper.Get<DataTable>(key);
        }
    }

    protected DataTable RunProcedureDataTable(string storedProcName, IDataParameter[] parameters)
    {
        string key = CacheHelper.Key(storedProcName, parameters);

        if (!CacheHelper.Exists(key))
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            MySqlDataAdapter da = new MySqlDataAdapter(storedProcName, Connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            foreach (MySqlParameter parameter in parameters)
            {
                if (parameter != null)
                    da.SelectCommand.Parameters.Add(parameter);
            }

            da.Fill(dt);

            CacheHelper.Add<DataTable>(dt, key);

            return dt;
        }
        else
        {
            return CacheHelper.Get<DataTable>(key);
        }
    }

    public DataTable RunQueryDataTable(string query)
    {
        return RunQueryDataTable(query, null);
    }

    public DataTable RunQueryDataTable(string query, IDataParameter[] parameters)
    {
        string key = CacheHelper.Key(query, parameters);

        if (!CacheHelper.Exists(key))
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();

            using (MySqlDataAdapter da = new MySqlDataAdapter(query, Connection))
            {
                da.SelectCommand.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    foreach (MySqlParameter parameter in parameters)
                    {
                        if (parameter != null)
                            da.SelectCommand.Parameters.Add(parameter);
                    }
                }

                da.Fill(dt);
                CacheHelper.Add<DataTable>(dt, key);
            }
            return dt;
        }
        else
        {
            return CacheHelper.Get<DataTable>(key);
        }
    }

    protected MySqlDataReader RunQueryDataReader(string query, IDataParameter[] parameters)
    {
        MySqlDataReader returnReader;
        Connection.Open();
        MySqlCommand command = new MySqlCommand(query, Connection);
        command.CommandType = CommandType.Text;

        foreach (MySqlParameter p in parameters)
        {
            if (p != null)
                command.Parameters.Add(p);
        }

        returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);

        return returnReader;
    }

    protected MySqlDataReader RunProcedureDataReader(string storedProcName)
    {
        MySqlDataReader returnReader;
        Connection.Open();
        MySqlCommand command = BuildQueryCommand(storedProcName);
        returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);

        return returnReader;
    }

    protected MySqlDataReader RunProcedureDataReader(string storedProcName, IDataParameter[] parameters)
    {
        MySqlDataReader returnReader;
        Connection.Open();
        MySqlCommand command = BuildQueryCommand(storedProcName, parameters);
        returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);

        return returnReader;
    }

    public string RunQueryString(string query)
    {
        return RunQueryString(query, null);
    }

    public string RunQueryString(string query, IDataParameter[] parameters)
    {
        Connection.Open();
        MySqlCommand command = new MySqlCommand(query, Connection);
        command.CommandType = CommandType.Text;

        if (parameters != null)
        {
            foreach (MySqlParameter parameter in parameters)
            {
                if (parameter != null)
                    command.Parameters.Add(parameter);
            }
        }

        object r = command.ExecuteScalar();
        Connection.Close();

        if (r != null)
        {
            return r.ToString();
        }
        return null;
    }

    public int RunQueryScalar(string query)
    {
        return RunQueryScalar(query, null);
    }

    public int RunQueryScalar(string query, IDataParameter[] parameters)
    {
        String r = RunQueryString(query, parameters);
        if (r != null)
        {
            int id;

            if (int.TryParse(r, out id))
            {
                return id;
            }
        }
        return 0;
    }

    public int ExecuteNonQuery(string query)
    {
        return ExecuteNonQuery(query, null);
    }

    public int ExecuteNonQuery(string query, IDataParameter[] parameters)
    {
        Connection.Open();
        MySqlCommand command = new MySqlCommand(query, Connection);
        command.CommandType = CommandType.Text;

        if (parameters != null)
        {
            foreach (MySqlParameter parameter in parameters)
            {
                if (parameter != null)
                    command.Parameters.Add(parameter);
            }
        }

        int affected = command.ExecuteNonQuery();
        Connection.Close();

        return affected;
    }

    public int ExecuteNonQuerySP(string sp, IDataParameter[] parameters)
    {
        Connection.Open();
        MySqlCommand command = new MySqlCommand(sp, Connection);
        command.CommandType = CommandType.StoredProcedure;

        if (parameters != null)
        {
            foreach (MySqlParameter parameter in parameters)
            {
                if (parameter != null)
                    command.Parameters.Add(parameter);
            }
        }

        int affected = command.ExecuteNonQuery();
        Connection.Close();

        return affected;
    }

    protected MySqlCommand BuildQueryCommand(string storedProcName)
    {
        MySqlCommand command = new MySqlCommand(storedProcName, Connection);
        command.CommandType = CommandType.StoredProcedure;

        return command;
    }

    protected MySqlCommand BuildQueryCommand(string storedProcName, IDataParameter[] parameters)
    {
        MySqlCommand command = new MySqlCommand(storedProcName, Connection);
        command.CommandType = CommandType.StoredProcedure;

        foreach (MySqlParameter parameter in parameters)
        {
            if (parameter != null)
                command.Parameters.Add(parameter);
        }

        return command;
    }

    public DataSet GetData(string query)
    {
        string key = CacheHelper.Key(query);

        if (!CacheHelper.Exists(key))
        {
            DataSet ds = new DataSet();

            using (MySqlDataAdapter da = new MySqlDataAdapter(query, Connection))
            {
                da.SelectCommand.CommandType = CommandType.Text;

                da.Fill(ds);
                CacheHelper.Add<DataSet>(ds, key);
            }
            return ds;
        }
        else
        {
            return CacheHelper.Get<DataSet>(key);
        }
    }

    public DataTable GetMonths()
    {
        //month datatable
        DataTable months = new DataTable();
        months.Columns.Add("Name");
        months.Columns.Add("Digit");

        for (int i = 1; i <= 12; i++)
        {
            DataRow month = months.NewRow();
            month["Name"] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(i);
            month["Digit"] = i;

            months.Rows.Add(month);
        }
        return months;
    }

}
