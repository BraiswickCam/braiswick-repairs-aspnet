﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Kits2.aspx.cs" Inherits="WebApplication2.Kits2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip();   
});
</script>
    <style>
            *{
                font-family: Ubuntu, sans-serif;
            }

            /*.btn {
                padding: 3px 5px;
                display: inline;
            }

            a.btn {
                background: linear-gradient(#f7f7f7, #FFF);
                border: 1px solid #808080;
                border-radius: 3px;
                text-align: right;
                margin-right: 5px;
                margin-bottom: 5px;
                float: right;
            }

            a.btn:hover {
                background: linear-gradient(#FFF, #f7f7f7);
                border: 1px solid black;
            }*/

            .top20 {
                margin-top: 20px;
            }

        </style>

    <div class="container-fluid">
        <div class="row top20">
            <div class="col-xs-12">
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <div class="row top20">
            <div class="col-sm-4">
                <div class="panel panel-default" id="photogPanel" runat="server">
                    <div class="panel-heading">Photographer</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-5 col-sm-5">Name: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="nameLabel" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-5 col-sm-5">Initials: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="initialLabel" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-5 col-sm-5">Office: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="officeLabel" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer">
                        <div class="row">
                            <div class="col-sm-12 text-right">
                                <a class="btn btn-default" id="photogAddRemove" runat="server"></a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="panel panel-default" id="cameraPanel" runat="server">
                    <div class="panel-heading">Camera</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-5 col-sm-5">Serial Number: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="camSNLabel" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-5 col-sm-5">Make: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="camMakeLabel" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-5 col-sm-5">Model: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="camModelLabel" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer">
                        <div class="row">
                            <div class="col-sm-12 text-right">
                                <a class="btn btn-default" id="cameraAddRemove" runat="server">Add/Remove</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="panel panel-default" id="laptopPanel" runat="server">
                    <div class="panel-heading">Laptop</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-5 col-sm-5">Serial Number: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="lapSNLabel" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-5 col-sm-5">Make: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="lapMakeLabel" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-5 col-sm-5">Model: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="lapModelLabel" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="panel-footer">
                        <div class="row">
                            <div class="col-sm-12 text-right">
                                <a class="btn btn-default" id="laptopAddRemove" runat="server">Add/Remove</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-12 table-responsive">
                <asp:GridView ID="historyGridView" runat="server" OnRowDataBound="historyGridView_RowDataBound" CssClass="table table-striped table-hover" GridLines="None"></asp:GridView>
            </div>
        </div>
    </div>   
</asp:Content>
