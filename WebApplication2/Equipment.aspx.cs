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
    public partial class Equipment : System.Web.UI.Page
    {
        SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", "C:\\datatest\\2016repairhistory.sqlite"));
        int activeCol, kitIdCol, kitPhCol, photogIdCol, photogInitialCol, otherIdCol;

        protected void Page_Load(object sender, EventArgs e)
        {
            activeCol = 4; kitIdCol = 5; kitPhCol = 6; photogIdCol = 7; photogInitialCol = 8; otherIdCol = 9;

            if (equipDrop.SelectedValue == "laptop")
            {
                activeCol = activeCol + 1;
                kitIdCol = kitIdCol + 1;
                kitPhCol = kitPhCol + 1;
                photogIdCol = photogIdCol + 1;
                photogInitialCol = photogInitialCol + 1;
                otherIdCol = otherIdCol + 1;
            }

            equipGrid.DataSource = equipDrop.SelectedValue == "laptop" ? GetLaptopList() : GetCameraList();
            equipGrid.DataBind();
            AddLinks();
        }

        protected DataTable GetLaptopList()
        {
            using (m_dbConnection)
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Laptops.LaptopID, Laptops.SerialNumber, Laptops.Make, Laptops.Model, Laptops.OS, Laptops.Active, Kits.KitID, Kits.KitPH, Kits.PhotogID, Photographers.Initials, Kits.CameraID FROM Laptops LEFT JOIN Kits ON Laptops.LaptopID = Kits.LaptopID LEFT JOIN Photographers ON Kits.PhotogID = Photographers.ID";
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

        protected DataTable GetCameraList()
        {
            using (m_dbConnection)
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Cameras.CameraID, Cameras.SerialNumber, Cameras.Make, Cameras.Model, Cameras.Active, Kits.KitID, Kits.KitPH, Kits.PhotogID, Photographers.Initials, Kits.LaptopID FROM Cameras LEFT JOIN Kits ON Cameras.CameraID = Kits.CameraID LEFT JOIN Photographers ON Kits.PhotogID = Photographers.ID";
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
            foreach (GridViewRow gr in equipGrid.Rows)
            {
                if (gr.Cells[kitIdCol] != null)
                {
                    string kitIDHolder = gr.Cells[kitIdCol].Text;
                    string photogIDHolder, otherIDHolder, photogInitialHolder;
                    string otherType = equipDrop.SelectedValue == "laptop" ? "Camera" : "Laptop";

                    if (gr.Cells[photogIdCol].Text == "&nbsp;") photogIDHolder = "none";
                    else photogIDHolder = gr.Cells[photogIdCol].Text;

                    if (gr.Cells[otherIdCol].Text == "&nbsp;") otherIDHolder = "none";
                    else otherIDHolder = gr.Cells[otherIdCol].Text;

                    if (gr.Cells[photogInitialCol].Text == "&nbsp;") photogInitialHolder = "none";
                    else photogInitialHolder = gr.Cells[photogInitialCol].Text;

                    gr.Cells[kitIdCol].Text =
                        String.Format("<div class=\"kitLink\"><a href=\"Kits2.aspx?KitID={0}\">{1}</a><div class=\"kitDrop\"><a href=\"PhotogDetails.aspx?PhotogID={2}\">Photog ID = {3}</a></br><a href=\"{4}s.aspx?{4}ID={5}\">{4} ID = {5}</a></div></div>",
                        kitIDHolder,
                        gr.Cells[kitPhCol].Text,
                        photogIDHolder,
                        photogInitialHolder,
                        otherType,
                        otherIDHolder);
                }
            }
        }


        protected void equipGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[kitPhCol].Visible = false;
            e.Row.Cells[photogIdCol].Visible = false;
            e.Row.Cells[photogInitialCol].Visible = false;
            e.Row.Cells[otherIdCol].Visible = false;
        }
    }
}