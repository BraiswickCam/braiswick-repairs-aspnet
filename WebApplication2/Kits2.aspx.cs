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
    public partial class Kits2 : System.Web.UI.Page
    {
        //Database Location
        string databaseLocation = "C:\\datatest\\2016repairhistory.sqlite";

        //History table index references
        int repairID = 0;
        int camID = 1;
        int camSerial = 2;
        int camMake = 3;
        int camModel = 4;
        int lapID = 5;
        int lapSerial = 6;
        int lapMake = 7;
        int lapModel = 8;
        int photogID = 9;
        int photogInitials = 10;
        int photogName = 11;
        int photogOffice = 12;

        protected void Page_Load(object sender, EventArgs e)
        {
            warningDiv.Attributes["class"] = "row top20 hidden";
            if (Request.QueryString["type"] != null)
            {
                if(ChangeItem()) { Response.Redirect(String.Format("~/Kits2.aspx?KitID={0}", Request.QueryString["KitID"])); }
            }

            if (!IsPostBack)
            {
                DataTable kits = GetKits();
                foreach (DataRow dr in kits.Rows)
                {
                    DropDownList1.Items.Add(new ListItem(dr[0].ToString(), dr[1].ToString()));
                }
                if (Request.QueryString["KitID"] != null) DropDownList1.SelectedValue = Request.QueryString["KitID"];
            }
            RefreshDetails();
        }

        protected DataTable GetFullKitRecord(string kitID)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Kits.KitID, Kits.KitPH, Photographers.ID, Photographers.Name, Photographers.Initials, Photographers.Office, Cameras.CameraID, Cameras.SerialNumber, Cameras.Make, Cameras.Model, Laptops.LaptopID, Laptops.SerialNumber, Laptops.Make, Laptops.Model, Laptops.OS, " +
                    "cam2.CameraID, cam2.SerialNumber, cam2.Make, cam2.Model, lap2.LaptopID, lap2.SerialNumber, lap2.Make, lap2.Model, lap2.OS FROM Kits " +
                    "LEFT JOIN Photographers ON Kits.PhotogID = Photographers.ID " +
                    "LEFT JOIN Cameras ON Kits.CameraID = Cameras.CameraID LEFT JOIN Cameras AS cam2 ON Kits.SpareCameraID = cam2.CameraID " +
                    "LEFT JOIN Laptops ON Kits.LaptopID = Laptops.LaptopID LEFT JOIN Laptops AS lap2 ON Kits.SpareLaptopID = lap2.LaptopID " +
                    "WHERE Kits.KitID = @KitID";
                command.Parameters.Add(new SQLiteParameter("@KitID", kitID));
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

        protected DataTable GetKits()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                string sql = String.Format("SELECT KitPH, KitID FROM Kits ORDER BY KitPH ASC");
                using (SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection))
                {
                    using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
                    {
                        command.Connection = m_dbConnection;
                        sda.SelectCommand = command;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }

        protected DataTable GetHistory(string kitID)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Repairs.RepairID, Repairs.CameraID, Cameras.SerialNumber, Cameras.Make, Cameras.Model, Repairs.LaptopID, Laptops.SerialNumber, Laptops.Make, Laptops.Model, Repairs.PhotogID, Photographers.Initials, Photographers.Name, Photographers.Office, Repairs.Date, Repairs.Fixed, Repairs.FixedDate, Repairs.TechInitials, Repairs.Notes, Repairs.RepairCost FROM Repairs " + 
                    "LEFT JOIN Cameras ON Repairs.CameraID = Cameras.CameraID " + 
                    "LEFT JOIN Laptops ON Repairs.LaptopID = Laptops.LaptopID " +
                    "LEFT JOIN Photographers ON Repairs.PhotogID = Photographers.ID " + 
                    "WHERE Repairs.KitID = @KitID " +
                    "ORDER BY Repairs.RepairID DESC";
                command.Parameters.Add(new SQLiteParameter("@KitID", kitID));
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

        protected void ToggleTooltip(bool toggle, System.Web.UI.HtmlControls.HtmlGenericControl div, string item = "none")
        {
            div.Attributes["class"] = toggle ? "panel panel-danger" : "panel panel-default";
            div.Attributes["data-toggle"] = toggle ? "tooltip" : "";
            div.Attributes["data-title"] = toggle ? String.Format("No {0} assigned!", item) : "";
            div.Attributes["data-placement"] = toggle ? "auto" : "";
        }

        protected void RefreshDetails()
        {
            DataTable details = GetFullKitRecord(DropDownList1.SelectedValue);

            //Populate Photographer details
            nameLabel.Text = details.Rows[0][3].ToString();
            initialLabel.Text = details.Rows[0][4].ToString();
            officeLabel.Text = details.Rows[0][5].ToString();
            if (string.IsNullOrEmpty(details.Rows[0][2].ToString())) ToggleTooltip(true, photogPanel, "photographer");
            else ToggleTooltip(false, photogPanel);

            //Populate Camera details
            camMakeLabel.Text = details.Rows[0][8].ToString();
            camModelLabel.Text = details.Rows[0][9].ToString();
            camSNLabel.Text = details.Rows[0][7].ToString();
            if (string.IsNullOrEmpty(details.Rows[0][6].ToString())) ToggleTooltip(true, cameraPanel, "camera");
            else ToggleTooltip(false, cameraPanel);

            //Populate Laptop details
            lapMakeLabel.Text = details.Rows[0][12].ToString();
            lapModelLabel.Text = details.Rows[0][13].ToString();
            lapSNLabel.Text = details.Rows[0][11].ToString();
            if (string.IsNullOrEmpty(details.Rows[0][10].ToString())) ToggleTooltip(true, laptopPanel, "laptop");
            else ToggleTooltip(false, laptopPanel);

            //Populate Spare Camera details
            spareCamSN.Text = details.Rows[0][16].ToString();
            spareCamMake.Text = details.Rows[0][17].ToString();
            spareCamModel.Text = details.Rows[0][18].ToString();
            if (string.IsNullOrEmpty(details.Rows[0][15].ToString()))
            {
                ToggleTooltip(true, spareCameraPanel, "spare camera");
                spareCamTab.InnerHtml = "Spare <span class=\"glyphicon glyphicon-remove-circle\"></span>";
                spareCamToMain.Attributes["class"] = "btn btn-info disabled";
            }
            else
            {
                ToggleTooltip(false, spareCameraPanel);
                spareCamTab.InnerHtml = "Spare <span class=\"glyphicon glyphicon-ok-circle\"></span>";
                spareCamToMain.Attributes["class"] = "btn btn-info";
            }

            //Populate Spare Laptop details
            spareLapSN.Text = details.Rows[0][20].ToString();
            spareLapMake.Text = details.Rows[0][21].ToString();
            spareLapModel.Text = details.Rows[0][22].ToString();
            if (string.IsNullOrEmpty(details.Rows[0][19].ToString()))
            {
                ToggleTooltip(true, spareLaptopPanel, "spare laptop");
                spareLapTab.InnerHtml = "Spare <span class=\"glyphicon glyphicon-remove-circle\"></span>";
                spareLapToMain.Attributes["class"] = "btn btn-info disabled";
            }
            else
            {
                ToggleTooltip(false, spareLaptopPanel);
                spareLapTab.InnerHtml = "Spare <span class=\"glyphicon glyphicon-ok-circle\"></span>";
                spareLapToMain.Attributes["class"] = "btn btn-info";
            }

            //Populate History records
            try
            {
                DataTable history = GetHistory(DropDownList1.SelectedValue);
                historyGridView.DataSource = history;
                historyGridView.DataBind();
                foreach (GridViewRow gr in historyGridView.Rows)
                {
                    //Add link to full repair record page
                    HyperLink hp = new HyperLink();
                    hp.CssClass = "btn";
                    hp.Target = "_blank";
                    hp.ToolTip = "Open repair record for RepairID " + gr.Cells[repairID].Text;
                    hp.Text = gr.Cells[repairID].Text;
                    hp.NavigateUrl = "~/Repairs2.aspx?repairID=" + hp.Text;
                    gr.Cells[repairID].Controls.Add(hp);

                    if (gr.Cells[camID].Text != "0" && gr.Cells[camID].Text != "&nbsp;")
                    {
                        string idHolder = gr.Cells[camID].Text;
                        gr.Cells[camID].Text = String.Format("<a href=\"Cameras.aspx?CameraID={0}\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}</br>{3}\">{0}</a>",
                            idHolder,
                            gr.Cells[camSerial].Text,
                            gr.Cells[camMake].Text,
                            gr.Cells[camModel].Text);
                    }

                    if (gr.Cells[lapID].Text != "0" && gr.Cells[lapID].Text != "&nbsp;")
                    {
                        string idHolder = gr.Cells[lapID].Text;
                        gr.Cells[lapID].Text = String.Format("<a href=\"Laptops.aspx?LaptopID={0}\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}</br>{3}\">{0}</a>",
                            idHolder,
                            gr.Cells[lapSerial].Text,
                            gr.Cells[lapMake].Text,
                            gr.Cells[lapModel].Text);
                    }

                    if (gr.Cells[photogID].Text != "0" && gr.Cells[photogID].Text != "&nbsp;")
                    {
                        string idHolder = gr.Cells[photogID].Text;
                        gr.Cells[photogID].Text = String.Format("<a href=\"PhotogDetails.aspx?PhotogID={0}\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}</br>{3}\">{0}</a>",
                            idHolder,
                            gr.Cells[photogInitials].Text,
                            gr.Cells[photogName].Text,
                            gr.Cells[photogOffice].Text);
                    }
                }
            }
            catch (SQLiteException)
            {

            }

            //Refreshes Add/Remove links

            string kitID = DropDownList1.SelectedValue;

            photogAddRemove.HRef = details.Rows[0][2].ToString() != string.Empty ? String.Format("Kits2.aspx?KitID={0}&type=remPhotog&ID={1}", kitID, details.Rows[0][2].ToString()) : String.Format("Photogs.aspx?KitID={0}", kitID);
            photogAddRemove.InnerHtml = details.Rows[0][2].ToString() != string.Empty ? "<span class=\"glyphicon glyphicon-minus\"></span> Remove Photographer" : "<span class=\"glyphicon glyphicon-plus\"></span> Add Photographer";
            photogAddRemove.Attributes["class"] = details.Rows[0][2].ToString() != string.Empty ? "btn btn-default" : "btn btn-primary";

            cameraAddRemove.HRef = details.Rows[0][6].ToString() != string.Empty ? String.Format("Kits2.aspx?KitID={0}&type=remCamera&ID={1}", kitID, details.Rows[0][6].ToString()) : String.Format("Equipment.aspx?KitID={0}&type=Camera", kitID);
            cameraAddRemove.InnerHtml = details.Rows[0][6].ToString() != string.Empty ? "<span class=\"glyphicon glyphicon-minus\"></span> Remove Camera" : "<span class=\"glyphicon glyphicon-plus\"></span> Add Camera";
            cameraAddRemove.Attributes["class"] = details.Rows[0][6].ToString() != string.Empty ? "btn btn-default" : "btn btn-primary";

            laptopAddRemove.HRef = details.Rows[0][10].ToString() != string.Empty ? String.Format("Kits2.aspx?KitID={0}&type=remLaptop&ID={1}", kitID, details.Rows[0][10].ToString()) : String.Format("Equipment.aspx?KitID={0}&type=Laptop", kitID);
            laptopAddRemove.InnerHtml = details.Rows[0][10].ToString() != string.Empty ? "<span class=\"glyphicon glyphicon-minus\"></span> Remove Laptop" : "<span class=\"glyphicon glyphicon-plus\"></span> Add Laptop";
            laptopAddRemove.Attributes["class"] = details.Rows[0][10].ToString() != string.Empty ? "btn btn-default" : "btn btn-primary";

            spareCamAddRemove.HRef = details.Rows[0][15].ToString() != string.Empty ? String.Format("#removelink") : String.Format("#addlink");
            spareCamAddRemove.InnerHtml = details.Rows[0][15].ToString() != string.Empty ? "<span class=\"glyphicon glyphicon-minus\"></span> Remove Spare Camera" : "<span class=\"glyphicon glyphicon-plus\"></span> Add Spare Camera";
            spareCamAddRemove.Attributes["class"] = details.Rows[0][15].ToString() != string.Empty ? "btn btn-default" : "btn btn-primary";

            spareLapAddRemove.HRef = details.Rows[0][19].ToString() != string.Empty ? String.Format("#removelink") : String.Format("#addlink");
            spareLapAddRemove.InnerHtml = details.Rows[0][19].ToString() != string.Empty ? "<span class=\"glyphicon glyphicon-minus\"></span> Remove Spare Laptop" : "<span class=\"glyphicon glyphicon-plus\"></span> Add Spare Laptop";
            spareLapAddRemove.Attributes["class"] = details.Rows[0][19].ToString() != string.Empty ? "btn btn-default" : "btn btn-primary";
        }

        protected void historyGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[camSerial].Visible = false;
            e.Row.Cells[camMake].Visible = false;
            e.Row.Cells[camModel].Visible = false;
            e.Row.Cells[lapSerial].Visible = false;
            e.Row.Cells[lapMake].Visible = false;
            e.Row.Cells[lapModel].Visible = false;
            e.Row.Cells[photogName].Visible = false;
            e.Row.Cells[photogInitials].Visible = false;
            e.Row.Cells[photogOffice].Visible = false;
        }

        protected bool ChangeItem()
        {
            string type = Request.QueryString["type"];
            int itemID;
            try { itemID = Convert.ToInt32(Request.QueryString["ID"]); }
            catch (System.FormatException) { return false; }
            int oldID = itemID;
            int kitID;
            try { kitID = Convert.ToInt32(Request.QueryString["KitID"]); }
            catch (System.FormatException) { return false; }

            if (type == "addLaptop") { type = "LaptopID"; }
            else if (type == "addCamera") { type = "CameraID"; }
            else if (type == "addPhotog") { type = "PhotogID"; }
            else if (type == "remLaptop") { type = "LaptopID"; itemID = 0; }
            else if (type == "remCamera") { type = "CameraID"; itemID = 0; }
            else if (type == "remPhotog") { type = "PhotogID"; itemID = 0; }
            else { return false; }

            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = String.Format("UPDATE Kits SET {0}=@newID WHERE KitID=@KitID; INSERT INTO Repairs ({0}, KitID, Date, Fixed, FixedDate, Notes) VALUES (@oldID, @KitID, \"{1}\", 1, \"{1}\", \"{2}\");", 
                    type,
                    DateTime.Now,
                    itemID > 0 ? String.Format("{0} added to kit.", type) : String.Format("{0} removed from kit.", type));
                command.Parameters.Add(new SQLiteParameter("@newID", itemID));
                command.Parameters.Add(new SQLiteParameter("@KitID", kitID));
                command.Parameters.Add(new SQLiteParameter("@oldID", oldID));
                m_dbConnection.Open();
                return command.ExecuteNonQuery() > 0;
            }
        }

        protected void saveNewKit_Click(object sender, EventArgs e)
        {
            string errormessage;
            int newkit = NewKit(newKitBox.Text, out errormessage);
            if (newkit > 0)
            {
                Response.Redirect("Kits2.aspx?KitID=" + newkit);
                //DropDownList1.SelectedValue = newkit.ToString();
            }
            else
            {
                warningDiv.Attributes["class"] = "row top20";
                warningLabel.Text = errormessage;
            }
        }

        protected int NewKit(string kitPH, out string error)
        {
            error = "";
            try
            {
                using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
                {
                    SQLiteCommand command = m_dbConnection.CreateCommand();
                    command.CommandText = "INSERT INTO Kits (KitPH) VALUES (@KitPH); SELECT last_insert_rowid();";
                    command.Parameters.Add(new SQLiteParameter("@KitPH", kitPH));
                    m_dbConnection.Open();
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
            catch (SQLiteException ex) { error = ex.Message; return 0;}
        }
    }
}