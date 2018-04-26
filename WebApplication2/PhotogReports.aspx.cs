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
            if (!IsPostBack)
            {
                FillPhotogsDropDown();
            }
        }

        protected DataTable GetPhotogs()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT ID, Initials, Name FROM Photographers WHERE Active = 1 ORDER BY Initials";
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

        protected bool SaveEntry()
        {
            try
            {
                using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
                {
                    SQLiteCommand command = m_dbConnection.CreateCommand();
                    command.CommandText = "INSERT INTO PReports (Date, Office, Job, School, Type, Cost, Photographer, Status, Notes) VALUES (@Date, @Office, @Job, @School, @Type, @Cost, @Photographer, @Status, @Notes)";
                    command.Parameters.Add(new SQLiteParameter("@Date", Convert.ToDateTime(reportDate.Text).ToString("yyyy-MM-dd")));
                    command.Parameters.Add(new SQLiteParameter("@Office", ""));
                    command.Parameters.Add(new SQLiteParameter("@Job", reportJob.Text));
                    command.Parameters.Add(new SQLiteParameter("@School", reportSchool.Text));
                    command.Parameters.Add(new SQLiteParameter("@Type", reportType.Text));
                    command.Parameters.Add(new SQLiteParameter("@Cost", Convert.ToDecimal(reportCost.Text)));
                    command.Parameters.Add(new SQLiteParameter("@Photographer", reportPhotographerDD.SelectedValue));
                    command.Parameters.Add(new SQLiteParameter("@Status", reportStatus.Text));
                    command.Parameters.Add(new SQLiteParameter("@Notes", reportNotes.Text));
                    m_dbConnection.Open();
                    command.ExecuteNonQuery();
                    m_dbConnection.Close();
                }
            }
            catch (SQLiteException)
            {
                return false;
            }

            return true;
        }

        protected void FillPhotogsDropDown()
        {
            DataTable dt = GetPhotogs();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add(new DataColumn("Text"));
            dt2.Columns.Add(new DataColumn("Value"));
            foreach (DataRow dr in dt.Rows)
            {
                dt2.Rows.Add(dr[1].ToString() + " | " + dr[2].ToString(), dr[0]);
            }
            reportPhotographerDD.DataSource = dt2;
            reportPhotographerDD.DataTextField = "Text";
            reportPhotographerDD.DataValueField = "Value";
            reportPhotographerDD.DataBind();
        }

        protected void reportSave_Click(object sender, EventArgs e)
        {
            SaveEntry();
        }
    }
}