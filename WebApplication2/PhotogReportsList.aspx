<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PhotogReportsList.aspx.cs" Inherits="WebApplication2.PhotogReportsList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headextra" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-xs-12 form-inline">
            <div class="inner-addon left-addon">
                <i class="glyphicon glyphicon-search"></i>
                <input type="text" id="equipSearch" onkeyup="searchFilter()" placeholder="Search..." class="form-control">
                <select id="searchCol" class="form-control" onchange="searchFilter()">
                    <option value="0" title="ID">ID</option>
                    <option value="1" title="Date">Date</option>
                    <option value="2" title="Office">Office</option>
                    <option value="3" title="Job">Job</option>
                    <option value="4" title="School">School</option>
                    <option value="5" title="Type">Type</option>
                    <option value="6" title="Cost">Cost</option>
                    <option value="7" title="Photographer">Photographer</option>
                    <option value="8" title="Status">Status</option>
                    <option value="9" title="Notes">Notes</option>
                </select>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <asp:GridView ID="reportsList" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="reportsList_RowDataBound"></asp:GridView>
        </div>
    </div>

    <script>
        function searchEquip() {
          // Declare variables
          var input, filter, table, tr, td, i, ii, current;
          input = document.getElementById('equipSearch');
          filter = input.value.toUpperCase();
          table = document.getElementById("<%= reportsList.ClientID %>");
          tr = table.getElementsByTagName("tr");

          // Loop through all table rows, and hide those who don't match the search query
          for (i = 1; i < tr.length; i++) {
              var count = 0;
              for (j = 0; j < 10; j++) {
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

        function searchFilter() {
          // Declare variables
          var input, filter, table, tr, td, i, ii, current, col, e;
          input = document.getElementById('equipSearch');
          filter = input.value.toUpperCase();
          table = document.getElementById("<%= reportsList.ClientID %>");
          tr = table.getElementsByTagName("tr");
            e = document.getElementById('searchCol');
            col = parseInt(e.options[e.selectedIndex].value);

          // Loop through all table rows, and hide those who don't match the search query
          for (i = 1; i < tr.length; i++) {
              var count = 0;
              for (j = col; j < col + 1; j++) {
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
</asp:Content>
