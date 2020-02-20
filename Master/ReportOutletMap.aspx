<%@ Page Language="C#" MasterPageFile="~/MasterPagePopup.Master" AutoEventWireup="true" CodeBehind="ReportOutletMap.aspx.cs" Inherits="ReportEngine.Master.ReportOutletMap" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <asp:HiddenField ID="hdnReportId" runat="server" Value="0" />
        <asp:CheckBoxList ID="chkListOutlets" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" CssClass="form-control">
        </asp:CheckBoxList>
    </div>
     <div class="row padding5"></div>
    <div class="row padding5">
        <div class="col-md-6">
            <asp:Button ID="btnSaveMapping" runat="server" Text="Save Mapping" OnClick="btnSaveMapping_Click" CssClass="btn btn-primary pull-right" Visible="false" />
        </div>
    </div>
</asp:Content>
