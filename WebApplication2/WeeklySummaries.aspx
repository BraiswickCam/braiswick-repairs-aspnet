<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WeeklySummaries.aspx.cs" Inherits="WebApplication2.WeeklySummaries" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headextra" runat="server">
    <style>
        .top10 {
            margin-top: 10px;
        }

        @media print {
            footer {
                display: none;
            }

            div.panel {
                display: none;
            }

            div.navbar {
                display: none;
            }

            div.aspNetHidden {
                display: none;
            }

            body {
                padding: 0px;
            }

            .top10 {
                margin-top: 0px;
            }

            h3 {
                margin-top: 0px;
            }

            td[data-min-width="20"] {
                min-width: 30vw;
            }

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
                        <h4>This week's reports</h4>
                        <ul>
                            <li>
                                Feedback by take date 
                                <asp:LinkButton runat="server" ID="thisWeekFeedbackButton" OnClick="thisWeekFeedbackButton_Click">(this week)</asp:LinkButton> 
                                <asp:LinkButton runat="server" ID="lastWeekFeedbackButton" OnClick="lastWeekFeedbackButton_Click">(last week)</asp:LinkButton>
                            </li>
                            <li>
                                Feedback by last updated date 
                                <asp:LinkButton runat="server" ID="thisWeekFeedbackByEditedButton" OnClick="thisWeekFeedbackByEditedButton_Click">(this week)</asp:LinkButton> 
                                <asp:LinkButton runat="server" ID="lastWeekFeedbackByEditedButton" OnClick="lastWeekFeedbackByEditedButton_Click">(last week)</asp:LinkButton>
                            </li>
                            <li>
                                Lab Reports by take date 
                                <asp:LinkButton runat="server" ID="thisWeekReportByTakeButton" OnClick="thisWeekReportByTakeButton_Click">(this week)</asp:LinkButton> 
                                <asp:LinkButton runat="server" ID="lastWeekReportByTakeButton" OnClick="lastWeekReportByTakeButton_Click">(last week)</asp:LinkButton>
                            </li>
                            <li>
                                Lab Reports by last updated date 
                                <asp:LinkButton runat="server" ID="thisWeekReportButton" OnClick="thisWeekReportButton_Click">(this week)</asp:LinkButton> 
                                <asp:LinkButton runat="server" ID="lastWeekReportButton" OnClick="lastWeekReportButton_Click">(last week)</asp:LinkButton>
                             </li>
                        </ul>
                        
                    </div>
                </div>
            </div>
            <div class="row top10">
                <h3 runat="server" id="printTableTitle"></h3>
                <asp:GridView runat="server" ID="printTable" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="printTable_RowDataBound" data-hide-cols=""></asp:GridView>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
