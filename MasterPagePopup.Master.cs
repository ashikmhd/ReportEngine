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
    public partial class MasterPagePopup : System.Web.UI.MasterPage
    {
        UserInfo Objuserinfo = null;
        DataTable PermissedMenus;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}