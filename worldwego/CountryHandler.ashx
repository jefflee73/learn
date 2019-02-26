<%@ WebHandler Language="C#" Class="CountryHandler" %>

using System;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Collections.Generic;

public class CountryHandler : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        string term = context.Request["term"] ?? "";
        List<string> listCountryNames = new List<string>();
        string cs = ConfigurationManager.ConnectionStrings["Geography"].ConnectionString;
        using (SqlConnection con = new SqlConnection(cs))
        {
            SqlCommand cmd = new SqlCommand("spGetCountryNames", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter()
            {
                ParameterName = "@term",
                Value = term
            });
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                listCountryNames.Add(rdr["country"].ToString());
            }
        }

        JavaScriptSerializer js = new JavaScriptSerializer();
        context.Response.Write(js.Serialize(listCountryNames));
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}