<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Equipment.aspx.cs" Inherits="WebApplication2.Equipment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<style>
        * {
            font-family: Ubuntu, sans-serif;
        }

        .controls {
            text-align: center;
            width: auto;
            margin-top: 20px;
            display: inline-block;
        }

        .link {
            padding: 4px 7px;
            background-color: cornflowerblue;
            color: antiquewhite;
        }

        .link:hover {
            color: yellow;
            background-color: blue;
        }

        .mainTable table {
            border-collapse: separate;
            border-spacing: 5px;
            border: none;
            text-align: left;
            margin: 0 auto;
        }

        .mainTable tr:hover {
            background-color: #f5f5f5;
        }

        .mainTable td, .mainTable th {
            padding: 4px;
            border: 0;
            border-bottom: 1px solid #ddd;
        }

        .mainTable td {
            padding-right: 25px;
        }

        .mainTable tr, .mainTable td {
            border-left: 0;
            border-right: 0;
        }

        .kitLink {
        position: relative;
        display: inline-block;
        }

        .kitDrop {
        display: none;
        position: absolute;
        background: -webkit-linear-gradient(left top, #FFF, #f7f7f7);
        min-width: 160px;
        width: auto;
        box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
        padding: 12px 16px;
        z-index: 1;
        border: 0;
        border-radius: 10px;
        }

        .kitLink:hover .kitDrop {
        display: block;
        }

        .top20 {
            margin-top: 20px;
        }

    </style>

    <div class="container-fluid">
        <div class="row top20">
            <div class="col-xs-12 text-center">
                <asp:DropDownList ID="equipDrop" runat="server" AutoPostBack="true">
                    <asp:ListItem Text="Laptops" Value="laptop"></asp:ListItem>
                    <asp:ListItem Text="Cameras" Value="camera"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row top20">
            <div class="col-xs-12 text-center">
                <a id="newEquipLink" class="btn btn-primary" runat="server">Add New <asp:Label ID="equipLabel" runat="server"></asp:Label></a>
            </div>
        </div>
        <div class="row top20">
            <div class="col-xs-12 table-responsive">
                <asp:GridView ID="equipGrid" runat="server" OnRowDataBound="equipGrid_RowDataBound" AllowSorting="true" OnSorting="equipGrid_Sorting" GridLines="None" CssClass="table table-striped table-hover"></asp:GridView>
            </div>
        </div>
    </div>

<%--    <div class="controls">
    
    <!--<p>
        <asp:CheckBox ID="activeBox" runat="server" AutoPostBack="True" Text="Show only active photographers" />
    </p>-->
        <p>
            <asp:DropDownList ID="equipDrop" runat="server" AutoPostBack="true">
                <asp:ListItem Text="Laptops" Value="laptop"></asp:ListItem>
                <asp:ListItem Text="Cameras" Value="camera"></asp:ListItem>
            </asp:DropDownList>
        </p>
    <p>
        <a ID="newEquipLink" class="link" runat="server">Add New <asp:Label ID="equipLabel" runat="server"></asp:Label></a>
    </p>
    <div class="mainTable">
        <asp:GridView ID="equipGrid" runat="server" OnRowDataBound="equipGrid_RowDataBound" AllowSorting="True" OnSorting="equipGrid_Sorting">
        </asp:GridView></div>
    
        </div>--%>
</asp:Content>
