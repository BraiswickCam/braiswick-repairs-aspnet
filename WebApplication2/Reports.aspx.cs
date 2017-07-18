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
        SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation));
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
                else if (report == "AssignedCameraPercent")
                {
                    AssignedCameraPercent();
                }
                else if (report == "LaptopRepairCount")
                {
                    LaptopRepairCount();
                }
            }
        }

        protected void RepairCost()
        {
            BindData(QueryDatabase("SELECT Photographers.ID, Photographers.Name, Photographers.Initials, Photographers.Active, Photographers.Office, sum(Repairs.RepairCost) AS \"Total Repair Cost\" FROM Photographers " + 
                "LEFT JOIN Repairs ON Photographers.ID = Repairs.PhotogID WHERE Repairs.RepairCost > 0 GROUP BY Photographers.ID ORDER BY \"Total Repair Cost\" DESC"));

            AddChars("£", "", 5);

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
            BindData(QueryDatabase("SELECT count(Laptops.LaptopID) AS \"Total Laptops\", count(distinct Kits.LaptopID) + count(distinct Kits.SpareLaptopID) AS \"Assigned Laptops\", " +
                    "round(((count(distinct Kits.LaptopID) + count(distinct Kits.SpareLaptopID) + 0.0) / count(Laptops.LaptopID) + 0.0) * 100.0, 2) AS \"Assigned Percentage\" FROM Laptops " +
                    "LEFT JOIN Kits ON Kits.LaptopID = Laptops.LaptopID OR Kits.SpareLaptopID = Laptops.LaptopID"));

            AddChars("", "%", 2);
        }

        protected void AssignedCameraPercent()
        {
            BindData(QueryDatabase("SELECT count(Cameras.CameraID) AS \"Total Cameras\", count(distinct Kits.CameraID) + count(distinct Kits.SpareCameraID) AS \"Assigned Cameras\", " +
                "round(((count(distinct Kits.CameraID) + count(distinct Kits.SpareCameraID) + 0.0) / count(Cameras.CameraID) + 0.0) * 100.0, 2) AS \"Assigned Percentage\" FROM Cameras " +
                "LEFT JOIN Kits ON Kits.CameraID = Cameras.CameraID OR Kits.SpareCameraID = Cameras.CameraID"));

            AddChars("", "%", 2);
        }

        protected void LaptopRepairCount()
        {
            BindData(QueryDatabase("SELECT Laptops.LaptopID, Laptops.SerialNumber, Laptops.Make, Laptops.Model, count(Repairs.RepairID) AS \"Repairs Total\" FROM Laptops " +
                "LEFT JOIN Repairs ON Laptops.LaptopID = Repairs.LaptopID WHERE Repairs.Date IS NOT Repairs.FixedDate AND Laptops.Active = 1 GROUP BY Laptops.LaptopID ORDER BY \"Repairs Total\" DESC"));

            AddEquipmentIDLinks(0, 2, 3, "Laptop");
        }

        protected void CameraRepairCount()
        {
            BindData(QueryDatabase("SELECT Cameras.CameraID, Cameras.SerialNumber, Cameras.Make, Cameras.Model, count(Repairs.RepairID) AS \"Repairs Total\" FROM Cameras " +
                "LEFT JOIN Repairs ON Cameras.CameraID = Repairs.CameraID WHERE Repairs.Date IS NOT Repairs.FixedDate AND Cameras.Active = 1 GROUP BY Cameras.CameraID ORDER BY \"Repairs Total\" DESC"));

            AddEquipmentIDLinks(0, 2, 3, "Camera");
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
                if (gvr.Cells[colIndexID].Text != "0" && gvr.Cells[colIndexID].Text != "&nbsp;")
                {
                    HyperLink hp = new HyperLink();
                    hp.Target = "_blank";
                    hp.ToolTip = "Edit details for " + gvr.Cells[colIndexName].Text;
                    hp.Text = String.Format("<span class=\"glyphicon glyphicon-edit\"></span> {0}", gvr.Cells[colIndexID].Text);
                    hp.CssClass = "btn btn-primary btn-sm";
                    hp.NavigateUrl = "~/PhotogDetails.aspx?PhotogID=" + gvr.Cells[colIndexID].Text;
                    gvr.Cells[colIndexID].Controls.Add(hp);
                }
            }
        }

        protected void AddEquipmentIDLinks(int colIndexID, int colIndexMake, int colIndexModel, string equipmentType)
        {
            foreach (GridViewRow gvr in resultsGrid.Rows)
            {
                if (gvr.Cells[colIndexID].Text != "0" && gvr.Cells[colIndexID].Text != "&nbsp;")
                {
                    HyperLink hp = new HyperLink();
                    hp.Target = "_blank";
                    hp.ToolTip = String.Format("Edit details for {0} {1}", gvr.Cells[colIndexMake].Text, gvr.Cells[colIndexModel].Text);
                    hp.Text = String.Format("<span class=\"glyphicon glyphicon-edit\"></span> {0}", gvr.Cells[colIndexID].Text);
                    hp.CssClass = "btn btn-primary btn-sm";
                    hp.NavigateUrl = String.Format("~/{1}s.aspx?{1}ID={0}", gvr.Cells[colIndexID].Text, equipmentType);
                    gvr.Cells[colIndexID].Controls.Add(hp);
                }
            }
        }

        protected void AddChars(string preString, string postString, int index)
        {
            foreach (GridViewRow gvr in resultsGrid.Rows)
            {
                if (gvr.Cells[index].Text != "0" && gvr.Cells[index].Text != "&nbsp;")
                {
                    string hold = gvr.Cells[index].Text;
                    gvr.Cells[index].Text = String.Format("{1}{0}{2}", hold, preString, postString);
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