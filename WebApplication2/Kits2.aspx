<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Kits2.aspx.cs" Inherits="WebApplication2.Kits2" %>
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
        <div class="row top20 hidden" id="warningDiv" runat="server">
            <div class="alert alert-danger fade in">
                <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
                <strong>Error! </strong><asp:Label ID="warningLabel" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row top20">
            <div class="col-xs-4 col-sm-4 col-md-2 col-lg-1">
                <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#newKitModal">New Kit</button>
            </div>
            <div class="col-xs-8 col-sm-8 col-sm-10 col-lg-11">
                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" CssClass="form-control"></asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <div class="row top20">
            <div class="col-sm-4">
                <div class="panel panel-default" id="photogPanel" runat="server" style="margin-top: 42px;">
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
                <ul class="nav nav-tabs">
                    <li class="active"><a data-toggle="tab" href="#mainCam">Main</a></li>
                    <li><a data-toggle="tab" href="#spareCam" id="spareCamTab" runat="server">Spare</a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane fade in active" id="mainCam">
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
                                <div class="btn-group" id="camCogDrop" runat="server">
                                    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="glyphicon glyphicon-cog"></span></button>
                                    <ul class="dropdown-menu">
                                        <li><a href="#" id="cameraEditDetails" runat="server"><span class="glyphicon glyphicon-edit"></span> Edit details</a></li>
                                        <li><a href="#" id="cameraSubmitRepair" runat="server"><span class="glyphicon glyphicon-wrench"></span> Submit repair</a></li>
                                    </ul>
                                </div>
                                <a class="btn btn-default" id="cameraAddRemove" runat="server">Add/Remove</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
                    <div class="tab-pane fade" id="spareCam">
                        <div class="panel panel-default" id="spareCameraPanel" runat="server">
                            <div class="panel-heading">Spare Camera</div>
                            <div class="panel-body">
                                <div class="row">
                            <div class="col-xs-5 col-sm-5">Serial Number: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="spareCamSN" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-5 col-sm-5">Make: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="spareCamMake" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-5 col-sm-5">Model: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="spareCamModel" runat="server"></asp:Label>
                            </div>
                        </div>
                            </div>
                            <div class="panel-footer">
                                <div class="row">
                            <div class="col-sm-12 text-right">
                                <a class="btn btn-info" id="spareCamToMain" runat="server" data-toggle="modal" data-target="#camMainModal"><span class="glyphicon glyphicon-repeat"></span> Set as main</a>
                                <a class="btn btn-default" id="spareCamAddRemove" runat="server">Add/Remove</a>
                            </div>
                        </div>
                            </div>
                        </div>
                    </div>
            </div>
            </div>
            <div class="col-sm-4">
                <ul class="nav nav-tabs">
                    <li class="active"><a data-toggle="tab" href="#mainLap">Main</a></li>
                    <li><a data-toggle="tab" href="#spareLap" id="spareLapTab" runat="server">Spare</a></li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane fade in active" id="mainLap">
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
                                <div class="btn-group" id="lapCogDrop" runat="server">
                                    <button type="button" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><span class="glyphicon glyphicon-cog"></span></button>
                                    <ul class="dropdown-menu">
                                        <li><a href="#" id="laptopEditDetails" runat="server"><span class="glyphicon glyphicon-edit"></span> Edit details</a></li>
                                        <li><a href="#" id="laptopSubmitRepair" runat="server"><span class="glyphicon glyphicon-wrench"></span> Submit repair</a></li>
                                    </ul>
                                </div>
                                <a class="btn btn-default" id="laptopAddRemove" runat="server">Add/Remove</a>
                            </div>
                        </div>
                    </div>
                </div>
                        </div>
                    <div class="tab-pane fade" id="spareLap">
                        <div class="panel panel-default" id="spareLaptopPanel" runat="server">
                            <div class="panel-heading">Spare Laptop</div>
                            <div class="panel-body">
                                <div class="row">
                            <div class="col-xs-5 col-sm-5">Serial Number: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="spareLapSN" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-5 col-sm-5">Make: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="spareLapMake" runat="server"></asp:Label>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-5 col-sm-5">Model: </div>
                            <div class="col-xs-7 col-sm-7">
                                <asp:Label ID="spareLapModel" runat="server"></asp:Label>
                            </div>
                        </div>
                            </div>
                            <div class="panel-footer">
                                <div class="row">
                            <div class="col-sm-12 text-right">
                                <a class="btn btn-info" id="spareLapToMain" runat="server" data-toggle="modal" data-target="#lapMainModal"><span class="glyphicon glyphicon-repeat"></span> Set as main</a>
                                <a class="btn btn-default" id="spareLapAddRemove" runat="server">Add/Remove</a>
                            </div>
                        </div>
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

    <!--Modal Content-->

    <div id="newKitModal" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Enter new kit PH:</h4>
                </div>
                <div class="modal-body">
                    <asp:TextBox ID="newKitBox" runat="server" CssClass="form-control"></asp:TextBox>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="saveNewKit" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="saveNewKit_Click" />
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <!--Spare to Main Camera modal-->
    <div id="camMainModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Set spare camera as main</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-xs-12">
                            <p><strong>Set spare camera as new main camera?</strong> This will remove the main camera from the kit and replace it with the spare camera assigned to the kit. Do <strong>NOT</strong> use if the main camera is in for repair and the spare is a temporary replacement.</p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <div class="panel panel-danger">
                                <div class="panel-heading" id="mainCamReplaceHead" runat="server"></div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-xs-5">Serial Number: </div>
                                        <div class="col-xs-7" id="camReplaceMainSN" runat="server"></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-5">Make: </div>
                                        <div class="col-xs-7" id="camReplaceMainMake" runat="server"></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-5">Model: </div>
                                        <div class="col-xs-7" id="camReplaceMainModel" runat="server"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-2 text-center" style="font-size: 4em;">
                            <span class="glyphicon glyphicon-arrow-left" style="vertical-align: middle;"></span>
                        </div>
                        <div class="col-xs-5">
                            <div class="panel panel-primary">
                                <div class="panel-heading" id="spareCamReplaceHead" runat="server"></div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-xs-5">Serial Number: </div>
                                        <div class="col-xs-7" id="camReplaceSpareSN" runat="server"></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-5">Make: </div>
                                        <div class="col-xs-7" id="camReplaceSpareMake" runat="server"></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-5">Model: </div>
                                        <div class="col-xs-7" id="camReplaceSpareModel" runat="server"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a class="btn btn-primary" id="spareCamConfirmA" runat="server">Confirm</a>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>


    <!--Spare to Main Laptop modal-->
    <div id="lapMainModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Set spare laptop as main</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-xs-12">
                            <p><strong>Set spare laptop as new main laptop?</strong> This will remove the main laptop from the kit and replace it with the spare laptop assigned to the kit. Do <strong>NOT</strong> use if the main laptop is in for repair and the spare is a temporary replacement.</p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <div class="panel panel-danger">
                                <div class="panel-heading" id="lapReplaceMainHead" runat="server"></div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-xs-5">Serial Number: </div>
                                        <div class="col-xs-7" id="lapReplaceMainSN" runat="server"></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-5">Make: </div>
                                        <div class="col-xs-7" id="lapReplaceMainMake" runat="server"></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-5">Model: </div>
                                        <div class="col-xs-7" id="lapReplaceMainModel" runat="server"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-2 text-center" style="font-size: 4em;">
                            <span class="glyphicon glyphicon-arrow-left" style="vertical-align: middle;"></span>
                        </div>
                        <div class="col-xs-5">
                            <div class="panel panel-primary">
                                <div class="panel-heading" id="lapReplaceSpareHead" runat="server"></div>
                                <div class="panel-body">
                                    <div class="row">
                                        <div class="col-xs-5">Serial Number: </div>
                                        <div class="col-xs-7" id="lapReplaceSpareSN" runat="server"></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-5">Make: </div>
                                        <div class="col-xs-7" id="lapReplaceSpareMake" runat="server"></div>
                                    </div>
                                    <div class="row">
                                        <div class="col-xs-5">Model: </div>
                                        <div class="col-xs-7" id="lapReplaceSpareModel" runat="server"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <a class="btn btn-primary" id="spareLapConfirmA" runat="server">Confirm</a>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
