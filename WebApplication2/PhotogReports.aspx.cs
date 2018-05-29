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
    public partial class MultiPhotog
    {
        public static List<int> multiPhotogs;
        public static List<string> multiPhotogsInitials;
        public static DataTable multiRecords;
    }

    public partial class PhotogReports : System.Web.UI.Page
    {
        protected List<int> multiPhotogs
        {
            get
            {
                if (ViewState["multiPhotogs"] == null)
                {
                    ViewState["multiPhotogs"] = new List<int>();
                }
                return (List<int>)ViewState["multiPhotogs"];
            }
            set
            {
                ViewState["multiPhotogs"] = value;
            }
        }

        protected List<string> multiPhotogsInitials
        {
            get
            {
                if (ViewState["multiPhotogsInitials"] == null)
                {
                    ViewState["multiPhotogsInitials"] = new List<string>();
                }
                return (List<string>)ViewState["multiPhotogsInitials"];
            }
            set
            {
                ViewState["multiPhotogsInitials"] = value;
            }
        }

        protected DataTable multiRecords
        {
            get
            {
                if (ViewState["multiRecords"] == null)
                {
                    ViewState["multiRecords"] = new DataTable();
                }
                return (DataTable)ViewState["multiRecords"];
            }
            set
            {
                ViewState["multiRecords"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            successAlert.Visible = false;
            errorAlert.Visible = false;
            reportUpdate.Visible = false;
            reportUpdateAll.Visible = false;
            if (!IsPostBack)
            {
                FillPhotogsDropDown(Request.QueryString["id"] != null);
                multiPhotogs = new List<int>();
                multiPhotogsInitials = new List<string>();
                if (Request.QueryString["id"] != null)
                {
                    string id = Request.QueryString["id"];
                    DataTable tdt = LoadEntry(id);
                    FillLoadedEntry(tdt);
                    if (tdt.Rows[0][11] != DBNull.Value)
                    {
                        multiRecords = LoadRelated(Convert.ToDateTime(tdt.Rows[0][11]), id);
                        relatedGV.DataSource = multiRecords;
                        relatedGV.DataBind();
                        if (multiRecords.Rows.Count > 0) { reportUpdateAll.Visible = true; AddLinks(); }
                    }
                    reportUpdate.Visible = true;
                    reportSave.Visible = false;
                    addPhotogButton.Visible = false;
                    removeAllPhotogsButton.Visible = false;
                }
            }
        }

        protected DataTable GetPhotogs(bool allPhotogs)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = String.Format("SELECT ID, Initials, Name FROM Photographers WHERE Active IN ({0}) ORDER BY Initials", allPhotogs ? "0,1" : "1");
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

        protected DataTable LoadEntry(string id)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM PReports WHERE ID = @id";
                command.Parameters.Add(new SQLiteParameter("@id", id));
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

        protected DataTable LoadRelated(DateTime related, string id)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT PReports.ID, PReports.Date, PReports.Office, PReports.Job, PReports.School, PReports.Type, PReports.Cost, PReports.Photographer, Photographers.Initials, Photographers.Name, PReports.Status, PReports.Notes FROM PReports " + 
                    "LEFT JOIN Photographers ON PReports.Photographer = Photographers.ID WHERE DateCreated = @DateCreated AND PReports.ID != @id";
                command.Parameters.Add(new SQLiteParameter("@DateCreated", related.ToString("yyyy-MM-dd HH:mm:ss")));
                command.Parameters.Add(new SQLiteParameter("@id", id));
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

        protected void FillLoadedEntry(DataTable dt)
        {
            DataRow dr = dt.Rows[0];
            reportID.Text = dr[0].ToString();
            reportDate.Text = dr[1].ToString() != "" ? Convert.ToDateTime(dr[1]).ToString("yyyy-MM-dd") : "";
            reportJob.Text = dr[3].ToString();
            reportSchool.Text = dr[4].ToString();
            reportType.Text = dr[5].ToString();
            reportCost.Text = Convert.ToDecimal(dr[6]).ToString();
            reportPhotographerDD.SelectedValue = dr[7].ToString();
            reportStatus.SelectedValue = dr[8].ToString();
            reportNotes.Text = dr[9].ToString();
            reportOfficeDD.SelectedValue = dr[2].ToString() == "MT" ? "MT" : dr[2].ToString() == "MF" ? "MF" : "";
            actionCheck.Checked = Convert.ToBoolean(dr[10]);
        }

        protected void ClearEntry()
        {
            reportID.Text = "";
            reportDate.Text = "";
            reportJob.Text = "";
            reportSchool.Text = "";
            reportType.Text = "";
            reportCost.Text = "";
            reportPhotographerDD.SelectedIndex = 0;
            reportStatus.SelectedIndex = 1;
            reportNotes.Text = "";
            reportOfficeDD.SelectedIndex = 0;
            actionCheck.Checked = false;
            multiPhotogs = new List<int>();
            multiPhotogsInitials = new List<string>();
            multiPhotogList.InnerHtml = "";
        }

        protected bool SaveEntry(out string saveErrorMessage)
        {
            string date;
            decimal cost;
            try
            {
                date = Convert.ToDateTime(reportDate.Text).ToString("yyyy-MM-dd");
                if (reportCost.Text != "")
                {
                    cost = Convert.ToDecimal(reportCost.Text);
                }
                else
                {
                    cost = 0;
                }
            }
            catch (Exception ex)
            {
                saveErrorMessage = "INPUTS: " + ex.Message;
                return false;
            }

            try
            {
                using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
                {
                    int[] photogArray;
                    if (multiPhotogs.Count > 0)
                    {
                        photogArray = multiPhotogs.ToArray();
                    }
                    else
                    {
                        photogArray = new int[] { Convert.ToInt32(reportPhotographerDD.SelectedValue) };
                    }

                    foreach (int i in photogArray)
                    {
                        SQLiteCommand command = m_dbConnection.CreateCommand();
                        command.CommandText = "INSERT INTO PReports (Date, Office, Job, School, Type, Cost, Photographer, Status, Notes, Action, DateCreated, DateEdited) VALUES (@Date, @Office, @Job, @School, @Type, @Cost, @Photographer, @Status, @Notes, @Action, @DateCreated, @DateEdited)";
                        command.Parameters.Add(new SQLiteParameter("@Date", date));
                        command.Parameters.Add(new SQLiteParameter("@Office", reportOfficeDD.SelectedValue));
                        command.Parameters.Add(new SQLiteParameter("@Job", reportJob.Text));
                        command.Parameters.Add(new SQLiteParameter("@School", reportSchool.Text));
                        command.Parameters.Add(new SQLiteParameter("@Type", reportType.Text));
                        command.Parameters.Add(new SQLiteParameter("@Cost", cost));
                        command.Parameters.Add(new SQLiteParameter("@Photographer", i));
                        command.Parameters.Add(new SQLiteParameter("@Status", reportStatus.Text));
                        command.Parameters.Add(new SQLiteParameter("@Notes", reportNotes.Text));
                        command.Parameters.Add(new SQLiteParameter("@Action", actionCheck.Checked ? "1" : "0"));
                        DateTime currentDate = DateTime.Now;
                        command.Parameters.Add(new SQLiteParameter("@DateCreated", currentDate.ToString("yyyy-MM-dd HH:mm:ss")));
                        command.Parameters.Add(new SQLiteParameter("@DateEdited", currentDate.ToString("yyyy-MM-dd HH:mm:ss")));
                        m_dbConnection.Open();
                        command.ExecuteNonQuery();
                        m_dbConnection.Close();
                    }
                }
            }
            catch (SQLiteException ex)
            {
                saveErrorMessage = "SQL: " + ex.Message;
                return false;
            }

            saveErrorMessage = "";
            return true;
        }

        protected bool UpdateEntry(string idIn, int photog, out string saveErrorMessage)
        {
            string date;
            decimal cost;
            int id;
            try
            {
                date = Convert.ToDateTime(reportDate.Text).ToString("yyyy-MM-dd");
                id = Convert.ToInt32(idIn);
                if (reportCost.Text != "")
                {
                    cost = Convert.ToDecimal(reportCost.Text);
                }
                else
                {
                    cost = 0;
                }
            }
            catch (Exception ex)
            {
                saveErrorMessage = "INPUTS: " + ex.Message;
                return false;
            }

            try
            {
                using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
                {
                    SQLiteCommand command = m_dbConnection.CreateCommand();
                    command.CommandText = "UPDATE PReports SET Date = @Date, Office = @Office, Job = @Job, School = @School, Type = @Type, Cost = @Cost, Photographer = @Photographer, Status = @Status, Notes = @Notes, Action = @Action, DateEdited = @DateEdited WHERE ID = @ID";
                    command.Parameters.Add(new SQLiteParameter("@ID", id));
                    command.Parameters.Add(new SQLiteParameter("@Date", date));
                    command.Parameters.Add(new SQLiteParameter("@Office", reportOfficeDD.SelectedValue));
                    command.Parameters.Add(new SQLiteParameter("@Job", reportJob.Text));
                    command.Parameters.Add(new SQLiteParameter("@School", reportSchool.Text));
                    command.Parameters.Add(new SQLiteParameter("@Type", reportType.Text));
                    command.Parameters.Add(new SQLiteParameter("@Cost", cost));
                    command.Parameters.Add(new SQLiteParameter("@Photographer", photog));
                    command.Parameters.Add(new SQLiteParameter("@Status", reportStatus.Text));
                    command.Parameters.Add(new SQLiteParameter("@Notes", reportNotes.Text));
                    command.Parameters.Add(new SQLiteParameter("@Action", actionCheck.Checked ? "1" : "0"));
                    DateTime currentDate = DateTime.Now;
                    command.Parameters.Add(new SQLiteParameter("@DateEdited", currentDate.ToString("yyyy-MM-dd HH:mm:ss")));
                    m_dbConnection.Open();
                    command.ExecuteNonQuery();
                    m_dbConnection.Close();
                }
            }
            catch (SQLiteException ex)
            {
                saveErrorMessage = "SQL: " + ex.Message;
                return false;
            }

            saveErrorMessage = "";
            return true;
        }

        protected bool UpdateAll(out string saveErrorMessage)
        {
            if (!UpdateEntry(reportID.Text, Convert.ToInt32(reportPhotographerDD.SelectedValue), out saveErrorMessage)) return false;
            foreach (DataRow dr in multiRecords.Rows)
            {
                if (!UpdateEntry(dr[0].ToString(), Convert.ToInt32(dr[7]), out saveErrorMessage)) return false;
            }
            return true;
        }

        protected void FillPhotogsDropDown(bool allPhotogs)
        {
            DataTable dt = GetPhotogs(allPhotogs);
            DataTable dt2 = new DataTable();
            dt2.Columns.Add(new DataColumn("Text"));
            dt2.Columns.Add(new DataColumn("Value"));
            dt2.Rows.Add("none", "0");
            foreach (DataRow dr in dt.Rows)
            {
                dt2.Rows.Add(dr[1].ToString() + " | " + dr[2].ToString(), dr[0]);
            }
            reportPhotographerDD.DataSource = dt2;
            reportPhotographerDD.DataTextField = "Text";
            reportPhotographerDD.DataValueField = "Value";
            reportPhotographerDD.DataBind();
        }

        protected void SelectOffice()
        {
            string office;
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Office FROM Photographers WHERE ID = @id";
                command.Parameters.Add(new SQLiteParameter("@id", Convert.ToInt32(reportPhotographerDD.SelectedValue)));
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
                {
                    sda.SelectCommand = command;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        office = dt.Rows[0][0].ToString();
                    }
                }
            }
            if (office == "MA" || office == "KT")
            {
                reportOfficeDD.SelectedValue = "MT";
            }
            else if (office == "MF")
            {
                reportOfficeDD.SelectedValue = "MF";
            }
        }

        protected void reportSave_Click(object sender, EventArgs e)
        {
            string saveErrorMessage;
            if (SaveEntry(out saveErrorMessage))
            {
                successAlert.Visible = true;
                ClearEntry();
            }
            else
            {
                errorAlert.Visible = true;
                errorMessage.InnerText = saveErrorMessage;
            }
        }

        protected void reportPhotographerDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reportPhotographerDD.SelectedValue != "0") SelectOffice();
        }

        protected void reportUpdate_Click(object sender, EventArgs e)
        {
            string saveErrorMessage;
            if (UpdateEntry(reportID.Text, Convert.ToInt32(reportPhotographerDD.SelectedValue), out saveErrorMessage))
            {
                successAlert.Visible = true;
            }
            else
            {
                errorAlert.Visible = true;
                errorMessage.InnerText = saveErrorMessage;
            }
            reportUpdate.Visible = true;
            reportSave.Visible = false;
        }

        protected void addPhotogButton_Click(object sender, EventArgs e)
        {
            int photog = Convert.ToInt32(reportPhotographerDD.SelectedValue);
            if (!multiPhotogs.Contains(photog))
            {
                multiPhotogs.Add(photog);
                multiPhotogsInitials.Add(reportPhotographerDD.SelectedItem.Text.Substring(0, 2));
            }
            multiPhotogList.InnerHtml = "";
            foreach (string s in multiPhotogsInitials)
            {
                multiPhotogList.InnerHtml += String.Format("<span style=\"margin: 3px;\" class=\"btn btn-default\">{0}</span>", s);
            }
        }

        protected void removeAllPhotogsButton_Click(object sender, EventArgs e)
        {
            multiPhotogs = new List<int>();
            multiPhotogsInitials = new List<string>();
            multiPhotogList.InnerHtml = "";
        }

        protected void relatedGV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[9].Visible = false;
        }

        protected void reportUpdateAll_Click(object sender, EventArgs e)
        {
            string saveErrorMessage;
            if (UpdateAll(out saveErrorMessage))
            {
                successAlert.Visible = true;
            }
            else
            {
                errorAlert.Visible = true;
                errorMessage.InnerText = saveErrorMessage;
            }
            reportUpdate.Visible = true;
            reportUpdateAll.Visible = true;
            reportSave.Visible = false;
        }

        protected void AddLinks()
        {
            foreach (GridViewRow gvr in relatedGV.Rows)
            {
                if (gvr.Cells[0].Text != "&nbsp;")
                {
                    string id = gvr.Cells[0].Text;
                    gvr.Cells[0].Text = String.Format("<a href=\"PhotogReports.aspx?id={0}\" class=\"btn btn-primary\">{0}</a>", id);
                }

                if (gvr.Cells[8].Text != "&nbsp;")
                {
                    string photogID = gvr.Cells[7].Text;
                    string photogInitials = gvr.Cells[8].Text;
                    string photogName = gvr.Cells[9].Text;

                    gvr.Cells[8].Text = String.Format("<a href=\"PhotogDetails.aspx?PhotogID={0}\" class=\"btn btn-default\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"ID: {0}</br>{2}\">{1}</a>",
                        photogID,
                        photogInitials,
                        photogName);
                }
            }
        }
    }
}