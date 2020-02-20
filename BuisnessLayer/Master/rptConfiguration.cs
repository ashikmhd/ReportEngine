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
    public class rptConfiguration : Base
    {
        public short Flag = 0;
        public int pki_ReportId;
        public int i_Type = 1;
        public string vc_ReportName;
        public string vc_ReportDescription;
        public string vc_ReportQuery;
        public string vc_InsertQuery;
        public string vc_UpdateQuery;
        public string ReportControls;
        public string ReportProperties;
        public int fki_CreatedBy;
        public string OutputMessage = "";

        public DataTable ControlType_Select()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "M_ControlType_Select";
                sqlCmd.Parameters.AddWithValue("@Flag", Flag);
                dt = ExecuteDataTable(sqlCmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable ReportSettingType_Select()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "M_ReportSettingType_Select";
                sqlCmd.Parameters.AddWithValue("@Flag", Flag);
                dt = ExecuteDataTable(sqlCmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ReportConfiguration_Save()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "Dynamic_Report_Save";
                sqlCmd.Parameters.AddWithValue("@Flag", Flag);
                sqlCmd.Parameters.Add("@pki_ReportId", SqlDbType.Int).Value = pki_ReportId;
                sqlCmd.Parameters["@pki_ReportId"].Direction = ParameterDirection.InputOutput;
                sqlCmd.Parameters.AddWithValue("@i_Type", i_Type);
                sqlCmd.Parameters.AddWithValue("@vc_ReportName", vc_ReportName);
                sqlCmd.Parameters.AddWithValue("@vc_ReportDescription", vc_ReportDescription);
                sqlCmd.Parameters.AddWithValue("@vc_ReportQuery", vc_ReportQuery);
                sqlCmd.Parameters.AddWithValue("@vc_InsertQuery", vc_InsertQuery);
                sqlCmd.Parameters.AddWithValue("@vc_UpdateQuery", vc_UpdateQuery);
                sqlCmd.Parameters.AddWithValue("@ReportControls", ReportControls);
                sqlCmd.Parameters.AddWithValue("@ReportProperties", ReportProperties);
                sqlCmd.Parameters.AddWithValue("@fki_CreatedBy", fki_CreatedBy);
                sqlCmd.Parameters.Add("@OutputMessage", SqlDbType.VarChar, 200).Value = OutputMessage;
                sqlCmd.Parameters["@OutputMessage"].Direction = ParameterDirection.Output;

                ExecuteNonQuery(sqlCmd);
                this.pki_ReportId = int.Parse(sqlCmd.Parameters["@pki_ReportId"].Value.ToString());
                this.OutputMessage = sqlCmd.Parameters["@OutputMessage"].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
