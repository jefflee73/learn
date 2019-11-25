using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Web;
using System.IO;

public class Geography : MSSQLObject
{
    #region USWego Game

    public DataRow GetRandomDestination()
    {
        string query = "SELECT TOP 1 * FROM usdestinations ORDER BY NEWID()";

        DataTable t = base.RunQueryDataTable(query);

        if (t != null && t.Rows.Count > 0)
            return t.Rows[0];
        return null;
    }

    public DataRow GetRandomDestination(int gameLevel)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@gameLevel", gameLevel)
            };

        string query = "SELECT * FROM usdestinations WHERE rankinstate = @gameLevel ORDER BY NEWID()";

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t.Rows[0];
        return null;
    }

    public DataRow GetRandomDestination(int gameLevel, List<int> prevDestList)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@gameLevel", gameLevel)
            };

        string andClause = "AND id NOT IN (0";
        foreach (var id in prevDestList)
        {
            andClause += id.ToString() + ", ";
        }
        andClause += "0)";  //id will never be 0, so no effect

        string query = "SELECT TOP 1 * FROM usdestinations WHERE rankinstate = @gameLevel " + andClause + " ORDER BY NEWID()";

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t.Rows[0];
        return null;
    }

    public DataRow GetDestination(int id)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id)
            };

        string query = "SELECT * FROM usdestinations d JOIN usstates s ON d.abbr = s.abbr WHERE d.id = @id";

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t.Rows[0];
        return null;
    }

    public DataRow GetDestination(string abbr, int rank)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@abbr",abbr),
                new SqlParameter("@rank",rank)
            };

        string query = "SELECT * FROM usdestinations d WHERE d.abbr = @abbr AND d.rankinstate = @rank";

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t.Rows[0];
        return null;
    }

    public DataRow GetState(string abbr)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@abbr",abbr)
            };

        string query = "SELECT * FROM usstates WHERE abbr = @abbr";

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t.Rows[0];
        return null;
    }

    public DataTable GetUSParks(string stateAbbr)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@stateAbbr", stateAbbr)
            };

        string query = "SELECT TOP 5 * FROM usparks WHERE abbr = @stateAbbr ORDER BY NEWID()";
        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t;
        return null;
    }

    public DataTable GetUSFeatures(string stateAbbr)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@stateAbbr", stateAbbr)
            };

        string query = "SELECT TOP 5 * FROM usfeatures WHERE abbr = @stateAbbr ORDER BY NEWID()";
        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
        {
            DataView dv = t.DefaultView;
            dv.Sort = "rankinstate asc";
            return dv.ToTable();
        }
        return null;
    }

    public DataTable GetUSCities(string abbr)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@abbr", abbr)
            };

        string query = "SELECT TOP 5 * FROM uscities WHERE abbr = @abbr ORDER BY rank";
        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
        {
            DataView dv = t.DefaultView;
            dv.Sort = "rank asc";
            return dv.ToTable();
        }
        return null;
    }

    public DataTable GetUSCities(string abbr, int n)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@abbr", abbr)
            };

        string strN = (n < 11 ? n.ToString() : "5");
        string query = "SELECT TOP " + strN + " * FROM uscities WHERE abbr = @abbr ORDER BY rank";
        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
        {
            DataView dv = t.DefaultView;
            dv.Sort = "rank asc";
            return dv.ToTable();
        }
        return null;
    }

    public DataTable GetUSNeighbors(string stateAbbr)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@stateAbbr", stateAbbr)
            };

        string query = "SELECT * FROM usneighbors WHERE abbr = @stateAbbr ORDER BY neighbor";
        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
        {
            DataView dv = t.DefaultView;
            dv.Sort = "neighbor asc";
            return dv.ToTable();
        }
        return null;
    }

    public void PutScore(string name, DateTime playdate, int level, int score)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@name", name),
                new SqlParameter("@playdate",playdate),
                new SqlParameter("@level",level),
                new SqlParameter("@score",score)
            };

        string query = "";
        query += "INSERT INTO us_scores ";
        query += "(name, playdate, level, score) VALUES ";
        query += "(@name, @playdate, @level, @score)";

        base.ExecuteNonQuery(query, parameters);
    }

    public int GetScoreId(string name, int level)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@name", name),
                new SqlParameter("@level", level)
            };

        string query = "SELECT TOP 1 id FROM us_scores WHERE name = @name AND level = @level ORDER BY playdate DESC";
        //int id = base.RunQueryScalar(query, parameters);

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return Convert.ToInt32(t.Rows[0]["id"].ToString());
        return 0;
    }

    public DataTable GetScoringHistory(string name, int level)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@name", name),
                new SqlParameter("@level",level)
            };

        string query = "";
        query += "SELECT TOP 5 [id], [name], [playdate], [level], [score] ";
        query += "FROM  us_scores ";
        query += "WHERE name LIKE @name AND level = @level ";
        query += "ORDER BY score DESC";

        DataTable t = base.RunQueryDataTable(query, parameters);
        if (t != null && t.Rows.Count > 0)
            return t;
        return null;
    }

    #endregion

    #region WorldWego Game

    public DataRow GetRandomWWDestination()
    {
        string query = "SELECT TOP 1 * FROM worlddestinations ORDER BY NEWID()";

        DataTable t = base.RunQueryDataTable(query);

        if (t != null && t.Rows.Count > 0)
            return t.Rows[0];
        return null;
    }

    public DataRow GetRandomWWDestination(int gameLevel)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@gameLevel", gameLevel)
            };

        string query = "SELECT * FROM worlddestinations WHERE level = @gameLevel ORDER BY NEWID()";

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t.Rows[0];
        return null;
    }

    public DataRow GetRandomWWDestination(int gameLevel, List<int> prevDestList)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@gameLevel", gameLevel)
            };

        string andClause = "AND id NOT IN (0";
        foreach (var id in prevDestList)
        {
            andClause += id.ToString() + ", ";
        }
        andClause += "0)";  //id will never be 0, so no effect

        string query = "SELECT TOP 1 * FROM worlddestinations WHERE level = @gameLevel " + andClause + " ORDER BY NEWID()";

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t.Rows[0];
        return null;
    }

    public DataRow GetWWDestination(int id)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id)
            };

        string query = "SELECT * FROM worlddestinations d JOIN worldcountries s ON d.a2 = s.a2 WHERE d.id = @id";

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t.Rows[0];
        return null;
    }

    public DataRow GetWWDestination(string a2, int level)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@a2",a2),
                new SqlParameter("@level",level)
            };

        string query = "SELECT * FROM worlddestinations d WHERE d.a2 = @a2 AND d.level = @level";

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t.Rows[0];
        return null;
    }

    public DataRow GetCountry(string a2)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@a2",a2)
            };

        string query = "SELECT * FROM worldcountries WHERE a2 = @a2";

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t.Rows[0];
        return null;
    }

    public void PutWWScore(string name, DateTime playdate, int level, int score)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@name", name),
                new SqlParameter("@playdate",playdate),
                new SqlParameter("@level",level),
                new SqlParameter("@score",score)
            };

        string query = "";
        query += "INSERT INTO world_scores ";
        query += "(name, playdate, level, score) VALUES ";
        query += "(@name, @playdate, @level, @score)";

        base.ExecuteNonQuery(query, parameters);
    }

    public int GetWWScoreId(string name, int level)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@name", name),
                new SqlParameter("@level", level)
            };

        string query = "SELECT TOP 1 id FROM world_scores WHERE name = @name AND level = @level ORDER BY playdate DESC";
        //int id = base.RunQueryScalar(query, parameters);

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return Convert.ToInt32(t.Rows[0]["id"].ToString());
        return 0;
    }

    public DataTable GetWWScoringHistory(string name, int level)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@name", name),
                new SqlParameter("@level",level)
            };

        string query = "";
        query += "SELECT TOP 5 [id], [name], [playdate], [level], [score] ";
        query += "FROM  world_scores ";
        query += "WHERE name LIKE @name AND level = @level ";
        query += "ORDER BY score DESC";

        DataTable t = base.RunQueryDataTable(query, parameters);
        //if (t != null && t.Rows.Count > 0)
        return t;
        //return null;
    }

    public void PutWWTurnScore(string gameId, DateTime playdate, int level, int playerNum, int turnScore, string destination)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@gameId", gameId),
                new SqlParameter("@playdate",playdate),
                new SqlParameter("@level",level),
                new SqlParameter("@playerNum",playerNum),
                new SqlParameter("@turnScore",turnScore),
                new SqlParameter("@destination",destination)
            };

        string query = "";
        query += "INSERT INTO world_turnscores ";
        query += "(gameId, playdate, level, playerNum, turnScore, destination) VALUES ";
        query += "(@gameId, @playdate, @level, @playerNum, @turnScore, @destination)";

        base.ExecuteNonQuery(query, parameters);
    }

    public DataTable GetWWGameScores(string gameId)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@gameId", gameId)
            };

        string query = "SELECT * FROM world_turnscores WHERE gameId = @gameId ORDER BY playdate";
        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t;
        return null;
    }

    public DataTable GetWWCities(string a2)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@a2", a2)
            };

        string query = "SELECT TOP 5 * FROM worldcities WHERE a2 = @a2 ORDER BY rank";
        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
        {
            DataView dv = t.DefaultView;
            dv.Sort = "rank asc";
            return dv.ToTable();
        }
        return null;
    }

    public DataTable GetWWCities(string a2, int n)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@a2", a2)
            };

        string strN = (n < 11 ? n.ToString() : "5");
        string query = "SELECT TOP " + strN + " * FROM worldcities WHERE a2 = @a2 ORDER BY rank";
        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
        {
            DataView dv = t.DefaultView;
            dv.Sort = "rank asc";
            return dv.ToTable();
        }
        return null;
    }

    public DataTable GetWWNeighbors(string a2)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@a2", a2)
            };

        string query = "SELECT * FROM worldneighbors WHERE a2 = @a2 ORDER BY neighbor";
        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
        {
            DataView dv = t.DefaultView;
            dv.Sort = "neighbor asc";
            return dv.ToTable();
        }
        return null;
    }

    #endregion

    #region Other resources

    public DataTable GetArticles(string startDate, string endDate, bool mapsOnly)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@startDate", startDate),
                new SqlParameter("@endDate", endDate),
            };

        string folioType = mapsOnly ? "'%map%'" : "'%%'";
        string query = "SELECT i.sequence as issueNum, [issueid], i.name as issueName, i.date as issueDate, ";
        query += "a.[id] as articleId, a.[sequence] as articleNum, a.[name] as articleName, ";
        query += "[description], [startpage],[folio], a.[url] ";
        query += "FROM [natgeo].[dbo].[ArchiveArticles] a ";
        query += "JOIN [natgeo].[dbo].[ArchiveIssues] i ON a.issueid = i.id ";
        query += "WHERE i.date >= @startDate AND i.date < @endDate AND folio LIKE " + folioType;
        query += "ORDER BY a.id";

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t;
        return null;
    }

    public void PutArticle(int issueid, int sequence, string name, string description, int startpage, string folio, string url)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@issueid", issueid),
                new SqlParameter("@sequence", sequence),
                new SqlParameter("@name", name),
                new SqlParameter("@description",description),
                new SqlParameter("@startpage",startpage),
                new SqlParameter("@folio",folio),
                new SqlParameter("@url",url)
            };

        string query = "";
        query += "INSERT INTO ArchiveArticles ";
        query += "(issueid, sequence, name, description, startpage, folio, url) VALUES ";
        query += "(@issueid, @sequence, @name, @description, @startpage, @folio, @url)";

        base.ExecuteNonQuery(query, parameters);
    }

    public DataRow GetQuestion(int id)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@id",id)
            };

        string query = "";
        query = "SELECT * FROM questions WHERE id = @id";

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t.Rows[0];
        return null;
    }

    public void RecordResult(string session, string username, int questionid, int guesses)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@session", session),
                new SqlParameter("@username", username),
                new SqlParameter("@questionid",questionid),
                new SqlParameter("@guesses",guesses),
            };

        string query = "";
        query += "INSERT INTO results ";
        query += "(session, username, questionid, questiondate, guesses) VALUES ";
        query += "(@session, @username, @questionid, NOW(), @guesses)";

        base.ExecuteNonQuery(query, parameters);
    }

    public int GetLastResult(string user)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@user",user)
            };

        string query = "";
        query = "SELECT MAX(questionid) FROM results WHERE username = @user";

        return base.RunQueryScalar(query, parameters);
    }

    public DataRow GetStats(string user)
    {
        SqlParameter[] parameters = new SqlParameter[]{
                new SqlParameter("@user",user)
            };

        string query = "";
        query = "SELECT MAX(questionid) AS answers, SUM(guesses) as tries, ";
        query += "FLOOR(100 * MAX(questionid) / SUM(guesses)) AS score, FLOOR(100 * MAX(questionid) / 1312) AS progress ";
        query += "FROM results WHERE username = @user";

        DataTable t = base.RunQueryDataTable(query, parameters);

        if (t != null && t.Rows.Count > 0)
            return t.Rows[0];
        return null;
    }

    public int execFile(string fileName)
    {
        string mapPath = HttpContext.Current.Server.MapPath("~");
        mapPath = (mapPath.ToLower().Contains("geobee")) ? HttpContext.Current.Server.MapPath("~/results/") : HttpContext.Current.Server.MapPath("~/geobee/results/");
        mapPath += fileName;
        string commands = File.ReadAllText(mapPath);
        return ExecuteNonQuery(commands);
    }

    #endregion

}
