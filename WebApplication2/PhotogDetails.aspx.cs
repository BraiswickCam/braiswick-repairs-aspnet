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
        SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", "C:\\datatest\\2016repairhistory.sqlite"));
        string photogID;
        int photogIntID;
        protected void Page_Load(object sender, EventArgs e)
        {
            photogID = Request.QueryString["PhotogID"];
            photogIntID = Convert.ToInt32(photogID);
            if (!IsPostBack)
            {
                if (photogIntID > 0)
                {
                    DataTable dt = GetDetails(photogID);
                    nameText.Text = dt.Rows[0][1].ToString();
                    initialText.Text = dt.Rows[0][2].ToString();
                    activeCheck.Checked = (bool)dt.Rows[0][3];
                    officeList.SelectedValue = (string)dt.Rows[0][4];
                }
            }
        }

        protected DataTable GetDetails(string photogID)
        {
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

        protected bool NewRecord(out string errorMessage)
        {
            try
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "INSERT INTO Photographers (Name, Initials, Active, Office) VALUES (@name, @initials, @active, @office)";
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

        protected void saveButton_Click(object sender, EventArgs e)
        {
            string errorMessage;
            if (Convert.ToInt32(photogID) > 0)
            {
                updateLabel.Text = UpdateDetails(out errorMessage) ? "Record updated successfully. " + DateTime.Now.ToString() : "An error occured updating the record! " + DateTime.Now.ToString() + "\n" + errorMessage;
            }
            else
            {
                updateLabel.Text = NewRecord(out errorMessage) ? "Record added successfully. " + DateTime.Now.ToString() : "An error occured adding the record! " + DateTime.Now.ToString() + "\n" + errorMessage;
            }
        }
    }
}