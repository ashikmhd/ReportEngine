<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageFile.master" AutoEventWireup="true" CodeBehind="ReportSelect.aspx.cs" Inherits="ReportEngine.Reports.ReportSelect" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/ReportControlScript.js"></script>
    <div class="box">
        <div class="box-header">
            <h3 class="box-title">
                <asp:Label ID="Label2" runat="server" Text="Report Page"></asp:Label></h3>
            <%--<a href="../Master/ReportConfigurationList.aspx" class="btn btn-primary pull-right">Back To List</a>--%>
        </div>
        <div class="box-body">
            <asp:HiddenField runat="server" ID="hdnQuery"></asp:HiddenField>
             <asp:HiddenField runat="server" ID="hdnConnection"></asp:HiddenField>            
            <asp:Label ID="lblmsg" runat="server"></asp:Label>
            <div class="row">
                <div class="col-md-1">
                    <asp:Label ID="Label1" runat="server" Text="Select Company" />
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="drpCompanyMasterPage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCompanyMasterPage_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <asp:Label ID="Label3" runat="server" Text="Select Outlet" />
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="drpOutletMasterPage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpOutletMasterPage_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="col-md-1">
                    <asp:Label ID="lbl" runat="server" Text="Select Report" />
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="drpReportName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpReportName_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="row" style="padding-top: 5px; padding-bottom: 5px"></div>
            <div class="row">
                <asp:PlaceHolder runat="server" ID="phDynamicControls"></asp:PlaceHolder>
            </div>
            <div class="table table-bordered">
                <div class="col-md-6">
                    <asp:Button ID="btnRunReport" runat="server" Text="Run Report" OnClick="btnRunReport_Click" CssClass="btn btn-primary pull-right" Visible="false" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
