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
    public partial class Reports : System.Web.UI.Page
    {
        SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", "C:\\datatest\\2016repairhistory.sqlite"));
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["rep"] == "repairCost")
            {
                BindData(RepairCost());
            }

        }

        protected DataTable RepairCost()
        {
            using (m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT Photographers.ID, Photographers.Name, Photographers.Initials, Photographers.Active, Photographers.Office, sum(Repairs.RepairCost) AS \"Total Repair Cost\" FROM Photographers LEFT JOIN Repairs ON Photographers.ID = Repairs.PhotogID GROUP BY Photographers.ID ORDER BY \"Total Repair Cost\" DESC";
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

        protected void BindData(DataTable dt)
        {
            resultsGrid.DataSource = dt;
            resultsGrid.DataBind();
        }
    }
}