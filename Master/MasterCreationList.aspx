<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageFile.Master" AutoEventWireup="true" CodeBehind="MasterCreationList.aspx.cs" Inherits="ReportEngine.Master.MasterCreationList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function OpenEditField(PrimaryKeyId) {
            var drpReportName = document.getElementById("<%= drpReportName.ClientID %>");
            var hlinkNew = document.getElementById("<%= hlinkNew.ClientID %>");
            Redirect("MasterCreation.aspx?PrimaryKeyId=" + PrimaryKeyId + "&ReportId=" + drpReportName.value + "&ReportName=Edit " + drpReportName.options[drpReportName.selectedIndex].text);
        }
    </script>
    <div class="box">
        <div class="box-header">
            <h3 class="box-title">
                <asp:Label ID="Label2" runat="server" Text="Master Creation Page"></asp:Label></h3>
            <%--<a href="../Master/ReportConfigurationList.aspx" class="btn btn-primary pull-right">Back To List</a>--%>
            <asp:HyperLink ID="hlinkNew" runat="server" CssClass="btn btn-primary pull-right" Visible="false">New</asp:HyperLink>
        </div>
        <div class="box-body">
            <div class="row padding5">                
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
                    <asp:Label ID="lbl" runat="server" Text="Select Master" />
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="drpReportName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpReportName_SelectedIndexChanged" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
            <div class="row padding5">
                <asp:GridView ID="gvDynamicMasters" runat="server" CssClass="table table-striped table-bordered table-hover"
                    AutoGenerateColumns="true">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl. No">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Edit">
                            <ItemTemplate>
                                <a href="#" onclick="<%# "javascript:return OpenEditField(" + Eval("PrimaryKeyId") + ")" %>">
                                    <i class="fa fa-edit"></i></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
