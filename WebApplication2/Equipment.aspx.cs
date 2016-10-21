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
            newEquipLink.HRef = equipDrop.SelectedValue == "laptop" ? "Laptops.aspx" : "Cameras.aspx";
            newEquipLink.InnerText = equipDrop.SelectedValue == "laptop" ? "Add New Laptop" : "Add New Camera";
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

                if (gr.Cells[0] != null)
                {
                    HyperLink hp = new HyperLink();
                    hp.Target = "_blank";
                    hp.ToolTip = "Edit details for " + gr.Cells[1].Text;
                    hp.Text = gr.Cells[0].Text;
                    hp.NavigateUrl = String.Format("~/{0}s.aspx?{0}ID={1}", equipDrop.SelectedValue == "laptop" ? "Laptop" : "Camera", gr.Cells[0].Text);
                    gr.Cells[0].Controls.Add(hp);
                }
            }
        }

        protected void equipGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[kitPhCol].Visible = false;
            e.Row.Cells[photogIdCol].Visible = false;
            e.Row.Cells[photogInitialCol].Visible = false;
            e.Row.Cells[otherIdCol].Visible = false;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                string[] headers = equipDrop.SelectedValue == "laptop" ? new string[] { "LaptopID", "SerialNumber", "Make", "Model", "OS", "Active", "KitID", "", "", "", "" } :
                    new string[] { "CameraID", "SerialNumber", "Make", "Model", "Active", "KitID", "", "", "", "" }; 
                for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    if (headers[i] == lastSort)
                    {
                        Image img = new Image();
                        img.ImageUrl = dir == SortDirection.Ascending ? "images/small_up.png" : "images/small_down.png";
                        e.Row.Cells[i].Controls.Add(img);
                    }

                }
            }
        }

        public SortDirection dir
        {
            get
            {
                if (ViewState["dirState"] == null)
                {
                    ViewState["dirState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["dirState"];
            }
            set
            {
                ViewState["dirState"] = value;
            }
        }

        public string lastSort
        {
            get
            {
                if (ViewState["lastSortState"] == null)
                {
                    ViewState["lastSortState"] = equipDrop.SelectedValue == "laptop" ? "LaptopID" : "CameraID";
                }
                return ViewState["lastSortState"].ToString();
            }

            set
            {
                ViewState["lastSortState"] = value;
            }
        }

        protected void equipGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortingDirection = string.Empty;
            if (e.SortExpression != lastSort) dir = SortDirection.Descending;
            if (dir == SortDirection.Ascending)
            {
                dir = SortDirection.Descending;
                sortingDirection = "Desc";
            }
            else
            {
                dir = SortDirection.Ascending;
                sortingDirection = "Asc";
            }
            DataView sortedView = new DataView(equipDrop.SelectedValue == "laptop" ? GetLaptopList() : GetCameraList());
            sortedView.Sort = e.SortExpression + " " + sortingDirection;
            lastSort = e.SortExpression;
            equipGrid.DataSource = sortedView;
            equipGrid.DataBind();
            AddLinks();
        }
    }
}