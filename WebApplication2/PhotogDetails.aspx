<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PhotogDetails.aspx.cs" Inherits="WebApplication2.PhotogDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
            <script>
$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip();   
});
</script>
    <style>
        * {
            font-family: Ubuntu, sans-serif;
        }

        .top20 {
            margin-top: 20px;
        }

        .top10 {
            margin-top: 10px;
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
        <div class="row top20">
            <div class="col-xs-10 col-xs-offset-1 col-sm-8 col-sm-offset-2 col-md-6 col-md-offset-3">
                <div class="panel panel-default">
                    <div class="panel-heading">Photographer ID: <span id="idHolder" runat="server"></span></div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-4">Name: </div>
                            <div class="col-xs-8">
                                <asp:TextBox ID="nameText" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row top10">
                            <div class="col-xs-4">Initials: </div>
                            <div class="col-xs-8">
                                <asp:TextBox ID="initialText" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row top10">
                            <div class="col-xs-4">Active?: </div>
                            <div class="col-xs-8">
                                <asp:CheckBox ID="activeCheck" runat="server" />
                            </div>
                        </div>
                        <div class="row top10">
                            <div class="col-xs-4">Office: </div>
                            <div class="col-xs-8">
                                <asp:DropDownList ID="officeList" runat="server" CssClass="form-control">
                                    <asp:ListItem>MA</asp:ListItem>
                                    <asp:ListItem>MF</asp:ListItem>
                                    <asp:ListItem>KT</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer">
                        <div class="row text-center">
                            <div class="col-xs-12">
                                <asp:Button ID="saveButton" runat="server" OnClick="saveButton_Click" Text="Save" CssClass="btn btn-primary" />
                            </div>
                        </div>
                        <div class="row text-center">
                            <div class="col-xs-12">
                                <asp:Label ID="updateLabel" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-xs-12">
                <div class="panel panel-success" style="margin-top: 20px;">
                    <div class="panel-heading" data-toggle="collapse" data-target="#reportsCollapse" style="cursor: pointer;">
                        <h4 class="panel-title panel-title2 collapsed" data-toggle="collapse" data-target="#reportsCollapse">Reports</h4>
                    </div>
                    <div class="panel-collapse collapse" id="reportsCollapse">
                        <div class="panel-body">
                            <asp:GridView ID="reportsGridView" runat="server" CssClass="table table-striped table-hover" GridLines="None" OnRowDataBound="reportsGridView_RowDataBound"></asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <div class="panel panel-primary" style="margin-top: 20px;">
                    <div class="panel-heading" data-toggle="collapse" data-target="#repairCollapse" style="cursor: pointer;">
                        <h4 class="panel-title panel-title2 collapsed" data-toggle="collapse" data-target="#repairCollapse">Repairs</h4>
                    </div>
                    <div class="panel-collapse collapse" id="repairCollapse">
                        <div class="panel-body">
                            <asp:GridView ID="historyGridView" runat="server" CssClass="table table-striped table-hover" GridLines="None" OnRowDataBound="historyGridView_RowDataBound"></asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

<%--    <p>
    &nbsp;</p>
<p>
    Name:
    <asp:TextBox ID="nameText" runat="server"></asp:TextBox>
</p>
<p>
    Initial:
    <asp:TextBox ID="initialText" runat="server"></asp:TextBox>
</p>
<p>
    Active:
    <asp:CheckBox ID="activeCheck" runat="server" />
</p>
<p>
    Office:
    <asp:DropDownList ID="officeList" runat="server">
        <asp:ListItem>MA</asp:ListItem>
        <asp:ListItem>MF</asp:ListItem>
        <asp:ListItem>KT</asp:ListItem>
    </asp:DropDownList>
</p>
<p>
    <asp:Button ID="saveButton" runat="server" OnClick="saveButton_Click" Text="Save" />
</p>
<p>
    <asp:Label ID="updateLabel" runat="server"></asp:Label>
</p>--%>
</asp:Content>
