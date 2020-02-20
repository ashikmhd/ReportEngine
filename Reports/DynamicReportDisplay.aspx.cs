using BuisnessLayer;
using BuisnessLayer.Reports;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportEngine.Reports
{
    public partial class DynamicReportDisplay : System.Web.UI.Page
    {
        ReportClass reportClass = new ReportClass();
        ReportDTO reportDTO = new ReportDTO();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    reportDTO = new ReportDTO();
                    reportDTO = (ReportDTO)Session["ReportDTO"];
                    Page.Title = reportDTO.ReportName;
                    LoadReportData();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = ex.Message;
                    div_report.Style.Add("display", "none");
                    div_noreport.Style.Add("display", "block");
                }
            }
        }
        private void LoadReportData()
        {
            reportClass = new ReportClass();
            reportClass.ReportDTO = reportDTO;

            div_report.Style.Add("display", "block");
            div_noreport.Style.Add("display", "none");

            string strReportPath = Server.MapPath("rptDynamicReport.rdlc");
            string strDatasource = "dsDynamicReport";
            DataTable dtRptData = reportClass.ReportDynamicQuery_Execute();

            if (dtRptData.Rows.Count > 0)
            {
                UnPivotTable unPivotTable = new UnPivotTable();
                DataTable dtRptDataUnPivot = unPivotTable.UnpivotDataTable(dtRptData);

                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = strReportPath;

                ReportViewer1.LocalReport.EnableHyperlinks = true;
                ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.DisplayName = reportDTO.ReportName;                

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportDataSource rdf = new ReportDataSource(strDatasource, dtRptDataUnPivot);
                this.ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rdf);
                //ReportViewer1.LocalReport.SetParameters(rptParam);
                ReportViewer1.LocalReport.Refresh();
            }
            else
            {
                lblMessage.Text = "No Report Data Found";
                div_report.Style.Add("display", "none");
                div_noreport.Style.Add("display", "block");
            }
        }
    }
}