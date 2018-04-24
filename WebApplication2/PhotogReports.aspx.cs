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
    public partial class PhotogReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FillPhotogsDropDown();
        }

        protected DataTable GetPhotogs()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT ID, Initials, Name FROM Photographers WHERE Active = 1 ORDER BY Initials";
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

        protected void FillPhotogsDropDown()
        {
            DataTable dt = GetPhotogs();
            DataTable dt2 = new DataTable();
            dt2.Columns.Add(new DataColumn("Text"));
            dt2.Columns.Add(new DataColumn("Value"));
            foreach (DataRow dr in dt.Rows)
            {
                dt2.Rows.Add(dr[1].ToString() + " | " + dr[2].ToString(), dr[0]);
            }
            reportPhotographerDD.DataSource = dt2;
            reportPhotographerDD.DataTextField = "Text";
            reportPhotographerDD.DataValueField = "Value";
            reportPhotographerDD.DataBind();
        }
    }
}