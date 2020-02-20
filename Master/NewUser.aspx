<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageFile.Master" AutoEventWireup="true" CodeBehind="NewUser.aspx.cs" Inherits="ReportEngine.Master.NewUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script language="javascript" type="text/javascript">
        function valdata() {
            var fname = document.getElementById('<%=txtfname.ClientID%>');
            var lname = document.getElementById('<%=txtlname.ClientID%>');
            var uid = document.getElementById('<%=txtuid.ClientID%>');
            var pwd = document.getElementById('<%=txtpwd.ClientID%>');
            var cempid = document.getElementById('<%=txtEmpId.ClientID%>');
            var ddlRole = document.getElementById('<%=ddlRole.ClientID%>');
            if (Trim(fname.value) == '') {
                alert('Specify First Name');
                fname.focus();
                return false;
            }
            if (Trim(lname.value) == '') {
                alert('Specify Last Name');
                lname.focus();
                return false;
            }
            if (Trim(uid.value) == '') {
                alert('Specify User Name');
                uid.focus();
                return false;
            }
            if (Trim(pwd.value) == '') {
                alert('Specify Password');
                pwd.focus();
                return false;
            }


            if (!ValidatePasswordPolicyPH(pwd.value)) {
                pwd.focus();
                return false;
            }

            if (!IsNumericWODec("Specify employee Id and it must be numeric.", cempid)) {
                cempid.focus();
                return false;
            }
            if (ddlRole.value == 0) {
                alert('Please Select Role');
                ddlRole.focus();
            }
        }

        function Trim(val) {
            var temp = val;
            for (var i = 0; i < temp.length; i++) {
                val = val.replace(' ', '');
            }

            return val;
        }





        function deact(user) {
            var res = confirm('Are you sure to deactivate ' + user.value);
            if (!res) {
                return false;
            }
            else {
                return true;
            }
        }
    </script>
    <script type="text/javascript">

</script>
    <div class="box">
        <div class="box-header">
            <h3 class="box-title">
                <asp:Label ID="Label2" runat="server" Text="NEW USER CREATION"></asp:Label></h3>
        </div>
        <div class="box-body">
            <asp:Label ID="lblinfo" runat="server"></asp:Label>
            <asp:Label ID="lblmsg" runat="server" EnableViewState="False" ForeColor="Red"></asp:Label>

            <div class="col-md-6 form-group">
                <label class="control-label">First Name </label>
                <asp:TextBox ID="txtfname" placeholder="Enter First Name" runat="server" CssClass="form-control" required></asp:TextBox>
            </div>
            <div class="col-md-6 form-group">
                <label class="control-label">Last Name</label>
                <asp:TextBox ID="txtlname" placeholder="Enter Last Name" runat="server" CssClass="form-control" required></asp:TextBox>
            </div>

            <div class="clearfix"></div>

            <div class="col-md-6 form-group">
                <label class="control-label">User Name </label>
                <asp:TextBox ID="txtuid" placeholder="Enter User Name" runat="server" CssClass="form-control" required></asp:TextBox>
            </div>
            <div class="col-md-6 form-group">
                <label class="control-label">Email Id</label>
                <asp:TextBox ID="txtEmail" placeholder="bajesh@it.thumbay.com" runat="server" CssClass="form-control" required></asp:TextBox>
            </div>

            <div class="clearfix"></div>

            <div class="col-md-6 form-group">
                <label class="control-label">Password</label>
                <asp:TextBox ID="txtpwd" runat="server" CssClass="form-control" TextMode="Password" required></asp:TextBox>
            </div>
            <div class="col-md-6 form-group">
                <label class="control-label">Status</label>
                <asp:DropDownList ID="ddlstatus" runat="server" CssClass="form-control">
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="0">InActive</asp:ListItem>
                </asp:DropDownList>
            </div>

            <div class="clearfix"></div>

            <div class="col-md-6 form-group">
                <label class="control-label">Employee ID</label>
                <asp:TextBox ID="txtEmpId" placeholder="Enter EmployeeID" runat="server" CssClass="form-control" required></asp:TextBox>
            </div>
            <div class="col-md-6 form-group">
                <label class="control-label">Role</label>
                <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" required="0">
                </asp:DropDownList>
            </div>


            <table class="table table-bordered ng-table-responsive">
                <tr align="center">
                    <td>
                        <asp:Button ID="btnsave" runat="server" CssClass="btn btn-primary" Text="Save" OnClick="btnsave_Click" />&nbsp;
                        <asp:Button ID="btnclear" runat="server" CssClass="btn btn-primary" Text="Clear"
                            OnClick="btnclear_Click" formnovalidate />
                        <input type="button" value="BackToList" onclick="Redirect('UserList.aspx');" class="btn btn-primary" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
