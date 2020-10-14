<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageFile.master" AutoEventWireup="true" CodeBehind="ReportConfiguration.aspx.cs" Inherits="ReportEngine.Master.ReportConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">
        function pageLoad() {
            $('.decimal_left').inputmask('decimal', { rightAlign: false });

            $('[id*=lstControlProperties]').multiselect({
                includeSelectAllOption: true
            });
            
        }
    </script>

    <div class="box">
        <asp:HiddenField ID="hdnReportId" runat="server" Value="0" />
        <div class="box-header">
            <h3 class="box-title">
                <asp:Label ID="Label2" runat="server" Text="Report Configuration"></asp:Label></h3>
             <a href="ReportConfigurationList.aspx" class="btn btn-primary pull-right">Back To List</a>
        </div>
        <div class="box-body">
            <div class="row padding5">
                <div class="col-md-2">
                    Type
                    <asp:DropDownList ID="drpType" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpType_SelectedIndexChanged">
                        <asp:ListItem Value="1">Report</asp:ListItem>
                        <asp:ListItem Value="2">Master</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    Report Name
                    <asp:TextBox ID="txtReportName" runat="server" CssClass="form-control" required></asp:TextBox>
                </div>
                <div class="col-md-3">
                    Report Description
                    <asp:TextBox ID="txtReportDescription" runat="server" CssClass="form-control" required></asp:TextBox>
                </div>
                <div class="col-md-2">
                    Active
                    <asp:DropDownList ID="drpActive" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="drpType_SelectedIndexChanged">
                        <asp:ListItem Value="1">Active</asp:ListItem>
                        <asp:ListItem Value="0">In Active</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="row padding5">
                <div class="col-md-12">
                    Report Query
                    <asp:TextBox ID="txtReportQuery" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" required></asp:TextBox>
                </div>
            </div>
            <div class="row padding5" runat="server" id="divInsertQuery" visible="false">
                <div class="col-md-12">
                    Insert Query
                    <asp:TextBox ID="txtInsertQuery" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" required></asp:TextBox>
                </div>
            </div>
            <div class="row padding5" runat="server" id="divUpdatetQuery" visible="false">
                <div class="col-md-12">
                    Update Query
                    <asp:TextBox ID="txtUpdateQuery" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" required></asp:TextBox>
                </div>
            </div>
            <div class="row padding5">
                <div class="col-md-2">
                    Control Type
                    <asp:DropDownList ID="drpControlType" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="col-md-2">
                    Control Label
                    <asp:TextBox ID="txtLabelName" runat="server" CssClass="form-control" required></asp:TextBox>
                </div>
                <div class="col-md-2">
                    Control ID
                    <asp:TextBox ID="txtControlId" runat="server" CssClass="form-control" required></asp:TextBox>
                </div>
                <div class="col-md-1">
                    Row Group
                    <asp:TextBox ID="txtRowGroup" runat="server" CssClass="form-control decimal_left" required></asp:TextBox>
                </div>
                <div class="col-md-1">
                    Col Width
                    <asp:TextBox ID="txtColWidth" runat="server" CssClass="form-control decimal_left" required></asp:TextBox>
                </div>
                <div class="col-md-1">
                    Display Order
                    <asp:TextBox ID="txtDisplayOrder" runat="server" CssClass="form-control decimal_left" required></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <div class="d-flex flex-column">
                        <div class="p-2">Control Properties</div>
                        <div class="p-2">
                            <asp:ListBox ID="lstControlProperties" runat="server" SelectionMode="Multiple" CssClass="form-control"></asp:ListBox>
                        </div>
                    </div>
                </div>
                <div class="col-md-1">
                    <div class="d-flex flex-column">
                        <div class="p-2">&nbsp;</div>
                        <div class="p-2">
                            <asp:Button ID="btnAdd" runat="server" Text="Add Control" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row padding5">
                <asp:GridView ID="gvDynamicControls" runat="server" CssClass="table table-striped table-bordered table-hover"
                    AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl. No">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                                <asp:HiddenField ID="hdnRowId" runat="server" Value='<%# Bind("RowId") %>'></asp:HiddenField>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Control Type">
                            <ItemTemplate>
                                <asp:Label ID="lblControlName" runat="server" Text='<%# Bind("ControlName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Control Label">
                            <ItemTemplate>
                                <asp:TextBox ID="txtLabelName" runat="server" Text='<%# Bind("LabelName") %>' CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Control ID">
                            <ItemTemplate>
                                <asp:TextBox ID="txtControlId" runat="server" Text='<%# Bind("ControlId") %>' CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="RowGroup">
                            <ItemTemplate>
                                <asp:TextBox ID="txtRowGroup" runat="server" Text='<%# Bind("RowGroup") %>' CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ColWidth">
                            <ItemTemplate>
                                <asp:TextBox ID="txtColWidth" runat="server" Text='<%# Bind("ColWidth") %>' CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ControlOrder">
                            <ItemTemplate>
                                <asp:TextBox ID="txtControlOrder" runat="server" Text='<%# Bind("ControlOrder") %>' CssClass="form-control"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Control Properties">
                            <ItemTemplate>
                                <asp:GridView ID="gvControlProperties" runat="server" AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sl. No">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                                <asp:HiddenField ID="hdnRowId" runat="server" Value='<%# Bind("RowId") %>'></asp:HiddenField>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Property">
                                            <ItemTemplate>
                                                <asp:Label ID="lblProperty" runat="server" Text='<%# Bind("ReportSettingName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="DefaultValue">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtDefaultValue" runat="server" Text='<%# Bind("ReportDefaultValue") %>' CssClass="form-control" TextMode="MultiLine"
                                                    Enabled='<%# Eval("DefaultValueEnable") %>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="row padding5">
                <asp:Button ID="btnSave" runat="server" Text="Save Report" CssClass="btn btn-primary" OnClick="btnSave_Click" formnovalidate />
            </div>
        </div>
    </div>
</asp:Content>
