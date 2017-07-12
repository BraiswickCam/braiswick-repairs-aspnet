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
    public partial class Reports : System.Web.UI.Page
    {
        SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", "C:\\datatest\\2016repairhistory.sqlite"));
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadReport();
        }

        protected void LoadReport()
        {
            if (Request.QueryString["rep"] != null)
            {
                string report = Request.QueryString["rep"];

                if (report == "repairCost")
                {
                    RepairCost();
                }
                else if (report == "OSCount")
                {
                    OSCount();
                }
                else if (report == "MakeCount")
                {
                    MakeCount();
                }
                else if (report == "RepairCount")
                {
                    RepairCount();
                }
                else if (report == "OfficeCountCost")
                {
                    OfficeCountCost();
                }
                else if (report == "AssignedLaptopPercent")
                {
                    AssignedLaptopPercent();
                }
            }
        }

        protected void RepairCost()
        {
            BindData(QueryDatabase("SELECT Photographers.ID, Photographers.Name, Photographers.Initials, Photographers.Active, Photographers.Office, sum(Repairs.RepairCost) AS \"Total Repair Cost\" FROM Photographers " + 
                "LEFT JOIN Repairs ON Photographers.ID = Repairs.PhotogID WHERE Repairs.RepairCost > 0 GROUP BY Photographers.ID ORDER BY \"Total Repair Cost\" DESC"));

            foreach (GridViewRow gvr in resultsGrid.Rows)
            {
                int totalCol = gvr.Cells.Count - 1;
                if (gvr.Cells[totalCol].Text != "0" && gvr.Cells[totalCol].Text != "&nbsp;")
                {
                    string hold = gvr.Cells[totalCol].Text;
                    gvr.Cells[totalCol].Text = String.Format("£{0}", hold);
                }
                else
                {
                    gvr.Cells[totalCol].Text = "&nbsp;";
                }
            }

            AddPhotogIDLinks(0, 1);
        }

        protected void OSCount()
        {
            BindData(QueryDatabase("SELECT OS, count(OS) AS \"Count\" FROM Laptops WHERE Active = 1 GROUP BY OS ORDER BY count(OS) DESC"));
        }

        protected void MakeCount()
        {
            BindData(QueryDatabase("SELECT Make, count(Make) AS \"Count\" FROM Laptops WHERE Active = 1 GROUP BY Make ORDER BY count(Make) DESC"));
        }

        protected void RepairCount()
        {
            BindData(QueryDatabase("SELECT Photographers.ID, Photographers.Name, Photographers.Initials, Photographers.Office, count(Repairs.RepairID) AS \"Repair Count\" FROM Photographers " +
                    "LEFT JOIN Repairs ON Photographers.ID = Repairs.PhotogID WHERE Repairs.Date IS NOT Repairs.FixedDate GROUP BY Repairs.PhotogID ORDER BY count(Repairs.RepairID) DESC"));

            AddPhotogIDLinks(0, 1);
        }

        protected void OfficeCountCost()
        {
            BindData(QueryDatabase("SELECT Photographers.Office, count(Repairs.RepairID) AS \"Repair Count\", sum(Repairs.RepairCost) AS \"Total Repair Cost\" FROM Photographers " +
                    "LEFT JOIN Repairs ON Photographers.ID = Repairs.PhotogID WHERE Repairs.Date IS NOT Repairs.FixedDate GROUP BY Photographers.Office ORDER BY count(Repairs.RepairID) DESC"));
        }

        protected void AssignedLaptopPercent()
        {
            BindData(QueryDatabase("SELECT count(Laptops.LaptopID) AS \"Total Laptops\", count(Kits.LaptopID) + count(Kits.SpareLaptopID) AS \"Assigned Laptops\", " +
                    "round(((count(Kits.LaptopID) + count(Kits.SpareLaptopID) + 0.0) / count(Laptops.LaptopID) + 0.0) * 100.0, 2) AS \"Assigned Percentage\" FROM Laptops " +
                    "LEFT JOIN Kits ON Kits.LaptopID = Laptops.LaptopID OR Kits.SpareLaptopID = Laptops.LaptopID"));

            foreach (GridViewRow gvr in resultsGrid.Rows)
            {
                int totalCol = gvr.Cells.Count - 1;
                if (gvr.Cells[totalCol].Text != "0" && gvr.Cells[totalCol].Text != "&nbsp;")
                {
                    string hold = gvr.Cells[totalCol].Text;
                    gvr.Cells[totalCol].Text = String.Format("{0}%", hold);
                }
            }
        }

        protected DataTable QueryDatabase(string query)
        {
            using (m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
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

        protected void AddPhotogIDLinks(int colIndexID, int colIndexName)
        {
            foreach (GridViewRow gvr in resultsGrid.Rows)
            {
                if (gvr.Cells[0].Text != "0" && gvr.Cells[0].Text != "&nbsp;")
                {
                    HyperLink hp = new HyperLink();
                    hp.Target = "_blank";
                    hp.ToolTip = "Edit details for " + gvr.Cells[colIndexName].Text;
                    //hp.Text = gr.Cells[0].Text;
                    hp.Text = String.Format("<span class=\"glyphicon glyphicon-edit\"></span> {0}", gvr.Cells[0].Text);
                    hp.CssClass = "btn btn-primary btn-sm";
                    hp.NavigateUrl = "~/PhotogDetails.aspx?PhotogID=" + gvr.Cells[0].Text;
                    gvr.Cells[0].Controls.Add(hp);
                }
            }
        }

        protected void BindData(DataTable dt)
        {
            resultsGrid.DataSource = dt;
            resultsGrid.DataBind();
        }
    }
}