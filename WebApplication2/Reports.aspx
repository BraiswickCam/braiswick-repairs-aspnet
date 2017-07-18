<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="WebApplication2.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/purl.js"></script>
    <script src="https://use.fontawesome.com/1acc7e1b75.js"></script>
    <script>
        $(document).ready(function () {
            var viewParam = $.url(window.location.href).param('rep');
            if (typeof viewParam === "undefined") {
                $('#reportCollapse').collapse("show");
            }
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
    </style>

    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-12">
                <div class="panel panel-primary" style="margin-top: 20px;">
                    <div class="panel-heading" data-toggle="collapse" data-target="#reportCollapse" style="cursor: pointer;">
                        <h4 class="panel-title panel-title2 collapsed" data-toggle="collapse" data-target="#reportCollapse">Reports</h4>
                    </div>
                    <div class="panel-collapse collapse" id="reportCollapse">
                        <div class="panel-body">
                            <div class="col-sm-6">
                                <h4><span class="glyphicon glyphicon-user"></span> Photographers</h4>
                                <ul style="list-style: none; padding-left: 0;">
                                    <li><a href="Reports.aspx?rep=repairCost"><i class="fa fa-wrench"></i> Repair cost per photographer</a></li>
                                    <li><a href="Reports.aspx?rep=RepairCount"><i class="fa fa-wrench"></i> Repairs per photographer</a></li>
                                    <li><a href="Reports.aspx?rep=OfficeCountCost"><i class="fa fa-wrench"></i> Repairs per office</a></li>
                                </ul>
                            </div>
                            <div class="col-sm-6">
                                <h4><span class="glyphicon glyphicon-camera"></span> Equipment</h4>
                                <ul style="list-style: none; padding-left: 0;">
                                    <li><a href="Reports.aspx?rep=OSCount"><i class="fa fa-laptop"></i> Laptop operating systems</a></li>
                                    <li><a href="Reports.aspx?rep=MakeCount"><i class="fa fa-laptop"></i> Laptop manufacturers</a></li>
                                    <li><a href="Reports.aspx?rep=AssignedLaptopPercent"><i class="fa fa-laptop"></i> % of Laptops assigned to kits</a></li>
                                    <li><a href="Reports.aspx?rep=LaptopRepairCount"><i class="fa fa-laptop"></i> Repairs per laptop</a></li>
                                    <li><a href="Reports.aspx?rep=AssignedCameraPercent"><i class="fa fa-camera"></i> % of Cameras assigned to kits</a></li>
                                    <li><a href="Reports.aspx?rep=CameraRepairCount"><i class="fa fa-camera"></i> Repairs per camera</a></li>
                                </ul>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-sm-12">
                <asp:GridView ID="resultsGrid" runat="server" CssClass="table table-striped" GridLines="None"></asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
