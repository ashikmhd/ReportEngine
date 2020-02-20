<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="ReportEngine.ForgotPassword"  UnobtrusiveValidationMode="None" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <style type="text/css">
         .style1 {
             color: #FF0000;
             text-align: center;
             font-size: large;
         }

         .style2 {
             color: #009900;
         }

         .style3 {
             color: #FF6600;
         }

         .style4 {
             text-align: center;
             font-size: x-large;
             color: #FF0000;
         }

         .style5 {
             color: #FF0000;
         }

         .btncls {
             background-color: #000000;
             color: Orange;
         }

         .style6 {
             background-color: #000000;
             color: Orange;
             font-size: x-large;
         }

         .style7 {
             font-size: large;
         }
     </style>
</head>
<body>
    <form id="form1" runat="server">
       <div>

        <table width="100%" >
            <tr>
                <td align="left">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/BNSLogo_New.png" Height="111px"
                        Width="201px" />
                </td>
                <td align="right" style="padding-top:15px;">
                    <asp:Image ID="Image3" runat="server" ImageUrl="~/images/TH_LogoNew.png" />
                
                </td>
            </tr>
            <tr style="width:100%">
                <td colspan="2" class="style6" align="center">
                    <strong>Body & Soul Health Club &amp; SPA </strong>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="padding-top:30px;padding-bottom:40px">
                    &nbsp;
                    <asp:Image ID="Image2" runat="server" Height="56px" ImageUrl="~/images/unlock-icon.png"
                        Style="text-align: center" Width="63px" />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="style1" style="padding-top:10px;padding-bottom:10px">
                    <strong>Did You Forgot Your Password? </strong>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Label ID="lblmsg" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style3" align="right" width="50%" style="padding-top:10px;padding-bottom:20px" >
                    <strong>Please Enter Your EmailID : </strong>
                </td>
                <td width="50%">
                    <asp:TextBox ID="txtEmail" runat="server" Style="color: #FF6600; width: 200px;"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*"
                        ControlToValidate="txtEmail"></asp:RequiredFieldValidator>
                    <span class="style5">*</span>
                </td>
            </tr>
            <tr>
                <td align="right" style="padding-right: 5px;">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" BackColor="#FF6600" Font-Bold="True"
                        ForeColor="White" Width="85px" OnClick="btnsubmit_Click" />
                </td>
                <td align="left" style="padding-left: 5px; padding-bottom:10px;">
                    <asp:Button ID="btnclear" runat="server" BackColor="#FF6600" Font-Bold="True" ForeColor="White"
                        Width="85px" Text="Clear" OnClientClick="btnclear" OnClick="btnclear_Click" />
                        &nbsp;
                         <asp:Button ID="btnBack" runat="server" BackColor="#FF6600" CausesValidation="false"
                        Font-Bold="True" ForeColor="White"
                        Width="120px" Text="Back To Login" onclick="btnBack_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="style4" bgcolor="#A8A8A8" ">
                    <span class="style7"><strong>Please Follow the Below Instructions to Reset your Password:</strong></span>
                    <strong>&nbsp;</strong>
                </td>
            </tr>
            <tr >
                <td colspan="2" class="style2">
                    <ul>
                        <li>Please enter the EmailID which you Provided for User Creation.</li>
                        <li>Password will be send automatically to your Email ID.</li>
                        <li>Please use the New password for login to the system.</li>
                        <li>If you are not getting any auto email, Please contact Mr. Bajesh/Mr. Ashik for the support.</li>
                    </ul>
                   
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    © All Rights Reserved <a href="http://www.thumbaytechnologies.com/" target="_blank">
                        - Thumbay Technologies</a>
                </td>
            </tr>
        </table>

    </div>
    </form>
</body>
</html>
