using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Globalization;

public class MSSQLObject
{
    protected string ConnectionString;
    protected SqlConnection Connection;

    public MSSQLObject()
    {
        ConnectionString = ConfigurationManager.ConnectionStrings["Geography"].ConnectionString;
        Connection = new SqlConnection(ConnectionString);
    }

    public MSSQLObject(string newConnectionString)
    {
        ConnectionString = newConnectionString;
        Connection = new SqlConnection(ConnectionString);
    }

    public static MSSQLObject FromConnectionStringName(string connectionStringName)
    {
        string connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
        return new MSSQLObject(connectionString);
    }

    public void Close()
    {
        Connection.Close();
    }

    protected DataTable RunProcedureDataTable(string storedProcName)
    {
        string key = MSSQLCacheHelper.Key(storedProcName);

        if (!MSSQLCacheHelper.Exists(key))
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            SqlDataAdapter da = new SqlDataAdapter(storedProcName, Connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.Fill(dt);

            MSSQLCacheHelper.Add<DataTable>(dt, key);

            return dt;
        }
        else
        {
            return MSSQLCacheHelper.Get<DataTable>(key);
        }
    }

    protected DataTable RunProcedureDataTable(string storedProcName, IDataParameter[] parameters)
    {
        string key = MSSQLCacheHelper.Key(storedProcName, parameters);

        if (!MSSQLCacheHelper.Exists(key))
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();
            SqlDataAdapter da = new SqlDataAdapter(storedProcName, Connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;

            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                    da.SelectCommand.Parameters.Add(parameter);
            }

            da.Fill(dt);

            MSSQLCacheHelper.Add<DataTable>(dt, key);

            return dt;
        }
        else
        {
            return MSSQLCacheHelper.Get<DataTable>(key);
        }
    }

    public DataTable RunQueryDataTable(string query)
    {
        return RunQueryDataTable(query, null);
    }

    public DataTable RunQueryDataTable(string query, IDataParameter[] parameters)
    {
        string key = MSSQLCacheHelper.Key(query, parameters);

        if (!MSSQLCacheHelper.Exists(key))
        {
            DataSet ds = new DataSet();
            DataTable dt = ds.Tables.Add();

            using (SqlDataAdapter da = new SqlDataAdapter(query, Connection))
            {
                da.SelectCommand.CommandType = CommandType.Text;

                if (parameters != null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        if (parameter != null)
                            da.SelectCommand.Parameters.Add(parameter);
                    }
                }

                da.Fill(dt);
                MSSQLCacheHelper.Add<DataTable>(dt, key);
            }
            return dt;
        }
        else
        {
            return MSSQLCacheHelper.Get<DataTable>(key);
        }
    }

    protected SqlDataReader RunQueryDataReader(string query, IDataParameter[] parameters)
    {
        SqlDataReader returnReader;
        Connection.Open();
        SqlCommand command = new SqlCommand(query, Connection);
        command.CommandType = CommandType.Text;

        foreach (SqlParameter p in parameters)
        {
            if (p != null)
                command.Parameters.Add(p);
        }

        returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);

        return returnReader;
    }

    protected SqlDataReader RunProcedureDataReader(string storedProcName)
    {
        SqlDataReader returnReader;
        Connection.Open();
        SqlCommand command = BuildQueryCommand(storedProcName);
        returnReader = command.ExecuteReader(CommandBehavior.CloseConnection);

        return returnReader;
    }

    protected SqlDataReader RunProcedureDataReader(string storedProcName, IDataParameter[] parameters)
    {
        SqlDataReader returnReader;
        Connection.Open();
        SqlCommand command = BuildQueryCommand(storedProcName, parameters);
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
        SqlCommand command = new SqlCommand(query, Connection);
        command.CommandType = CommandType.Text;

        if (parameters != null)
        {
            foreach (SqlParameter parameter in parameters)
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
        SqlCommand command = new SqlCommand(query, Connection);
        command.CommandType = CommandType.Text;

        if (parameters != null)
        {
            foreach (SqlParameter parameter in parameters)
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
        SqlCommand command = new SqlCommand(sp, Connection);
        command.CommandType = CommandType.StoredProcedure;

        if (parameters != null)
        {
            foreach (SqlParameter parameter in parameters)
            {
                if (parameter != null)
                    command.Parameters.Add(parameter);
            }
        }

        int affected = command.ExecuteNonQuery();
        Connection.Close();

        return affected;
    }

    protected SqlCommand BuildQueryCommand(string storedProcName)
    {
        SqlCommand command = new SqlCommand(storedProcName, Connection);
        command.CommandType = CommandType.StoredProcedure;

        return command;
    }

    protected SqlCommand BuildQueryCommand(string storedProcName, IDataParameter[] parameters)
    {
        SqlCommand command = new SqlCommand(storedProcName, Connection);
        command.CommandType = CommandType.StoredProcedure;

        foreach (SqlParameter parameter in parameters)
        {
            if (parameter != null)
                command.Parameters.Add(parameter);
        }

        return command;
    }

    public DataSet GetData(string query)
    {
        string key = MSSQLCacheHelper.Key(query);

        if (!MSSQLCacheHelper.Exists(key))
        {
            DataSet ds = new DataSet();

            using (SqlDataAdapter da = new SqlDataAdapter(query, Connection))
            {
                da.SelectCommand.CommandType = CommandType.Text;

                da.Fill(ds);
                MSSQLCacheHelper.Add<DataSet>(ds, key);
            }
            return ds;
        }
        else
        {
            return MSSQLCacheHelper.Get<DataSet>(key);
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
