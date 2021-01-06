<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageFile.Master" AutoEventWireup="true" CodeBehind="MasterCreation.aspx.cs" Inherits="ReportEngine.Master.MasterCreation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script src="../Scripts/ReportControlScript.js"></script>
    <div class="box">
        <div class="box-header">
            <h3 class="box-title">
                <asp:Label ID="lblMasterName" runat="server" Text="Master Creation"></asp:Label></h3>
            <%--<a href="../Master/ReportConfigurationList.aspx" class="btn btn-primary pull-right">Back To List</a>--%>
            <asp:HyperLink ID="hlinkBack" runat="server" CssClass="btn btn-primary pull-right">Back To List</asp:HyperLink>
        </div>
        <div class="box-body">
            <asp:HiddenField runat="server" ID="hdnQuery"></asp:HiddenField>
            <asp:HiddenField runat="server" ID="hdnInsertQuery"></asp:HiddenField>
            <asp:HiddenField runat="server" ID="hdnUpdateQuery"></asp:HiddenField>
            <asp:HiddenField runat="server" ID="hdnConnection"></asp:HiddenField>

            <div class="row">
                <asp:PlaceHolder runat="server" ID="phDynamicControls"></asp:PlaceHolder>
            </div>
            <div class="table table-bordered">
                <div class="col-md-6">
                    <asp:Button ID="btnSave" runat="server" Text="Save Details" OnClick="btnSave_Click" CssClass="btn btn-primary pull-right" Visible="false" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
