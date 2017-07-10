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
    public partial class RepairList : System.Web.UI.Page
    {
        SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void LoadData()
        {
            DataTable dt = GetRepairList();
            repairListGrid.DataSource = dt;
            repairListGrid.DataBind();
            AddLinks();
            for (int i = 11; i < 22; i++)
            {
                foreach (GridViewRow gvr in repairListGrid.Rows)
                {
                    gvr.Cells[i].Visible = false;
                }

                repairListGrid.HeaderRow.Cells[i].Visible = false;
            }
        }

        protected DataTable GetRepairList()
        {
            SQLiteCommand command = m_dbConnection.CreateCommand();
            command.CommandText = "SELECT Repairs.*, Cameras.SerialNumber, Cameras.Make, Cameras.Model, Laptops.SerialNumber, Laptops.Make, Laptops.Model, Laptops.OS, Kits.KitPH, Photographers.Name, Photographers.Initials, Photographers.Office FROM Repairs "
                + "LEFT JOIN Cameras ON Repairs.CameraID = Cameras.CameraID "
                + "LEFT JOIN Laptops ON Repairs.LaptopID = Laptops.LaptopID "
                + "LEFT JOIN Kits ON Repairs.KitID = Kits.KitID "
                + "LEFT JOIN Photographers ON Repairs.PhotogID = Photographers.ID "
                + "ORDER BY RepairID DESC";
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

        protected void AddLinks()
        {
            int repairID = 0, camID = 1, lapID = 2, kitID = 3, photogID = 4, camSN = 11, camMake = 12, camModel = 13, lapSN = 14, lapMake = 15, lapModel = 16, lapOS = 17, kitPH = 18, photogName = 19, photogInitials = 20, photogOffice = 21;
            foreach (GridViewRow gr in repairListGrid.Rows)
            {
                if (gr.Cells[repairID].Text != "0" && gr.Cells[repairID].Text != "&nbsp;")
                {
                    HyperLink hp = new HyperLink();
                    hp.CssClass = "btn btn-primary";
                    hp.Target = "_blank";
                    hp.ToolTip = "Open repair record for RepairID " + gr.Cells[repairID].Text;
                    hp.Text = gr.Cells[repairID].Text;
                    hp.NavigateUrl = String.Format("~/Repairs2.aspx?repairID={0}", hp.Text);
                    gr.Cells[repairID].Controls.Add(hp);
                }

                if (gr.Cells[camID].Text != "0" && gr.Cells[camID].Text != "&nbsp;")
                {
                    string hold = gr.Cells[camID].Text;
                    gr.Cells[camID].Text = String.Format("<a href=\"Cameras.aspx?CameraID={0}\" class=\"btn btn-default\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}</br>{3}\">{0}</a>",
                        hold,
                        gr.Cells[camSN].Text,
                        gr.Cells[camMake].Text,
                        gr.Cells[camModel].Text);
                }

                if (gr.Cells[lapID].Text != "0" && gr.Cells[lapID].Text != "&nbsp;")
                {
                    string hold = gr.Cells[lapID].Text;
                    gr.Cells[lapID].Text = String.Format("<a href=\"Laptops.aspx?LaptopID={0}\" class=\"btn btn-default\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}</br>{3}</br>{4}\">{0}</a>",
                        hold,
                        gr.Cells[lapSN].Text,
                        gr.Cells[lapMake].Text,
                        gr.Cells[lapModel].Text,
                        gr.Cells[lapOS].Text);
                }

                if (gr.Cells[photogID].Text != "0" && gr.Cells[photogID].Text != "&nbsp;")
                {
                    string hold = gr.Cells[photogID].Text;
                    gr.Cells[photogID].Text = String.Format("<a href=\"PhotogDetails.aspx?PhotogID={0}\" class=\"btn btn-default\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}</br>{3}\">{0}</a>",
                        hold,
                        gr.Cells[photogName].Text,
                        gr.Cells[photogInitials].Text,
                        gr.Cells[photogOffice].Text);
                }

                if (gr.Cells[kitID].Text != "0" && gr.Cells[kitID].Text != "&nbsp;")
                {
                    string hold = gr.Cells[kitID].Text;
                    gr.Cells[kitID].Text = String.Format("<a href=\"Kits2.aspx?KitID={0}\" class=\"btn btn-default\">{1}</a>",
                        hold,
                        gr.Cells[kitPH].Text);
                }
            }
        }

        protected void repairListGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            repairListGrid.PageIndex = e.NewPageIndex;
            LoadData();
        }
    }
}