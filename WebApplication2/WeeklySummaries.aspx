<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WeeklySummaries.aspx.cs" Inherits="WebApplication2.WeeklySummaries" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headextra" runat="server">
    <style>
        .top10 {
            margin-top: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="row top10">
                <div class="panel panel-success">
                    <div class="panel-heading"></div>
                    <div class="panel-body">
                        <asp:LinkButton runat="server" ID="thisWeekFeedbackButton" OnClick="thisWeekFeedbackButton_Click">This Week Feedback</asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="row top10">
                <asp:GridView runat="server" ID="printTable" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="printTable_RowDataBound"></asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
