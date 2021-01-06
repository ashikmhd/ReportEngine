using BuisnessLayer.Master;
using BusinessLayer.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportEngine.Master
{
    public partial class UserList : System.Web.UI.Page
    {
        UserInfo ObjUserInfo = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            ((MasterPageFile)this.Master).CheckSessionExpiry();
            if (Page.IsPostBack == false)
            {
                BindGrid();
            }
        }

        void BindGrid()
        {
            DataTable dt;
            try
            {
                ObjUserInfo = new UserInfo();
                if (drptype.SelectedValue == "All")
                {
                    dt = ObjUserInfo.Bind_User(0, 0);
                }
                else
                {
                    dt = ObjUserInfo.Search_User(txtsearch.Text.Trim(), Convert.ToInt16(drptype.SelectedValue));
                }
                grduser.DataSource = dt;
                grduser.DataBind();
                if (dt.Rows.Count == 0)
                {
                    lblmsg.Text = ObjUserInfo.Information("No Records Exists");
                }
                Common.enableUserAccessibleGrid(grduser);
            }
            catch (Exception ex)
            {

            }
        }
        protected void btnsearch_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            ObjUserInfo = new UserInfo();
            try
            {
                BindGrid();

            }
            catch (Exception ex)
            {
                ObjUserInfo.Error(ex.Message);
            }
        }
        protected void grduser_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            ObjUserInfo = new UserInfo();
            try
            {
                if (e.CommandName == "edt")
                {
                    //btnsave.Text = "Update";
                    //Label luid = (Label)grduser.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lbluid");
                    //DataTable dt = ObjUserInfo.Bind_User(luid.Text, 1);
                    //if (dt.Rows.Count > 0)
                    //{
                    //    txtuid.Text = Convert.ToString(dt.Rows[0]["uid"]);
                    //    txtfname.Text = Convert.ToString(dt.Rows[0]["fname"]);
                    //    txtlname.Text = Convert.ToString(dt.Rows[0]["lname"]);
                    //    txtEmpId.Text = Convert.ToString(dt.Rows[0]["empid"]);
                    //    ddlRole.SelectedValue = Convert.ToString(dt.Rows[0]["RoleID"]);
                    //    //txtpwd.Text = Convert.ToString(dt.Rows[0]["pwd"]);
                    //    //txtcpwd.Text = Convert.ToString(dt.Rows[0]["pwd"]);
                    //}
                    //dt.Dispose();

                }
                if (e.CommandName == "del")
                {
                    //General Objgen = new General();
                    //Label luid = (Label)grduser.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lbluid");
                    //int result1 = ObjUserInfo.User_Edit(luid.Text, "", "", "", 1, 0, 0);
                    //if (result1 > 0)
                    //{
                    //    lblmsg.Text = Objgen.Success("User Deleted Successfully");
                    //}
                    //else
                    //{
                    //    lblmsg.Text = Objgen.Information("User Deletion Failed");
                    //}
                    //DataTable dt1 = ObjUserInfo.Bind_User("", 0);
                    //grduser.DataSource = dt1;
                    //grduser.DataBind();
                    //dt1.Dispose();
                }
            }
            catch (Exception ex)
            {
                lblmsg.Text = ObjUserInfo.Error(ex.Message);
            }
        }

    }
}