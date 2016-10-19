<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cameras.aspx.cs" Inherits="WebApplication2.Cameras" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>

    </style>

    <div class="mainTable">
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
    </div>
    <div class="controls">
        <asp:Button ID="saveButton" runat="server" Text="Save"/>
    </div>
    <div class="messages">
        <asp:Label ID="messageLabel" runat="server"></asp:Label>
    </div>
</asp:Content>
