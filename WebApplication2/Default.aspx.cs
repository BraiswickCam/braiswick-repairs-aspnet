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
        string databaseLocation = "C:\\datatest\\2016repairhistory.sqlite";
        protected void Page_Load(object sender, EventArgs e)
        {
            outRepairsGrid.DataSource = GetOutstandingRepairs();
            outRepairsGrid.DataBind();
            AddTooltipsLinks();
            outRepairsBadge.InnerText = OutstandingRepairsCount();
        }

        protected DataTable GetOutstandingRepairs()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
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

        protected void AddTooltipsLinks()
        {
            foreach (GridViewRow gr in outRepairsGrid.Rows)
            {
                //RepairID link to repair record
                if (gr.Cells[0].Text != "0" && gr.Cells[0].Text != "&nbsp;")
                {
                    string holder = gr.Cells[0].Text;
                    gr.Cells[0].Text = String.Format("<a href=\"Repairs2.aspx?repairID={0}\" class=\"btn btn-primary\">{0}</a>", holder);
                }

                //CameraID tooltip and link to camera record
                if (gr.Cells[1].Text != "0" && gr.Cells[1].Text != "&nbsp;")
                {
                    string holder = gr.Cells[1].Text;
                    gr.Cells[1].Text = String.Format("<a href=\"Cameras.aspx?CameraID={0}\" class=\"btn btn-default\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}</br>{3}\">{0}</a>",
                        holder,
                        gr.Cells[7].Text,
                        gr.Cells[8].Text,
                        gr.Cells[9].Text);
                }

                //LaptopID tooltip and link to laptop record
                if (gr.Cells[2].Text != "0" && gr.Cells[2].Text != "&nbsp;")
                {
                    string holder = gr.Cells[2].Text;
                    gr.Cells[2].Text = String.Format("<a href=\"Laptops.aspx?LaptopID={0}\" class=\"btn btn-default\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}</br>{3}\">{0}</a>",
                        holder,
                        gr.Cells[10].Text,
                        gr.Cells[11].Text,
                        gr.Cells[12].Text);
                }

                //PhotogID tooltip and link to photographers record
                if (gr.Cells[4].Text != "0" && gr.Cells[4].Text != "&nbsp;")
                {
                    string holder = gr.Cells[4].Text;
                    gr.Cells[4].Text = String.Format("<a href=\"PhotogDetails.aspx?PhotogID={0}\" class=\"btn btn-default\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>Office: {3}\">{2}</a>",
                        holder,
                        gr.Cells[13].Text,
                        gr.Cells[14].Text,
                        gr.Cells[15].Text);
                }

                //KitID link to kit record
                if (gr.Cells[3].Text != "0" && gr.Cells[3].Text != "&nbsp;")
                {
                    string holder = gr.Cells[3].Text;
                    gr.Cells[3].Text = String.Format("<a href=\"Kits2.aspx?KitID={0}\" class=\"btn btn-default\">{1}</a>", holder, gr.Cells[16].Text);
                }
            }
        }

        protected void outRepairsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 7; i <= 16; i++) e.Row.Cells[i].Visible = false;
        }

        protected string OutstandingRepairsCount()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT count (*) FROM Repairs WHERE Fixed = 0";
                m_dbConnection.Open();
                return command.ExecuteScalar().ToString();
            }
        }
    }
}