using BuisnessLayer.General;
using BusinessLayer;
using ReportEngine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Master
{
   public class UserInfo : Base
    {
        public UserInfo() : base() { }
        //public UserInfo(string Cnstr) : base(Cnstr) { }
        public int id = 0;
        public String uid = "";
        public String pwd = "";
        public String fname = "";
        public String lname = "";
        public int isdeleted = 0;
        public long pcode = 0;
        public int usertype = 0;
        public DateTime deactivedate;
        public long Empid = 0;
        public bool isnew = false;
        public String encryptedpwd = "";
        public int RoleID = 0;
        public string RoleName;
        public string EmailId;
        public int CreatedBy;

        public int OutletId = 0;
        public string OutletName = "";
        public string OutletAddress = "";
        public string OutletPOBox = "";
        public string OutletEmail = "";
        public string OutletPhone = "";
        public string OutletMobile = "";
        public string OutletWebSite = "";
        public string OutletHeaderImage = "";
        public string OutletOracleOrgCode = "";
        public string OutletOracleInvCode = "";
        public int OutletRenewMin;
        public int OutletRenewMax;

        public DataTable dtAccessableOutlets = null;
        public int Flag;

        private ClsUserLoginDetails ObjUserLoginDetails;
        public ClsUserLoginDetails UserLoginDetails
        {
            get { return ObjUserLoginDetails; }
            set { ObjUserLoginDetails = value; }
        }
        public void Authentication()
        {
            try
            {
                BeginTransaction();
                sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "UserInfo_Login_new";
                sqlCmd.Parameters.AddWithValue("@Flag", Flag);
                sqlCmd.Parameters.Add("@uid", SqlDbType.VarChar, 255);
                sqlCmd.Parameters["@uid"].Value = uid;
                sqlCmd.Parameters.Add("@pwd", SqlDbType.VarChar, 50);
                sqlCmd.Parameters["@pwd"].Value = pwd;
                //Encoded pwd kept as new parmater to safe guard non encrypted pwd login of existing users
                //whose pwd is not encrypted
                sqlCmd.Parameters.Add("@encryptedpwd", SqlDbType.VarChar, 250);
                sqlCmd.Parameters["@encryptedpwd"].Value = Encode(pwd);
                DataTable DtUser = BaseDataLayer.FetchResultAsTable(sqlCmd);
                if (DtUser != null)
                {
                    if (DtUser.Rows.Count > 0)
                    {
                        this.id = int.Parse(DtUser.Rows[0]["pki_UserId"].ToString());

                        uid = DtUser.Rows[0]["vc_UserName"].ToString();
                        fname = DtUser.Rows[0]["vc_FirstName"].ToString();
                        lname = DtUser.Rows[0]["vc_LastName"].ToString();
                        Empid = int.Parse(DtUser.Rows[0]["i_EmployeeID"].ToString());
                        isnew = bool.Parse(DtUser.Rows[0]["b_FirstUser"].ToString());
                        RoleID = int.Parse(DtUser.Rows[0]["fki_RoleID"].ToString());
                        RoleName = DtUser.Rows[0]["vc_RoleName"].ToString();

                        //    OutletClass _OutletClass = new OutletClass();
                        //    DataTable dtOutlets = _OutletClass.Fetch_UserOutlet(0, this.id);

                        //    if (dtOutlets.Rows.Count > 0)
                        //    {
                        //        dtAccessableOutlets = dtOutlets;
                        //        bindOutletDetails(dtOutlets);
                        //        uid = DtUser.Rows[0]["vc_UserName"].ToString();
                        //        fname = DtUser.Rows[0]["vc_FirstName"].ToString();
                        //        lname = DtUser.Rows[0]["vc_LastName"].ToString();
                        //        Empid = int.Parse(DtUser.Rows[0]["i_EmployeeID"].ToString());
                        //        isnew = bool.Parse(DtUser.Rows[0]["b_FirstUser"].ToString());
                        //        RoleID = int.Parse(DtUser.Rows[0]["fki_RoleID"].ToString());
                        //        RoleName = DtUser.Rows[0]["vc_RoleName"].ToString();
                        //    }
                        //    else
                        //    {
                        //        this.Errorstatus = true;
                        //        this.ErrorRemarks = "You dont have access to any Outlet";
                        //    }
                    }
                }
                if (this.id > 0)
                {
                    ObjUserLoginDetails.NB_UserID = this.id;
                    ObjUserLoginDetails.UserLoginAttemptType_CH = "S";
                    ObjUserLoginDetails.TransVariables = this.TransVariables.Clone();
                    ObjUserLoginDetails.Save();
                    if (ObjUserLoginDetails.LogId_ID <= 0)
                    {
                        //this.Errorstatus = true;
                        //this.ErrorRemarks = ObjUserLoginDetails.TransVariables.ErrorRemarks;
                    }
                    Fetch_UserRights(1, this.id, "");
                }
                else
                {
                    sqlCmd.Parameters.Clear();//saving Failed Attempt
                    sqlCmd.CommandType = CommandType.StoredProcedure;
                    sqlCmd.CommandText = "Fetch_PreviousLogInDtls_New";
                    sqlCmd.Parameters.AddWithValue("@UserName", uid);
                    sqlCmd.Parameters.AddWithValue("@Flag", 3);
                    String ID = BaseDataLayer.ExecuteScalar(sqlCmd);
                    if (ID != "") this.id = int.Parse(ID);
                    if (this.id > 0)
                    {
                        ObjUserLoginDetails.NB_UserID = this.id;
                        ObjUserLoginDetails.UserLoginAttemptType_CH = "F";
                        ObjUserLoginDetails.TransVariables = this.TransVariables.Clone();
                        ObjUserLoginDetails.Save();
                    }
                    this.Errorstatus = true;
                    this.ErrorRemarks = "Invalid Login Crdentials";

                }
                EndTransaction();
            }
            catch (Exception ex)
            {
                EndTransaction();
                throw ex;
            }
        }
        public void bindOutletDetails(DataTable dtOutlets)
        {
            OutletId = int.Parse(dtOutlets.Rows[0]["pki_OutletId"].ToString());
            OutletName = dtOutlets.Rows[0]["vc_OutletName"].ToString();
            OutletAddress = dtOutlets.Rows[0]["vc_OuletAddress"].ToString();
            OutletPOBox = dtOutlets.Rows[0]["vc_OuletPO"].ToString();
            OutletEmail = dtOutlets.Rows[0]["vc_OutletEmail"].ToString();
            OutletPhone = dtOutlets.Rows[0]["vc_OutletTelephone"].ToString();
            OutletMobile = dtOutlets.Rows[0]["vc_OutletMobile"].ToString();
            OutletWebSite = dtOutlets.Rows[0]["vc_OutletWebSite"].ToString();
            OutletHeaderImage = dtOutlets.Rows[0]["vc_OutletHeader"].ToString();
            OutletOracleOrgCode = dtOutlets.Rows[0]["vc_OrgCode"].ToString();
            OutletOracleInvCode = dtOutlets.Rows[0]["vc_InvCode"].ToString();
            OutletRenewMin = int.Parse(dtOutlets.Rows[0]["i_RenewMinDays"].ToString());
            OutletRenewMax = int.Parse(dtOutlets.Rows[0]["i_RenewMaxDays"].ToString());
        }
        public DataTable DT_PermissedMenus = null;
        public void Fetch_UserRights(Int16 flag, long userId, string utype)
        {
            try
            {
                sqlCmd = new SqlCommand();
                sqlCmd = new SqlCommand("GetUserRights");
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@Flag", SqlDbType.TinyInt).Value = flag;
                sqlCmd.Parameters.Add("@UserId", SqlDbType.VarChar, 30).Value = userId;
                sqlCmd.Parameters.Add("@Utype", SqlDbType.VarChar, 5).Value = utype;
                DT_PermissedMenus = BaseDataLayer.FetchResultAsTable(sqlCmd);
                if (DT_PermissedMenus.Rows.Count == 0)
                {
                    this.ErrorRemarks = "UserPermissions Are Not Set!";
                    this.Errorstatus = true;
                }
            }
            catch (Exception ex)
            {
                this.Errorstatus = true;
                this.ErrorRemarks = ex.Message;
            }
        }

        public int User_Add(string uid, string pwd, string fname, string lname, Int32 empId, int isNew, int RoleID, string EmailId, long CreatedBy)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "BS_USER_ADD";
                sqlCmd.Parameters.Add("@uid", SqlDbType.VarChar, 25).Value = uid;
                sqlCmd.Parameters.Add("@pwd", SqlDbType.VarChar, 25).Value = pwd;
                sqlCmd.Parameters.Add("@fname", SqlDbType.VarChar, 25).Value = fname;
                sqlCmd.Parameters.Add("@lname", SqlDbType.VarChar, 50).Value = lname;
                sqlCmd.Parameters.Add("@empid", SqlDbType.Int).Value = empId;
                sqlCmd.Parameters.Add("@isnew", SqlDbType.Int).Value = isNew;
                sqlCmd.Parameters.Add("@RoleID", SqlDbType.Int).Value = RoleID;
                sqlCmd.Parameters.Add("@EmailId", SqlDbType.VarChar, 50).Value = EmailId;
                sqlCmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CreatedBy;
                int retval = ExecuteNonQuery(sqlCmd);
                return retval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int User_Edit(long UserId, string pwd, string fname, string lname, short flag, Int32 empId, int RoleID, string EmailId, long ModifiedBy, int IsActive = 1)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "BS_USER_EDIT";
                sqlCmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
                sqlCmd.Parameters.Add("@pwd", SqlDbType.VarChar, 25).Value = pwd;
                sqlCmd.Parameters.Add("@fname", SqlDbType.VarChar, 25).Value = fname;
                sqlCmd.Parameters.Add("@lname", SqlDbType.VarChar, 50).Value = lname;
                sqlCmd.Parameters.Add("@flag", SqlDbType.TinyInt).Value = flag;
                sqlCmd.Parameters.Add("@empid", SqlDbType.Int).Value = empId;
                sqlCmd.Parameters.Add("@RoleID", SqlDbType.Int).Value = RoleID;
                sqlCmd.Parameters.Add("@EmailId", SqlDbType.VarChar, 50).Value = EmailId;
                sqlCmd.Parameters.Add("@ModifiedBy", SqlDbType.Int).Value = ModifiedBy;
                sqlCmd.Parameters.Add("@IsActive", SqlDbType.Int).Value = IsActive;
                int retval = ExecuteNonQuery(sqlCmd);
                return retval;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Changepassword(long Uid, string oldPwd, string pwd)
        {
            sqlCmd = new SqlCommand();
            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.CommandText = "BS_Passchange";

            sqlCmd.Parameters.Add("@Userid", SqlDbType.Int);
            sqlCmd.Parameters["@Userid"].Value = Uid;


            sqlCmd.Parameters.Add("@oldPwd", SqlDbType.VarChar, 50);
            sqlCmd.Parameters["@oldPwd"].Value = MD5Encoder.Encode(oldPwd);

            string EncryPwd = MD5Encoder.Encode(pwd);

            sqlCmd.Parameters.Add("@NewPwd", SqlDbType.VarChar, 50);
            sqlCmd.Parameters["@NewPwd"].Value = EncryPwd;

            SqlParameter retVal = sqlCmd.Parameters.Add("@retValue", SqlDbType.BigInt);
            retVal.Direction = ParameterDirection.ReturnValue;
            try
            {
                ExecuteNonQuery(ref sqlCmd);
                return Convert.ToInt32(retVal.Value);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Bind_User(long UserId, short flag)
        {
            DataTable dtable = new DataTable();
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "BS_USER_VIEWDETAILS";
                sqlCmd.Parameters.Add("@UserId", SqlDbType.Int).Value = UserId;
                sqlCmd.Parameters.Add("@flag", SqlDbType.TinyInt).Value = flag;
                dtable = ExecuteDataTable(sqlCmd);
                return dtable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable Search_User(string search, short type)
        {
            DataTable dtable = new DataTable();
            try
            {

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "BS_USER_SEARCH";
                sqlCmd.Parameters.Add("@search", SqlDbType.VarChar, 25).Value = search;
                sqlCmd.Parameters.Add("@type", SqlDbType.TinyInt).Value = type;
                dtable = ExecuteDataTable(sqlCmd);
                return dtable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable FetchUserRole()
        {
            DataTable Dt = new DataTable();
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.Text;
                sqlCmd.CommandText = "select * from M_Role where b_IsActive=1";
                Dt = ExecuteDataTable(sqlCmd, CommandType.Text);
                return Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable ForgotPassword(short Flag, string EmailId, string NewPassword)
        {
            DataTable dtable = new DataTable();
            try
            {
                string EncryPwd = MD5Encoder.Encode(NewPassword);

                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "BS_ForgotPassword";
                sqlCmd.Parameters.Add("@Flag", SqlDbType.TinyInt).Value = Flag;
                sqlCmd.Parameters.Add("@EmailId", SqlDbType.VarChar, 50).Value = EmailId;
                sqlCmd.Parameters.Add("@NewPassword", SqlDbType.VarChar, 250).Value = EncryPwd;
                dtable = ExecuteDataTable(sqlCmd);
                return dtable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
