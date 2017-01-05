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

        .panel-title2:after {
            /* symbol for "opening" panels */
            font-family: 'Glyphicons Halflings';  /* essential for enabling glyphicon */
            content: "\e113";    /* adjust as needed, taken from bootstrap.css */
            float: right;        /* adjust as needed */
            color: white;         /* adjust as needed */
        }

        .panel-title2.collapsed:after {
            /* symbol for "collapsed" panels */
            content: "\e114";    /* adjust as needed, taken from bootstrap.css */
        }

        .large-icon {
            font-size: 400%;
        }

        .round-btn {
            border-radius: 50%;
            padding: 20px 22px;
        }
    </style>

    <div class="container-fluid">
        <div class="row"><br /></div>
        <div class="row">
            <div class="col-xs-12 col-md-12">
                <div class="panel panel-primary" style="text-align: center;">
                    <div class="panel-heading"><h4 class="panel-title"><!--<span class="glyphicon glyphicon-dashboard"></span> -->Dashboard</h4></div>
                    <div class="panel-body">
                        <div class="col-xs-3">
                            <a href="NewRepair.aspx" class="btn btn-primary round-btn"><span class="glyphicon glyphicon-wrench large-icon"></span></a><p>Submit Repair</p>
                        </div>
                        <div class="col-xs-3">
                            <a href="Kits2.aspx" class="btn btn-primary round-btn"><span class="glyphicon glyphicon-briefcase large-icon"></span></a><p>Kits</p>
                        </div>
                        <div class="col-xs-3">
                            <a href="Equipment.aspx" class="btn btn-primary round-btn"><span class="glyphicon glyphicon-camera large-icon"></span></a><p>Equipment</p>
                        </div>
                        <div class="col-xs-3">
                            <a href="Photogs.aspx" class="btn btn-primary round-btn"><span class="glyphicon glyphicon-user large-icon"></span></a><p>Photographers</p>
                        </div>
                   </div>
               </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-md-12">
                <div class="panel panel-primary">
                    <div class="panel-heading" data-toggle="collapse" data-target="#outcollapse" style="cursor: pointer;">
                        <h4 class="panel-title panel-title2 collapsed" data-toggle="collapse" data-target='#outcollapse'><span class="glyphicon glyphicon-wrench"></span> Outstanding Repairs <span class="badge" id="outRepairsBadge" runat="server"></span></h4>
                    </div>
                    <div class="panel-collapse collapse" id="outcollapse">
                        <div class="panel-body">
                            <asp:GridView ID="outRepairsGrid" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="outRepairsGrid_RowDataBound"></asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-md-12">
                <div class="panel panel-primary">
                    <div class="panel-heading" data-toggle="collapse" data-target="#recentcollapse" style="cursor: pointer;">
                        <h4 class="panel-title panel-title2 collapsed" data-toggle="collapse" data-target="#recentcollapse"><span class="glyphicon glyphicon-time"></span> Recent Activity (last 3 days) <span class="badge" id="recentBadge" runat="server"></span></h4>
                    </div>
                    <div class="panel-collapse collapse" id="recentcollapse">
                        <div class="panel-body">
                            <asp:GridView ID="recentGrid" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="recentGrid_RowDataBound" ></asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
