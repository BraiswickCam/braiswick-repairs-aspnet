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
    public partial class Photogs : System.Web.UI.Page
    {
        SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", "C:\\datatest\\2016repairhistory.sqlite"));
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataSource = GetPhotogList(activeBox.Checked);
            GridView1.DataBind();
            AddLinks();
        }

        protected void AddLinks()
        {
            foreach (GridViewRow gr in GridView1.Rows)
            {
                if (gr.Cells[5] != null)
                {
                    //HyperLink hp = new HyperLink();
                    //hp.Target = "_blank";
                    //hp.ToolTip = "Open kit record for " + gr.Cells[1].Text;
                    //hp.Text = gr.Cells[6].Text;
                    //hp.NavigateUrl = "~/Kits.aspx?KitID=" + gr.Cells[5].Text;
                    //gr.Cells[5].Controls.Add(hp);

                    string kitIDHolder = gr.Cells[5].Text;
                    string cameraIDHolder, laptopIDHolder;

                    if (gr.Cells[7].Text == "&nbsp;") cameraIDHolder = "none";
                    else cameraIDHolder = gr.Cells[7].Text;

                    if (gr.Cells[8].Text == "&nbsp;") laptopIDHolder = "none";
                    else laptopIDHolder = gr.Cells[8].Text;

                    gr.Cells[5].Text =
                        String.Format("<div class=\"kitLink\"><a href=\"Kits2.aspx?KitID={0}\">{1}</a><div class=\"kitDrop\"><a href=\"Cameras.aspx?CameraID={2}\">Camera ID = {2}</a></br><a href=\"Laptops.aspx?LaptopID={3}\">Laptop ID = {3}</a></div></div>",
                        kitIDHolder,
                        gr.Cells[6].Text,
                        cameraIDHolder,
                        laptopIDHolder);
                }

                if (gr.Cells[0] != null)
                {
                    HyperLink hp = new HyperLink();
                    hp.Target = "_blank";
                    hp.ToolTip = "Edit details for " + gr.Cells[1].Text;
                    hp.Text = gr.Cells[0].Text;
                    //hp.ImageUrl = "~/edit-icon-1901.png";
                    hp.NavigateUrl = "~/PhotogDetails.aspx?PhotogID=" + gr.Cells[0].Text;
                    gr.Cells[0].Controls.Add(hp);
                    //Image editImg = new Image();
                    //editImg.ImageUrl = "~/edit-icon-1901.png";
                    //gr.Cells[0].Controls.Add(editImg);
                }
            }
        }

        protected DataTable GetPhotogList(bool active)
        {
            using (m_dbConnection)
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Photographers.ID, Photographers.Name, Photographers.Initials, Photographers.Active, Photographers.Office, Kits.KitID, Kits.KitPH, Kits.CameraID, Kits.LaptopID FROM Photographers LEFT JOIN Kits ON Photographers.ID=Kits.PhotogID WHERE Photographers.Active = @Active OR Photographers.Active = @Active2";
                command.Parameters.Add(new SQLiteParameter("@Active", active ? "1" : "0"));
                command.Parameters.Add(new SQLiteParameter("@Active2", active ? "1" : "1"));
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

        protected DataTable GetPhotogKitList(bool active)
        {
            DataTable dt = GetPhotogList(active);
            dt.Columns.Add("KitID", typeof(int));
            dt.Columns.Add("KitPH", typeof(string));
            dt.Columns.Add("CameraID", typeof(int));
            dt.Columns.Add("LaptopID", typeof(int));
            foreach (DataRow dr in dt.Rows)
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT KitID, KitPH, CameraID, LaptopID FROM Kits WHERE PhotogID = @PhotogID";
                command.Parameters.Add(new SQLiteParameter("@PhotogID", dr[0]));
                m_dbConnection.Open();
                SQLiteDataReader sqldr = command.ExecuteReader();
                while (sqldr.Read())
                {
                    dr["KitID"] = sqldr["KitID"];
                    dr["KitPH"] = sqldr["KitPH"];
                    dr["CameraID"] = sqldr["CameraID"];
                    dr["LaptopID"] = sqldr["LaptopID"];
                }
                m_dbConnection.Close();
            }
            return dt;
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = false;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                string[] headers = new string[] { "ID", "Name", "Initials", "Active", "Office", "KitID", "", "", "" };
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
                    ViewState["lastSortState"] = "ID";
                }
                return ViewState["lastSortState"].ToString();
            }

            set
            {
                ViewState["lastSortState"] = value;
            }
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
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
            DataView sortedView = new DataView(GetPhotogList(activeBox.Checked));
            sortedView.Sort = e.SortExpression + " " + sortingDirection;
            lastSort = e.SortExpression;
            GridView1.DataSource = sortedView;
            GridView1.DataBind();
            AddLinks();
            AddSortImage(e.SortExpression, sortingDirection);
        }

        protected void AddSortImage(string column, string direction)
        {
            for (int i = 0; i < GridView1.HeaderRow.Cells.Count; i++)
            {
                if (GridView1.HeaderRow.Cells[i].Text == column)
                {
                    Image img = new Image();
                    img.ImageUrl = direction == "Asc" ? "images/1476734841_arrow-up-01.png" : "images/1476734822_arrow-down-01.png";
                    GridView1.HeaderRow.Cells[i].Controls.Add(img);
                }
                
            }

            
            
        }

    }
}