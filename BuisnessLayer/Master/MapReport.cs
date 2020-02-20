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
    public class MapReport : Base
    {
        public short Flag = 0;
        public int fki_ReportId;
        public string OutletList;
        public string UserList;
        public int UserId;

        public DataTable MapReportOutlet_Select()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "Map_Report_Outlet_Select";
                sqlCmd.Parameters.AddWithValue("@Flag", Flag);
                sqlCmd.Parameters.AddWithValue("@fki_ReportId", fki_ReportId);
                dt = ExecuteDataTable(sqlCmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int MapReportOutlet_Save()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "Map_Report_Outlet_Save";
                sqlCmd.Parameters.AddWithValue("@Flag", Flag);
                sqlCmd.Parameters.AddWithValue("@fki_ReportId", fki_ReportId);
                sqlCmd.Parameters.AddWithValue("@OutletList", OutletList);
                sqlCmd.Parameters.AddWithValue("@UserId", UserId);
                return ExecuteNonQuery(sqlCmd);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable MapReportUser_Select()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "Map_Report_User_Select";
                sqlCmd.Parameters.AddWithValue("@Flag", Flag);
                sqlCmd.Parameters.AddWithValue("@fki_ReportId", fki_ReportId);
                dt = ExecuteDataTable(sqlCmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int MapReportUser_Save()
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "Map_Report_User_Save";
                sqlCmd.Parameters.AddWithValue("@Flag", Flag);
                sqlCmd.Parameters.AddWithValue("@fki_ReportId", fki_ReportId);
                sqlCmd.Parameters.AddWithValue("@UserList", UserList);
                sqlCmd.Parameters.AddWithValue("@UserId", UserId);
                return ExecuteNonQuery(sqlCmd);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
