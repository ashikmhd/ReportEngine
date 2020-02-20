<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageFile.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="ReportEngine.Master.ChangePassword" UnobtrusiveValidationMode="None" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <script language="javascript" type="text/javascript">
       function Validate() {
           var txtValue = document.getElementById('<%=txtnew.ClientID%>').value;
    var txtoldValue = document.getElementById('<%=txtold.ClientID%>').value;
    var txtreType = document.getElementById('<%=txtretype.ClientID%>').value;

    if (txtValue == txtoldValue) {

        alert("New password and old password should not be same.");
        return false;
    }

    if (txtValue != txtreType) {

        alert("Password mismatch.");
        return false;
    }




    if (!ValidatePasswordPolicyPH(txtValue)) {
        document.getElementById('<%=txtnew.ClientID%>').focus();
               return false;
           }
       }
</script>
<div class="box">
<div class="box-header">
 <h3 class="box-title"><asp:Label ID="Label2" runat="server" Text="Change Password"></asp:Label></h3>

</div>
<div class="box-body">
  <table class="table table-striped">
                                <tr>
                                    <td colspan="4" style="height: 21px; text-align: center; color: #FF3300;">
                                        <strong id="banner" runat="server"></strong></td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="height: 21px; text-align: center">
                                        <asp:Label ID="lblstatus" runat="server" ForeColor="#C04000" 
                                            EnableViewState="False"></asp:Label></td>
                                </tr>
                                <tr style="color: #000000">
                                    <td style="width: 159px">
                                        User Name</td>
                                    <td style="width: 13px">
                                        :</td>
                                    <td style="width: 167px">
                                        <asp:TextBox ID="txtuid" runat="server" CssClass="form-control" MaxLength="25" Width="180px"></asp:TextBox></td>
                                    <td style="width: 176px">
                                    </td>
                                </tr>
                                <tr style="color: #000000">
                                    <td style="width: 159px">
                                        Old Password</td>
                                    <td style="width: 13px">
                                        :</td>
                                    <td style="width: 167px">
                                        <asp:TextBox ID="txtold" runat="server" CssClass="form-control" MaxLength="25" TextMode="Password"
                                            Width="180px"></asp:TextBox></td>
                                    <td style="width: 176px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtold"
                                            ErrorMessage="Old Password"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr style="color: #000000">
                                    <td style="width: 159px">
                                        New Passord</td>
                                    <td style="width: 13px">
                                        :</td>
                                    <td style="width: 167px">
                                        <asp:TextBox ID="txtnew" runat="server" CssClass="form-control" MaxLength="25" TextMode="Password"
                                            Width="180px"></asp:TextBox></td>
                                    <td style="width: 176px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtnew"
                                            ErrorMessage="New Password"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td style="width: 159px; height: 24px">
                                        Re-type new password</td>
                                    <td style="width: 13px; height: 24px">
                                        :</td>
                                    <td style="width: 167px; height: 24px">
                                        <asp:TextBox ID="txtretype" runat="server" CssClass="form-control" MaxLength="25" TextMode="Password"
                                            Width="180px"></asp:TextBox></td>
                                    <td style="width: 176px; height: 24px">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtretype"
                                            ErrorMessage="Re-type new password"></asp:RequiredFieldValidator></td>
                                </tr>
                                <tr>
                                    <td style="width: 159px; height: 25px">
                                    </td>
                                    <td style="width: 13px; height: 25px">
                                    </td>
                                    <td style="width: 167px; height: 25px">
                                        <asp:Button ID="btnsave" runat="server" CssClass="btn btn-primary" OnClick="btnsave_Click"  
                                            OnClientClick= "return Validate()" Text="  Save  " />
                                    </td>
                                    <td style="width: 176px; height: 25px">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4" style="height: 25px">
                                        <asp:CompareValidator ID="CompareValidator1" runat="server" ControlToCompare="txtnew"
                                            ControlToValidate="txtretype" ErrorMessage="Password mismatching..."></asp:CompareValidator></td>
                                </tr>
                            </table>
                    </div>
</asp:Content>
