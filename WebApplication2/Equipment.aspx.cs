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
    public partial class Equipment : System.Web.UI.Page
    {
        SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", "C:\\datatest\\2016repairhistory.sqlite"));
        int activeCol = 4, kitIdCol = 5, kitPhCol = 6, photogIdCol = 7, photogInitialCol = 8, otherIdCol = 9;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (equipDrop.SelectedValue == "laptop")
            {
                activeCol = activeCol + 1;
                kitIdCol = kitIdCol + 1;
                kitPhCol = kitPhCol + 1;
                photogIdCol = photogIdCol + 1;
                photogInitialCol = photogInitialCol + 1;
                otherIdCol = otherIdCol + 1;
            }
            equipGrid.DataSource = equipDrop.SelectedValue == "laptop" ? GetLaptopList() : GetCameraList();
            equipGrid.DataBind();
        }

        protected DataTable GetLaptopList()
        {
            using (m_dbConnection)
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Laptops.LaptopID, Laptops.SerialNumber, Laptops.Make, Laptops.Model, Laptops.OS, Laptops.Active, Kits.KitID, Kits.KitPH, Kits.PhotogID, Photographers.Initials, Kits.CameraID FROM Laptops LEFT JOIN Kits ON Laptops.LaptopID = Kits.LaptopID LEFT JOIN Photographers ON Kits.PhotogID = Photographers.ID";
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

        protected DataTable GetCameraList()
        {
            using (m_dbConnection)
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Cameras.CameraID, Cameras.SerialNumber, Cameras.Make, Cameras.Model, Cameras.Active, Kits.KitID, Kits.KitPH, Kits.PhotogID, Photographers.Initials, Kits.LaptopID FROM Cameras LEFT JOIN Kits ON Cameras.CameraID = Kits.CameraID LEFT JOIN Photographers ON Kits.PhotogID = Photographers.ID";
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

        protected void AddLinks()
        {

        }
    }
}