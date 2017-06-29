<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Photogs.aspx.cs" Inherits="WebApplication2.Photogs" %>
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

        .row {
            margin-top: 15px;
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
          var input, filter, table, tr, td, i, ii, current;
          input = document.getElementById("equipSearch");
          filter = input.value.toUpperCase();
          table = document.getElementById("<%= GridView1.ClientID %>");
          tr = table.getElementsByTagName("tr");

          // Loop through all table rows, and hide those who don't match the search query
          for (i = 1; i < tr.length; i++) {
              var count = 0;
              for (j = 0; j < 3; j++) {
                  if (j == 0) {
                      td = tr[i].getElementsByTagName("td")[0].getElementsByTagName("a")[0];
                      if (td) {
                          if (td.innerText.toUpperCase().indexOf(filter) > -1) {
                              count = count + 1;
                          }
                      }
                  } else {
                      td = tr[i].getElementsByTagName("td")[j];
                      if (td) {
                          if (td.innerHTML.toUpperCase().indexOf(filter) > -1) {
                              count = count + 1;
                          }
                      }
                  }
              }
              if (count > 0) {
                  tr[i].style.display = "";
              } else {
                  tr[i].style.display = "none";
              }
            }
          }
    </script>

    <div class="container-fluid">
        <div class="row">
            <div class="col-xs-12 text-center">
                <asp:CheckBox ID="activeBox" runat="server" AutoPostBack="true" Text=" Show only active photographers" />
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 text-center">
                <a class="btn btn-primary" href="PhotogDetails.aspx?PhotogID=0">Add New Photographer</a>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 form-inline">
                <div class="inner-addon left-addon">
                    <i class="glyphicon glyphicon-search"></i>
                    <input type="text" id="equipSearch" onkeyup="searchEquip()" placeholder="Search for photographers.." class="form-control">
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" AllowSorting="true" OnSorting="GridView1_Sorting" GridLines="None" CssClass="table table-striped table-hover"></asp:GridView>
            </div>
        </div>
    </div>

<%--    <div class="container-fluid">
    <div class="row">
        <div class="col-sm-6 col-sm-offset-3 text-center">
        <asp:CheckBox ID="activeBox" runat="server" AutoPostBack="True" Text="Show only active photographers" />
            </div>
    </div>
        <div class="row">
        <div class="col-sm-6 col-sm-offset-3 text-center">
        <a href="/PhotogDetails.aspx?PhotogID=0" class="link">Add New Photographer</a>
            </div>
    </div>
        <div class="row">
            <div class="col-sm-12">
    <div class="mainTable">
        <asp:GridView ID="GridView1" runat="server" OnRowDataBound="GridView1_RowDataBound" AllowSorting="True" OnSorting="GridView1_Sorting">
        </asp:GridView></div>
    </div>
        </div>
        </div>--%>
</asp:Content>
