<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewRepair.aspx.cs" Inherits="WebApplication2.NewRepair" %>
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

        .panel-body {
            word-break: break-all;
        }
    </style>

    <div class="container-fluid" id="stepone" runat="server">
        <div class="row">
            <div class="col-xs-12">
                <p>To add a new repair entry, please choose the item from one of the lists below. Ideally, use the kits list as this will auto-fill in any other information such as photographer details.</p>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <ul class="nav nav-pills">
                    <li class="active"><a data-toggle="tab" href="#kits">Kits</a></li>
                    <li><a data-toggle="tab" href="#laptops">Laptops</a></li>
                    <li><a data-toggle="tab" href="#cameras">Cameras</a></li>
                </ul>
                <div class="tab-content">
                    <div id="kits" class="tab-pane fade in active">
                        <asp:GridView ID="kitsGrid" runat="server" GridLines="None" CssClass="table table-striped table-hover" OnRowDataBound="kitsGrid_RowDataBound"></asp:GridView>
                    </div>
                    <div id="laptops" class="tab-pane fade">
                        <asp:GridView ID="laptopsGrid" runat="server"></asp:GridView>
                    </div>
                    <div id="cameras" class="tab-pane fade">
                        <asp:GridView ID="camerasGrid" runat="server"></asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal -->
    <div id="testmodal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Modal Header</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-xs-6">
                            <div class="panel panel-primary">
                                <div class="panel-heading">Laptop ID: </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-xs-6">Serial Number: </div>
                                        <div class="col-xs-6">{SN}</div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-6">Make: </div>
                                        <div class="col-xs-6">{Make}</div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-6">Model: </div>
                                        <div class="col-xs-6">{Model}</div>
                                    </div>
                                </div>
                                <div class="panel-footer">
                                    <a href="#" class="btn btn-primary" role="button">New Repair</a>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-6">
                            <div class="panel panel-primary">
                                <div class="panel-heading">Camera ID: </div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-xs-6">Serial Number: </div>
                                        <div class="col-xs-6">{SN}</div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-6">Make: </div>
                                        <div class="col-xs-6">{Make}</div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-6">Model: </div>
                                        <div class="col-xs-6">{Model}</div>
                                    </div>
                                </div>
                                <div class="panel-footer">
                                    <a href="#" class="btn btn-primary" role="button">New Repair</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
