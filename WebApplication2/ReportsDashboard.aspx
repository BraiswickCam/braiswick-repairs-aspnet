<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportsDashboard.aspx.cs" Inherits="WebApplication2.ReportsDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headextra" runat="server">
    <style>
        * {
            font-family: Ubuntu, sans-serif;
        }

        .top-10 {
            margin-top: 10px;
        }

        div.panel-body.main-body {
            text-align: center;
        }

        div.link-container {
            display: inline-block;
            margin: 5px 40px;
        }

        div.link-container > a {
            /*background-color: #5cb85c;
            border: 2px solid #4cae4c;*/
            border-radius: 50%;
            color: white;
            padding: 20px 22px;
            /*height: 100px;
            width: 100px;*/
        }

        div.link-container > a > span {
            font-size: 400%;
        }

        div.link-container > p {
            font-size: 20px;
        }

        .panel-title.main-title {
            text-align: center;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row top-10">
        <div class="panel panel-success">
            <div class="panel-heading"><h4 class="panel-title main-title">Reports Dashboard</h4></div>
            <div class="panel-body main-body">
                <div class="link-container"">
                    <a class="btn btn-success" href="PhotogReports.aspx">
                        <span class="glyphicon glyphicon-plus" style="left: 2px;"></span>
                    </a>
                    <p>New Report</p>
                </div>
                <div class="link-container"">
                    <a class="btn btn-success" href="PhotogReportsList.aspx">
                        <span class="glyphicon glyphicon-list"></span>
                    </a>
                    <p>List Reports</p>
                </div>
            </div>
        </div>
    </div>
    <div class="row top-10">
        <div class="panel panel-success">
            <div class="panel-heading"><h4 class="panel-title"><span class="glyphicon glyphicon-exclamation-sign"></span> Manningtree - Action Required <span class="badge" id="actionRequiredCountBadge" runat="server"></span></h4></div>
            <div class="panel-body">
                <ul class="nav nav-tabs">
                  <li class="active"><a data-toggle="tab" href="#mtComplaint">Complaint <span class="badge" id="mtComplaintBadge" runat="server"></span></a></li>
                  <li><a data-toggle="tab" href="#mtFeedback">Feedback <span class="badge" id="mtFeedbackBadge" runat="server"></span></a></li>
                  <li><a data-toggle="tab" href="#mtLoss">Loss <span class="badge" id="mtLossBadge" runat="server"></span></a></li>
                    <li><a data-toggle="tab" href="#mtReport">Report <span class="badge" id="mtReportBadge" runat="server"></span></a></li>
                    <li><a data-toggle="tab" href="#mtRetake">Retake <span class="badge" id="mtRetakeBadge" runat="server"></span></a></li>
                    <li><a data-toggle="tab" href="#mtSiteVisit">Site Visit <span class="badge" id="mtSiteVisitBadge" runat="server"></span></a></li>
                </ul>
                <div class="tab-content">
                  <div id="mtComplaint" class="tab-pane fade in active">
                    <asp:GridView ID="mtComplaintGV" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="actionTable_RowDataBound"></asp:GridView>
                  </div>
                  <div id="mtFeedback" class="tab-pane fade in">
                    <asp:GridView ID="mtFeedbackGV" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="actionTable_RowDataBound"></asp:GridView>
                  </div>
                  <div id="mtLoss" class="tab-pane fade in">
                    <asp:GridView ID="mtLossGV" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="actionTable_RowDataBound"></asp:GridView>
                  </div>
                  <div id="mtReport" class="tab-pane fade in">
                    <asp:GridView ID="mtReportGV" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="actionTable_RowDataBound"></asp:GridView>
                  </div>
                  <div id="mtRetake" class="tab-pane fade in">
                    <asp:GridView ID="mtRetakeGV" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="actionTable_RowDataBound"></asp:GridView>
                  </div>
                  <div id="mtSiteVisit" class="tab-pane fade in">
                    <asp:GridView ID="mtSiteVisitGV" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="actionTable_RowDataBound"></asp:GridView>
                  </div>
                </div>
            </div>
        </div>
    </div>
    <div class="row top-10">
        <div class="panel panel-success">
            <div class="panel-heading"><h4 class="panel-title"><span class="glyphicon glyphicon-exclamation-sign"></span> Mansfield - Action Required <span class="badge" id="badgeMF" runat="server"></span></h4></div>
            <div class="panel-body">
                <ul class="nav nav-tabs">
                  <li class="active"><a data-toggle="tab" href="#mfComplaint">Complaint <span class="badge" id="mfComplaintBadge" runat="server"></span></a></li>
                  <li><a data-toggle="tab" href="#mfFeedback">Feedback <span class="badge" id="mfFeedbackBadge" runat="server"></span></a></li>
                  <li><a data-toggle="tab" href="#mfLoss">Loss <span class="badge" id="mfLossBadge" runat="server"></span></a></li>
                    <li><a data-toggle="tab" href="#mfReport">Report <span class="badge" id="mfReportBadge" runat="server"></span></a></li>
                    <li><a data-toggle="tab" href="#mfRetake">Retake <span class="badge" id="mfRetakeBadge" runat="server"></span></a></li>
                    <li><a data-toggle="tab" href="#mfSiteVisit">Site Visit <span class="badge" id="mfSiteVisitBadge" runat="server"></span></a></li>
                </ul>
                <div class="tab-content">
                  <div id="mfComplaint" class="tab-pane fade in active">
                    <asp:GridView ID="mfComplaintGV" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="actionTable_RowDataBound"></asp:GridView>
                  </div>
                  <div id="mfFeedback" class="tab-pane fade in">
                    <asp:GridView ID="mfFeedbackGV" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="actionTable_RowDataBound"></asp:GridView>
                  </div>
                  <div id="mfLoss" class="tab-pane fade in">
                    <asp:GridView ID="mfLossGV" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="actionTable_RowDataBound"></asp:GridView>
                  </div>
                  <div id="mfReport" class="tab-pane fade in">
                    <asp:GridView ID="mfReportGV" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="actionTable_RowDataBound"></asp:GridView>
                  </div>
                  <div id="mfRetake" class="tab-pane fade in">
                    <asp:GridView ID="mfRetakeGV" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="actionTable_RowDataBound"></asp:GridView>
                  </div>
                  <div id="mfSiteVisit" class="tab-pane fade in">
                    <asp:GridView ID="mfSiteVisitGV" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="actionTable_RowDataBound"></asp:GridView>
                  </div>
                </div>
            </div>
        </div>
    </div>

    

    <script>
        $(document).ready(function(){
            $('[data-toggle="tooltip"]').tooltip();   
        });
    </script>
</asp:Content>
