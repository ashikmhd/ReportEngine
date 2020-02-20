<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Authentication.aspx.cs" Inherits="ReportEngine.Authentication" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>HealthClub</title>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Login</title>
    <link rel="icon" href="images/BSLogo.ico" type="image/png">
    <link href="~/../bower_components/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="~/../dist/css/font-awesome.css" rel="stylesheet">
    <link href="~/../dist/css/animate.css" rel="stylesheet">
    <link href="~/../dist/css/style.css" rel="stylesheet">
    <!-- HTML5 Shim and Respond.js IE8 support of HTML5 elements and media queries -->
    <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
    <!--[if lt IE 9]>
  <script src="https://oss.maxcdn.com/html5shiv/3.7.3/html5shiv.min.js"></script>
  <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
  <![endif]-->
    <!-- Google Font -->
    <style type="text/css">
        .style1
        {
            color: #FFCC00;
            font-size: medium;
        }
        .style2
        {
            color: #FF9900;
            font-size: medium;
        }
        .style3
        {
            color: #FFFFFF;
            font-size: small;
        }
    </style>
    
    <script src='<%=ResolveUrl("~/bower_components/jquery/dist/jquery.min.js") %>' type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            if ($("#" + "<%= hdnClientIpAddress.ClientID %>").val() != "")
                localStorage.setItem('clientIpAddress', $("#" + "<%= hdnClientIpAddress.ClientID %>").val());
        });
    </script>
</head>
<body>
    <%--  <asp:Image ID="Image2" align="left" runat="server" ImageUrl="~/images/logo.png" />--%>
    <form id="form1" runat="server">
    <!-- Loading -->
    <%--<section class="loading">
        <div class="loading_wrapper">            
                <img src="Images/thumbay-pharmacy-loader.gif" />            
        </div>
    </section>--%>
    <div class="middle-box text-center loginscreen animated fadeInDown">
        <div>
            <asp:HiddenField ID="hdnClientIpAddress" runat="server" />
            <asp:Label ID="lblmsg" runat="server"></asp:Label>
            <div class="login-logo">
                <a href="http://bodyandsoulHealthClub.com/" title="Body &amp; Soul">
                    <img src="images/BSAnimated.gif" alt="Logo" />
                    <%--  <img src="images/thumbay-pharmacy-loader.gif" />--%>
                </a>
                <%--<asp:Image ID="Image1" align="right" runat="server" ImageUrl="~/images/logo.png" />--%>
            </div>
            <!-- /.login-logo -->
            <div>
                <a href="#"><b style="color: #f57d20;"><span style="font-family: Segoe UI; font-size: 22px;">
                    BODY & SOUL HEALTH CLUB</span></b></a>
            </div>
            <div>
                <marquee behavior="alternate" style="font-size: large; color: #FF0000; text-decoration: underline;"> <a href="Reports/HelpFiles/html_App_SupportFiles/HealthclubManagementSystem.html" target="Help" class="fa-file-pdf-o"> Help File(ClickHere)</a></marquee>
            </div>
            <%-- <div><marquee behavior="scroll" direction="left" scrollamount="3" >
            <a target="newpdf" href="TTechCircular.pdf">
             <img src="images/IMPCIRCULAR.png" width="210" height="60" alt="smile">
            </a>
            </marquee>
            </div>--%>
            <div class="login-box-body">
                <p class="login-box-msg" style="color: #737b35">
                    Sign in to start your session</p>
            </div>
            <div class="form-group has-feedback">
                <asp:TextBox ID="txtusername" runat="server" class="form-control" placeholder="Username"
                    required></asp:TextBox>
                <span class="glyphicon glyphicon-user form-control-feedback"></span>
            </div>
            <div class="form-group has-feedback">
                <asp:TextBox ID="txtpassword" runat="server" class="form-control" TextMode="Password"
                    placeholder="Password" required></asp:TextBox>
                <span class="glyphicon glyphicon-lock form-control-feedback"></span>
            </div>
            <div>
                <asp:Button ID="btnlogin" runat="server" class="btn btn-primary block full-width m-b"
                    Text="Login" OnClick="btnlogin_Click"></asp:Button>
            </div>
            <!-- /.col -->
            <a href="forgotpassword.aspx"><small>Forgot password?</small></a>
            <br />
            <br />
            <div>
                <span class="style1">People Happiness Survey 2018</span> <a href="https://www.surveymonkey.com/r/BodyandSoulHealthClubPHS-2018"
                    target="_blank"><span class="style2">- For Link (Click Here)</span></a>
                <br />
                &nbsp;
            </div>
            <!-- END LOGIN -->
            <!-- BEGIN COPYRIGHT -->
            <div class="copyright">
                © All Rights Reserved <a href="http://www.thumbaytechnologies.com/" target="_blank">
                    - Thumbay Technologies</a>
                <br />
                <asp:Image ID="Image2" Style="padding-top: 50px; opacity: 0.5;" runat="server" ImageUrl="~/images/TH_Logo.png" />
            </div>
            <!-- END COPYRIGHT -->
            <!-- BEGIN JAVASCRIPTS(Load javascripts at bottom, this will reduce page load time) -->
            <%--        <a href="register.html" class="text-center">Register a new membership</a>--%>
        </div>
        <!-- /.login-box-body -->
    </div>
    <!-- /.login-box -->
    </div>
    <!-- Mainly scripts -->
    <script src="~/../bower_components/jquery/dist/jquery.min.js"></script>
    <script src="~/../bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
    </form>
</body>
</html>