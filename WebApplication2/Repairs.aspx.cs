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
    public partial class Repairs : System.Web.UI.Page
    {
        string databaseLocation = "C:\\datatest\\2016repairhistory.sqlite";
        string repair;
        DataTable details;
        protected void Page_Load(object sender, EventArgs e)
        {
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
                repairID.Text = details.Rows[0][0].ToString();
                cameraID.Text = details.Rows[0][1].ToString();
                laptopID.Text = details.Rows[0][2].ToString();
                kitID.Text = details.Rows[0][3].ToString();
                photogID.Text = details.Rows[0][4].ToString();
                dateText.Text = details.Rows[0][5].ToString();
                fixedCheck.Checked = Convert.ToBoolean(details.Rows[0][6]);
                fixedDate.Text = details.Rows[0][7].ToString();
                techInitials.Text = details.Rows[0][8].ToString();
                notesText.Text = details.Rows[0][9].ToString();
                repairCost.Text = details.Rows[0][10].ToString();
            }
            catch (SQLiteException)
            {

            }
        }

        protected DataTable GetRepairDetails(string ID)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
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
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
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
            object[] detailsOut = new object[] { cameraID.Text, laptopID.Text, kitID.Text, photogID.Text, dateText.Text, fixedCheck.Checked, fixedDate.Text, techInitials.Text, notesText.Text, repairCost.Text };
            List<object> detailsOutList = new List<object>();
            foreach (object o in detailsOut)
            {
                detailsOutList.Add(string.IsNullOrWhiteSpace(o.ToString()) ? DBNull.Value : o);
            }
            details.Rows.Add(details.Rows[0][0], detailsOutList[0], detailsOutList[1], detailsOutList[2], detailsOutList[3], detailsOutList[4], detailsOutList[5], detailsOutList[6], detailsOutList[7], detailsOutList[8], detailsOutList[9]);
            SaveDetails(details);
        }

        protected void SaveDetails(DataTable dt)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "UPDATE Repairs SET CameraID = @cameraID, LaptopID = @laptopID, KitID = @kitID, PhotogID = @photogID, Date = @date, Fixed = @fixed, FixedDate = @fixedDate, TechInitials = @techInitials, Notes = @notes, RepairCost = @repairCost WHERE RepairID = @repairID";
                command.Parameters.Add(new SQLiteParameter("@repairID", dt.Rows[1][0]));
                command.Parameters.Add(new SQLiteParameter("@cameraID", dt.Rows[1][1]));
                command.Parameters.Add(new SQLiteParameter("@laptopID", dt.Rows[1][2]));
                command.Parameters.Add(new SQLiteParameter("@kitID", dt.Rows[1][3]));
                command.Parameters.Add(new SQLiteParameter("@photogID", dt.Rows[1][4]));
                command.Parameters.Add(new SQLiteParameter("@date", dt.Rows[1][5]));
                command.Parameters.Add(new SQLiteParameter("@fixed", dt.Rows[1][6]));
                command.Parameters.Add(new SQLiteParameter("@fixedDate", dt.Rows[1][7]));
                command.Parameters.Add(new SQLiteParameter("@techInitials", dt.Rows[1][8]));
                command.Parameters.Add(new SQLiteParameter("@notes", dt.Rows[1][9]));
                command.Parameters.Add(new SQLiteParameter("@repairCost", dt.Rows[1][10]));
                m_dbConnection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}