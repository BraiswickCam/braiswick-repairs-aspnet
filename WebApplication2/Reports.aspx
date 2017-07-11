<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="WebApplication2.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
                <div class="panel panel-primary">
                    <div class="panel-heading" data-toggle="collapse" data-target="#reportCollapse" style="cursor: pointer;">
                        <h4 class="panel-title panel-title2 collapsed" data-toggle="collapse" data-target="#reportCollapse">Reports</h4>
                    </div>
                    <div class="panel-collapse collapse" id="reportCollapse">
                        <div class="panel-body">
                            <div class="col-sm-6">
                                <h4><span class="glyphicon glyphicon-user"></span> Photographers</h4>
                                <ul style="list-style: none; padding-left: 0;">
                                    <li><a href="Reports.aspx?rep=repairCost">Repair cost per photographer</a></li>
                                </ul>
                            </div>
                            <div class="col-sm-6">
                                <h4><span class="glyphicon glyphicon-camera"></span> Equipment</h4>
                                <ul style="list-style: none; padding-left: 0;">
                                    <li><a href="Reports.aspx?rep=OSCount">Laptop operating systems</a></li>
                                    <li><a href="Reports.aspx?rep=MakeCount">Laptop manufacturers</a></li>
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
