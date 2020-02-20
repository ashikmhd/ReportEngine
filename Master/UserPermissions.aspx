<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageFile.Master" AutoEventWireup="true" CodeBehind="UserPermissions.aspx.cs" Inherits="ReportEngine.Master.UserPermissions" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        function OnTreeClick(evt) {
            var src = window.event != window.undefined ? window.event.srcElement : evt.target;
            var isChkBoxClick = (src.tagName.toLowerCase() == "input" && src.type == "checkbox");
            if (isChkBoxClick) {
                var parentTable = GetParentByTagName("table", src);
                var nxtSibling = parentTable.nextSibling;
                if (nxtSibling && nxtSibling.nodeType == 1)//check if nxt sibling is not null & is an element node
                {
                    if (nxtSibling.tagName.toLowerCase() == "div") //if node has children
                    {
                        //check or uncheck children at all levels
                        CheckUncheckChildren(parentTable.nextSibling, src.checked);
                    }
                }
                //check or uncheck parents at all levels
                CheckUncheckParents(src, src.checked);
            }
        }

        function CheckUncheckChildren(childContainer, check) {
            var childChkBoxes = childContainer.getElementsByTagName("input");
            var childChkBoxCount = childChkBoxes.length;
            for (var i = 0; i < childChkBoxCount; i++) {
                childChkBoxes[i].checked = check;
            }
        }

        function CheckUncheckParents(srcChild, check) {
            var parentDiv = GetParentByTagName("div", srcChild);
            var parentNodeTable = parentDiv.previousSibling;

            if (parentNodeTable) {
                var checkUncheckSwitch;

                if (check) //checkbox checked
                {
                    var isAllSiblingsChecked = AreAllSiblingsChecked(srcChild);
                    if (isAllSiblingsChecked)
                        checkUncheckSwitch = true;
                    else
                        checkUncheckSwitch = true; //do not need to check parent if any(one or more) child not checked
                }
                else //checkbox unchecked
                {
                    checkUncheckSwitch = true;
                }

                var inpElemsInParentTable = parentNodeTable.getElementsByTagName("input");
                if (inpElemsInParentTable.length > 0) {
                    var parentNodeChkBox = inpElemsInParentTable[0];
                    parentNodeChkBox.checked = checkUncheckSwitch;
                    //do the same recursively
                    CheckUncheckParents(parentNodeChkBox, checkUncheckSwitch);
                }
            }
        }

        function AreAllSiblingsChecked(chkBox) {
            var parentDiv = GetParentByTagName("div", chkBox);
            var childCount = parentDiv.childNodes.length;
            for (var i = 0; i < childCount; i++) {
                if (parentDiv.childNodes[i].nodeType == 1) //check if the child node is an element node
                {
                    if (parentDiv.childNodes[i].tagName.toLowerCase() == "table") {
                        var prevChkBox = parentDiv.childNodes[i].getElementsByTagName("input")[0];
                        //if any of sibling nodes are not checked, return false
                        if (!prevChkBox.checked) {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        //utility function to get the container of an element by tagname
        function GetParentByTagName(parentTagName, childElementObj) {
            var parent = childElementObj.parentNode;
            while (parent.tagName.toLowerCase() != parentTagName.toLowerCase()) {
                parent = parent.parentNode;
            }
            return parent;
        }

    </script>
    <div class="box ">
        <div class="box-header">
            <h3 class="box-title">User Rights</h3>
        </div>
        <div class="box-body">
            <asp:Literal ID="ltrlmsg" runat="server"></asp:Literal>
            <table class="table table-bordered">
                <tr>
                    <td>RoleName</td>
                    <td>
                        <asp:TextBox ID="txtUserRole" runat="server" CssClass="form-control"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSaveRole" runat="server" Text="Save Role"
                            onclick="btnSaveRole_Click" CssClass="btn btn-primary" />
                    </td>
                </tr>
                <tr>
                    <td>User Role</td>
                    <td>
                        <asp:DropDownList ID="ddlRole" runat="server" AutoPostBack="True" CssClass="form-control"
                            onselectedindexchanged="ddlRole_SelectedIndexChanged">
                        </asp:DropDownList>&nbsp;
                            &nbsp; 
                    </td>
                </tr>
                <tr>
                    <td>Module</td>
                    <td>
                        <asp:DropDownList ID="ddlModule" runat="server" AutoPostBack="True" CssClass="form-control"
                            onselectedindexchanged="ddlModule_SelectedIndexChanged">
                            <asp:ListItem Value="0">Select</asp:ListItem>
                            <asp:ListItem Value="W">Windows</asp:ListItem>
                            <asp:ListItem Value="PH">Web</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table class="table table-bordered">
                <tr>
                    <td>

                        <div class="box box-solid" style="overflow: auto; height: 200px;">
                            <table>
                                <tr>
                                    <td>
                                        <asp:TreeView ID="TreeMenus" runat="server"
                                            ShowCheckBoxes="All" AutoPostBack="true" onclick="OnTreeClick(event)"
                                            onselectednodechanged="TreeMenus_SelectedNodeChanged">
                                        </asp:TreeView>
                                    </td>
                                </tr>
                            </table>

                        </div>
                        <div>
                            <asp:Button ID="btnSavePermission" runat="server"
                                onclick="btnSavePermission_Click" Text="Save Permission" CssClass="btn btn-primary" OnClientClick="javascript:return confirm('Do You Want to Proceed');" />
                        </div>
                    </td>
                </tr>
            </table>


            <div class="col-md-4" id="popup">
                <!-- Success box -->
                <div class="box box-solid box-success">
                    <div class="box-header">
                        <h3 class="box-title">Menu Creation</h3>
                        <div class="box-tools pull-right">
                            <button class="btn btn-success btn-sm" data-widget="collapse"><i class="fa fa-minus"></i></button>
                            <button class="btn btn-success btn-sm" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>
                    <div>




                        <table class="table table-bordered">
                            <tr>
                                <td>Menu Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMenuName" runat="server" CssClass="form-control"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Menu Path
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPath" runat="server" CssClass="form-control"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Child Menu Name
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChildMenuName" runat="server" CssClass="form-control"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td>Chile Menu Path</td>
                                <td>
                                    <asp:TextBox ID="txtChildPath" runat="server" CssClass="form-control"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary " onclick="btnSave_Click" />
                                    &nbsp;
                                  <asp:Button ID="btnDelete" runat="server" Text="Delete"
                                      onclick="btnDelete_Click" CssClass="btn btn-primary" />
                                    <asp:HiddenField ID="hdnFileID" runat="server" />
                                    <asp:HiddenField ID="hdnScreen" runat="server" />
                                    <asp:HiddenField ID="hdnParentFileID" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div style="overflow: auto; height: 200px;">
                            <asp:TreeView ID="TreeView1" runat="server" Autopostback="true"
                                onselectednodechanged="TreeView1_SelectedNodeChanged">
                            </asp:TreeView>
                        </div>
                        <asp:HiddenField ID="hdntarget" runat="server" />




                    </div>
                    <!-- /.box-body -->
                </div>
                <!-- /.box -->
            </div>
            <!-- /.col -->
            <asp:ModalPopupExtender ID="md1" runat="server" TargetControlID="hdntarget" PopupControlID="popup">
            </asp:ModalPopupExtender>
        </div>
    </div>
</asp:Content>
