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
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            outRepairsGrid.DataSource = GetOutstandingRepairs();
            outRepairsGrid.DataBind();
            AddTooltipsLinks(outRepairsGrid, 0, 1, 2, 3, 4, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16);
            outRepairsBadge.InnerText = OutstandingRepairsCount();

            recentGrid.DataSource = GetRecentActivity();
            recentGrid.DataBind();
            AddTooltipsLinks(recentGrid, 0, 1, 2, 3, 4, 11, 12, 13, 14, 15, 16, 18, 19, 20, 17);
            recentBadge.InnerText = recentGrid.Rows.Count.ToString();
        }

        protected DataTable GetOutstandingRepairs()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Repairs.RepairID, Repairs.CameraID, Repairs.LaptopID, Repairs.KitID, Repairs.PhotogID, Repairs.Date, Repairs.Notes, Cameras.SerialNumber, Cameras.Make, Cameras.Model, " + 
                    "Laptops.SerialNumber, Laptops.Make, Laptops.Model, Photographers.Name, Photographers.Initials, Photographers.Office, Kits.KitPH FROM Repairs " +
                    "LEFT JOIN Cameras ON Repairs.CameraID = Cameras.CameraID " +
                    "LEFT JOIN Laptops ON Repairs.LaptopID = Laptops.LaptopID " +
                    "LEFT JOIN Photographers ON Repairs.PhotogID = Photographers.ID " +
                    "LEFT JOIN Kits ON Repairs.KitID = Kits.KitID " +
                    "WHERE Repairs.Fixed = 0";
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

        protected void AddTooltipsLinks(GridView gv, int repairID, int cameraID, int laptopID, int kitID, int photogID, int cameraSN, int cameraMake, int cameraModel, int laptopSN, int laptopMake, int laptopModel, int photogName, int photogInitial, int photogOffice, int kitPH)
        {
            foreach (GridViewRow gr in gv.Rows)
            {
                //RepairID link to repair record
                if (gr.Cells[repairID].Text != "0" && gr.Cells[repairID].Text != "&nbsp;")
                {
                    string holder = gr.Cells[repairID].Text;
                    gr.Cells[repairID].Text = String.Format("<a href=\"Repairs2.aspx?repairID={0}\" class=\"btn btn-primary\">{0}</a>", holder);
                }

                //CameraID tooltip and link to camera record
                if (gr.Cells[cameraID].Text != "0" && gr.Cells[cameraID].Text != "&nbsp;")
                {
                    string holder = gr.Cells[1].Text;
                    gr.Cells[cameraID].Text = String.Format("<a href=\"Cameras.aspx?CameraID={0}\" class=\"btn btn-default\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}</br>{3}\">{0}</a>",
                        holder,
                        gr.Cells[cameraSN].Text,
                        gr.Cells[cameraMake].Text,
                        gr.Cells[cameraModel].Text);
                }

                //LaptopID tooltip and link to laptop record
                if (gr.Cells[laptopID].Text != "0" && gr.Cells[laptopID].Text != "&nbsp;")
                {
                    string holder = gr.Cells[laptopID].Text;
                    gr.Cells[laptopID].Text = String.Format("<a href=\"Laptops.aspx?LaptopID={0}\" class=\"btn btn-default\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}</br>{3}\">{0}</a>",
                        holder,
                        gr.Cells[laptopSN].Text,
                        gr.Cells[laptopMake].Text,
                        gr.Cells[laptopModel].Text);
                }

                //PhotogID tooltip and link to photographers record
                if (gr.Cells[photogID].Text != "0" && gr.Cells[photogID].Text != "&nbsp;")
                {
                    string holder = gr.Cells[photogID].Text;
                    gr.Cells[photogID].Text = String.Format("<a href=\"PhotogDetails.aspx?PhotogID={0}\" class=\"btn btn-default\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>Office: {3}\">{2}</a>",
                        holder,
                        gr.Cells[photogName].Text,
                        gr.Cells[photogInitial].Text,
                        gr.Cells[photogOffice].Text);
                }

                //KitID link to kit record
                if (gr.Cells[kitID].Text != "0" && gr.Cells[kitID].Text != "&nbsp;")
                {
                    string holder = gr.Cells[kitID].Text;
                    gr.Cells[kitID].Text = String.Format("<a href=\"Kits2.aspx?KitID={0}\" class=\"btn btn-default\">{1}</a>", holder, gr.Cells[kitPH].Text);
                }
            }
        }

        protected void outRepairsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 7; i <= 16; i++) e.Row.Cells[i].Visible = false;
        }

        protected string OutstandingRepairsCount()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT count (*) FROM Repairs WHERE Fixed = 0";
                m_dbConnection.Open();
                return command.ExecuteScalar().ToString();
            }
        }

        protected DataTable GetRecentActivity()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Repairs.*, Cameras.SerialNumber, Cameras.Make, Cameras.Model, Laptops.SerialNumber, Laptops.Make, Laptops.Model, Kits.KitPH, Photographers.Name, Photographers.Initials, Photographers.Office FROM Repairs " +
                    "LEFT JOIN Cameras ON Repairs.CameraID = Cameras.CameraID " +
                    "LEFT JOIN Laptops ON Repairs.LaptopID = Laptops.LaptopID " +
                    "LEFT JOIN Kits ON Repairs.KitID = Kits.KitID " +
                    "LEFT JOIN Photographers ON Repairs.PhotogID = Photographers.ID " +
                    "WHERE substr(FixedDate,7,4)||substr(FixedDate,4,2)||substr(FixedDate,1,2) BETWEEN @DatePrev AND @DateToday " +
                    "ORDER BY Repairs.RepairID DESC";
                string todayMonth = DateTime.Today.Month.ToString();
                todayMonth = todayMonth.Length == 1 ? "0" + todayMonth : todayMonth;
                string todayDay = DateTime.Today.Day.ToString();
                todayDay = todayDay.Length == 1 ? "0" + todayDay: todayDay;
                command.Parameters.Add(new SQLiteParameter("@DateToday", DateTime.Today.Year.ToString() + todayMonth + todayDay));
                DateTime previousDate = DateTime.Today.AddDays(-3);
                string prevMonth = previousDate.Month.ToString();
                prevMonth = prevMonth.Length == 1 ? "0" + prevMonth : prevMonth;
                string prevDay = previousDate.Day.ToString();
                prevDay = prevDay.Length == 1 ? "0" + prevDay : prevDay;
                string prevDateString = previousDate.Year.ToString() + prevMonth + prevDay;
                command.Parameters.Add(new SQLiteParameter("@DatePrev", prevDateString));
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

        protected void recentGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 10; i <= 20; i++) e.Row.Cells[i].Visible = false;
        }
    }
}