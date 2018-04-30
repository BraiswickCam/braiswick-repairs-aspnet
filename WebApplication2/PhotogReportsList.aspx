<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PhotogReportsList.aspx.cs" Inherits="WebApplication2.PhotogReportsList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="headextra" runat="server">
    <style>
        .top10 {
            margin-top: 10px;
        }

        #searchTermsList {
            display: inline-block;
        }

        div[id^="filter_"] {
            display: inline-block;
            border: 2px solid #3D9970;
            border-radius: 10px;
            padding: 3px 6px;
            background-color: #01FF70;
            box-shadow: 1px 1px 5px 0px rgba(0,0,0,0.75);
            margin: 0px 3px;
        }

        span.filter-close {
            border: 2px solid #3D9970;
            border-radius: 50%;
            padding: 0px 4px;
            cursor: pointer;
            background-color: #2ECC40;
            color: white;
        }

        span.filter-text {
            padding-left: 5px;
            padding-right: 3px;
        }

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <div class="col-xs-12 form-inline top10">
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
                <button type="button" id="addFilterButton" class="btn btn-success" onclick="addFilter(); return false;">+</button>
                <div id="searchTermsList"></div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 top10">
            <asp:GridView ID="reportsList" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="reportsList_RowDataBound"></asp:GridView>
        </div>
    </div>

    <script>
        $(document).ready(function(){
            $('[data-toggle="tooltip"]').tooltip();   
        });

        var searchTerms = [];

        function innerSearch(j, td, filter, currentCount) {
            var count = currentCount;
            if (j == 0 || j == 7) {
                tda = td.getElementsByTagName("a")[0];
                if (tda) {
                    if (tda.innerHTML.toUpperCase().indexOf(filter) == -1) {
                        count = count + 1;
                    }
                } else {
                    if (td) {
                        if (td.innerHTML.toUpperCase().indexOf(filter) == -1) {
                            count = count + 1;
                        }
                    }
                }
            } else {
                if (td) {
                    if (td.innerHTML.toUpperCase().indexOf(filter) == -1) {
                        count = count + 1;
                    }
                }
            }
            return count;
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
                  td = tr[i].getElementsByTagName("td")[j];
                  count = innerSearch(j, td, filter, count);
              }
              for (jj = 0; jj < searchTerms.length; jj++) {
                  var column = parseInt(searchTerms[jj].columnIndex);
                  td = tr[i].getElementsByTagName("td")[column];
                  count = innerSearch(column, td, searchTerms[jj].searchTerm.toUpperCase(), count);
              }
              if (count > 0) {
                  tr[i].style.display = "none";
              } else {
                  tr[i].style.display = "";
              }
            }
        }

        function addFilter() {
            var colName, colIndex, searchTerm, e;
            e = document.getElementById('searchCol');
            colName = e.options[e.selectedIndex].text;
            colIndex = e.options[e.selectedIndex].value;
            searchTerm = document.getElementById('equipSearch').value;

            searchTerms.push({ 'columnName': colName, 'columnIndex': colIndex, 'searchTerm': searchTerm })
            writeFilters();
            document.getElementById('equipSearch').value = "";
            searchFilter();
        }

        function clearFilters() {
            searchTerms = [];
            writeFilters();
            searchFilter();
        }

        function removeFilter(index) {
            searchTerms.splice(parseInt(index), 1);
            writeFilters();
            searchFilter();
        }

        function writeFilters() {
            var filtersHTML = '<div class="filters">';
            for (i = 0; i < searchTerms.length; i++) {
                filtersHTML += '<div class="filter" id="filter_' + i + '"><span class="filter-close" onclick="removeFilter(' + i + ')">&times;</span><span class="filter-text">' + searchTerms[i].columnName + ': <strong>' + searchTerms[i].searchTerm + '</strong></span></div>';
            }
            filtersHTML += '</div>';
            document.getElementById('searchTermsList').innerHTML = filtersHTML;
        }
    </script>
</asp:Content>
