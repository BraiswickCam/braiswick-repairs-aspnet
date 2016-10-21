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
            historyGridView.CssClass = "history";
        }

        protected DataTable GetFullKitRecord(string kitID)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Kits.KitID, Kits.KitPH, Photographers.ID, Photographers.Name, Photographers.Initials, Photographers.Office, Cameras.CameraID, Cameras.SerialNumber, Cameras.Make, Cameras.Model, Laptops.LaptopID, " +
                    "Laptops.SerialNumber, Laptops.Make, Laptops.Model, Laptops.OS FROM Kits LEFT JOIN Photographers ON Kits.PhotogID = Photographers.ID LEFT JOIN Cameras ON Kits.CameraID = Cameras.CameraID " +
                    "LEFT JOIN Laptops ON Kits.LaptopID = Laptops.LaptopID WHERE KitID = @KitID";
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
                command.CommandText = "SELECT * FROM Repairs WHERE KitID = @KitID";
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

        protected void RefreshDetails()
        {
            DataTable details = GetFullKitRecord(DropDownList1.SelectedValue);

            //Populate Photographer details
            nameLabel.Text = details.Rows[0][3].ToString();
            initialLabel.Text = details.Rows[0][4].ToString();
            officeLabel.Text = details.Rows[0][5].ToString();

            //Populate Camera details
            camMakeLabel.Text = details.Rows[0][8].ToString();
            camModelLabel.Text = details.Rows[0][9].ToString();
            camSNLabel.Text = details.Rows[0][7].ToString();

            //Populate Laptop details
            lapMakeLabel.Text = details.Rows[0][12].ToString();
            lapModelLabel.Text = details.Rows[0][13].ToString();
            lapSNLabel.Text = details.Rows[0][11].ToString();

            //Populate History records
            try
            {
                DataTable history = GetHistory(DropDownList1.SelectedValue);
                historyGridView.DataSource = history;
                historyGridView.DataBind();
                foreach (GridViewRow gr in historyGridView.Rows)
                {
                    HyperLink hp = new HyperLink();
                    hp.CssClass = "btn";
                    hp.Target = "_blank";
                    hp.ToolTip = "Open repair record for RepairID " + gr.Cells[0].Text;
                    hp.Text = gr.Cells[0].Text;
                    hp.NavigateUrl = "~/Repairs2.aspx?repairID=" + hp.Text;
                    gr.Cells[0].Controls.Add(hp);
                }
            }
            catch (SQLiteException)
            {

            }
        }

        protected void historyGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            
        }
    }
}