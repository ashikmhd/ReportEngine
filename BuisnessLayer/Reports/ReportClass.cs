using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Reports
{
    public class ReportClass : Base
    {
        public short Flag = 0;
        public int pki_ReportId;
        public int i_Type;
        public ReportDTO ReportDTO;
        public int UserId;
        public int fki_CompanyId = 0;
        public int fki_OutletId = 0;
        public DataSet ReportControls_Select()
        {
            DataSet dataSet = new DataSet();
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "T_ReportControls_Select";
                sqlCmd.Parameters.AddWithValue("@Flag", Flag);
                sqlCmd.Parameters.AddWithValue("@pki_ReportId", pki_ReportId);
                sqlCmd.Parameters.AddWithValue("@i_Type", i_Type);
                sqlCmd.Parameters.AddWithValue("@UserId", UserId);
                sqlCmd.Parameters.AddWithValue("@fki_CompanyId", fki_CompanyId);
                sqlCmd.Parameters.AddWithValue("@fki_OutletId", fki_OutletId);
                dataSet = ExecuteDataset(sqlCmd);
                return dataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable ReportDynamicQuery_Execute()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                //if (ReportDTO.ConnectionString == "")
                //    ReportDTO.ConnectionString = "server=.;database=BSHealthclub_Feb05_2020;uid=sa;pwd=gmchrc05;";

                //ReportDTO.ConnectionString = System.Configuration.ConfigurationManager.AppSettings["ConnString"];
                BaseConnectionString = ReportDTO.ConnectionString;
                sqlCmd.CommandText = ReportDTO.ReportQuery;
                dt = ExecuteDataTable(sqlCmd, CommandType.Text);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*public DataTable bindDynamicData()
        {
            SqlConnection sqlConnection = new SqlConnection(ReportDTO.ConnectionString);
            DataTable dt = new DataTable();
            SqlCommand sqlCmd = new SqlCommand();

            sqlCmd.Connection = sqlConnection;
            sqlCmd.CommandType = CommandType.Text;
            sqlCmd.CommandText = "exec bs_GetCustomerDetailsRptData @StartDATE='2019-01-01 00:00:00',@ENDDATE='2020-02-12 00:00:00',@TypeId=0,@EmiratesId=0,@OutletId=0,@fki_EmiratesIdLivingIn=0,@fki_MarketingModeId=0,@fki_JoiningReasonId=0,@fki_SportsId=0";
            //sqlCmd.Parameters.AddWithValue("@Flag", 0);
            //sqlCmd.Parameters.AddWithValue("@Query", "SELECT pki_BankId,vc_PayingBankId,vc_BankName FROM dbo.M_Bank_Info mbi");
            sqlConnection.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd);
            da.Fill(dt);
            sqlConnection.Close();

            return dt;
        }*/
    }
}
