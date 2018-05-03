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
    public partial class PhotogReportsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                reportsList.DataSource = GetFullList();
                reportsList.DataBind();
                AddLinks();
            }
        }

        protected DataTable GetFullList()
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT PReports.ID, PReports.Date, PReports.Office, PReports.Job, PReports.School, PReports.Type, PReports.Cost, PReports.Photographer, Photographers.Initials, Photographers.Name, PReports.Status, PReports.Notes FROM PReports LEFT JOIN Photographers ON PReports.Photographer = Photographers.ID";
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
            foreach (GridViewRow gvr in reportsList.Rows)
            {
                if (gvr.Cells[0].Text != "&nbsp;")
                {
                    string id = gvr.Cells[0].Text;
                    gvr.Cells[0].Text = String.Format("<a href=\"PhotogReports.aspx?id={0}\" class=\"btn btn-primary\">{0}</a>", id);
                }

                if (gvr.Cells[8].Text != "&nbsp;")
                {
                    string photogID = gvr.Cells[7].Text;
                    string photogInitials = gvr.Cells[8].Text;
                    string photogName = gvr.Cells[9].Text;

                    gvr.Cells[8].Text = String.Format("<a href=\"PhotogDetails.aspx?PhotogID={0}\" class=\"btn btn-default\" data-toggle=\"tooltip\" data-placement=\"right\" data-html=\"true\" title=\"ID: {0}</br>{2}\">{1}</a>",
                        photogID,
                        photogInitials,
                        photogName);
                }
            }
        }

        protected void reportsList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[9].Visible = false;
            foreach (TableCell tc in e.Row.Cells)
            {
                tc.Attributes.Add("data-value", tc.Text);
            }
        }
    }
}