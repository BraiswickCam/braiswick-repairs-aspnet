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
    public partial class RepairReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Populate();

        }

        protected bool Populate()
        {
            string equipIDHolder, equipTypeHolder;
            if (Request.QueryString["type"] != null)
            {
                equipTypeHolder = Request.QueryString["type"];
            }
            else return false;

            if (Request.QueryString["id"] != null)
            {
                equipIDHolder = Request.QueryString["id"];
            }
            else return false;

            DataTable dt = GetDetails(equipTypeHolder, equipIDHolder);
            repairHead.InnerText = String.Format("Repair ID: {0}", dt.Rows[0][0]);
            notesText.InnerText = dt.Rows[0][1].ToString();
            dateHead.InnerText = String.Format("Repair Date: {0}", dt.Rows[0][2]);
            kitHead.InnerText = String.Format("Kit: {0}", dt.Rows[0][3]);
            photogPanelHead.InnerText = String.Format("Photographer ID: {0}", dt.Rows[0][4]);
            photogName.InnerText = dt.Rows[0][5].ToString();
            photogInitials.InnerText = dt.Rows[0][6].ToString();
            photogOffice.InnerText = dt.Rows[0][7].ToString();
            equipPanelHead.InnerText = String.Format("{0}: {1}", equipTypeHolder == "laptop" ? "LaptopID" : "CameraID", dt.Rows[0][8]);
            equipSN.InnerText = dt.Rows[0][9].ToString();
            equipMake.InnerText = dt.Rows[0][10].ToString();
            equipModel.InnerText = dt.Rows[0][11].ToString();
            if (equipTypeHolder == "laptop")
            {
                optionalRow.Visible = true;
                equipOption.InnerText = dt.Rows[0][12].ToString();
            }
            else optionalRow.Visible = false;
            return true;

        }

        protected DataTable GetDetails(string type, string id)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                if (type == "laptop")
                {
                    command.CommandText = "SELECT Repairs.RepairID, Repairs.Notes, Repairs.Date, Kits.KitPH, Repairs.PhotogID, Photographers.Name, Photographers.Initials, Photographers.Office, Repairs.LaptopID, Laptops.SerialNumber, Laptops.Make, Laptops.Model, Laptops.OS FROM Repairs " +
                        "LEFT JOIN Laptops ON Repairs.LaptopID = Laptops.LaptopID LEFT JOIN Photographers ON Repairs.PhotogID = Photographers.ID LEFT JOIN Kits ON Repairs.KitID = Kits.KitID WHERE Repairs.LaptopID = @ID ORDER BY Repairs.RepairID DESC LIMIT 1";
                }
                else if (type == "camera")
                {
                    command.CommandText = "SELECT Repairs.RepairID, Repairs.Notes, Repairs.Date, Kits.KitPH, Repairs.PhotogID, Photographers.Name, Photographers.Initials, Photographers.Office, Repairs.CameraID, Cameras.SerialNumber, Cameras.Make, Cameras.Model FROM Repairs " +
                        "LEFT JOIN Cameras ON Repairs.CameraID = Cameras.CameraID LEFT JOIN Photographers ON Repairs.PhotogID = Photographers.ID LEFT JOIN Kits ON Repairs.KitID = Kits.KitID WHERE Repairs.CameraID = @ID ORDER BY Repairs.RepairID DESC LIMIT 1";
                }
                command.Parameters.Add(new SQLiteParameter("@ID", id));
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