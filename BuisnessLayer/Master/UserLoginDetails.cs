using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using BusinessLayer;

namespace ReportEngine
{
    public class ClsUserLoginDetails : Base
    {
        public long LogId_ID = 0;
        public String Username_VC = "";
        public DateTime LoggedIn_DT = DateTime.Now;
        public DateTime LoggedOut_DT = DateTime.Parse("01/01/1900");
        public String IPAddress_VC = "";
        public DateTime CreatedDate_DT;
        SqlCommand Com = null;
        public Boolean ErrorStatus = false;
        public String ComputerName_VC = "";
        public string SessionId_VC = "";
        public String ModuleName_VC = "Web";
        public String BrowserDtls_VC = "";
        public String ProjectType_VC = "";
        public String UserLoginAttemptType_CH = "";
        public long NB_UserID = 0;
        public void Save()
        {
            try
            {
                BaseDataLayer = this.TransVariables.Datalayer_Exe;
                Com = new SqlCommand();
                Com.CommandText = "Ins_UserLogDtls_NEW";
                Com.CommandType = CommandType.StoredProcedure;
                Com.Parameters.AddWithValue("@NB_UserID", NB_UserID);
                Com.Parameters.AddWithValue("@LoggedIn_DT", LoggedIn_DT);
                Com.Parameters.AddWithValue("@LoggedOut_DT", LoggedOut_DT);
                Com.Parameters.AddWithValue("@IPAddress_VC", IPAddress_VC);
                Com.Parameters.AddWithValue("@ComputerName_VC", ComputerName_VC);
                Com.Parameters.AddWithValue("@SessionId_VC", SessionId_VC);
                Com.Parameters.AddWithValue("@ModuleName_VC", ModuleName_VC);
                Com.Parameters.AddWithValue("@BrowserDtls_VC", BrowserDtls_VC);
                Com.Parameters.AddWithValue("@ProjectType_VC", ProjectType_VC);
                Com.Parameters.AddWithValue("@Flag", 0);
                Com.Parameters.AddWithValue("@CH_UserLoginAttemptType", UserLoginAttemptType_CH);
                Com.Parameters.AddWithValue("@LogId_ID", LogId_ID).Direction = System.Data.ParameterDirection.Output;
                if (BaseDataLayer != null) BaseDataLayer.ExecuteQuery(Com);
                else { ExecuteNonQuery(Com); }
                BaseDataLayer.ExecuteQuery(Com);
                LogId_ID = long.Parse(Com.Parameters["@LogId_ID"].Value.ToString());
                if (!(LogId_ID > 0))
                {
                    ErrorStatus = true; ErrorRemarks = "Unable To Save LogIndetails!";
                }
            }
            catch (Exception ex)
            {
                ErrorStatus = true; ErrorRemarks = ex.Message;
            }
        }
        public void UpdateLogOut()
        {
            int update = 0;
            try
            {
                Com = new SqlCommand();
                Com.CommandText = "Fetch_PreviousLogInDtls_New";
                Com.CommandType = CommandType.StoredProcedure;
                Com.Parameters.AddWithValue("@NB_UserID", NB_UserID);
                Com.Parameters.AddWithValue("@LogId_ID", LogId_ID);
                Com.Parameters.AddWithValue("@LoggedOut_DT", LoggedOut_DT);
                Com.Parameters.AddWithValue("@Flag", 1);
                update = ExecuteNonQuery(ref Com);
                if (!(update > 0)) { ErrorStatus = true; ErrorRemarks = "Unable To Update LogOut Time!"; }
            }
            catch (Exception ex)
            { ErrorStatus = true; ErrorRemarks = ex.Message; }
        }
        public DataTable Dt_PrevloginDtls;
        public void Fetch_PrevUserloginDetails()
        {
            try
            {
                Com = new SqlCommand();
                Com.CommandText = "Fetch_PreviousLogInDtls_New";
                Com.Parameters.AddWithValue("@NB_UserID", NB_UserID);
                Com.Parameters.AddWithValue("@LogId_ID", LogId_ID);
                Com.Parameters.AddWithValue("@LoggedOut_DT", LoggedOut_DT);
                Com.Parameters.AddWithValue("@Flag", 0);
                Com.CommandType = CommandType.StoredProcedure;
                Dt_PrevloginDtls = ExecuteDataTable(ref Com);
            }
            catch (Exception ex)
            { ErrorStatus = true; ErrorRemarks = ex.Message; }
        }
        public void Fetch_AllPrevUserloginDetails()
        {
            try
            {
                Com = new SqlCommand();
                Com.CommandText = "Fetch_PreviousLogInDtls_New";
                Com.Parameters.AddWithValue("@NB_UserID", NB_UserID);
                Com.Parameters.AddWithValue("@LogId_ID", LogId_ID);
                Com.Parameters.AddWithValue("@LoggedOut_DT", LoggedOut_DT);
                Com.Parameters.AddWithValue("@Flag", 2);
                Com.CommandType = CommandType.StoredProcedure;
                Dt_PrevloginDtls = ExecuteDataTable(ref Com);
            }
            catch (Exception ex)
            { ErrorStatus = true; ErrorRemarks = ex.Message; }
        }

    }
}