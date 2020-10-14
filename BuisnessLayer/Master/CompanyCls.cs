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
    public class CompanyCls : Base
    {
        public short Flag = 0;
        public int pki_CompanyId;
        public string vc_CompanyName;
        public int UserId;

        public DataTable Company_Select()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandText = "M_Company_Select";
                sqlCmd.Parameters.AddWithValue("@Flag", Flag);
                sqlCmd.Parameters.AddWithValue("@pki_CompanyId", pki_CompanyId);
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
