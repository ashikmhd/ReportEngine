<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageFile.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="ReportEngine.Master.UserList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function valsearch() {
            var search = document.getElementById('<%=txtsearch.ClientID%>');

            if (Trim(search.value) == '') {
                alert('Search must not be blank');
                search.focus();
                return false;
            }
        }
        function openMapping(userid) {
            return Redirect("USEROutletMapping.aspx?uid=" + userid);
        }
        function openClubUserMap(userid) {
            return Redirect("UserClubUserMapping.aspx?uid=" + userid);
        }
    </script>
    <div class="box">
        <div class="box-header">
            <h3 class="box-title">
                <asp:Label ID="Label1" runat="server" Text="USER DETAILS"></asp:Label>
            </h3>
            <a href="NewUser.aspx" class="btn-sm btn-primary AddNew"><i class="fa fa-plus">AddNew</i></a>
        </div>
        <div class="box-body">
            <asp:Label ID="lblmsg" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>
            <div class="row">
                <div class="col-sm-3">
                    <asp:DropDownList ID="drptype" runat="server" CssClass="form-control">
                        <asp:ListItem Selected="True" Value="All">All</asp:ListItem>
                        <asp:ListItem Value="0">User Name</asp:ListItem>
                        <asp:ListItem Value="1">First Name</asp:ListItem>
                        <asp:ListItem Value="2">Last Name</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-3">
                    <asp:TextBox ID="txtsearch" runat="server" CssClass="form-control"></asp:TextBox></div>
                <div class="col-sm-2">
                    <asp:Button ID="btnsearch" runat="server" CssClass="btn btn-primary" OnClientClick="return valsearch();"
                        Text="Search" OnClick="btnsearch_Click" formnovalidate /></div>
            </div>
            <table class="table table-bordered ng-table-responsive">
                <tr>
                    <td>
                        <asp:GridView ID="grduser" runat="server" AutoGenerateColumns="False" class="table table-striped table-bordered table-hover">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl. No">
                                    <ItemTemplate>
                                        <asp:Label ID="lblslno" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="First Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblfname" runat="server" Text='<%# Bind("vc_FirstName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Last Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbllname" runat="server" Text='<%#Bind("vc_LastName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="User Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbluid" runat="server" Text='<%# Bind("vc_UserName") %>'></asp:Label>
                                        <asp:HiddenField ID="hdnuid" runat="server" Value='<%# Bind("vc_UserName") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Role Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRole" runat="server" Text='<%# Bind("vc_RoleName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Empl Id">
                                    <ItemTemplate>
                                        <asp:Label ID="lbleid" runat="server" Text='<%# Bind("i_EmployeeID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lblstatus" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <a href="#" onclick="<%# "javascript:return Redirect('NewUser.aspx?uid=" + Eval("pki_UserId") + "')" %>">
                                            <i class="fa fa-edit"></i></a>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:TemplateField HeaderText="Map Outlet">
                                    <ItemTemplate>
                                        <%--<%--<a href="#" title="Map" class="btn btn-primary" onclick="<%# "javascript:return Redirect('USEROutletMapping.aspx?uid=" + Eval("pki_UserId") + "')" %>">
                                            Map</a>--%>
                                        <%--<asp:Button ID="btnMap" runat="server" Text="Map Outlet" CssClass="btn btn-primary btn-sm" OnClientClick='<%# Eval("pki_UserId", "return openMapping(\"{0}\"); return false;") %>' Visible='<%# Convert.ToBoolean(Eval("b_IsActive")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                  <asp:TemplateField HeaderText="Map ClubUser">
                                    <ItemTemplate>
                                        <asp:Button ID="btnClubMap" runat="server" Text="Map Clubuser" CssClass="btn btn-primary btn-sm" OnClientClick='<%# Eval("pki_UserId", "return openClubUserMap(\"{0}\"); return false;") %>' Visible='<%# Convert.ToBoolean(Eval("b_IsActive")) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                            <EmptyDataTemplate>
                                No Data Found!
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <script src='<%=ResolveUrl("~/bower_components/metisMenu/jquery.metisMenu.js") %>'
        type="text/javascript"></script>
    <script src='<%=ResolveUrl("~/bower_components/slimscroll/jquery.slimscroll.min.js") %>'
        type="text/javascript"></script>
    <script type="text/javascript">

        //        $(document).ready(function () {
        function pageLoad() {
            var ID = '<%= grduser.ClientID %>';
            GridDisplay(ID)
        }
    </script>
</asp:Content>
