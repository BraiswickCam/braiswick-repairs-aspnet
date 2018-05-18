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

        protected DateTime GetLastWeekStart()
        {
            return DateTime.Now.StartOfWeek(DayOfWeek.Monday).AddDays(-7);
        }

        protected string[] GetOffice()
        {
            return officeDD.SelectedValue == "both" ? new string[] { "MT", "MF" } : officeDD.SelectedValue == "MT" ? new string[] { "MT" } : new string[] { "MF" };
        }

        protected void LoadWeeklyFeedback(DateTime dateWeek, string dateSearchCol, string[] office)
        {
            printTable.Attributes["data-hide-cols"] = "6,10,12";
            LoadWeekly(new string[] { "FEEDBACK" }, "Feedback", dateSearchCol, dateWeek, office);
            printTable.Attributes["data-hide-cols"] = "";
        }

        protected void LoadWeeklyReports(DateTime dateWeek, string dateSearchCol, string[] office)
        {
            printTable.Attributes["data-hide-cols"] = "6,10";
            LoadWeekly(new string[] { "REPORT" }, "Lab Reports", dateSearchCol, dateWeek, office);
            printTable.Attributes["data-hide-cols"] = "";
        }

        protected void LoadWeekly(string[] status, string displayStatus, string dateType, DateTime startDate, string[] office)
        {
            printTable.DataSource = GetWeekly(startDate, startDate.AddDays(7), dateType, status, office);
            printTable.DataBind();
            printTableTitle.InnerText = String.Format("{1} for week beginning {0}", startDate.ToShortDateString(), displayStatus + (office.Length > 1 ? "" : " (" + office[0] + ")"));
        }

        protected DataTable GetWeekly(DateTime weekStart, DateTime weekEnd, string dateType, string[] status, string[] office)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = String.Format("SELECT PReports.ID, PReports.Date, PReports.Office, PReports.Job, PReports.School, PReports.Type, PReports.Cost, PReports.Photographer, Photographers.Initials, Photographers.Name, PReports.Status, PReports.Notes, cast((NOT PReports.Action) AS BOOL) AS 'Actioned?' FROM PReports " + 
                    "LEFT JOIN Photographers ON PReports.Photographer = Photographers.ID WHERE PReports.Status IN ('{0}') AND {1} >= @StartWeek AND {1} < @EndWeek AND PReports.Office IN ('{2}') ORDER BY Date ASC", 
                    String.Join("', '", status), dateType, String.Join("', '", office));
                command.Parameters.Add(new SQLiteParameter("@StartWeek", weekStart.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new SQLiteParameter("@EndWeek", weekEnd.ToString("yyyy-MM-dd")));
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

        protected void printTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[11].Attributes.Add("data-min-width", "20");
            e.Row.Cells[7].Visible = false;
            //e.Row.Cells[8].Visible = false;

            if (e.Row.Cells[12].Text == "1")
            {
                e.Row.Cells[12].Text = "<span class=\"glyphicon glyphicon-ok\"></span>";
            }
            else if (e.Row.Cells[12].Text == "0")
            {
                e.Row.Cells[12].Text = "<span class=\"glyphicon glyphicon-remove\"></span>";
            }

            GridView gv = (GridView)sender;
            string[] hideCols = printTable.Attributes["data-hide-cols"].Split(',');
            List<int> hideColsIndex = new List<int>();
            try
            {
                foreach (string s in hideCols)
                {
                    hideColsIndex.Add(Convert.ToInt32(s));
                }
            }
            catch (Exception) { }
            foreach (int i in hideColsIndex)
            {
                e.Row.Cells[i].Visible = false;
            }
        }

        protected void thisWeekFeedbackButton_Click(object sender, EventArgs e)
        {
            LoadWeeklyFeedback(GetCurrentWeekStart(), "Date", GetOffice());
        }

        protected void lastWeekFeedbackButton_Click(object sender, EventArgs e)
        {
            LoadWeeklyFeedback(GetLastWeekStart(), "Date", GetOffice());
        }

        protected void thisWeekFeedbackByEditedButton_Click(object sender, EventArgs e)
        {
            LoadWeeklyFeedback(GetCurrentWeekStart(), "DateEdited", GetOffice());
        }

        protected void lastWeekFeedbackByEditedButton_Click(object sender, EventArgs e)
        {
            LoadWeeklyFeedback(GetLastWeekStart(), "DateEdited", GetOffice());
        }

        protected void thisWeekReportButton_Click(object sender, EventArgs e)
        {
            LoadWeeklyReports(GetCurrentWeekStart(), "DateEdited", GetOffice());
        }

        protected void lastWeekReportButton_Click(object sender, EventArgs e)
        {
            LoadWeeklyReports(GetLastWeekStart(), "DateEdited", GetOffice());
        }

        protected void thisWeekReportByTakeButton_Click(object sender, EventArgs e)
        {
            LoadWeeklyReports(GetCurrentWeekStart(), "Date", GetOffice());
        }

        protected void lastWeekReportByTakeButton_Click(object sender, EventArgs e)
        {
            LoadWeeklyReports(GetLastWeekStart(), "Date", GetOffice());
        }

        protected void officeDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            printTable.DataSource = "";
            printTable.DataBind();
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