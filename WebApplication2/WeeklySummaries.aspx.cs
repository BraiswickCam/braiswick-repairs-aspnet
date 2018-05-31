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

        protected DataTable GetPhotogReport(string[] office)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = String.Format("SELECT PReports.ID, PReports.Date, PReports.Office, PReports.Job, PReports.School, PReports.Type, PReports.Cost, PReports.Photographer, Photographers.Initials, Photographers.Name, PReports.Status, PReports.Notes, cast((NOT PReports.Action) AS BOOL) AS 'Actioned?' FROM PReports " +
                    "LEFT JOIN Photographers ON PReports.Photographer = Photographers.ID WHERE PReports.Office IN ('{0}') ORDER BY Date DESC",
                    String.Join("', '", office));
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

        protected DataTable GetCustomPhotogReport(string[] office, DateTime startDate, DateTime endDate)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = String.Format("SELECT PReports.ID, PReports.Date, PReports.Office, PReports.Job, PReports.School, PReports.Type, PReports.Cost, PReports.Photographer, Photographers.Initials, Photographers.Name, PReports.Status, PReports.Notes, cast((NOT PReports.Action) AS BOOL) AS 'Actioned?' FROM PReports " +
                    "LEFT JOIN Photographers ON PReports.Photographer = Photographers.ID WHERE PReports.Office IN ('{0}') AND DateCreated >= @StartDate AND DateCreated <= @EndDate ORDER BY Date DESC",
                    String.Join("', '", office));
                command.Parameters.Add(new SQLiteParameter("@StartDate", startDate.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new SQLiteParameter("@EndDate", endDate.ToString("yyyy-MM-dd")));
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

        protected DataTable GetPhotogs(string[] office)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = String.Format("SELECT * FROM Photographers WHERE Office IN ('{0}') AND Active = 1 ORDER BY Name ASC",
                    String.Join("', '", office));
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

        protected DataTable GetMansfieldSummary()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM MansfieldSummary";
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

        protected DataTable GetManningtreeSummary()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM ManningtreeSummary";
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

        protected DataTable GetBothSummary()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM BothSummary";
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

        protected DataTable GetCustomSummary(string[] office, DateTime startDate, DateTime endDate)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = String.Format("SELECT t0.Name AS 'Name', t0.Initials AS 'Initials', t1.count AS 'Complaints', t2.count AS 'Loss', t3.count AS 'Lab Reports', t4.count AS 'Retakes', t5.count AS 'NRBs' FROM " + 
                    "(SELECT * FROM Photographers) t0 " +
                    "LEFT JOIN (SELECT Photographers.ID, count(PReports.Status) AS 'count' FROM Photographers LEFT JOIN PReports ON PReports.Photographer = Photographers.ID WHERE PReports.Status = 'COMPLAINT' AND DateCreated >= @StartDate AND DateCreated <= @EndDate GROUP BY PReports.Photographer) t1 " + 
                    "ON t0.ID = t1.ID "+
                    "LEFT JOIN (SELECT Photographers.ID, count(PReports.Status) AS 'count' FROM Photographers LEFT JOIN PReports ON PReports.Photographer = Photographers.ID WHERE PReports.Status = 'LOSS' AND DateCreated >= @StartDate AND DateCreated <= @EndDate GROUP BY PReports.Photographer) t2 " + 
                    "ON t0.ID = t2.ID " +
                    "LEFT JOIN (SELECT Photographers.ID, count(PReports.Status) AS 'count' FROM Photographers LEFT JOIN PReports ON PReports.Photographer = Photographers.ID WHERE PReports.Status = 'REPORT' AND DateCreated >= @StartDate AND DateCreated <= @EndDate GROUP BY PReports.Photographer) t3 " + 
                    "ON t0.ID = t3.ID " +
                    "LEFT JOIN (SELECT Photographers.ID, count(PReports.Status) AS 'count' FROM Photographers LEFT JOIN PReports ON PReports.Photographer = Photographers.ID WHERE PReports.Status = 'RETAKE' AND DateCreated >= @StartDate AND DateCreated <= @EndDate GROUP BY PReports.Photographer) t4 " + 
                    "ON t0.ID = t4.ID " +
                    "LEFT JOIN (SELECT Photographers.ID, count(PReports.Status) AS 'count' FROM Photographers LEFT JOIN PReports ON PReports.Photographer = Photographers.ID WHERE PReports.Status IN ('PENDING', 'REBOOKED') AND DateCreated >= @StartDate AND DateCreated <= @EndDate GROUP BY PReports.Photographer) t5 " + 
                    "ON t0.ID = t5.ID " + 
                    "WHERE t0.Active = 1 AND t0.Office IN('{0}') "  + 
                    "GROUP BY t0.ID " + 
                    "ORDER BY t0.Name",
                    String.Join("', '", office));
                command.Parameters.Add(new SQLiteParameter("@StartDate", startDate.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new SQLiteParameter("@EndDate", endDate.ToString("yyyy-MM-dd")));
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
            string[] hideCols = gv.Attributes["data-hide-cols"].Split(',');
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

        protected void LoadPhotographerReports(DataTable summaryDataTable, DataTable reportDataTable)
        {
            DataTable dts = summaryDataTable;
            DataView dvs = new DataView(dts);
            summaryTable.DataSource = dvs;
            summaryTable.DataBind();
            summaryTableTitle.InnerText = officeDD.SelectedValue == "both" ? "Braiswick Photographer Summary" : officeDD.SelectedValue == "MT" ? "Manningtree Photographer Summary" : "Mansfield Photographer Summary";

            DataTable dt = reportDataTable; //GetPhotogReport(GetOffice());
            DataTable photogs = GetPhotogs(officeDD.SelectedValue == "both" ? new string[] { "MA", "KT", "MF" } : officeDD.SelectedValue == "MT" ? new string[] { "MA", "KT" } : new string[] { "MF" });
            foreach (DataRow dr in photogs.Rows)
            {
                DataView dv = new DataView(dt);
                dv.RowFilter = String.Format("Photographer IN ({0})", Convert.ToInt32(dr[0]));
                if (dv.Count > 0)
                {
                    GridView gv = new GridView();
                    gv.CssClass = "table table-striped table-hover";
                    gv.GridLines = GridLines.None;
                    gv.DataSource = dv;
                    gv.RowDataBound += printTable_RowDataBound;
                    gv.Attributes.Add("data-hide-cols", "8,12");
                    gv.DataBind();
                    System.Web.UI.HtmlControls.HtmlGenericControl header = new System.Web.UI.HtmlControls.HtmlGenericControl("h3");
                    header.InnerText = dr[1].ToString();
                    customReportDiv.Controls.Add(header);
                    customReportDiv.Controls.Add(gv);
                }
            }
        }

        protected void reportsPerPhotog_Click(object sender, EventArgs e)
        {
            DataTable dt = officeDD.SelectedValue == "both" ? GetBothSummary() : officeDD.SelectedValue == "MT" ? GetManningtreeSummary() : GetMansfieldSummary();
            LoadPhotographerReports(dt, GetPhotogReport(GetOffice()));
        }

        protected void summaryTable_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void reportsPerPhotogCustomDate_Click(object sender, EventArgs e)
        {
            string[] offices = officeDD.SelectedValue == "both" ? new string[] { "MA", "KT", "MF" } : officeDD.SelectedValue == "MT" ? new string[] { "MA", "KT" } : new string[] { "MF" };
            DateTime startDate = Convert.ToDateTime(customStartDate.Text);
            DateTime endDate = Convert.ToDateTime(customEndDate.Text);
            DataTable dt = GetCustomSummary(offices, startDate, endDate);
            DataTable dt2 = GetCustomPhotogReport(GetOffice(), startDate, endDate);
            LoadPhotographerReports(dt, dt2);
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