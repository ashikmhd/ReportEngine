using BuisnessLayer.Master;
using BusinessLayer.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportEngine
{
    public partial class Authentication : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ipAddress"] != null)
            {
                hdnClientIpAddress.Value = Request.QueryString["ipAddress"].ToString();
            }

            if (Page.IsPostBack == false)
            {
                txtusername.Focus();


                if ((Request.Browser.Type.ToUpper().Contains("FIREFOX") || Request.Browser.Type.ToUpper().Contains("SAFARI") || (Request.Browser.Type.ToUpper().Contains("CHROME") && !Request.UserAgent.ToUpper().Contains("EDGE"))) && Request.Browser.EcmaScriptVersion.Major >= 1)
                {

                }
                else
                {
                    General G = new General();
                    lblmsg.Text = G.Error("Combatible with only Firefox ,Safari & Chrome Browsers.");
                    txtusername.Visible = false;
                    txtpassword.Visible = false;
                    btnlogin.Visible = false;
                }



                if (((BaseRequest)Session["cache"]) != null)
                {
                    if (Request.QueryString["page"] == "lo")
                    {
                        ClsUserLoginDetails Objlogindtls = new ClsUserLoginDetails();
                        Objlogindtls.LogId_ID = ((BaseRequest)Session["cache"]).LoggedID;
                        if (Objlogindtls.LogId_ID != 0) Objlogindtls.UpdateLogOut();
                    }
                }
                Session.Abandon();
                Session.RemoveAll();
                Session.Clear();
                HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(false);

            }
        }

        protected void btnlogin_Click(object sender, EventArgs e)
        {
            try
            {
                CreateCache();
            }
            catch (Exception ex)
            {
                General G = new General();
                lblmsg.Text = G.Error(ex.Message);
            }
        }
        void CreateCache()
        {
            try
            {
                Session["cache"] = null;
                BaseRequest Base = new BaseRequest();
                Base.LogindateTime = DateTime.Now;
                UserComponent UComp = new UserComponent();
                UComp.User = new UserInfo();
                UComp.User.uid = txtusername.Text;
                UComp.User.pwd = txtpassword.Text;
                UComp.User.UserLoginDetails = new ClsUserLoginDetails();
                UComp.User.UserLoginDetails.IPAddress_VC = GetUser_IP();
                //obj.UserComponent.User.UserLoginDetails.UserLoginAttemptCompName_VC = GetComputerName(obj.UserComponent.User.UserLoginDetails.UserLoginAttemptFromIP_VC);
                UComp.User.UserLoginDetails.BrowserDtls_VC = GetBrowserDtls();
                UComp.User.Authentication();
                if (UComp.User.Errorstatus == true)
                {
                    txtusername.Focus();
                    General G = new General();
                    lblmsg.Text = G.Error(UComp.User.ErrorRemarks);
                    return;
                }
                Base.LoggedID = UComp.User.UserLoginDetails.LogId_ID;
                Base.BaseUserfirstName = UComp.User.fname;
                Base.BaseUserName = UComp.User.uid;
                Base.BaseUserId = UComp.User.id;
                Base.RoleID = UComp.User.RoleID;
                Base.BaseCompany = "Health Club";
                Base.BaseOutletAddress = "Address 1 ,Address 2";
                Base.BaseEmail = "HealthClub@health.com";
                Base.PermissedMenus = UComp.User.DT_PermissedMenus;
                Base.BaseRoleName = UComp.User.RoleName;

                Base.BaseOutletId = UComp.User.OutletId;
                Base.BaseOutletName = UComp.User.OutletName;
                Base.BaseOutletAddress = UComp.User.OutletAddress;
                Base.BaseOutletPOBox = UComp.User.OutletPOBox;
                Base.BaseOutletEmail = UComp.User.OutletEmail;
                Base.BaseOutletPhone = UComp.User.OutletPhone;
                Base.BaseOutletMobile = UComp.User.OutletMobile;
                Base.BaseOutletWebSite = UComp.User.OutletWebSite;
                Base.BaseOutletHeaderImage = UComp.User.OutletHeaderImage;
                Base.dtAccessableOutlets = UComp.User.dtAccessableOutlets;
                Base.OutletOracleOrgCode = UComp.User.OutletOracleOrgCode;
                Base.OutletOracleInvCode = UComp.User.OutletOracleInvCode;
                Base.BaseOutletRenewMin = UComp.User.OutletRenewMin;
                Base.BaseOutletRenewMax = UComp.User.OutletRenewMax;

                Session["cache"] = Base;
                Session["UNAME"] = Base.BaseUserName;

                if (UComp.User.isnew == true)
                {
                    Base.FirstLogin = "1";
                    Response.Redirect("~/Masters/User/ChangePassword.aspx?type=new");
                }
                else
                { Response.Redirect("~/Main.aspx"); }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected string GetUser_IP()
        {
            string ipaddress;
            ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ipaddress == "" || ipaddress == null)
                ipaddress = Request.ServerVariables["REMOTE_ADDR"];
            return ipaddress;
        }
        protected string GetUser_ComputerName()
        {
            string comname = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_HOST"]).HostName;
            return comname;
        }
        public string GetComputerName(string clientIP)
        {
            try
            {
                var hostEntry = System.Net.Dns.GetHostEntry(clientIP);
                return hostEntry.HostName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        String GetBrowserDtls()
        {
            HttpBrowserCapabilities browser = Request.Browser;
            string Brwsr = ""
            + "Type = " + browser.Type + "\n"
            + "Name = " + browser.Browser + "\n"
            + "Version = " + browser.Version + "\n"
            //+ "Major Version = " + browser.MajorVersion + "\n"
            //+ "Minor Version = " + browser.MinorVersion + "\n"
            + "Platform = " + browser.Platform + "\n";
            //+ "Is Beta = " + browser.Beta + "\n"
            //+ "Is Crawler = " + browser.Crawler + "\n"
            //+ "Is AOL = " + browser.AOL + "\n"
            //+ "Is Win16 = " + browser.Win16 + "\n"
            //+ "Is Win32 = " + browser.Win32 + "\n"
            //+ "Supports Frames = " + browser.Frames + "\n"
            //+ "Supports Tables = " + browser.Tables + "\n"
            //+ "Supports Cookies = " + browser.Cookies + "\n"
            //+ "Supports VBScript = " + browser.VBScript + "\n"
            //+ "Supports JavaScript = " +
            //    browser.EcmaScriptVersion.ToString() + "\n"
            //+ "Supports Java Applets = " + browser.JavaApplets + "\n"
            //+ "Supports ActiveX Controls = " + browser.ActiveXControls
            //      + "\n"
            //+ "Supports JavaScript Version = " +
            //    browser["JavaScriptVersion"] + "\n";
            return Brwsr;
        }
    }

}