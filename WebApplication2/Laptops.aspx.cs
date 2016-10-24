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
    public partial class Laptops : System.Web.UI.Page
    {
        Laptop laptop;
        string mainLaptopID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["LaptopID"] != null && Request.QueryString["LaptopID"] != "none")
            {
                mainLaptopID = Request.QueryString["LaptopID"];
                if (!IsPostBack) LoadDetails(mainLaptopID);
            }
            else
            {
                mainLaptopID = "0";
            }

            if (mainLaptopID != "0" && !IsPostBack)
            {
                historyGridView.DataSource = laptop.GetLaptopHistory();
                historyGridView.DataBind();
            }

            if (mainLaptopID != "0")
            {
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
        }

        protected void LoadDetails(string lapID)
        {
            laptop = new Laptop(lapID);
            idLabel.Text = laptop.LaptopID;
            makeText.Text = laptop.Make;
            modelText.Text = laptop.Model;
            snText.Text = laptop.SerialNumber;
            osText.Text = laptop.OS;
            activeCheck.Checked = laptop.Active;
            mainLaptopID = laptop.LaptopID;
        }

        protected void UpdateDetails(string lapID)
        {
            laptop = new Laptop(lapID);
            laptop.SetLaptopDetails(makeText.Text, modelText.Text, snText.Text, osText.Text, activeCheck.Checked);
            messageLabel.Text = laptop.UpdateLaptopDatabase() ? "Record updated successfully!" : "An error occured!";
        }

        protected void NewDetails()
        {
            laptop = new Laptop();
            laptop.SetLaptopDetails(makeText.Text, modelText.Text, snText.Text, osText.Text, activeCheck.Checked);
            mainLaptopID = laptop.NewLaptopRecord().ToString();
            messageLabel.Text = mainLaptopID != "0" ? "New record added successfully!" : "An error occured!";
            Response.Redirect("~/Laptops.aspx?LaptopID=" + mainLaptopID);
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            if (mainLaptopID != "0") UpdateDetails(mainLaptopID);
            else NewDetails();
        }

    }

    public class Laptop
    {
        private string databaseLocation = "C:\\datatest\\2016repairhistory.sqlite";
        private string laptopID, serialNumber, make, model, os;
        private bool active;

        public string LaptopID
        {
            get { return laptopID; }
            set { laptopID = value; }
        }

        public string SerialNumber
        {
            get { return serialNumber; }
            set { serialNumber = value; }
        }

        public string Make
        {
            get { return make; }
            set { make = value; }
        }

        public string Model
        {
            get { return model; }
            set { model = value; }
        }

        public bool Active
        {
            get { return active; }
            set { active = Convert.ToBoolean(value); }
        }

        public string OS
        {
            get { return os; }
            set { os = value; }
        }

        public Laptop()
        {

        }

        public Laptop(string lapID)
        {
            this.LaptopID = lapID;
            GetLaptopDetails();
        }

        private void GetLaptopDetails()
        {
            DataTable dt = new DataTable();
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Laptops WHERE LaptopID = @ID";
                command.Parameters.Add(new SQLiteParameter("@ID", this.LaptopID));
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
                {
                    sda.SelectCommand = command;
                    sda.Fill(dt);
                }
            }
            this.SerialNumber = dt.Rows[0][1].ToString();
            this.Make = dt.Rows[0][2].ToString();
            this.Model = dt.Rows[0][3].ToString();
            this.OS = dt.Rows[0][4].ToString();
            this.Active = Convert.ToBoolean(dt.Rows[0][5]);
        }

        public void SetLaptopDetails(string newMake, string newModel, string newSN, string newOS, bool newActive)
        {
            this.Make = newMake;
            this.Model = newModel;
            this.SerialNumber = newSN;
            this.OS = newOS;
            this.Active = newActive;
        }

        public bool UpdateLaptopDatabase()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "UPDATE Laptops SET Make=@Make, Model=@Model, SerialNumber=@SN, OS=@OS, Active=@Active WHERE LaptopID=@ID";
                command.Parameters.Add(new SQLiteParameter("@Make", this.Make));
                command.Parameters.Add(new SQLiteParameter("@Model", this.Model));
                command.Parameters.Add(new SQLiteParameter("@SN", this.SerialNumber));
                command.Parameters.Add(new SQLiteParameter("@OS", this.OS));
                command.Parameters.Add(new SQLiteParameter("@Active", this.Active));
                command.Parameters.Add(new SQLiteParameter("@ID", this.LaptopID));
                m_dbConnection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }

        public int NewLaptopRecord()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "INSERT INTO Laptops (Make, Model, SerialNumber, OS, Active) VALUES(@Make, @Model, @SN, @OS, @Active); SELECT last_insert_rowid();";
                command.Parameters.Add(new SQLiteParameter("@Make", this.Make));
                command.Parameters.Add(new SQLiteParameter("@Model", this.Model));
                command.Parameters.Add(new SQLiteParameter("@SN", this.SerialNumber));
                command.Parameters.Add(new SQLiteParameter("@OS", this.OS));
                command.Parameters.Add(new SQLiteParameter("@Active", this.Active));
                m_dbConnection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public DataTable GetLaptopHistory()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Repairs WHERE LaptopID = @LaptopID";
                command.Parameters.Add(new SQLiteParameter("@LaptopID", this.LaptopID));
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
    }
}