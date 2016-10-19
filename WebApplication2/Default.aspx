<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication2._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Repairs Database</h1>
        <p>
            <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" OnTextChanged="DropDownList1_SelectedIndexChanged">
                <asp:ListItem>Cameras</asp:ListItem>
                <asp:ListItem>Laptops</asp:ListItem>
            </asp:DropDownList>
        </p>
        <p>
            <asp:GridView ID="GridView1" runat="server">
            </asp:GridView>
        </p>
    </div>

    </asp:Content>
