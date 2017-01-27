<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="WebApplication2.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        * {
            font-family: Ubuntu, sans-serif;
        }
    </style>

    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-12">
                <asp:GridView ID="resultsGrid" runat="server" CssClass="table table-striped" GridLines="None"></asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
