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
        public List<ChartDetails> GetChartData()
        {
            DataTable dt = QueryDatabase("SELECT * FROM RepairAmountOverview");
            List<ChartDetails> dataList = new List<ChartDetails>();
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
            return dataList;
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
    }

    
}
