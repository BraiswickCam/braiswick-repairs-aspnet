<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Laptops.aspx.cs" Inherits="WebApplication2.Laptops" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        * {
            font-family: Ubuntu, sans-serif;
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

        .topBox {
            background: -webkit-linear-gradient(left top, #FFF, #f7f7f7);
            border: 1px solid Black;
            border-radius: 5px;
        }

        .rowMain {
            margin-top: 15px;
        }

        .textInput {
            width: 100%;
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
  <div class="row rowMain">
    <div class="col-lg-4 col-lg-offset-4 col-sm-6 col-sm-offset-3 col-xs-12 topBox">
      <div class="row">
        <div class="col-sm-12">
          <h3>Laptop ID: <asp:Label ID="idLabel" runat="server"></asp:Label></h3>
        </div>
      </div>
      <div class="row">
        <div class="col-sm-5">
          <p>Serial Number: </p>
        </div>
        <div class="col-sm-7">
          <asp:TextBox ID="snText" runat="server" CssClass="textInput"></asp:TextBox>
        </div>
      </div>
      <div class="row">
        <div class="col-sm-5">
          <p>Make: </p>
        </div>
        <div class="col-sm-7">
          <asp:TextBox ID="makeText" runat="server" CssClass="textInput"></asp:TextBox>
        </div>
      </div>
      <div class="row">
        <div class="col-sm-5">
          <p>Model: </p>
        </div>
        <div class="col-sm-7">
          <asp:TextBox ID="modelText" runat="server" CssClass="textInput"></asp:TextBox>
        </div>
      </div>
      <div class="row">
        <div class="col-sm-5">
          <p>Operating System: </p>
        </div>
        <div class="col-sm-7">
          <asp:TextBox ID="osText" runat="server" CssClass="textInput"></asp:TextBox>
        </div>
      </div>
      <div class="row">
        <div class="col-sm-5">
          <p>Active?: </p>
        </div>
        <div class="col-sm-7">
          <asp:CheckBox ID="activeCheck" runat="server" />
        </div>
      </div>
      <div class="row">
        <div class="col-sm-12">
          <asp:Button ID="saveButton" runat="server" Text="Save" OnClick="saveButton_Click" CssClass="link"/>
        </div>
      </div>
      <div class="row">
        <div class="col-sm-12">
          <asp:Literal ID="messageLabel" runat="server"></asp:Literal>
        </div>
      </div>
    </div>
  </div>
    </div>
    <div class="container-fluid">
  <div class="row rowMain">
    <div class="col-sm-12">
      <asp:GridView ID="historyGridView" runat="server" CssClass="history"></asp:GridView>
    </div>
  </div>
</div>
</asp:Content>
