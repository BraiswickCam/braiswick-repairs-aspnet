<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PhotogDetails.aspx.cs" Inherits="WebApplication2.PhotogDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
    &nbsp;</p>
<p>
    Name:
    <asp:TextBox ID="nameText" runat="server"></asp:TextBox>
</p>
<p>
    Initial:
    <asp:TextBox ID="initialText" runat="server"></asp:TextBox>
</p>
<p>
    Active:
    <asp:CheckBox ID="activeCheck" runat="server" />
</p>
<p>
    Office:
    <asp:DropDownList ID="officeList" runat="server">
        <asp:ListItem>MA</asp:ListItem>
        <asp:ListItem>MF</asp:ListItem>
        <asp:ListItem>KT</asp:ListItem>
    </asp:DropDownList>
</p>
<p>
    <asp:Button ID="saveButton" runat="server" OnClick="saveButton_Click" Text="Save" />
</p>
<p>
    <asp:Label ID="updateLabel" runat="server"></asp:Label>
</p>
</asp:Content>
