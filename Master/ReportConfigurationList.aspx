<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageFile.master" AutoEventWireup="true" CodeBehind="ReportConfigurationList.aspx.cs" Inherits="ReportEngine.Master.ReportConfigurationList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function pageLoad() {
            $('.decimal_left').inputmask('decimal', { rightAlign: false });

            $('[id*=lstControlProperties]').multiselect({
                includeSelectAllOption: true
            });

            /*Remove this class for modal popup conflict*/
            $('#headerDiv').removeClass('fadeInRight');

            GridDisplay('<%= gvReportDetails.ClientID %>', 'Report List');
        }

        function openModelMapWindow(ReportId, Flag) {
            var lbl = "Outlet Map";
            var link = "ReportOutletMap.aspx?From=modal&ReportId=" + ReportId;

            if (Flag == 1) {
                lbl = "User Map";
                link = "ReportUserMap.aspx?From=modal&ReportId=" + ReportId;
            }

            $('[id*=lblModalTitle]').text(lbl);
            $('[id*=iFrMap]').attr('src', link);

            $('#modal-ForMapping').modal('show');

        }

        function closeModelMapWindow() {
            $('#modal-ForMapping').modal('hide');
        }
    </script>

    <div class="box">
        <asp:HiddenField ID="hdnReportId" runat="server" Value="0" />
        <div class="box-header">
            <h3 class="box-title">
                <asp:Label ID="Label2" runat="server" Text="Report List"></asp:Label></h3>
            <a href="ReportConfiguration.aspx" class="btn btn-primary pull-right"><i class="fa fa-plus">AddNew</i></a>
            <%--<a href="../Reports/ReportSelect.aspx" class="btn btn-primary pull-right">Render Reports</a>--%>
        </div>
        <div class="box-body">
            <div class="row padding5" style="width:auto; ">
                <asp:GridView ID="gvReportDetails" runat="server" CssClass="table table-striped table-bordered table-hover"
                    AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl. No">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--<asp:BoundField DataField="pki_ReportId" HeaderText="Report Id" ItemStyle-Width="5%"  />--%>
                        <asp:BoundField DataField="TypeName" HeaderText="Type"  ItemStyle-Width="5%"  />
                        <asp:BoundField DataField="vc_ReportName" HeaderText="Report Name"  ItemStyle-Width="5%" />
                        <asp:BoundField DataField="vc_ReportDescription" HeaderText="Report Description"  ItemStyle-Width="5%"  />
                        <asp:BoundField DataField="vc_ReportQuery" HeaderText="Select Query"  ItemStyle-Width="20%" />
                        <asp:BoundField DataField="vc_InsertQuery" HeaderText="Insert Query"  ItemStyle-Width="20%" />
                        <asp:BoundField DataField="vc_UpdateQuery" HeaderText="Update Query"  ItemStyle-Width="20%" />
                        <%--<asp:BoundField DataField="IsActiveText" HeaderText="Active"/>--%>
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                  <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("IsActiveText") %>' BackColor= '<%# System.Drawing.ColorTranslator.FromHtml(Eval("IsActiveBgColor").ToString())%>'> </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <a href="#" onclick="<%# "javascript:return Redirect('ReportConfiguration.aspx?ReportId=" + Eval("pki_ReportId") + "')" %>" title="Edit">
                                    <i class="fa fa-edit"></i></a>
                                 <a href="#" onclick="javascript:openModelMapWindow('<%# Eval("pki_ReportId") %>',0)" title="Map Outlet">
                                    <i class="fa fa-briefcase"></i></a>
                                <a href="#" onclick="javascript:openModelMapWindow('<%# Eval("pki_ReportId") %>',1)" title="Map User">
                                    <i class="fa fa-users"></i></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>

        </div>
    </div>
    <div class="modal fade" id="modal-ForMapping">
        <div class="modal-dialog modal-lg" style="width: 70%;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title">
                        <asp:Label ID="lblModalTitle" runat="server" Text="Label"></asp:Label></h4>
                </div>
                <div class="modal-body" style="height: 400px;">
                    <iframe id="iFrMap" style="border: 0px;" src=""
                        width="100%" height="100%"></iframe>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
