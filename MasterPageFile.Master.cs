using BuisnessLayer.Master;
using BusinessLayer.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ReportEngine
{
    public partial class MasterPageFile : System.Web.UI.MasterPage
    {
        UserInfo Objuserinfo = null;
        DataTable PermissedMenus;
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckSessionExpiry();
            if (Page.IsPostBack == false)
            {
                LoadMenus();
                Loggeduser.InnerHtml = ((BaseRequest)Session["cache"]).BaseUserfirstName;
                spanuserrole.InnerHtml = ((BaseRequest)Session["cache"]).BaseRoleName;
                Spanuser.InnerHtml = ((BaseRequest)Session["cache"]).BaseUserfirstName;
                //lblOutlet.Text = ((BaseRequest)Session["cache"]).BaseOutletAddress;
                //puser.InnerHtml = ((BaseRequest)Session["cache"]).BaseUserfirstName;
                //iframrefresh.ResolveUrl("~/Refresh.aspx");
                //iframrefresh.Attributes.Add("src", Page.ResolveUrl("~/Refresh.aspx"));
                //BindOutlets();
            }
        }
        public void CheckSessionExpiry()
        {
            if (((BaseRequest)Session["cache"]) == null) Response.Redirect("~/Authentication.aspx");
        }
        private void LoadMenus(object sender = null, EventArgs e = null)
        {
            try
            {
                HtmlGenericControl UL = new HtmlGenericControl("ul");
                UL.Attributes.Add("class", "sidebar-menu");
                UL.Attributes.Add("data-widget", "tree");
                HtmlGenericControl A;
                Boolean MenuVisible = true;
                if (Request.RawUrl.Contains("ChangePassword.aspx?type=new") == true)//password first time setting 
                { MenuVisible = false; }

                Objuserinfo = new UserInfo();
                PermissedMenus = ((BaseRequest)Session["cache"]).PermissedMenus;
                foreach (DataRow Module in PermissedMenus.Select("ParentID=0"))
                {
                    HtmlGenericControl LI = new HtmlGenericControl("li");
                    LI.Visible = MenuVisible;
                    LI.Attributes.Add("class", "treeview");
                    A = new HtmlGenericControl("a");
                    LI.ID = Module["description"] + "$" + Module["pki_Menuid"].ToString();
                    string Urlparent = "";
                    if (Module["filename"].ToString() != "#")
                    {
                        Urlparent = Page.ResolveUrl(Module["filename"].ToString());
                    }
                    else
                    {
                        Urlparent = "#";
                    }
                    A.Attributes.Add("href", Urlparent);
                    string imgClass = Convert.ToString(Module["imagen"]);
                    if (imgClass == "") { imgClass = "fa fa-edit"; }
                    if (Module["newmenucss"].ToString() != "")
                    {
                        A.InnerHtml = "<i class='" + imgClass + "'></i><span>" + Module["description"] + "</span>" + Module["newmenucss"].ToString();
                    }
                    else
                    {
                        A.InnerHtml = "<i class='" + imgClass + "'></i><span>" + Module["description"] + "</span> <i class='fa fa-angle-left pull-right'></i>";
                    }
                    LI.Controls.Add(A);
                    Loadgroups(LI, long.Parse(Module["pki_Menuid"].ToString()));
                    UL.Controls.Add(LI);
                    cssmenu.Controls.Add(UL);
                }
            }
            catch (Exception ex)
            {
                //Response.Redirect("~/Error.aspx");
            }

        }
        void Loadgroups(HtmlGenericControl menu, long ModuleID)
        {
            HtmlGenericControl LI = null;
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            ul.Attributes.Add("class", "treeview-menu");
            //ul.Attributes.Add("data-widget", "tree");
            foreach (DataRow group in PermissedMenus.Select("ParentID=" + ModuleID))
            {
                LI = new HtmlGenericControl("li");
                if (PermissedMenus.Select("ParentID=" + group["pki_Menuid"].ToString()).Length > 0) { LI.Attributes.Add("class", "treeview"); };
                LI.ID = group["description"] + "$" + group["pki_Menuid"].ToString();
                string url = "";
                if (group["filename"].ToString() == "#")
                { url = group["filename"].ToString(); }
                else
                {
                    url = Page.ResolveUrl(group["filename"].ToString());
                }
                string arrow = "";
                string imgClass = Convert.ToString(group["imagen"]);
                if (imgClass == "") { imgClass = "fa fa-circle-o"; }
                if (group["newmenucss"].ToString() != "")
                {
                    if (PermissedMenus.Select("ParentID=" + group["pki_Menuid"].ToString()).Length > 0) { arrow = "<i class='fa fa-angle-left pull-right' ></i>"; };
                    LI.InnerHtml = "<a href='" + url + "'><i class='" + imgClass + "'></i>" + group["description"] + arrow + group["newmenucss"].ToString() + " </a>  ";
                }
                else
                {
                    if (PermissedMenus.Select("ParentID=" + group["pki_Menuid"].ToString()).Length > 0) { arrow = "<i class='fa fa-angle-left pull-right' ></i>"; };
                    LI.InnerHtml = "<a href='" + url + "'><i class='" + imgClass + "'></i>" + arrow + group["description"] + " </a>  ";
                }


                Loadgroups(LI, long.Parse(group["pki_Menuid"].ToString()));

                ul.Controls.Add(LI);
                menu.Controls.Add(ul);
            }
        }
        /*void BindOutlets()
        {
             try
             {
                 DataTable dt = new DataTable();
                 dt = ((BaseRequest)Session["cache"]).dtAccessableOutlets;
                 if (dt.Rows.Count > 0)
                 {
                     ltrOutletLists.Text = "";
                     foreach (DataRow drNew in dt.Rows)
                     {
                         string defaultColor = "bg-maroon";
                         if (drNew["vc_OuletAddress"].ToString() == lblOutlet.Text)
                             defaultColor = "bg-olive";
                         ltrOutletLists.Text += "<li><a class='btn btn-lg btn-flat " + defaultColor + "' href='javascript:search(" + drNew["pki_OutletId"] + ")'>"
                                 + drNew["vc_OuletAddress"] + "</a></li>";
                     }
                 }
             }
             catch (Exception ex)
             {

             }
        }*/
    }
}