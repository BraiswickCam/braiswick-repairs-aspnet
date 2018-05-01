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
            border: 2px solid #286090;
            border-radius: 10px;
            padding: 3px 6px;
            background-color: #337ab7;
            box-shadow: 1px 1px 5px 0px rgba(0,0,0,0.75);
            margin: 0px 3px;
            color: white;
        }

        div.or[id^="filter_"] {
            background-color: #286090;
            border-color: #204d74;
        }

        span.filter-close {
            border: 2px solid #c9302c;
            border-radius: 50%;
            padding: 0px 4px;
            cursor: pointer;
            background-color: #d9534f;
            color: white;
        }

        span.filter-close:hover {
            background-color: #c9302c;
            border-color: #ac2925;
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
                <button type="button" id="addFilterButton" class="btn btn-primary" onclick="addFilter(); return false;">+</button>
                <button type="button" id="clearAllFilters" class="btn btn-danger" onclick="clearFilters(); return false;">&times;</button>
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
            var tdv = td.getAttribute("data-value");
            var searchTerm;
            var innerCount = 0;

            for (i = 0; i < filter.searchTerm.length; i++) {
                searchTerm = filter.searchTerm[i].toUpperCase();
                if (tdv) {
                    if (tdv.toUpperCase().indexOf(searchTerm) > -1) {
                        innerCount = innerCount + 1;
                    }
                } else {

                }
            }

            if (innerCount === 0) {
                count = count + 1;
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
        var liveSearch = { 'columnIndex': col, 'searchTerm': [filter] };

            var searchNow = JSON.parse(JSON.stringify(searchTerms));
            var searchExists = false;
            for (f = 0; f < searchNow.length; f++) {
                if (searchNow[f].columnIndex == col) {
                    if (filter != "") {
                        searchNow[f].searchTerm.push(filter);
                        searchExists = true;
                    } else {

                    }
                }
            }
            if (!searchExists) {
                searchNow.push(liveSearch);
            }
        

          // Loop through all table rows, and hide those who don't match the search query
          for (i = 1; i < tr.length; i++) {
              var count = 0;
              //for (j = col; j < col + 1; j++) {
              //    td = tr[i].getElementsByTagName("td")[j];
              //    count = innerSearch(j, td, liveSearch, count);
              //}
              for (jj = 0; jj < searchNow.length; jj++) {
                  var column = parseInt(searchNow[jj].columnIndex);
                  td = tr[i].getElementsByTagName("td")[column];
                  count = innerSearch(column, td, searchNow[jj], count);
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
            var exisiting = -1;
            e = document.getElementById('searchCol');
            colName = e.options[e.selectedIndex].text;
            colIndex = parseInt(e.options[e.selectedIndex].value);
            searchTerm = document.getElementById('equipSearch').value;

            for (i = 0; i < searchTerms.length; i++) {
                if (searchTerms[i].columnIndex == colIndex) {
                    exisiting = i;
                }
            }
            if (exisiting > -1) {
                searchTerms[exisiting].searchTerm.push(searchTerm);
            } else {
                searchTerms.push({ 'columnName': colName, 'columnIndex': colIndex, 'searchTerm': [searchTerm] });
            }
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
                var searchTermHTML = "";
                for (ii = 0; ii < searchTerms[i].searchTerm.length; ii++) {
                    if (ii > 0) {
                        searchTermHTML += " or ";
                    }
                    searchTermHTML += "<strong>" + searchTerms[i].searchTerm[ii] + "</strong>";
                }
                var classes = searchTerms[i].searchTerm.length > 1 ? "filter or" : "filter";
                filtersHTML += '<div class="' + classes + '" id="filter_' + i + '"><span class="filter-close" onclick="removeFilter(' + i + ')">&times;</span><span class="filter-text">' + searchTerms[i].columnName + ': ' + searchTermHTML + '</span></div>';
            }
            filtersHTML += '</div>';
            document.getElementById('searchTermsList').innerHTML = filtersHTML;
        }
    </script>
</asp:Content>
