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
                }
            }
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
    }
}