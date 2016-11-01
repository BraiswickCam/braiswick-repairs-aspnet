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
    public partial class NewRepair : System.Web.UI.Page
    {
        string databaseLocation = "C:\\datatest\\2016repairhistory.sqlite";
        int kitKitPH = 0, kitPhotogName = 1, kitLaptopID = 2, kitLaptopSN = 3, kitLaptopMake = 4, kitLaptopModel = 5, kitCameraID = 6, kitCameraSN = 7, kitCameraMake = 8, kitCameraModel = 9;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["type"] == null)
            {
                steptwo.Attributes["class"] = "hidden";
                kitsGrid.DataSource = GetKitList();
                kitsGrid.DataBind();
                //AddLinks();
            }
            else
            {
                stepone.Attributes["class"] = "hidden";
                if (Request.QueryString["type"] == "laptop")
                {
                    DataTable dt = GetLaptopDetails(Request.QueryString["id"]);
                    equipPanel.InnerText = String.Format("Laptop ID: {0}", dt.Rows[0][0]);
                    equipSN.InnerText = dt.Rows[0][1].ToString();
                    equipMake.InnerText = dt.Rows[0][2].ToString();
                    equipModel.InnerText = dt.Rows[0][3].ToString();
                    equipOption.InnerText = dt.Rows[0][4].ToString();
                    photogPanel.InnerText = String.Format("Photographer ID: {0}", dt.Rows[0][5]);
                    photogName.InnerText = dt.Rows[0][6].ToString();
                    photogInitial.InnerText = dt.Rows[0][7].ToString();
                    photogOffice.InnerText = dt.Rows[0][8].ToString();
                }
                if (Request.QueryString["type"] == "camera")
                {
                    DataTable dt = GetCameraDetails(Request.QueryString["id"]);
                    equipPanel.InnerText = String.Format("Camera ID: {0}", dt.Rows[0][0]);
                    equipSN.InnerText = dt.Rows[0][1].ToString();
                    equipMake.InnerText = dt.Rows[0][2].ToString();
                    equipModel.InnerText = dt.Rows[0][3].ToString();
                    equipOptionalRow.Attributes["class"] = "row hidden";
                    photogPanel.InnerText = String.Format("Photographer ID: {0}", dt.Rows[0][4]);
                    photogName.InnerText = dt.Rows[0][5].ToString();
                    photogInitial.InnerText = dt.Rows[0][6].ToString();
                    photogOffice.InnerText = dt.Rows[0][7].ToString();
                }
            }
        }

        protected DataTable GetKitList()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Kits.KitPH, Photographers.Name, Laptops.LaptopID, Laptops.SerialNumber, Laptops.Make, Laptops.Model, Cameras.CameraID, Cameras.SerialNumber, Cameras.Make, Cameras.Model FROM Kits " + 
                    "LEFT JOIN Photographers ON Kits.PhotogID = Photographers.ID LEFT JOIN Laptops ON Kits.LaptopID = Laptops.LaptopID LEFT JOIN Cameras ON Kits.CameraID = Cameras.CameraID ORDER BY Kits.KitPH ASC";
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

        protected DataTable GetLaptopDetails(string laptopID)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Kits.LaptopID, Laptops.SerialNumber, Laptops.Make, Laptops.Model, Laptops.OS, Kits.PhotogID, Photographers.Name, Photographers.Initials, Photographers.Office FROM Kits " +
                    "LEFT JOIN Laptops ON Kits.LaptopID = Laptops.LaptopID LEFT JOIN Photographers ON Kits.PhotogID = Photographers.ID WHERE Kits.LaptopID = @LaptopID";
                command.Parameters.Add(new SQLiteParameter("@LaptopID", laptopID));
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

        protected DataTable GetCameraDetails(string cameraID)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Kits.CameraID, Cameras.SerialNumber, Cameras.Make, Cameras.Model, Kits.PhotogID, Photographers.Name, Photographers.Initials, Photographers.Office FROM Kits " +
                    "LEFT JOIN Cameras ON Kits.CameraID = Cameras.CameraID LEFT JOIN Photographers ON Kits.PhotogID = Photographers.ID WHERE Kits.CameraID = @CameraID";
                command.Parameters.Add(new SQLiteParameter("@CameraID", cameraID));
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

        protected void AddLinks()
        {
            foreach (GridViewRow gr in kitsGrid.Rows)
            {
                if (gr.Cells[kitLaptopID].Text != "0" && gr.Cells[kitLaptopID].Text != "&nbsp;")
                {
                    string idHolder = gr.Cells[kitLaptopID].Text;
                    gr.Cells[kitLaptopID].Text = String.Format("<a href=\"NewRepair.aspx?LaptopID={0}\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}\">{0}</a>",
                        idHolder,
                        gr.Cells[kitLaptopMake].Text,
                        gr.Cells[kitLaptopModel].Text);
                    idHolder = gr.Cells[kitLaptopSN].Text;
                    gr.Cells[kitLaptopSN].Text = String.Format("<a href=\"NewRepair.aspx?LaptopID={0}\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}\">{0}</a>",
                        idHolder,
                        gr.Cells[kitLaptopMake].Text,
                        gr.Cells[kitLaptopModel].Text);
                }
            }
        }

        protected void kitsGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[kitLaptopMake].Visible = false;
            e.Row.Cells[kitLaptopModel].Visible = false;
            e.Row.Cells[kitCameraMake].Visible = false;
            e.Row.Cells[kitCameraModel].Visible = false;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Creates a modal for each row that displays that kits laptop and camera details and gives the option for a new repair

                System.Web.UI.HtmlControls.HtmlGenericControl createDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
                createDiv.ID = e.Row.Cells[kitKitPH].Text;
                createDiv.Attributes["class"] = "modal fade";
                createDiv.Attributes["role"] = "dialog";

                string laptopPresent = "", cameraPresent = "";
                if (e.Row.Cells[kitLaptopID].Text == "0" || e.Row.Cells[kitLaptopID].Text == "&nbsp;") laptopPresent = " disabled";
                if (e.Row.Cells[kitCameraID].Text == "0" || e.Row.Cells[kitCameraID].Text == "&nbsp;") cameraPresent = " disabled";

                string panels = String.Format("<div class=\"row\"><div class=\"col-xs-6\"><div class=\"panel panel-primary\"><div class=\"panel-heading\">Laptop ID: {0}</div><div class=\"panel-body\" style=\"word-break: break-all;\">" +
                    "<div class=\"row\"><div class=\"col-xs-6\">Serial Number: </div><div class=\"col-xs-6\">{1}</div></div><div class=\"row\"><div class=\"col-xs-6\">Make: </div><div class=\"col-xs-6\">{2}</div></div>" +
                    "<div class=\"row\"><div class=\"col-xs-6\">Model: </div><div class=\"col-xs-6\">{3}</div></div></div><div class=\"panel-footer text-center\"><a href=\"NewRepair.aspx?type=laptop&id={0}\" class=\"btn btn-primary{8}\" role=\"button\">New Repair</a></div></div></div>" +
                    "<div class=\"col-xs-6\"><div class=\"panel panel-primary\"><div class=\"panel-heading\">Camera ID: {4}</div><div class=\"panel-body\" style=\"word-break: break-all;\"><div class=\"row\"><div class=\"col-xs-6\">Serial Number: </div><div class=\"col-xs-6\">{5}</div></div><div class=\"row\">" +
                    "<div class=\"col-xs-6\">Make: </div><div class=\"col-xs-6\">{6}</div></div><div class=\"row\"><div class=\"col-xs-6\">Model: </div><div class=\"col-xs-6\">{7}</div></div></div><div class=\"panel-footer text-center\"><a href=\"NewRepair?type=camera&id={4}\" class=\"btn btn-primary{9}\" role=\"button\">New Repair</a></div></div></div></div>",
                    e.Row.Cells[kitLaptopID].Text,
                    e.Row.Cells[kitLaptopSN].Text,
                    e.Row.Cells[kitLaptopMake].Text,
                    e.Row.Cells[kitLaptopModel].Text,
                    e.Row.Cells[kitCameraID].Text,
                    e.Row.Cells[kitCameraSN].Text,
                    e.Row.Cells[kitCameraMake].Text,
                    e.Row.Cells[kitCameraModel].Text,
                    laptopPresent, cameraPresent);

                createDiv.InnerHtml = String.Format("<div class=\"modal-dialog modal-lg\"><div class=\"modal-content\"><div class=\"modal-header\"><button type=\"button\" class=\"close\" data-dismiss=\"modal\">&times;</button>" + 
                    "<h4 class=\"modal-title\">New Repair for kit {0}</h4></div><div class=\"modal-body\">{1}</div><div class=\"modal-footer\"><button type=\"button\" class=\"btn btn-default\" data-dismiss=\"modal\">Close</button></div></div></div>", e.Row.Cells[kitKitPH].Text, panels);

                this.Controls.Add(createDiv);
                e.Row.Attributes["data-toggle"] = "modal";
                e.Row.Attributes["data-target"] = String.Format("#{0}", e.Row.Cells[kitKitPH].Text);
                e.Row.Attributes["style"] = "cursor: pointer;";
            }
        }
    }
}