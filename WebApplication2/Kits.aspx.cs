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
    public partial class Kits : System.Web.UI.Page
    {
        string databaseLocation = "C:\\datatest\\2016repairhistory.sqlite";
        protected void Page_Load(object sender, EventArgs e)
        {
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

        protected DataTable GetAssociatedIDs(string kitID)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                string sql = String.Format("SELECT * FROM Kits WHERE KitID = {0}", kitID);
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

        protected DataTable GetKits()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                string sql = String.Format("SELECT KitPH, KitID FROM Kits");
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

        protected DataTable GetPhotog(string photogID)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                string sql = String.Format("SELECT * FROM Photographers WHERE ID = {0}", photogID);
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

        protected DataTable GetCamera(string cameraID)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                string sql = String.Format("SELECT * FROM Cameras WHERE CameraID = {0}", cameraID);
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

        protected DataTable GetLaptop(string laptopID)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                string sql = String.Format("SELECT * FROM Laptops WHERE LaptopID = {0}", laptopID);
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
                string sql = String.Format("SELECT * FROM Repairs WHERE KitID = {0}", kitID);
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

        protected void RefreshDetails()
        {
            DataTable ids = GetAssociatedIDs(DropDownList1.SelectedValue);
            try
            {
                DataTable photog = GetPhotog(ids.Rows[0][2].ToString());
                photogNameText.Text = photog.Rows[0][1].ToString();
                photogInitialText.Text = photog.Rows[0][2].ToString();
                photogOfficeText.Text = photog.Rows[0][4].ToString();
            }
            catch (SQLiteException)
            {
                photogNameText.Text = "";
                photogInitialText.Text = "";
                photogOfficeText.Text = "";
            }

            try
            {
                DataTable camera = GetCamera(ids.Rows[0][4].ToString());
                cameraMake.Text = camera.Rows[0][2].ToString();
                cameraModel.Text = camera.Rows[0][3].ToString();
                cameraSN.Text = camera.Rows[0][1].ToString();
            }
            catch (SQLiteException)
            {
                cameraMake.Text = "";
                cameraModel.Text = "";
                cameraSN.Text = "";
            }

            try
            {
                DataTable laptop = GetLaptop(ids.Rows[0][3].ToString());
                laptopMake.Text = laptop.Rows[0][2].ToString();
                laptopModel.Text = laptop.Rows[0][3].ToString();
                laptopOS.Text = laptop.Rows[0][4].ToString();
                laptopSN.Text = laptop.Rows[0][1].ToString();
            }
            catch (SQLiteException)
            {
                laptopMake.Text = "";
                laptopModel.Text = "";
                laptopOS.Text = "";
                laptopSN.Text = "";
            }

            try
            {
                DataTable history = GetHistory(DropDownList1.SelectedValue);
                historyGrid.DataSource = history;
                historyGrid.DataBind();
                foreach (GridViewRow gr in historyGrid.Rows)
                {
                    HyperLink hp = new HyperLink();
                    hp.Target = "_blank";
                    hp.ToolTip = "Open repair record for RepairID " + gr.Cells[0].Text;
                    hp.Text = gr.Cells[0].Text;
                    hp.NavigateUrl = "~/Repairs.aspx?repairID=" + hp.Text;
                    gr.Cells[0].Controls.Add(hp);
                }
            }
            catch (SQLiteException)
            {
                
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshDetails();
        }

        //For testing
        protected DataSet GetPhotogSet(string photogID)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                string sql = String.Format("SELECT * FROM Photographers WHERE ID = {0}", photogID);
                using (SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection))
                {
                    using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
                    {
                        command.Connection = m_dbConnection;
                        sda.SelectCommand = command;
                        using (DataSet dt = new DataSet())
                        {
                            sda.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }
    }
}