using BuisnessLayer;
using BuisnessLayer.Master;
using BuisnessLayer.Reports;
using BuisnessLayer.Transactions;
using BusinessLayer.General;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportEngine.Reports
{
    public partial class ReportSelect : System.Web.UI.Page
    {
        ReportClass reportClass = new ReportClass();
        List<Control> controlList = new List<Control>();
        List<ReportParameters> reportParamList = new List<ReportParameters>();
        ReportDTO reportDTO = new ReportDTO();
        DynamicControls dynamicControls = new DynamicControls();
        protected void Page_Load(object sender, EventArgs e)
        {
            reportDTO = new ReportDTO();            

            if (!IsPostBack)
            {
                Session["phDynamicControls"] = null;
                controlList = new List<Control>();
                reportParamList = new List<ReportParameters>();

                BindCompany();
                BindOutlet();
                BindReportNames();
                //bindControls();
            }
            else
            {
                recreateControls();
            }
        }
        private void BindCompany()
        {
            CompanyCls companyCls = new CompanyCls();
            companyCls.Flag = 0;
            DataTable dt = companyCls.Company_Select();

            drpCompanyMasterPage.DataSource = dt;
            drpCompanyMasterPage.DataTextField = "vc_CompanyName";
            drpCompanyMasterPage.DataValueField = "pki_CompanyId";
            drpCompanyMasterPage.DataBind();
            drpCompanyMasterPage.Items.Insert(0, new ListItem("All", "0"));
        }
        private void BindOutlet()
        {
            OutletCls outletCls = new OutletCls();
            outletCls.Flag = 0;
            outletCls.fki_CompanyId = Int32.Parse(drpCompanyMasterPage.SelectedValue);
            DataTable dt = outletCls.Outlet_Select();

            drpOutletMasterPage.DataSource = dt;
            drpOutletMasterPage.DataTextField = "vc_OutletName";
            drpOutletMasterPage.DataValueField = "pki_OutletId";
            drpOutletMasterPage.DataBind();
            drpOutletMasterPage.Items.Insert(0, new ListItem("All", "0"));
        }
        private void BindReportNames()
        {
            reportClass = new ReportClass();
            reportClass.Flag = 0;
            reportClass.i_Type = 1;
            reportClass.pki_ReportId = 0;
            reportClass.fki_CompanyId = Int32.Parse(drpCompanyMasterPage.SelectedValue);
            reportClass.fki_OutletId = Int32.Parse(drpOutletMasterPage.SelectedValue);
            reportClass.UserId = ((BaseRequest)Session["cache"]).BaseUserId;
            DataSet ds = reportClass.ReportControls_Select();

            DataTable dt = ds.Tables[0];
            drpReportName.DataSource = dt;
            drpReportName.DataTextField = "ReportName";
            drpReportName.DataValueField = "pki_ReportId";
            drpReportName.DataBind();

            drpReportName.Items.Insert(0, new ListItem("Select", "0"));
        }
        private void bindControls()
        {
            try
            {
                int reportId = int.Parse(drpReportName.SelectedValue);
                if (reportId > 0)
                {
                    reportClass = new ReportClass();
                    reportClass.Flag = 1;
                    reportClass.pki_ReportId = reportId;
                    DataSet ds = reportClass.ReportControls_Select();

                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        hdnQuery.Value = dt.Rows[0]["vc_ReportQuery"].ToString();
                        hdnConnection.Value = dt.Rows[0]["vc_DBConnection"].ToString();
                        btnRunReport.Visible = true;
                    }
                    else
                        btnRunReport.Visible = false;

                    dynamicControls = new DynamicControls();
                    dynamicControls.BindDynamciControlsToPH(ds, phDynamicControls);

                    

                    Session["phDynamicControls"] = phDynamicControls;
                }
                else
                {
                    btnRunReport.Visible = false;
                    Session["phDynamicControls"] = null;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void recreateControls()
        {
            //phDynamicControls = (((PlaceHolder)Session["phDynamicControls"]));
            //var a = ((PlaceHolder)Session["phDynamicControls"]).Controls[0];
            //phDynamicControls.Controls.Add(((PlaceHolder)Session["phDynamicControls"]).Controls[0]);
            bindControls();
        }
        
             
        protected void btnRunReport_Click(object sender, EventArgs e)
        {

            if (Session["phDynamicControls"] != null)
            {
                dynamicControls = new DynamicControls();
                PlaceHolder placeHolder = (PlaceHolder)Session["phDynamicControls"];

                dynamicControls.FindControls(placeHolder, controlList);
                dynamicControls.GenerateReportParameters(controlList, reportParamList);
                if (reportParamList.Count > 0)
                {
                    reportDTO.ReportQuery = dynamicControls.GenerateDynamicQuery(hdnQuery.Value, reportParamList);
                    reportDTO.ReportName = drpReportName.SelectedItem.Text;
                    reportDTO.ReportParamList = reportParamList;
                    reportDTO.ConnectionString = hdnConnection.Value;

                    Session["ReportDTO"] = reportDTO;
                    //Response.Redirect("DynamicReportDisplay.aspx");
                    string reportPage = "window.open('DynamicReportDisplay.aspx')";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", reportPage, true);
                }
            }
        }

        protected void drpReportName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //bindControls();
        }
        protected void drpCompanyMasterPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOutlet();
            BindReportNames();
        }
        protected void drpOutletMasterPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindReportNames();
        }
    }
}