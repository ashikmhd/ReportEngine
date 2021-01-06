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
    public partial class Authentication : CommonBaseClass
    {
        
        BaseRequest Base = new BaseRequest();
        protected void Page_Load(object sender, EventArgs e)
        {           
            Base = new BaseRequest();

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
       public void CreateCache()
        {
            try
            {
                UserComponent UComp = Authenticate(0, txtusername.Text, txtpassword.Text);

                if (UComp.User.Errorstatus == true)
                {
                    txtusername.Focus();
                    General G = new General();
                    lblmsg.Text = G.Error(UComp.User.ErrorRemarks);
                    return;
                }

                CreateBaseCache(UComp);

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
       

        
       
    }

}