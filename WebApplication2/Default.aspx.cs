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
    public partial class _Default : Page
    {
        string databaseLocation = "C:\\datatest\\2016repairhistory.sqlite";
        RepairsSQL repSQL = new RepairsSQL();
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadTable(DropDownList1.SelectedValue.ToString());
        }

        protected void LoadTable(string option)
        {
            DataTable dt = this.GetData(option);
            GridView1.DataSource = dt;
            GridView1.DataBind();    
        }

        protected DataTable GetData(string item)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation)))
            {
                string sql = String.Format("SELECT * FROM {0}", item);
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

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadTable(DropDownList1.SelectedValue.ToString());
        }
    }
}