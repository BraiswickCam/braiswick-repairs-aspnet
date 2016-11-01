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
                        <p>Laptops table will go here</p>
                        <asp:GridView ID="laptopsGrid" runat="server"></asp:GridView>
                    </div>
                    <div id="cameras" class="tab-pane fade">
                        <p>Cameras table will go here</p>
                        <asp:GridView ID="camerasGrid" runat="server"></asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="container-fluid" id="steptwo" runat="server" style="margin-top: 30px;">
        <div class="row">
            <div class="col-xs-6 col-md-4 col-md-offset-2">
                <div class="panel panel-primary">
                    <div class="panel-heading" id="equipPanel" runat="server"></div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-5">Serial Number: </div>
                            <div class="col-xs-7" id="equipSN" runat="server"></div>
                        </div>
                        <div class="row">
                            <div class="col-xs-5">Make: </div>
                            <div class="col-xs-7" id="equipMake" runat="server"></div>
                        </div>
                        <div class="row">
                            <div class="col-xs-5">Model: </div>
                            <div class="col-xs-7" id="equipModel" runat="server"></div>
                        </div>
                        <div class="row" id="equipOptionalRow" runat="server">
                            <div class="col-xs-5">OS: </div>
                            <div class="col-xs-7" id="equipOption" runat="server"></div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-6 col-md-4">
                <div class="panel panel-primary">
                    <div class="panel-heading" id="photogPanel" runat="server"></div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-5">Name: </div>
                            <div class="col-xs-7" id="photogName" runat="server"></div>
                        </div>
                        <div class="row">
                            <div class="col-xs-5">Initials: </div>
                            <div class="col-xs-7" id="photogInitial" runat="server"></div>
                        </div>
                        <div class="row">
                            <div class="col-xs-5">Office: </div>
                            <div class="col-xs-7" id="photogOffice" runat="server"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-md-8 col-md-offset-2">
                <div class="panel panel-primary">
                    <div class="panel-heading">Issues:</div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <asp:TextBox ID="notesText" TextMode="MultiLine" Rows="8" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12"><p>Please be as descriptive as possible, mentioning any error messages that appear and what the user was doing leading up to the error.</p></div>
                        </div>
                    </div>
                    <div class="panel-footer text-center">
                        <asp:Button ID="submitRepair" runat="server" Text="Submit New Repair" CssClass="btn btn-primary" />
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
