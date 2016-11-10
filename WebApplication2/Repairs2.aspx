<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Repairs2.aspx.cs" Inherits="WebApplication2.Repairs2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        * {
            font-family: Ubuntu, sans-serif;
        }

        .mainContainer {
            width: auto;
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

        .tableHeaderCell {
            padding-right: 10px;
            width: 100%;
            border-bottom: 2px solid black;
            font-size: 20px;
        }

        .tableIDs td {
            padding: 2px 4px;
        }

        .tableMainAlt {
            display: inline-block;
            width: auto;
            min-width: 62%;
            margin: 20px;
            padding: 0px 10px 2px 10px;
            text-align: left;
            background: -webkit-linear-gradient(left top, #FFF, #f7f7f7);
            border: 1px solid Black;
            border-radius: 5px;
        }

        .tableMainAlt tr:nth-child(2) td {
            padding-top: 10px;
        }

        .tableMainAlt tr:last-child td {
            padding-bottom: 10px;
        }

        .notesBox {
            width: 100%;
            min-width: 600px;
            min-height: 200px;
        }

        .controls {
            width: auto;
            text-align: center;
        }

        .controls input {
            font-family: Ubuntu, sans-serif;
            font-size: 16px;
            text-transform: uppercase;
            padding: 5px 10px;
            background-color: cornflowerblue;
            color: white;
            border: none;
        }

        .controls input:hover {
            background-color: blue;
        }

        .top10 {
            margin-top: 10px;
        }

        .top20 {
            margin-top: 20px;
        }
    </style>
    <div class="alert alert-success hidden" id="mainAlert" runat="server">
        <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
        <p id="mainAlertText" runat="server"><strong>Success!</strong> Repair entry updated </p>
    </div>
    <div class="container-fluid">
        <div class="row top20">
            <div class="col-xs-6 col-sm-4 col-sm-offset-2">
                <div class="panel panel-default">
                    <div class="panel-heading">IDs:</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-5">Repair ID: </div>
                            <div class="col-xs-7">
                                <asp:Label ID="repairIDLabel" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row top10">
                            <div class="col-xs-5">Camera ID: </div>
                            <div class="col-xs-7">
                                <asp:TextBox ID="cameraIDBox" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row top10">
                            <div class="col-xs-5">Laptop ID:</div>
                            <div class="col-xs-7">
                                <asp:TextBox ID="laptopIDBox" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row top10">
                            <div class="col-xs-5">Kit ID: </div>
                            <div class="col-xs-7">
                                <asp:TextBox ID="kitIDBox" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row top10">
                            <div class="col-xs-5">Photographer ID: </div>
                            <div class="col-xs-7">
                                <asp:TextBox ID="photogIDBox" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-6 col-sm-4">
                <div class="panel panel-default">
                    <div class="panel-heading">Repair Details:</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-5">Date: </div>
                            <div class="col-xs-7">
                                <asp:TextBox ID="dateBox" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row top10">
                            <div class="col-xs-5">Fixed?: </div>
                            <div class="col-xs-7">
                                <asp:CheckBox ID="fixedCheck" runat="server" />
                            </div>
                        </div>
                        <div class="row top10">
                            <div class="col-xs-5">Fixed Date: </div>
                            <div class="col-xs-7">
                                <asp:TextBox ID="fixedDateBox" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row top10">
                            <div class="col-xs-5">Tech Initials: </div>
                            <div class="col-xs-7">
                                <asp:TextBox ID="techInitialsBox" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row top10">
                            <div class="col-xs-5">Repair Cost: </div>
                            <div class="col-xs-7">
                                <asp:TextBox ID="repairCostBox" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-8 col-sm-offset-2">
                <div class="panel panel-default">
                    <div class="panel-heading">Notes:</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:TextBox ID="notesText" TextMode="MultiLine" Rows="8" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 text-center">
                <asp:Button runat="server" ID="saveButton" Text="Save" OnClick="saveButton_Click" CssClass="btn btn-primary" />
            </div>
        </div>
    </div>

    <%--<div class="mainContainer">
    <div class="tableMain">
        <asp:Table ID="IDsTable" runat="server" CssClass="tableIDs">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell ColumnSpan="2" CssClass="tableHeaderCell">IDs:</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell>Repair ID:</asp:TableCell>
                <asp:TableCell><asp:Label ID="repairIDLabel" runat="server"></asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Camera ID:</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="cameraIDBox" runat="server"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Laptop ID:</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="laptopIDBox" runat="server"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Kit ID:</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="kitIDBox" runat="server"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Photographer ID:</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="photogIDBox" runat="server"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        </div>
        <div class="tableMain">
        <asp:Table ID="DetailsTable" runat="server" CssClass="tableIDs">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell ColumnSpan="2" CssClass="tableHeaderCell">Repair Details:</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell>Date:</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="dateBox" runat="server" TextMode="DateTime"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Fixed?:</asp:TableCell>
                <asp:TableCell><asp:CheckBox ID="fixedCheck" runat="server" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Fixed Date:</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="fixedDateBox" runat="server"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Tech Initials:</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="techInitialsBox" runat="server"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Repair Cost:</asp:TableCell>
                <asp:TableCell><asp:TextBox ID="repairCostBox" runat="server"></asp:TextBox></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    </div>
    <div class="mainContainer">
        <div class="tableMainAlt">
            <asp:Table ID="notesTable" runat="server" CssClass="tableIDs">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell CssClass="tableHeaderCell">Notes:</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableRow>
                    <asp:TableCell>
                        <asp:TextBox ID="notesText" TextMode="MultiLine" runat="server" CssClass="notesBox"></asp:TextBox>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </div>
    </div>
    <div class="controls">
        <asp:Button runat="server" ID="saveButton" Text="Save" OnClick="saveButton_Click" />
    </div>--%>
</asp:Content>
