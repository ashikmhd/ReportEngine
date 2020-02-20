using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Master
{
    public class ClsUserRollPermission : Base
    {
        public long userid = 0;
        public long fileid = 0;
        public int RoleID = 0;
        public int IsActive = 1;
        public string Flag = "";
        //public string ErrorRemarks = "";
        //public Boolean Errorstatus;
        public ClsUserRollPermission Clone()
        {
            return (ClsUserRollPermission)this.MemberwiseClone();
        }
        public List<ClsUserRollPermission> UserRollPermissionCol = null;
        public DataTable GetUserRoles(int RoleId)
        {
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.CommandText = "UR_GetUserRoles";
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@RoleId", RoleId);
            return ExecuteDataTable(ref sqlcmd);
        }
        public void SaveUserRole(int RoleId, string RoleName)
        {
            SqlCommand sqlcmd = new SqlCommand();
            sqlcmd.CommandText = "UR_SaveUserRole";
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@RoleId", RoleId);
            sqlcmd.Parameters.AddWithValue("@RoleName", RoleName);
            if (ExecuteNonQuery(ref sqlcmd) <= 0)
            {
                this.ErrorRemarks = "Unable To Save User Role!";
                this.Errorstatus = true;
            }
        }
        /// <summary>
        /// bineesh added on 12-12-2014
        /// </summary>
        /// <param name="ModuleID"></param>
        /// <param name="Parent2"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public DataTable Fetch_Menus(String ModuleCode, int Parent2, int RoleId)
        {
            DataTable Dt_Menus = new DataTable();
            try
            {
                SqlCommand sqlcmd = new SqlCommand();
                sqlcmd.CommandText = "Fetch_Menus_new";
                sqlcmd.CommandType = CommandType.StoredProcedure;
                sqlcmd.Parameters.AddWithValue("@RoleId", RoleId);
                sqlcmd.Parameters.AddWithValue("@ModuleCode", ModuleCode);
                sqlcmd.Parameters.AddWithValue("@Parentid", Parent2);
                Dt_Menus = ExecuteDataTable(ref sqlcmd);
                return Dt_Menus;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Bineesh  Added on 18-09-2014
        /// </summary>
        public void SaveUserRollPermission()
        {
            SqlCommand sqlcmd = null;
            int Execute = 0;
            try
            {
                BeginTransaction(true);
                sqlcmd = new SqlCommand();
                foreach (ClsUserRollPermission Obj in UserRollPermissionCol)
                {
                    sqlcmd.Parameters.Clear();
                    sqlcmd.CommandText = "INS_UserRollMenuRights_New";
                    sqlcmd.CommandType = CommandType.StoredProcedure;
                    sqlcmd.Parameters.AddWithValue("@FileId", Obj.fileid);
                    sqlcmd.Parameters.AddWithValue("@RoleId", Obj.RoleID);
                    sqlcmd.Parameters.AddWithValue("@IsActive", Obj.IsActive);
                    sqlcmd.Parameters.AddWithValue("@Uid", Obj.userid);
                    sqlcmd.Parameters.AddWithValue("@Flag", Obj.Flag);
                    Execute = BaseDataLayer.ExecuteQuery(sqlcmd);
                    if (Execute <= 0)
                    {
                        Errorstatus = true;
                        ErrorRemarks = "Error Occured While Inserting RollPermission!";
                        goto END;
                    }
                }
                EndTransaction();
                return;
            END:
                if (BaseDataLayer != null) { RollBackTransaction(); BaseDataLayer = null; }
            }
            catch (Exception ex)
            {
                Errorstatus = true;
                ErrorRemarks = ex.Message;
                if (BaseDataLayer != null) { RollBackTransaction(); BaseDataLayer = null; }
            }
        }
        public DataTable Fetch_UserRolls()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "Fetch_UserRolls";
                sqlCmd.CommandType = CommandType.StoredProcedure;
                return ExecuteDataTable(ref sqlCmd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable Fetch_MenuDetails(Int64 FileID)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.CommandText = "GetMenuDetails";
                sqlCmd.Parameters.Add("@fileID", SqlDbType.BigInt).Value = FileID;
                return ExecuteDataTable(ref sqlCmd);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int InsertMenuDetails(Int64 ParentfileId, Int64 FileID, string ParentMenuName, string ParentFilePath, string ChildMenuName, string ChildFilePath, string Screen)
        {
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandText = "INS_MenuDetails";
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.Add("@ParentfileId", SqlDbType.BigInt).Value = ParentfileId;
            sqlCmd.Parameters.Add("@fileid", SqlDbType.BigInt).Value = FileID;
            sqlCmd.Parameters.Add("@MenuName", SqlDbType.VarChar, 50).Value = ParentMenuName;
            sqlCmd.Parameters.Add("@MenuPath", SqlDbType.VarChar, 200).Value = ParentFilePath;
            sqlCmd.Parameters.Add("@ChildMenuName", SqlDbType.VarChar, 50).Value = ChildMenuName;
            sqlCmd.Parameters.Add("@ChildMenuPath", SqlDbType.VarChar, 200).Value = ChildFilePath;
            sqlCmd.Parameters.Add("@Screen", SqlDbType.VarChar, 1).Value = Screen;
            SqlParameter retval = new SqlParameter();
            retval = sqlCmd.Parameters.Add("retval", SqlDbType.Int);
            retval.Direction = ParameterDirection.ReturnValue;
            ExecuteNonQuery(ref sqlCmd);

            return Convert.ToInt32(retval.Value);
        }

        public int Delete_MenuDetails(Int64 FileID)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "DEL_MenuDetails";
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.Add("@FileID", SqlDbType.BigInt).Value = FileID;
                return ExecuteNonQuery(ref sqlCmd);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
