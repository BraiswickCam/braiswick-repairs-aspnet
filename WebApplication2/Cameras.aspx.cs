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
    public partial class Cameras : System.Web.UI.Page
    {
        Camera camera;
        string mainCameraID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["msg"] != null && !IsPostBack)
            {
                if (Request.QueryString["msg"] == "new") messageLabel.Text = "<span class=\"glyphicon glyphicon-ok-sign\"></span> New record added successfully!";
            }
            if (Request.QueryString["CameraID"] != null && Request.QueryString["CameraID"] != "none")
            {
                mainCameraID = Request.QueryString["CameraID"];
                if (!IsPostBack) LoadDetails(mainCameraID);
            }
            else
            {
                mainCameraID = "0";
            }

            if (mainCameraID != "0" && !IsPostBack)
            {
                historyGridView.DataSource = camera.GetCameraHistory();
                historyGridView.DataBind();
            }

            if (mainCameraID != "0")
            {
                foreach (GridViewRow gr in historyGridView.Rows)
                {
                    HyperLink hp = new HyperLink();
                    hp.CssClass = "btn";
                    hp.Target = "_blank";
                    hp.ToolTip = "Open repair record for RepairID " + gr.Cells[0].Text;
                    hp.Text = gr.Cells[0].Text;
                    hp.CssClass = "btn btn-primary";
                    hp.NavigateUrl = "~/Repairs2.aspx?repairID=" + hp.Text;
                    gr.Cells[0].Controls.Add(hp);
                }
                saveButton.Text = "Update";
            }
        }

        protected void LoadDetails(string cameraID)
        {
            camera = new Camera(cameraID);
            idLabel.Text = camera.CameraID;
            makeText.Text = camera.Make;
            modelText.Text = camera.Model;
            snText.Text = camera.SerialNumber;
            activeCheck.Checked = camera.Active;
            mainCameraID = camera.CameraID;
        }

        protected void UpdateDetails(string cameraID)
        {
            camera = new Camera(cameraID);
            camera.SetCameraDetails(makeText.Text, modelText.Text, snText.Text, activeCheck.Checked);
            messageLabel.Text = camera.UpdateCameraDatabase() ? "<span class=\"glyphicon glyphicon-ok-sign\"></span> Record updated successfully!" : "<span class=\"glyphicon glyphicon-remove-sign\"></span> An error occured!";
        }

        protected void NewDetails()
        {
            camera = new Camera();
            camera.SetCameraDetails(makeText.Text, modelText.Text, snText.Text, activeCheck.Checked);
            mainCameraID = camera.NewCameraRecord().ToString();
            messageLabel.Text = mainCameraID != "0" ? "<span class=\"glyphicon glyphicon-ok-sign\"></span> New record added successfully!" : "<span class=\"glyphicon glyphicon-remove-sign\"></span> An error occured!";
            Response.Redirect(String.Format("~/Cameras.aspx?CameraID={0}&msg=new", mainCameraID));
        }

        protected void saveButton_Click(object sender, EventArgs e)
        {
            if (mainCameraID != "0") UpdateDetails(mainCameraID);
            else NewDetails();
        }

    }

    public class Camera
    {
        private string cameraID, serialNumber, make, model;
        private bool active;

        public string CameraID
        {
            get { return cameraID; }
            set { cameraID = value; }
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

        public Camera()
        {

        }

        public Camera(string camID)
        {
            this.CameraID = camID;
            GetCameraDetails();
        }

        private void GetCameraDetails()
        {
            DataTable dt = new DataTable();
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Cameras WHERE CameraID = @ID";
                command.Parameters.Add(new SQLiteParameter("@ID", this.CameraID));
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
                {
                    sda.SelectCommand = command;
                    sda.Fill(dt);
                }
            }
            this.SerialNumber = dt.Rows[0][1].ToString();
            this.Make = dt.Rows[0][2].ToString();
            this.Model = dt.Rows[0][3].ToString();
            this.Active = Convert.ToBoolean(dt.Rows[0][4]);
        }

        public void SetCameraDetails(string newMake, string newModel, string newSN, bool newActive)
        {
            this.Make = newMake;
            this.Model = newModel;
            this.SerialNumber = newSN;
            this.Active = newActive;
        }

        public bool UpdateCameraDatabase()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "UPDATE Cameras SET Make=@Make, Model=@Model, SerialNumber=@SN, Active=@Active WHERE CameraID=@ID";
                command.Parameters.Add(new SQLiteParameter("@Make", this.Make));
                command.Parameters.Add(new SQLiteParameter("@Model", this.Model));
                command.Parameters.Add(new SQLiteParameter("@SN", this.SerialNumber));
                command.Parameters.Add(new SQLiteParameter("@Active", this.Active));
                command.Parameters.Add(new SQLiteParameter("@ID", this.CameraID));
                m_dbConnection.Open();
                return command.ExecuteNonQuery() == 1;
            }
        }

        public int NewCameraRecord()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "INSERT INTO Cameras (Make, Model, SerialNumber, Active) VALUES(@Make, @Model, @SN, @Active); SELECT last_insert_rowid();";
                command.Parameters.Add(new SQLiteParameter("@Make", this.Make));
                command.Parameters.Add(new SQLiteParameter("@Model", this.Model));
                command.Parameters.Add(new SQLiteParameter("@SN", this.SerialNumber));
                command.Parameters.Add(new SQLiteParameter("@Active", this.Active));
                m_dbConnection.Open();
                return Convert.ToInt32(command.ExecuteScalar());
            }
        }

        public DataTable GetCameraHistory()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Repairs WHERE CameraID = @CameraID ORDER BY RepairID DESC";
                command.Parameters.Add(new SQLiteParameter("@CameraID", this.CameraID));
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