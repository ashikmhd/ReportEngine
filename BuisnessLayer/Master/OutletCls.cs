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
    public class OutletCls : Base
    {
        public short Flag = 0;
        public int pki_OutletId;
        public int fki_CompanyId;
        public int UserId;

        public DataTable Outlet_Select()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "M_Outlet_Select";
                sqlCmd.Parameters.AddWithValue("@Flag", Flag);
                sqlCmd.Parameters.AddWithValue("@pki_OutletId", pki_OutletId);
                sqlCmd.Parameters.AddWithValue("@fki_CompanyId", fki_CompanyId);
                dt = ExecuteDataTable(sqlCmd);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
