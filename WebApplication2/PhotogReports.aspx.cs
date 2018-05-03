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
    public partial class PhotogReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            successAlert.Visible = false;
            errorAlert.Visible = false;
            reportUpdate.Visible = false;
            if (!IsPostBack)
            {
                FillPhotogsDropDown();
                if (Request.QueryString["id"] != null)
                {
                    string id = Request.QueryString["id"];
                    FillLoadedEntry(LoadEntry(id));
                    reportUpdate.Visible = true;
                    reportSave.Visible = false;
                }
            }
        }

        protected DataTable GetPhotogs()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT ID, Initials, Name FROM Photographers ORDER BY Initials";
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
                    SQLiteCommand command = m_dbConnection.CreateCommand();
                    command.CommandText = "INSERT INTO PReports (Date, Office, Job, School, Type, Cost, Photographer, Status, Notes) VALUES (@Date, @Office, @Job, @School, @Type, @Cost, @Photographer, @Status, @Notes)";
                    command.Parameters.Add(new SQLiteParameter("@Date", date));
                    command.Parameters.Add(new SQLiteParameter("@Office", reportOfficeDD.SelectedValue));
                    command.Parameters.Add(new SQLiteParameter("@Job", reportJob.Text));
                    command.Parameters.Add(new SQLiteParameter("@School", reportSchool.Text));
                    command.Parameters.Add(new SQLiteParameter("@Type", reportType.Text));
                    command.Parameters.Add(new SQLiteParameter("@Cost", cost));
                    command.Parameters.Add(new SQLiteParameter("@Photographer", reportPhotographerDD.SelectedValue));
                    command.Parameters.Add(new SQLiteParameter("@Status", reportStatus.Text));
                    command.Parameters.Add(new SQLiteParameter("@Notes", reportNotes.Text));
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

        protected bool UpdateEntry(out string saveErrorMessage)
        {
            string date;
            decimal cost;
            int id;
            try
            {
                date = Convert.ToDateTime(reportDate.Text).ToString("yyyy-MM-dd");
                id = Convert.ToInt32(reportID.Text);
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
                    command.CommandText = "UPDATE PReports SET Date = @Date, Office = @Office, Job = @Job, School = @School, Type = @Type, Cost = @Cost, Photographer = @Photographer, Status = @Status, Notes = @Notes WHERE ID = @ID";
                    command.Parameters.Add(new SQLiteParameter("@ID", id));
                    command.Parameters.Add(new SQLiteParameter("@Date", date));
                    command.Parameters.Add(new SQLiteParameter("@Office", reportOfficeDD.SelectedValue));
                    command.Parameters.Add(new SQLiteParameter("@Job", reportJob.Text));
                    command.Parameters.Add(new SQLiteParameter("@School", reportSchool.Text));
                    command.Parameters.Add(new SQLiteParameter("@Type", reportType.Text));
                    command.Parameters.Add(new SQLiteParameter("@Cost", cost));
                    command.Parameters.Add(new SQLiteParameter("@Photographer", reportPhotographerDD.SelectedValue));
                    command.Parameters.Add(new SQLiteParameter("@Status", reportStatus.Text));
                    command.Parameters.Add(new SQLiteParameter("@Notes", reportNotes.Text));
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

        protected void FillPhotogsDropDown()
        {
            DataTable dt = GetPhotogs();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add(new DataColumn("Text"));
            dt2.Columns.Add(new DataColumn("Value"));
            dt2.Rows.Add("none", "");
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
            if (office == "MA")
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
            }
            else
            {
                errorAlert.Visible = true;
                errorMessage.InnerText = saveErrorMessage;
            }
        }

        protected void reportPhotographerDD_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reportPhotographerDD.SelectedValue != "") SelectOffice();
        }

        protected void reportUpdate_Click(object sender, EventArgs e)
        {
            string saveErrorMessage;
            if (UpdateEntry(out saveErrorMessage))
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
    }
}