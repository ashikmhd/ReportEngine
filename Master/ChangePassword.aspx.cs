using BuisnessLayer.Master;
using BusinessLayer.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportEngine.Master
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        UserInfo usrinf = new UserInfo();
        int dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                txtuid.Text = ((BaseRequest)Session["cache"]).BaseUserName;
                General G = new General();
                banner.InnerHtml = G.InformationBanner("Password must contain Password must contain&nbsp; atlest one Aplhabets,Numerics and Special characters(!,@,#,$,%,&amp;,*)");
            }
        }
        protected void btnsave_Click(object sender, EventArgs e)
        {
            dt = usrinf.Changepassword(((BaseRequest)Session["cache"]).BaseUserId, txtold.Text, txtnew.Text);
            if (dt == 1)
            {
                lblstatus.Text = "Successfully Changed";
                Response.Redirect("~/Main.aspx");
            }
        }
    }
}