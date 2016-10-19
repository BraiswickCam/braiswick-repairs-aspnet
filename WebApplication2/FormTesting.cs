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
    public class FormTesting
    {
        string databaseLocation = "C:\\datatest\\2016repairhistory.sqlite";
        public DataSet GetPhotogSet2(string photogID)
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

        //For testing
        public DataSet GetPhotogSet()
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", databaseLocation));
            using (m_dbConnection)
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT * FROM Photographers";
                using (SQLiteDataAdapter sda = new SQLiteDataAdapter())
                {
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