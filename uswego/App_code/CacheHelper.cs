using System;
using System.Web;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Configuration;

public static class CacheHelper
{
    public static string Key(string query)
    {
        return Key(query, null);
    }

    public static string Key(string query, IDataParameter[] parameters)
    {
        string key = query;

        if (parameters != null)
        {
            foreach (MySqlParameter p in parameters)
            {
                if (p != null)
                    key += p.ParameterName + "=" + p.Value;
            }
        }

        MD5 md5 = MD5.Create();

        byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(key);
        byte[] hash = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
        {
            sb.Append(hash[i].ToString("X2"));
        }
        return sb.ToString();
    }

    /// <summary>
    /// Insert value into the cache using
    /// appropriate name/value pairs
    /// </summary>
    /// <typeparam name="T">Type of cached item</typeparam>
    /// <param name="o">Item to be cached</param>
    /// <param name="key">Name of item</param>
    public static void Add<T>(T o, string key) where T : class
    {
        // NOTE: Apply expiration parameters as you see fit.
        // In this example, I want an absolute
        // timeout so changes will always be reflected
        // at that time. Hence, the NoSlidingExpiration.
        if (key.Length > 0)
        {
            HttpContext.Current.Cache.Insert(
                key,
                o,
                null,
                DateTime.Now.AddMinutes(60),
                System.Web.Caching.Cache.NoSlidingExpiration);
        }
    }

    /// <summary>
    /// Remove item from cache
    /// </summary>
    /// <param name="key">Name of cached item</param>
    public static void Clear(string key)
    {
        HttpContext.Current.Cache.Remove(key);
    }

    /// <summary>
    /// Check for item in cache
    /// </summary>
    /// <param name="key">Name of cached item</param>
    /// <returns></returns>
    public static bool Exists(string key)
    {
        bool cache = ConfigurationManager.AppSettings["DB_CACHE"] != null && ConfigurationManager.AppSettings["DB_CACHE"] == "true" ? true : false;

        if (!cache)
            return false;

        return HttpContext.Current.Cache[key] != null;
    }

    /// <summary>
    /// Retrieve cached item
    /// </summary>
    /// <typeparam name="T">Type of cached item</typeparam>
    /// <param name="key">Name of cached item</param>
    /// <returns>Cached item as type</returns>
    public static T Get<T>(string key) where T : class
    {
        try
        {
            return (T)HttpContext.Current.Cache[key];
        }
        catch
        {
            return null;
        }
    }
}
