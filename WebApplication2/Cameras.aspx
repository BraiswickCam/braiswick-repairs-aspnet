<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cameras.aspx.cs" Inherits="WebApplication2.Cameras" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        * {
            font-family: Ubuntu, sans-serif;
        }

        .tableContainer {
            width: 100%;
            text-align: center;
        }

        .tableMain {
            display: inline-block;
            width: auto;
            min-width: 25%;
            margin: 20px;
            padding: 0px 10px 2px 10px;
            text-align: left;
            background: -webkit-linear-gradient(left top, #FFF, #f7f7f7);
            border: 1px solid Black;
            border-radius: 5px;
        }

        .tableMain tr:nth-child(2) td {
            padding-top: 10px;
        }

        .tableMain tr:last-child td {
            padding-bottom: 10px;
        }

        .tableMain td {
            padding: 2px 4px;
        }

        .controls {
            text-align: center;
            width: 100%;
        }

        .link {
            padding: 4px 7px;
            background-color: cornflowerblue;
            color: antiquewhite;
            text-align: left;
        }

        .link:hover {
            color: yellow;
            background-color: blue;
        }

        .messages {
            margin-top: 10px;
        }

        .top20 {
           margin-top: 20px;
        }

        .top10 {
            margin-top: 10px;
        }

        .historyContainer {
                margin-top: 40px;
                text-align: center;
                width: 100%;
                height: auto;
            }

            .history {
                text-align: left;
                margin: 0 auto;
            }

            .history th, .history td {
                padding: 4px;
            }

    </style>

    <div class="container-fluid">
        <div class="row top20">
            <div class="col-xs-8 col-xs-offset-2 col-sm-6 col-sm-offset-3 col-md-4 col-md-offset-4">
                <div class="panel panel-default" id="mainPanel" runat="server">
                    <div class="panel-heading">Camera ID: <asp:Label ID="idLabel" runat="server"></asp:Label></div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-5">Serial Number: </div>
                            <div class="col-xs-7">
                                <asp:TextBox ID="snText" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row top10">
                            <div class="col-xs-5">Make: </div>
                            <div class="col-xs-7">
                                <asp:TextBox ID="makeText" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row top10">
                            <div class="col-xs-5">Model: </div>
                            <div class="col-xs-7">
                                <asp:TextBox ID="modelText" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row top10">
                            <div class="col-xs-5">Active?: </div>
                            <div class="col-xs-7">
                                <asp:CheckBox ID="activeCheck" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer">
                        <div class="row">
                            <div class="col-xs-6 col-xs-offset-3 col-md-6 col-md-offset-3 text-center">
                                <asp:Button ID="saveButton" runat="server" Text="Save" OnClick="saveButton_Click" CssClass="btn btn-primary" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 text-center">
                                <asp:Literal ID="messageLabel" runat="server"></asp:Literal>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-xs-12 table-responsive">
                <asp:GridView ID="historyGridView" runat="server" CssClass="table table-striped table-hover" GridLines="None"></asp:GridView>
            </div>
        </div>
    </div>

    <%--<div class="tableContainer">
    <div class="tableMain">
    <asp:Table ID="CameraDetailsTable" runat="server">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell ColumnSpan="2">Camera ID: <asp:Label ID="idLabel" runat="server"></asp:Label></asp:TableHeaderCell>
        </asp:TableHeaderRow>
        <asp:TableRow>
            <asp:TableCell>Serial Number:</asp:TableCell>
            <asp:TableCell><asp:TextBox ID="snText" runat="server"></asp:TextBox></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Make:</asp:TableCell>
            <asp:TableCell><asp:TextBox ID="makeText" runat="server"></asp:TextBox></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Model:</asp:TableCell>
            <asp:TableCell><asp:TextBox ID="modelText" runat="server"></asp:TextBox></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>Active?:</asp:TableCell>
            <asp:TableCell><asp:CheckBox ID="activeCheck" runat="server" /></asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <div class="controls">
        <asp:Button ID="saveButton" runat="server" Text="Save" OnClick="saveButton_Click" CssClass="link"/>
    </div>
    <div class="messages">
        <asp:Literal ID="messageLabel" runat="server"></asp:Literal>
    </div>
        </div>
        </div>--%>
<%--    <div class="historyContainer">
            <asp:GridView ID="historyGridView" runat="server" CssClass="history">
            </asp:GridView>
        </div>--%>
</asp:Content>
