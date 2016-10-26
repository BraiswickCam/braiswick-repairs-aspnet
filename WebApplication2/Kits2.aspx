<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Kits2.aspx.cs" Inherits="WebApplication2.Kits2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
            *{
                font-family: Ubuntu, sans-serif;
            }

            .tableHeader {
                font-size: 20px;
                width: 100%;
            }

            .topTables {
                text-align: center;
                width: auto;
            }

            .tableContainer {
                display: inline-block;
                width: auto;
                min-width: 25%;
                margin: 20px;
                padding: 0px 10px 2px 10px;
                text-align: left;
                background: -webkit-linear-gradient(left top, #FFF, #f7f7f7);
                border: 1px solid Black;
                border-radius: 5px;
            }

            .detailTables {
                width: 100%;
                padding: 0px;
            }

            .detailTables td {
                padding: 2px 4px;
            }

            .tableHeaderCell {
                padding-right: 10px;
                width: 100%;
                border-bottom: 2px solid black;
            }

            .rightCell {
                padding-left: 5px;
                padding-right: 10px;  
                font-weight: bold;
                width: inherit;      
            }

            .historyContainer {
                margin-top: 40px;
                text-align: center;
                width: 100%;
                height: auto;
            }

            .history {
                text-align: left;
                margin: 0 auto;
            }

            .history th, .history td {
                padding: 4px;
            }

            .btn {
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
            }

            .dropdown {
                border: 1px solid black;
                border-radius: 4px;
            }

            .kitLink {
        position: relative;
        display: inline-block;
        }

        .kitDrop {
        display: none;
        position: absolute;
        background: -webkit-linear-gradient(left top, #FFF, #f7f7f7);
        min-width: 160px;
        width: auto;
        box-shadow: 0px 8px 16px 0px rgba(0,0,0,0.2);
        padding: 12px 16px;
        z-index: 1;
        border: 0;
        border-radius: 10px;
        }

        .kitLink:hover .kitDrop {
        display: block;
        }

        </style>
    <p>
        &nbsp;</p>
    <div class="kitSelect">
        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" CssClass="dropdown">
        </asp:DropDownList>
    </div>
    <div class="topTables">  
      <div class="tableContainer">
        <asp:Table runat="server" CssClass="detailTables">
            <asp:TableHeaderRow CssClass="tableHeader">
                <asp:TableHeaderCell ColumnSpan="2" CssClass="tableHeaderCell">Photographer</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell>Name:</asp:TableCell>
                <asp:TableCell CssClass="rightCell"><asp:Label ID="nameLabel" runat="server"></asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Initial:</asp:TableCell>
                <asp:TableCell CssClass="rightCell"><asp:Label ID="initialLabel" runat="server"></asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Office:</asp:TableCell>
                <asp:TableCell CssClass="rightCell"><asp:Label ID="officeLabel" runat="server"></asp:Label></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
          <br />
          <a class="btn" id="photogAddRemove" runat="server">Add/Remove</a>
          </div>
        <div class="tableContainer">
        <asp:Table runat="server" CssClass="detailTables">
            <asp:TableHeaderRow CssClass="tableHeader">
                <asp:TableHeaderCell ColumnSpan="2" CssClass="tableHeaderCell">Camera</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell>Make:</asp:TableCell>
                <asp:TableCell CssClass="rightCell"><asp:Label ID="camMakeLabel" runat="server"></asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Model:</asp:TableCell>
                <asp:TableCell CssClass="rightCell"><asp:Label ID="camModelLabel" runat="server"></asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Serial Number:</asp:TableCell>
                <asp:TableCell CssClass="rightCell"><asp:Label ID="camSNLabel" runat="server"></asp:Label></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
            <br />
          <a class="btn" id="cameraAddRemove" runat="server">Add/Remove</a><a class="btn">New Repair</a>
            </div>
        <div class="tableContainer">
        <asp:Table runat="server" CssClass="detailTables">
            <asp:TableHeaderRow CssClass="tableHeader">
                <asp:TableHeaderCell ColumnSpan="2" CssClass="tableHeaderCell">Laptop</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell>Make:</asp:TableCell>
                <asp:TableCell CssClass="rightCell"><asp:Label ID="lapMakeLabel" runat="server"></asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Model:</asp:TableCell>
                <asp:TableCell CssClass="rightCell"><asp:Label ID="lapModelLabel" runat="server"></asp:Label></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>Serial Number:</asp:TableCell>
                <asp:TableCell CssClass="rightCell"><asp:Label ID="lapSNLabel" runat="server"></asp:Label></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
           <br />
          <a class="btn" id="laptopAddRemove" runat="server">Add/Remove</a><a class="btn">New Repair</a>
    </div>
        </div>
        <div class="historyContainer">
            <asp:GridView ID="historyGridView" runat="server" CssClass="history" OnRowDataBound="historyGridView_RowDataBound">
            </asp:GridView>
        </div>
    
</asp:Content>
