using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Data.SQLite;
using System.Web.Script.Services;

namespace WebApplication2
{
    /// <summary>
    /// Summary description for ColumnChart1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ColumnChart1 : System.Web.Services.WebService
    {
        [ScriptMethod]
        [WebMethod]
        public List<List<object>> GetChartData()
        {
            DataTable dt = QueryDatabase("SELECT * FROM RepairAmountOverview");
            List<object> dataList = new List<object>();
            List<object> columnList = GetColumns(dt);
            foreach (DataRow dr in dt.Rows)
            {
                ChartDetails details = new ChartDetails();
                details.Month = dr[0].ToString();
                if (string.IsNullOrEmpty(dr[1].ToString())) dr[1] = 0;
                details.SubmittedRepairs = Convert.ToInt32(dr[1]);
                if (string.IsNullOrEmpty(dr[2].ToString())) dr[2] = 0;
                details.FixedRepairs = Convert.ToInt32(dr[2]);
                dataList.Add(details);
            }
            List<List<object>> results = new List<List<object>>();
            results.Add(dataList);
            results.Add(columnList);
            return results;
        }

        [ScriptMethod]
        [WebMethod]
        public List<List<object>> GetOSData()
        {
            DataTable dt = QueryDatabase("SELECT OS, count(OS) AS \"Count\" FROM Laptops WHERE Active = 1 GROUP BY OS ORDER BY count(OS) DESC");
            List<object> dataList = new List<object>();
            List<object> columnList = GetColumns(dt);
            foreach (DataRow dr in dt.Rows)
            {
                OSDetails details = new OSDetails();
                details.OS = dr[0].ToString();
                details.Count = Convert.ToInt32(dr[1]);
                dataList.Add(details);
            }
            List<List<object>> results = new List<List<object>>();
            results.Add(dataList);
            results.Add(columnList);
            return results;
        }

        [ScriptMethod]
        [WebMethod]
        public List<List<object>> GetMakeData()
        {
            DataTable dt = QueryDatabase("SELECT Make, count(Make) AS \"Count\" FROM Laptops WHERE Active = 1 GROUP BY Make ORDER BY count(Make) DESC");
            List<object> dataList = new List<object>();
            List<object> columnList = GetColumns(dt);
            foreach (DataRow dr in dt.Rows)
            {
                MakeDetails details = new MakeDetails();
                details.Make = dr[0].ToString();
                details.Count = Convert.ToInt32(dr[1]);
                dataList.Add(details);
            }
            List<List<object>> results = new List<List<object>>();
            results.Add(dataList);
            results.Add(columnList);
            return results;
        }

        public static DataTable QueryDatabase(string query)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = query;
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
                {
                    sda.SelectCommand = command;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public class ChartDetails
        {
            public string Month;
            public int SubmittedRepairs;
            public int FixedRepairs;
        }

        public class OSDetails
        {
            public string OS;
            public int Count;
        }

        public class MakeDetails
        {
            public string Make;
            public int Count;
        }

        public class ColumnDetails
        {
            public string Type;
            public string Name;

            public ColumnDetails(Type dtype, string _name)
            {
                this.Name = _name;
                if (dtype == typeof(System.String)) this.Type = "string";
                if (dtype == typeof(System.Int64)) this.Type = "number";
                else this.Type = "string";
            }
        }

        public static List<object> GetColumns(DataTable dt)
        {
            List<object> results = new List<object>();
            foreach (DataColumn dc in dt.Columns)
            {
                ColumnDetails details = new ColumnDetails(dc.DataType, dc.Caption);
                results.Add(details);
            }
            return results;
        }


    }

    
}
