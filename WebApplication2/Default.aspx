<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication2._Default" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <script>
$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip();
});

</script>

    <style>
        * {
            font-family: Ubuntu, sans-serif;
        }

        .panel-heading {
            cursor: pointer;
        }

        .panel-title:after {
            /* symbol for "opening" panels */
            font-family: 'Glyphicons Halflings';  /* essential for enabling glyphicon */
            content: "\e113";    /* adjust as needed, taken from bootstrap.css */
            float: right;        /* adjust as needed */
            color: white;         /* adjust as needed */
        }

        .panel-title.collapsed:after {
            /* symbol for "collapsed" panels */
            content: "\e114";    /* adjust as needed, taken from bootstrap.css */
        }
    </style>

    <div class="container-fluid">
        <div class="row">
            <div class="col-xs-12">
                <h2>Repairs Database Dashboard</h2>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-md-12">
                <div class="panel panel-primary">
                    <div class="panel-body">
                        <div class="btn-group btn-group-justified">
                            <a href="NewRepair.aspx" class="btn btn-primary">Submit Repair</a>
                            <a href="#" class="btn btn-primary">New Equipment</a>
                            <a href="#" class="btn btn-primary">New Photographer</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-md-12">
                <div class="panel panel-primary">
                    <div class="panel-heading" data-toggle="collapse" data-target="#outcollapse" style="cursor: pointer;">
                        <h4 class="panel-title collapsed" data-toggle="collapse" data-target='#collapse1'>Outstanding Repairs <span class="badge" id="outRepairsBadge" runat="server"></span></h4>
                    </div>
                    <div class="panel-collapse collapse" id="outcollapse">
                        <div class="panel-body">
                        <asp:GridView ID="outRepairsGrid" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="outRepairsGrid_RowDataBound"></asp:GridView>
                    </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
