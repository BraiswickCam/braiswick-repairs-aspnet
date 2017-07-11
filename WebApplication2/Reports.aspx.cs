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
                    BindData(RepairCost());
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
                else if (report == "OSCount")
                {
                    BindData(OSCount());
                }
                else if (report == "MakeCount")
                {
                    BindData(MakeCount());
                }
                else if (report == "RepairCount")
                {
                    BindData(RepairCount());
                    AddPhotogIDLinks(0, 1);
                }
                else if (report == "OfficeCountCost")
                {
                    BindData(OfficeCountCost());
                }
            }
        }

        protected DataTable RepairCost()
        {
            using (m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Photographers.ID, Photographers.Name, Photographers.Initials, Photographers.Active, Photographers.Office, sum(Repairs.RepairCost) AS \"Total Repair Cost\" FROM Photographers LEFT JOIN Repairs ON Photographers.ID = Repairs.PhotogID WHERE Repairs.RepairCost > 0 GROUP BY Photographers.ID ORDER BY \"Total Repair Cost\" DESC";
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

        protected DataTable OSCount()
        {
            using (m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT OS, count(OS) AS \"Count\" FROM Laptops WHERE Active = 1 GROUP BY OS ORDER BY count(OS) DESC";
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

        protected DataTable MakeCount()
        {
            using (m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Make, count(Make) AS \"Count\" FROM Laptops WHERE Active = 1 GROUP BY Make ORDER BY count(Make) DESC";
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

        protected DataTable RepairCount()
        {
            using (m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Photographers.ID, Photographers.Name, Photographers.Initials, Photographers.Office, count(Repairs.RepairID) AS \"Repair Count\" FROM Photographers " +
                    "LEFT JOIN Repairs ON Photographers.ID = Repairs.PhotogID WHERE Repairs.Date IS NOT Repairs.FixedDate GROUP BY Repairs.PhotogID ORDER BY count(Repairs.RepairID) DESC";
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

        protected DataTable OfficeCountCost()
        {
            using (m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Photographers.Office, count(Repairs.RepairID) AS \"Repair Count\", sum(Repairs.RepairCost) AS \"Total Repair Cost\" FROM Photographers " +
                    "LEFT JOIN Repairs ON Photographers.ID = Repairs.PhotogID WHERE Repairs.Date IS NOT Repairs.FixedDate GROUP BY Photographers.Office ORDER BY count(Repairs.RepairID) DESC";
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