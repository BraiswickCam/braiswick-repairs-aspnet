using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SQLite;
using System.Data;
using System.Text;

namespace WebApplication2
{
    public partial class WeeklySummaries : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected DateTime GetCurrentWeekStart()
        {
            return DateTime.Now.StartOfWeek(DayOfWeek.Monday);
        }

        protected void LoadWeeklyFeedback()
        {
            LoadWeekly("FEEDBACK", "Feedback");
        }

        protected void LoadWeeklyReports()
        {
            LoadWeekly("REPORT", "Lab Reports");
        }

        protected void LoadWeekly(string status, string displayStatus)
        {
            DateTime startDate = GetCurrentWeekStart();
            printTable.DataSource = GetWeekly(startDate, startDate.AddDays(7), status);
            printTable.DataBind();
            printTableTitle.InnerText = String.Format("{1} for week beginning {0}", startDate.ToShortDateString(), displayStatus);
        }

        protected DataTable GetWeekly(DateTime weekStart, DateTime weekEnd, string status)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT PReports.ID, PReports.Date, PReports.Office, PReports.Job, PReports.School, PReports.Type, PReports.Cost, PReports.Photographer, Photographers.Initials, Photographers.Name, PReports.Status, PReports.Notes FROM PReports " + 
                    "LEFT JOIN Photographers ON PReports.Photographer = Photographers.ID WHERE PReports.Status = @Status AND DateCreated >= @StartWeek AND DateCreated < @EndWeek";
                command.Parameters.Add(new SQLiteParameter("@StartWeek", weekStart.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new SQLiteParameter("@EndWeek", weekEnd.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new SQLiteParameter("@Status", status));
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

        protected void thisWeekFeedbackButton_Click(object sender, EventArgs e)
        {
            LoadWeeklyFeedback();
        }

        protected void printTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[11].Attributes.Add("data-min-width", "20");
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = false;
        }

        protected void thisWeekReportButton_Click(object sender, EventArgs e)
        {
            LoadWeeklyReports();
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}