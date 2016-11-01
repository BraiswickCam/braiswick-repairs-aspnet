<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NewRepair.aspx.cs" Inherits="WebApplication2.NewRepair" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container-fluid">
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
                        <asp:GridView ID="kitsGrid" runat="server"></asp:GridView>
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

</asp:Content>
