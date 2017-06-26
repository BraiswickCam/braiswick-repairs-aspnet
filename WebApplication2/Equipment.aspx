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

        /* enable absolute positioning */
        .inner-addon { 
            position: relative; 
        }

        /* style icon */
        .inner-addon .glyphicon {
          position: absolute;
          padding: 10px;
          pointer-events: none;
        }

        /* align icon */
        .left-addon .glyphicon  { left:  0px;}
        .right-addon .glyphicon { right: 0px;}

        /* add padding  */
        .left-addon input  { padding-left:  30px; }
        .right-addon input { padding-right: 30px; }

    </style>

    <script>
        function searchEquip() {
          // Declare variables
          var input, filter, table, tr, td, i, ii;
          input = document.getElementById("equipSearch");
          filter = input.value.toUpperCase();
          table = document.getElementById("<%= equipGrid.ClientID %>");
          tr = table.getElementsByTagName("tr");
          ii = document.getElementById("searchDrop").value;

          // Loop through all table rows, and hide those who don't match the search query
          for (i = 0; i < tr.length; i++) {
            td = tr[i].getElementsByTagName("td")[ii];
            if (td) {
              if (td.innerHTML.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
              } else {
                tr[i].style.display = "none";
              }
            }
          }
        }

        function equipCheck() {
            var current = document.getElementById("<%= equipDrop.ClientID %>");
            if (current.value == "camera") {
                document.getElementById("optOS").hidden = true;
            }
            if (current.value == "laptop") {
                document.getElementById("optOS").hidden = false;
            }
        }
    </script>

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
            <div class="col-xs-12 form-inline">
                <div class="inner-addon left-addon">
                    <i class="glyphicon glyphicon-search"></i>
                    <input type="text" id="equipSearch" onkeyup="searchEquip()" placeholder="Search for equipment.." class="form-control">
                    <select name="searchOptions" id="searchDrop" onchange="searchEquip()" class="form-control" style="max-width: 250px;" onclick="equipCheck()">
                        <option value="1">Serial Number</option>
                        <option value="2">Make</option>
                        <option value="3">Model</option>
                        <option value="4" id="optOS">OS</option>
                    </select>
                </div>
            </div>
        </div>
        <div class="row top20">
            <div class="col-xs-12">
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
