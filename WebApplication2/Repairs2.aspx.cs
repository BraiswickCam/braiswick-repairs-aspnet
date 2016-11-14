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
    public partial class Repairs2 : System.Web.UI.Page
    {
        string repair;
        DataTable details;
        protected void Page_Load(object sender, EventArgs e)
        {
            mainAlert.Attributes["class"] = "alert alert-success hidden";
            repair = Request.QueryString["repairID"];
            details = NewGetRepairDetails(repair);
            if (!IsPostBack)
            {
                if (repair != null) LoadRepair(repair);
            }
            foreach (DataColumn dc in details.Columns)
            {
                dc.AllowDBNull = true;
            }
        }

        protected void LoadRepair(string ID)
        {
            try
            {
                //details = NewGetRepairDetails(ID);
                repairIDLabel.Text = details.Rows[0][0].ToString();
                cameraIDBox.Text = details.Rows[0][1].ToString();
                laptopIDBox.Text = details.Rows[0][2].ToString();
                kitIDBox.Text = details.Rows[0][3].ToString();
                photogIDBox.Text = details.Rows[0][4].ToString();
                dateBox.Text = details.Rows[0][5].ToString();
                fixedCheck.Checked = Convert.ToBoolean(details.Rows[0][6]);
                fixedDateBox.Text = details.Rows[0][7].ToString();
                techInitialsBox.Text = details.Rows[0][8].ToString();
                notesText.Text = details.Rows[0][9].ToString();
                repairCostBox.Text = details.Rows[0][10].ToString();
            }
            catch (SQLiteException)
            {

            }
        }

        protected DataTable GetRepairDetails(string ID)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                string sql = String.Format("SELECT * FROM Repairs WHERE RepairID = {0}", ID);
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

        protected DataTable NewGetRepairDetails(string ID) //Uses SQLiteParameters to prevent SQL injection
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Repairs WHERE RepairID = @ID";
                command.Parameters.Add(new SQLiteParameter("@ID", ID));
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

        protected void saveButton_Click(object sender, EventArgs e)
        {
            object[] detailsOut = new object[] { cameraIDBox.Text, laptopIDBox.Text, kitIDBox.Text, photogIDBox.Text, dateBox.Text, fixedCheck.Checked, fixedDateBox.Text, techInitialsBox.Text, notesText.Text, repairCostBox.Text };
            List<object> detailsOutList = new List<object>();
            foreach (object o in detailsOut)
            {
                detailsOutList.Add(string.IsNullOrWhiteSpace(o.ToString()) ? DBNull.Value : o);
            }
            details.Rows.Add(details.Rows[0][0], detailsOutList[0], detailsOutList[1], detailsOutList[2], detailsOutList[3], detailsOutList[4], detailsOutList[5], detailsOutList[6], detailsOutList[7], detailsOutList[8], detailsOutList[9]);
            SaveDetails(details);
            mainAlert.Attributes["class"] = "alert alert-success";
            mainAlertText.InnerHtml = String.Format("<strong>Success!</strong> Repair entry updated {0}", DateTime.Now.ToString());
        }

        protected void SaveDetails(DataTable dt)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "UPDATE Repairs SET CameraID = @cameraID, LaptopID = @laptopID, KitID = @kitID, PhotogID = @photogID, Date = @date, Fixed = @fixed, FixedDate = @fixedDate, TechInitials = @techInitials, Notes = @notes, RepairCost = @repairCost WHERE RepairID = @repairID";
                command.Parameters.Add(new SQLiteParameter("@repairID", dt.Rows[1][0]));
                command.Parameters.Add(new SQLiteParameter("@cameraID", dt.Rows[1][1]));
                command.Parameters.Add(new SQLiteParameter("@laptopID", dt.Rows[1][2]));
                command.Parameters.Add(new SQLiteParameter("@kitID", dt.Rows[1][3]));
                command.Parameters.Add(new SQLiteParameter("@photogID", dt.Rows[1][4]));
                command.Parameters.Add(new SQLiteParameter("@date", dt.Rows[1][5].ToString())); //.ToString() keeps dd/mm/yyyy date formatting. This could be changed for better compatiblity with SQL but would require changes elsewhere
                command.Parameters.Add(new SQLiteParameter("@fixed", dt.Rows[1][6]));
                command.Parameters.Add(new SQLiteParameter("@fixedDate", dt.Rows[1][7].ToString()));
                command.Parameters.Add(new SQLiteParameter("@techInitials", dt.Rows[1][8]));
                command.Parameters.Add(new SQLiteParameter("@notes", dt.Rows[1][9]));
                command.Parameters.Add(new SQLiteParameter("@repairCost", dt.Rows[1][10]));
                m_dbConnection.Open();
                command.ExecuteNonQuery();
            }
        }

        protected void fixedCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (fixedCheck.Checked) fixedDateBox.Text = DateTime.Now.ToString();
            else fixedDateBox.Text = string.Empty;
        }
    }
}