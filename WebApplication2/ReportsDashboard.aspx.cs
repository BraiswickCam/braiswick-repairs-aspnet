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
    public partial class ReportsDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ManningtreeTables();
                MansfieldTables();
            }
        }

        protected void ManningtreeTables()
        {
            DataTable dt = GetActionList("MT");

            System.Web.UI.HtmlControls.HtmlGenericControl[] tabs = 
                new System.Web.UI.HtmlControls.HtmlGenericControl[] { mtReportLI, mtRetakeLI, mtComplaintLT, mtFeedbackLT, mtSiteVisitLT, mtLossLT, mtOtherLT };

            System.Web.UI.HtmlControls.HtmlGenericControl[] tables = 
                new System.Web.UI.HtmlControls.HtmlGenericControl[] { mtReport, mtRetake, mtComplaint, mtFeedback, mtSiteVisit, mtLoss, mtOther };

            int totalAction = 0;
            int[] add = new int[] { FilterTable(dt, mtReportGV, mtReportBadge, "Status = 'REPORT'"),
                FilterTable(dt, mtRetakeGV, mtRetakeBadge, "Status = 'RETAKE'"),
                FilterTable(dt, mtComplaintGV, mtComplaintBadge, "Status = 'COMPLAINT'"),
                FilterTable(dt, mtFeedbackGV, mtFeedbackBadge, "Status = 'FEEDBACK'"),
                FilterTable(dt, mtSiteVisitGV, mtSiteVisitBadge, "Status = 'SITE VISIT'"),
                FilterTable(dt, mtLossGV, mtLossBadge, "Status = 'LOSS'"),
                FilterTable(dt, mtOtherGV, mtOtherBadge, "Status NOT IN ('COMPLAINT', 'FEEDBACK', 'LOSS', 'REPORT', 'RETAKE', 'SITE VISIT')")
            };

            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i].Visible = add[i] > 0 ? true : false;
                if (tabs[i].Visible && totalAction == 0)
                {
                    tabs[i].Attributes["class"] = "active";
                    tables[i].Attributes["class"] = "tab-pane fade in active";
                }
                totalAction += add[i]; 
            }

            actionRequiredCountBadge.InnerText = totalAction.ToString();
        }

        protected void MansfieldTables()
        {
            DataTable dt = GetActionList("MF");

            System.Web.UI.HtmlControls.HtmlGenericControl[] tabs =
                new System.Web.UI.HtmlControls.HtmlGenericControl[] { mfReportLI, mfRetakeLI, mfComplaintLI, mfFeedbackLI, mfSiteVisitLI, mfLossLI, mfOtherLI };

            System.Web.UI.HtmlControls.HtmlGenericControl[] tables =
                new System.Web.UI.HtmlControls.HtmlGenericControl[] { mfReport, mfRetake, mfComplaint, mfFeedback, mfSiteVisit, mfLoss, mfOther };

            int totalAction = 0;
            int[] add = new int[] { FilterTable(dt, mfReportGV, mfReportBadge, "Status = 'REPORT'"),
                FilterTable(dt, mfRetakeGV, mfRetakeBadge, "Status = 'RETAKE'"),
                FilterTable(dt, mfComplaintGV, mfComplaintBadge, "Status = 'COMPLAINT'"),
                FilterTable(dt, mfFeedbackGV, mfFeedbackBadge, "Status = 'FEEDBACK'"),
                FilterTable(dt, mfSiteVisitGV, mfSiteVisitBadge, "Status = 'SITE VISIT'"),
                FilterTable(dt, mfLossGV, mfLossBadge, "Status = 'LOSS'"),
                FilterTable(dt, mfOtherGV, mfOtherBadge, "Status NOT IN ('COMPLAINT', 'FEEDBACK', 'LOSS', 'REPORT', 'RETAKE', 'SITE VISIT')")
            };

            for (int i = 0; i < tabs.Length; i++)
            {
                tabs[i].Visible = add[i] > 0 ? true : false;
                if (tabs[i].Visible && totalAction == 0)
                {
                    tabs[i].Attributes["class"] = "active";
                    tables[i].Attributes["class"] = "tab-pane fade in active";
                }
                totalAction += add[i];
            }

            badgeMF.InnerText = totalAction.ToString();
        }

        protected int FilterTable(DataTable dt, GridView gv, System.Web.UI.HtmlControls.HtmlGenericControl badge, string filter)
        {
            DataView dv = new DataView(dt);
            dv.RowFilter = filter;
            gv.DataSource = dv;
            gv.DataBind();
            AddLinks(gv, badge);
            return gv.Rows.Count;
        }

        protected DataTable GetActionList(string office)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(String.Format("Data Source={0};Version=3;datetimeformat=CurrentCulture;", GlobalVars.dbLocation)))
            {
                SQLiteCommand command = m_dbConnection.CreateCommand();
                command.CommandText = "SELECT PReports.ID, PReports.Date, PReports.Office, PReports.Job, PReports.School, PReports.Type, PReports.Cost, PReports.Photographer, Photographers.Initials, Photographers.Name, PReports.Status, PReports.Notes FROM PReports LEFT JOIN Photographers ON PReports.Photographer = Photographers.ID WHERE PReports.Action = 1 AND PReports.Office = @Office";
                command.Parameters.Add(new SQLiteParameter("@Office", office));
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

        protected void AddLinks(GridView gv, System.Web.UI.HtmlControls.HtmlGenericControl badge)
        {
            foreach (GridViewRow gvr in gv.Rows)
            {
                if (gvr.Cells[0].Text != "&nbsp;")
                {
                    string id = gvr.Cells[0].Text;
                    gvr.Cells[0].Text = String.Format("<a href=\"PhotogReports.aspx?id={0}\" class=\"btn btn-primary\"><span class=\"glyphicon glyphicon-edit\"></span> {0}</a>", id);
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

            if (gv.Rows.Count > 0) badge.InnerText = gv.Rows.Count.ToString();
        }

        protected void actionTable_RowDataBound(object sender, GridViewRowEventArgs e)
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