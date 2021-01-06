using BuisnessLayer.General;
using BuisnessLayer.Reports;
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
    public partial class ReportConfigurationList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ((MasterPageFile)this.Master).CheckSessionExpiry();
            if (!IsPostBack)
            {
                bindReportDetails();
            }
        }
        private void bindReportDetails()
        {
            ReportClass reportClass = new ReportClass();
            reportClass.Flag = 2;
            reportClass.i_Type = 0;
            reportClass.pki_ReportId = 0;
            DataSet ds = reportClass.ReportControls_Select();

            gvReportDetails.DataSource = ds.Tables[0];
            gvReportDetails.DataBind();
            gvReportDetails.enableUserAccessibleGrid();
        }
    }
}