using BuisnessLayer.General;
using BuisnessLayer.Master;
using BusinessLayer.General;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportEngine
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitializePage();
            }
        }
        private void InitializePage()
        {
            txtEmail.Text = "";
        }
        protected void btnsubmit_Click(object sender, EventArgs e)
        {

            try
            {
                Common _com = new Common();
                string RandomPwd = _com.generateRandomValue();

                UserInfo usrinf = new UserInfo();
                DataTable dtUser = usrinf.ForgotPassword(0, txtEmail.Text.Trim(), RandomPwd);

                if (dtUser.Rows.Count > 0)
                {
                    //string EmailHost = ConfigurationManager.AppSettings["EmailHost"].ToString();
                    //string EmailPort = ConfigurationManager.AppSettings["EmailPort"].ToString();
                    //string EmailUsername = ConfigurationManager.AppSettings["EmailUsername"].ToString();
                    //string EmailPwd = ConfigurationManager.AppSettings["EmailPwd"].ToString();
                    //bool EmailSsl = bool.Parse(System.Configuration.ConfigurationManager.AppSettings["EmailSsl"].ToString());

                    string host = HttpContext.Current.Request.Url.Host;
                    string port = HttpContext.Current.Request.Url.Port.ToString();
                    if (host.ToLower() == "www.gmulive.ac.ae" && port == "80")
                        port = "6060";
                    string AppHost = HttpContext.Current.Request.Url.Scheme + "://" + host;
                    if (port != "")
                        AppHost += ":" + port;
                    AppHost += "/" + HttpContext.Current.Request.Url.Segments[1];

                    string subject = "Your New Password For " + ConfigurationManager.AppSettings["heading"].ToString();
                    string body = "<html><table border='1' style='width:100%'>";
                    body += "<tr><td align='center'><img src='http://www.bodyandsoulhealthclub.com/wp-content/uploads/2016/05/bns.png'  height='96px' width='165px'/></td></tr>";
                    body += "<tr><td align='center' style='background-color:#dcdcdc;font-size:20pt'><b> " + ConfigurationManager.AppSettings["heading"].ToString() + "</b></td></tr>";
                    body += "<tr><td align='left' style='font-size:14pt'> Your UserName is: <b>" + dtUser.Rows[0]["vc_UserName"] + "</b></td></tr>";
                    body += "<tr><td align='left' style='font-size:14pt'> Your New Password is: <b>" + RandomPwd + "</b></td></tr>";
                    body += "<tr><td align='left' style='font-size:12pt'> Link: " + AppHost + "</td></tr>";
                    body += "</table></html>";

                    Email _mail = new Email();
                    string mailResult = _mail.SendMail(true, txtEmail.Text, "", body, subject, ConfigurationManager.AppSettings["heading"].ToString());

                    if (mailResult == "1")
                    {
                        lblmsg.Text = "Password has been sent to your email address: " + txtEmail.Text;
                        InitializePage();
                    }
                    else
                        lblmsg.Text = "Error While Saving: " + mailResult;
                }
                else
                {
                    lblmsg.Text = "This email address does not match with our records";
                }
            }
            catch (Exception ex)
            {

                lblmsg.Text = "Error While Saving: " + ex.Message;
            }

        }

        protected void btnclear_Click(object sender, EventArgs e)
        {
            InitializePage();
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Authentication.aspx");
        }
    }
}