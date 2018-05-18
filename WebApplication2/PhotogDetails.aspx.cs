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
    public partial class PhotogDetails : System.Web.UI.Page
    {
        SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation));
        string photogID;
        int photogIntID;
        protected void Page_Load(object sender, EventArgs e)
        {
            photogID = Request.QueryString["PhotogID"];
            try { photogIntID = Convert.ToInt32(photogID); }
            catch (System.FormatException) { photogIntID = 0; }
            if (!IsPostBack)
            {
                if (photogIntID > 0)
                {
                    DataTable dt = GetDetails(photogID);
                    nameText.Text = dt.Rows[0][1].ToString();
                    initialText.Text = dt.Rows[0][2].ToString();
                    activeCheck.Checked = (bool)dt.Rows[0][3];
                    officeList.SelectedValue = (string)dt.Rows[0][4];
                    if (Request.QueryString["msg"] != null)
                    {
                        if (Request.QueryString["msg"] == "new")
                        {
                            updateLabel.Text = "Record added successfully. " + DateTime.Now.ToString();
                        }
                    }
                    saveButton.Text = "Update";
                    idHolder.InnerText = photogIntID.ToString();

                    historyGridView.DataSource = GetHistory();
                    historyGridView.DataBind();
                    reportsGridView.DataSource = GetReports();
                    reportsGridView.DataBind();
                }
            }
            if (photogIntID > 0) AddLinks();
        }

        protected DataTable GetDetails(string photogID)
        {
            m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation));
            SQLiteCommand command = m_dbConnection.CreateCommand();
            command.CommandText = "SELECT * FROM Photographers WHERE ID = @ID";
            command.Parameters.Add(new SQLiteParameter("@ID", photogID));
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

        protected bool UpdateDetails(out string errorMessage)
        {
            m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation));
            try
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "UPDATE Photographers SET Name = @name, Initials = @initials, Active = @active, Office = @office WHERE ID = @ID";
                command.Parameters.Add(new SQLiteParameter("@ID", photogID));
                command.Parameters.Add(new SQLiteParameter("@name", nameText.Text));
                command.Parameters.Add(new SQLiteParameter("@initials", initialText.Text));
                command.Parameters.Add(new SQLiteParameter("@active", activeCheck.Checked));
                command.Parameters.Add(new SQLiteParameter("@office", officeList.SelectedValue));
                m_dbConnection.Open();
                command.ExecuteNonQuery();
                m_dbConnection.Close();
                errorMessage = "";
                return true;
            }
            catch (SQLiteException ex)
            {
                errorMessage = ex.Message;
                m_dbConnection.Close();
                return false;
            }
        }

        protected int NewRecord(out string errorMessage)
        {
            m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation));
            try
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "INSERT INTO Photographers (Name, Initials, Active, Office) VALUES (@name, @initials, @active, @office); SELECT last_insert_rowid();";
                command.Parameters.Add(new SQLiteParameter("@name", nameText.Text));
                command.Parameters.Add(new SQLiteParameter("@initials", initialText.Text));
                command.Parameters.Add(new SQLiteParameter("@active", activeCheck.Checked));
                command.Parameters.Add(new SQLiteParameter("@office", officeList.SelectedValue));
                m_dbConnection.Open();
                int insertID = Convert.ToInt32(command.ExecuteScalar());
                m_dbConnection.Close();
                errorMessage = "";
                return insertID;
            }
            catch (SQLiteException ex)
            {
                errorMessage = ex.Message;
                m_dbConnection.Close();
                return 0;
            }
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            string errorMessage;
            if (photogIntID > 0)
            {
                updateLabel.Text = UpdateDetails(out errorMessage) ? "Record updated successfully. " + DateTime.Now.ToString() : "An error occured updating the record! " + DateTime.Now.ToString() + "\n" + errorMessage;
            }
            else
            {
                //updateLabel.Text = NewRecord(out errorMessage) > 0 ? "Record added successfully. " + DateTime.Now.ToString() : "An error occured adding the record! " + DateTime.Now.ToString() + "\n" + errorMessage;
                int insertRow = NewRecord(out errorMessage);
                if (insertRow > 0)
                {
                    Response.Redirect(String.Format("PhotogDetails.aspx?PhotogID={0}&msg=new", insertRow));
                }
            }
        }

        protected DataTable GetHistory()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Repairs.*, Cameras.SerialNumber, Cameras.Make, Cameras.Model, Laptops.SerialNumber, Laptops.Make, Laptops.Model, Kits.KitPH, Photographers.Name, Photographers.Initials, Photographers.Office FROM Repairs " +
                    "LEFT JOIN Cameras ON Repairs.CameraID = Cameras.CameraID " +
                    "LEFT JOIN Laptops ON Repairs.LaptopID = Laptops.LaptopID " +
                    "LEFT JOIN Kits ON Repairs.KitID = Kits.KitID " +
                    "LEFT JOIN Photographers ON Repairs.PhotogID = Photographers.ID " +
                    "WHERE Repairs.PhotogID = @PhotogID ORDER BY RepairID DESC";
                command.Parameters.Add(new SQLiteParameter("@PhotogID", photogIntID));
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

        protected DataTable GetReports()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT ID, Date, Job, School, Type, Cost, Status, Notes FROM PReports WHERE Photographer = @PhotogID ORDER BY ID DESC";
                command.Parameters.Add(new SQLiteParameter("@PhotogID", photogIntID));
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
            foreach (GridViewRow gr in historyGridView.Rows)
            {
                HyperLink hp = new HyperLink();
                hp.CssClass = "btn";
                hp.Target = "_blank";
                hp.ToolTip = "Open repair record for RepairID " + gr.Cells[0].Text;
                hp.Text = gr.Cells[0].Text;
                hp.CssClass = "btn btn-primary";
                hp.NavigateUrl = "~/Repairs2.aspx?repairID=" + hp.Text;
                gr.Cells[0].Controls.Add(hp);

                if (!IsPostBack)
                {
                    int camID = 1, camSerial = 11, camMake = 12, camModel = 13, lapID = 2, lapSerial = 14, lapMake = 15, lapModel = 16,
                        photogID = 4, photogInitials = 19, photogName = 18, photogOffice = 20, kitID = 3, kitPH = 17;

                    if (gr.Cells[camID].Text != "0" && gr.Cells[camID].Text != "&nbsp;")
                    {
                        string idHolder = gr.Cells[camID].Text;
                        gr.Cells[camID].Text = String.Format("<a href=\"Cameras.aspx?CameraID={0}\" class=\"btn btn-default\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}</br>{3}\">{0}</a>",
                            idHolder,
                            gr.Cells[camSerial].Text,
                            gr.Cells[camMake].Text,
                            gr.Cells[camModel].Text);
                    }

                    if (gr.Cells[lapID].Text != "0" && gr.Cells[lapID].Text != "&nbsp;")
                    {
                        string idHolder = gr.Cells[lapID].Text;
                        gr.Cells[lapID].Text = String.Format("<a href=\"Laptops.aspx?LaptopID={0}\" class=\"btn btn-default\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}</br>{3}\">{0}</a>",
                            idHolder,
                            gr.Cells[lapSerial].Text,
                            gr.Cells[lapMake].Text,
                            gr.Cells[lapModel].Text);
                    }

                    if (gr.Cells[photogID].Text != "0" && gr.Cells[photogID].Text != "&nbsp;")
                    {
                        string idHolder = gr.Cells[photogID].Text;
                        gr.Cells[photogID].Text = String.Format("<a href=\"PhotogDetails.aspx?PhotogID={0}\" class=\"btn btn-default\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"{1}</br>{2}</br>{3}\">{0}</a>",
                            idHolder,
                            gr.Cells[photogInitials].Text,
                            gr.Cells[photogName].Text,
                            gr.Cells[photogOffice].Text);
                    }

                    if (gr.Cells[kitID].Text != "0" && gr.Cells[kitPH].Text != "&nbsp;")
                    {
                        string idHolder = gr.Cells[kitID].Text;
                        gr.Cells[kitID].Text = String.Format("<a class=\"btn btn-default\" href=\"Kits2.aspx?KitID={0}\">{1}</a>", idHolder, gr.Cells[kitPH].Text);
                    }
                }

            }

            foreach (GridViewRow gr in reportsGridView.Rows)
                {
                    HyperLink hp = new HyperLink();
                    hp.CssClass = "btn";
                    hp.Target = "_blank";
                    hp.Text = gr.Cells[0].Text;
                    hp.CssClass = "btn btn-primary";
                    hp.NavigateUrl = "~/PhotogReports.aspx?id=" + hp.Text;
                    gr.Cells[0].Controls.Add(hp);
                }
            }

        protected void historyGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            for (int i = 11; i <= 20; i++)
            {
                e.Row.Cells[i].Visible = false;
            }
        }

        protected void reportsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}