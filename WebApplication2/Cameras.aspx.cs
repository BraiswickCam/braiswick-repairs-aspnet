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
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadDetails("23");
        }

        protected void LoadDetails(string cameraID)
        {
            camera = new Camera(cameraID);
            idLabel.Text = camera.CameraID;
            makeText.Text = camera.Make;
            modelText.Text = camera.Model;
            snText.Text = camera.SerialNumber;
            activeCheck.Checked = camera.Active;
        }

    }

    public class Camera
    {
        private string databaseLocation = "C:\\datatest\\2016repairhistory.sqlite";
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
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
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
    }
}