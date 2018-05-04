<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportsDashboard.aspx.cs" Inherits="WebApplication2.ReportsDashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headextra" runat="server">
    <style>
        * {
            font-family: Ubuntu, sans-serif;
        }

        .top-10 {
            margin-top: 10px;
        }

        div.panel-body {
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row top-10">
        <div class="panel panel-success">
            <div class="panel-heading"><h4 class="panel-title">Reports Dashboard</h4></div>
            <div class="panel-body">
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
</asp:Content>
