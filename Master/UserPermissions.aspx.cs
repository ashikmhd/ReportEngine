using BuisnessLayer.Master;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportEngine.Master
{
    public partial class UserPermissions : System.Web.UI.Page
    {
        ClsUserRollPermission ObjRollPermission = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            ((MasterPageFile)this.Master).CheckSessionExpiry();
            if (Page.IsPostBack == false)
            {
                //fillModules();
                hdnParentFileID.Value = "0";
                fillRoles();
            }
        }
        public void fillRoles()
        {
            ObjRollPermission = new ClsUserRollPermission();
            ddlRole.Items.Clear();
            ddlRole.DataSource = ObjRollPermission.GetUserRoles(0);
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataValueField = "RoleId";
            ddlRole.DataBind();
            ddlRole.Items.Insert(0, new ListItem("--SELECT--", "0"));

        }
        protected void btnSaveRole_Click(object sender, EventArgs e)
        {
            ObjRollPermission = new ClsUserRollPermission();
            ltrlmsg.Text = "";
            General Objgen = new General();
            if (txtUserRole.Text != "")
            {
                ObjRollPermission.SaveUserRole(0, txtUserRole.Text);
                if (ObjRollPermission.Errorstatus == false)
                {
                    fillRoles();
                    ltrlmsg.Text = Objgen.Success("Roll Saved SuccessFully!");
                }
                else
                {
                    ltrlmsg.Text = Objgen.Error(ObjRollPermission.ErrorRemarks);
                }
            }
        }
        protected void ddlModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            ltrlmsg.Text = "";
            try
            {
                if (ddlModule.SelectedValue != "0")
                {
                    FillMenus(TreeMenus);
                }
                else
                {
                    TreeMenus.Nodes.Clear();
                }
            }
            catch (Exception ex)
            {
            }
        }
        void FillMenus(TreeView TreeMenus)
        {
            try
            {
                ObjRollPermission = new ClsUserRollPermission();
                TreeMenus.Nodes.Clear();
                DataTable Dt = ObjRollPermission.Fetch_Menus(ddlModule.SelectedValue, 0, int.Parse(ddlRole.SelectedValue));
                TreeNode Node = null;
                foreach (DataRow Dr in Dt.Select("Parentid=0"))
                {
                    Node = new TreeNode(Dr["description"].ToString(), Dr["pki_Menuid"].ToString() + "$" + Dr["UId"]);
                    Node.Checked = (Boolean)Dr["Added"];
                    TreeMenus.Nodes.Add(Node);
                    FillSubMenus(Node, Dt);
                }
            }
            catch (Exception ex)
            {

            }
        }
        void FillSubMenus(TreeNode Node, DataTable Dt)
        {
            try
            {
                foreach (DataRow Dr in Dt.Select("Parentid=" + Node.Value.Split('$')[0]))
                {
                    TreeNode Node1 = new TreeNode(Dr["description"].ToString(), Dr["pki_Menuid"].ToString() + "$" + Dr["UId"]);
                    Node1.Checked = (Boolean)Dr["Added"];
                    Node.ChildNodes.Add(Node1);
                    FillSubMenus(Node1, Dt);
                }
            }
            catch (Exception ex)
            {
            }
        }
        //bineesh added on 17-09-2014
        protected void btnSavePermission_Click(object sender, EventArgs e)
        {
            General Objgen = new General();
            ltrlmsg.Text = "";
            ObjRollPermission = new ClsUserRollPermission();
            ObjRollPermission.UserRollPermissionCol = new List<ClsUserRollPermission>();
            try
            {
                if (ddlRole.SelectedValue == "0") { ltrlmsg.Text = "Pls Select Roll!"; return; }
                if (ddlModule.SelectedValue == "0") { ltrlmsg.Text = "Pls Select Module!"; return; }
                //Obj.UserRollPermissionCol = new List<ClsUserRollPermission>();
                ClsUserRollPermission Obj = null;
                foreach (TreeNode Node in TreeMenus.Nodes)
                {
                    Obj = new ClsUserRollPermission();
                    Obj.userid = long.Parse(Node.Value.Split('$')[1]);
                    Obj.fileid = long.Parse(Node.Value.Split('$')[0]);
                    Obj.RoleID = int.Parse(ddlRole.SelectedValue);
                    if (Node.Checked == false && Node.Value.Split('$')[1] != "0")//DeLete
                    {
                        Obj.IsActive = 0;
                        Obj.Flag = "D";
                        ObjRollPermission.UserRollPermissionCol.Add(Obj.Clone());
                    }
                    else if (Node.Checked == true && Node.Value.Split('$')[1] == "0")//Save
                    {
                        Obj.IsActive = 1;
                        Obj.Flag = "S";
                        ObjRollPermission.UserRollPermissionCol.Add(Obj.Clone());
                    }
                    CheckCheckBox(Node);
                }

                if (ObjRollPermission.UserRollPermissionCol.Count > 0)
                {
                    ObjRollPermission.SaveUserRollPermission();
                    if (ObjRollPermission.Errorstatus == false)

                    { ltrlmsg.Text = Objgen.Success("User Roll Permissions SuccessFully Saved!"); FillMenus(TreeMenus); }
                    else { ltrlmsg.Text = ObjRollPermission.ErrorRemarks; }
                }
                else
                {
                    ltrlmsg.Text = Objgen.Success("No Changes Found with Existing Permission!");
                }
            }
            catch (Exception ex)
            {
                ltrlmsg.Text = Objgen.Error(ex.Message);
            }
        }

        void CheckCheckBox(TreeNode Node1)
        {
            ClsUserRollPermission Obj = null;
            foreach (TreeNode Node in Node1.ChildNodes)
            {
                Obj = new ClsUserRollPermission();
                Obj.userid = long.Parse(Node.Value.Split('$')[1]);
                Obj.fileid = long.Parse(Node.Value.Split('$')[0]);
                Obj.RoleID = int.Parse(ddlRole.SelectedValue);
                if (Node.Checked == false && Node.Value.Split('$')[1] != "0")//Delete
                {
                    Obj.IsActive = 0;
                    Obj.Flag = "D";
                    ObjRollPermission.UserRollPermissionCol.Add(Obj.Clone());
                }
                else if (Node.Checked == true && Node.Value.Split('$')[1] == "0")//Save
                {
                    Obj.IsActive = 1;
                    Obj.Flag = "S";
                    ObjRollPermission.UserRollPermissionCol.Add(Obj.Clone());
                }
                CheckCheckBox(Node);
            }
        }
        protected void ddlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            ltrlmsg.Text = "";
            try
            {
                ddlModule_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {

            }
        }
        protected void TreeMenus_SelectedNodeChanged(object sender, EventArgs e)
        {
            ObjRollPermission = new ClsUserRollPermission();
            string selectedValue = TreeMenus.SelectedNode.Value;
            string[] Arr = new string[2];
            txtMenuName.Text = TreeMenus.SelectedNode.Text;
            Arr = selectedValue.Split('$');
            hdnFileID.Value = Arr[0];
            DataTable dt = ObjRollPermission.Fetch_MenuDetails(Convert.ToInt64(Arr[0]));
            if (dt != null && dt.Rows.Count > 0)
            {
                txtPath.Text = dt.Rows[0]["filename"].ToString();
                hdnScreen.Value = dt.Rows[0]["screen"].ToString();
            }
            FillMenus(TreeView1);
            md1.Show();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            General Objgen = new General();
            if (!string.IsNullOrEmpty(hdnFileID.Value))
            {
                ObjRollPermission = new ClsUserRollPermission();
                int Result = ObjRollPermission.InsertMenuDetails(Convert.ToInt64(hdnParentFileID.Value), Convert.ToInt64(hdnFileID.Value), txtMenuName.Text.Trim(), txtPath.Text.Trim(), txtChildMenuName.Text.Trim(), txtChildPath.Text.Trim(), hdnScreen.Value);
                if (Result > 0)
                {
                    FillMenus(TreeMenus);
                    FillMenus(TreeView1);
                    lblMessage.Text = Objgen.Success("Menu Inserted/Updated Successfully");
                    ClearControls();
                }
                else
                    lblMessage.Text = Objgen.Information("Menu name  exists under this parent menu or no change in the parent menu name/path");

            }
            md1.Show();
        }
        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            string selectedValue = TreeView1.SelectedNode.Value;
            string[] Arr = new string[2];
            Arr = selectedValue.Split('$');
            hdnParentFileID.Value = Arr[0];
            md1.Show();
        }

        private void ClearControls()
        {
            txtChildMenuName.Text = string.Empty;
            txtChildPath.Text = string.Empty;
            hdnParentFileID.Value = "0";
        }
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ObjRollPermission = new ClsUserRollPermission();
            General Objgen = new General();
            if (!string.IsNullOrEmpty(hdnFileID.Value))
            {
                int Result = ObjRollPermission.Delete_MenuDetails(Convert.ToInt64(hdnFileID.Value));
                if (Result > 0)
                {
                    FillMenus(TreeMenus);
                    FillMenus(TreeView1);
                    lblMessage.Text = Objgen.Success("Menu Deleted Successfully");
                    md1.Show();
                }
            }

        }
    }
}