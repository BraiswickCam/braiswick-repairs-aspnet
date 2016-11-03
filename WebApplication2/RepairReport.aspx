<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RepairReport.aspx.cs" Inherits="WebApplication2.RepairReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %> - My ASP.NET Application</title>

    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>
    <webopt:bundlereference runat="server" path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="https://fonts.googleapis.com/css?family=Ubuntu" rel="stylesheet"> 
    <style>
        body {
            margin: 20px;
        }

        * {
            font-family: Ubuntu, sans-serif;
        }
    </style>
</head>
<body>
        <div class="container-fluid">
            <div class="row">
                <div class="col-xs-12">
                    <h3 id="repairHead" runat="server">Repair ID: </h3>
                    <h3 id="kitHead" runat="server">Kit: </h3>
                    <h3 id="dateHead" runat="server">Date: </h3>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-6">
                    <div class="panel panel-default">
                        <div class="panel-heading" id="photogPanelHead" runat="server"></div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-5">Name: </div>
                                <div class="col-xs-7" id="photogName" runat="server"></div>
                            </div>
                            <div class="row">
                                <div class="col-xs-5">Initials: </div>
                                <div class="col-xs-7" id="photogInitials" runat="server"></div>
                            </div>
                            <div class="row">
                                <div class="col-xs-5">Office: </div>
                                <div class="col-xs-7" id="photogOffice" runat="server"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-6">
                    <div class="panel panel-default">
                        <div class="panel-heading" id="equipPanelHead" runat="server"></div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xs-5">Serial Number: </div>
                                <div class="col-xs-7" id="equipSN" runat="server"></div>
                            </div>
                            <div class="row">
                                <div class="col-xs-5">Make: </div>
                                <div class="col-xs-7" id="equipMake" runat="server"></div>
                            </div>
                            <div class="row">
                                <div class="col-xs-5">Model: </div>
                                <div class="col-xs-7" id="equipModel" runat="server"></div>
                            </div>
                            <div class="row" id="optionalRow" runat="server">
                                <div class="col-xs-5">OS: </div>
                                <div class="col-xs-7" id="equipOption" runat="server"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">ISSUES: </div>
                        <div class="panel-body" id="notesText" runat="server"></div>
                    </div>
                </div>
            </div>
        </div>
</body>
</html>
