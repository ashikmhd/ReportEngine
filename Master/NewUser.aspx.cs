using BuisnessLayer.General;
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
    public partial class NewUser : System.Web.UI.Page
    {
        UserInfo ObjUserInfo = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ((MasterPageFile)this.Master).CheckSessionExpiry();
                if (!IsPostBack)
                {
                    BindRole();
                    General Objgen = new General();
                    lblinfo.Text = Objgen.InformationBanner("Password must contain Alphabets, Numerics and Special characters(!,@,#,$,%,&amp;,*)");
                    if (Request.QueryString["uid"] != null)
                    {
                        btnsave.Text = "Update";
                        FillControls();
                    }
                    else
                    {
                    }

                }
            }
            catch (Exception ex)
            {
                ObjUserInfo.Error(ex.Message);
            }
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            General Objgen = new General();
            ObjUserInfo = new UserInfo();
            try
            {
                string encodedPassword = MD5Encoder.Encode(txtpwd.Text.Trim());
                if (btnsave.Text == "Save")
                {
                    int result = ObjUserInfo.User_Add(txtuid.Text.Trim(), encodedPassword, txtfname.Text.Trim(), txtlname.Text.Trim(), Convert.ToInt32(txtEmpId.Text.Trim()), 1, int.Parse(ddlRole.SelectedValue), txtEmail.Text.Trim(), ((BaseRequest)Session["cache"]).BaseUserId);
                    if (result > 0)
                    {
                        lblmsg.Text = Objgen.Success("User Saved Successfully");
                        btnclear_Click(null, null);
                    }
                    else
                    {
                        lblmsg.Text = Objgen.Information("User Saving Failed/User Already Exist");
                    }
                }
                else
                {

                    int result1 = ObjUserInfo.User_Edit(Convert.ToInt64(Request.QueryString["uid"]), encodedPassword, txtfname.Text.Trim(), txtlname.Text.Trim(), 0, Convert.ToInt32(txtEmpId.Text.Trim()), int.Parse(ddlRole.SelectedValue), txtEmail.Text.Trim(), ((BaseRequest)Session["cache"]).BaseUserId, Convert.ToInt32(ddlstatus.SelectedValue));
                    if (result1 > 0)
                    {
                        lblmsg.Text = Objgen.Success("User Updated Successfully");
                    }
                    else
                    {
                        lblmsg.Text = Objgen.Information("User Updation Failed/No User Exists");
                    }
                }
            }
            catch (Exception ex)
            {
                lblmsg.Text = Objgen.Error(ex.Message);
            }

        }

        protected void btnclear_Click(object sender, EventArgs e)
        {
            btnsave.Text = "Save";
            txtuid.Text = "";
            txtfname.Text = "";
            txtlname.Text = "";
            txtpwd.Text = "";
            //txtcpwd.Text = "";
            txtEmpId.Text = "";
            txtEmail.Text = "";
        }
        void BindRole()
        {
            UserInfo ObjUser = new UserInfo();
            try
            {
                DataTable Dt = new DataTable();
                Dt = ObjUser.FetchUserRole();
                ddlRole.DataSource = Dt;
                ddlRole.DataTextField = "vc_RoleName";
                ddlRole.DataValueField = "Pki_RoleID";
                ddlRole.DataBind();
                ddlRole.Items.Insert(0, new ListItem("--SELECT--", ""));
            }
            catch (Exception ex)
            {

            }
        }
        void FillControls()
        {
            ObjUserInfo = new UserInfo();
            try
            {

                string Uid = Request.QueryString["uid"];
                DataTable dt = ObjUserInfo.Bind_User(Convert.ToInt64(Uid), 1);
                if (dt.Rows.Count > 0)
                {
                    txtuid.Text = Convert.ToString(dt.Rows[0]["vc_UserName"]);
                    txtfname.Text = Convert.ToString(dt.Rows[0]["vc_FirstName"]);
                    txtlname.Text = Convert.ToString(dt.Rows[0]["vc_LastName"]);
                    txtEmpId.Text = Convert.ToString(dt.Rows[0]["i_EmployeeID"]);
                    txtEmail.Text = Convert.ToString(dt.Rows[0]["vc_EmailId"]);
                    ddlRole.SelectedValue = Convert.ToString(dt.Rows[0]["fki_RoleID"]);
                    if (Convert.ToBoolean(dt.Rows[0]["b_IsActive"]) == true) { ddlstatus.SelectedValue = "1"; } else { ddlstatus.SelectedValue = "0"; }
                }
            }
            catch (Exception ex)
            {
                lblmsg.Text = ObjUserInfo.Error(ex.Message);
            }

        }
    }
}