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

            table {
                page-break-after: always;
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
                        <div class="col-xs-6">
                            <h4>This week's reports</h4>
                            <div class="form-inline">
                                Office: 
                                <asp:DropDownList ID="officeDD" CssClass="form-control" runat="server" OnSelectedIndexChanged="officeDD_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Text="Both" Value="both"></asp:ListItem>
                                    <asp:ListItem Text="Manningtree" Value="MT"></asp:ListItem>
                                    <asp:ListItem Text="Mansfield" Value="MF"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
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
                                <li>
                                    Reports per photographer
                                    <asp:LinkButton runat="server" ID="reportsPerPhotog" OnClick="reportsPerPhotog_Click">(all time)</asp:LinkButton>
                                    <asp:LinkButton runat="server" ID="reportsPerPhotogCustomDate" OnClick="reportsPerPhotogCustomDate_Click">(custom date)</asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                        <div class="col-xs-6">
                            <h4>Custom date range</h4>
                            <div class="form-inline">
                                <div class="form-group">
                                    <label for="customStartDate">Start:</label>
                                    <asp:TextBox ID="customStartDate" runat="server" CssClass="form-control" TextMode="Date" ClientIDMode="Static"></asp:TextBox>
                                </div>
                                <div class="form-group">
                                    <label for="customEndDate">End:</label>
                                    <asp:TextBox ID="customEndDate" runat="server" CssClass="form-control" TextMode="Date" ClientIDMode="Static"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row top10">
                <h3 runat="server" id="printTableTitle"></h3>
                <asp:GridView runat="server" ID="printTable" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="printTable_RowDataBound" data-hide-cols=""></asp:GridView>
            </div>
            <div class="row top10">
                <h3 runat="server" id="summaryTableTitle"></h3>
                <asp:GridView runat="server" ID="summaryTable" GridLines="Both" CssClass="table table-striped table-hover" OnRowDataBound="summaryTable_RowDataBound" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Initials" HeaderText="Initials" />
                        <asp:BoundField DataField="Complaints" HeaderText="Complaints" />
                        <asp:BoundField DataField="Loss" HeaderText="Loss" />
                        <asp:BoundField DataField="Lab Reports" HeaderText="Lab Reports" />
                        <asp:BoundField DataField="Retakes" HeaderText="Retakes" />
                        <asp:BoundField DataField="NRBs" HeaderText="NRBs" />
                    </Columns>
                </asp:GridView>
            </div>
            <div class="row top10">
                <div id="customReportDiv" runat="server"></div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
