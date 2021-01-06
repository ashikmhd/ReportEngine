using BuisnessLayer;
using BuisnessLayer.Master;
using BuisnessLayer.Reports;
using BuisnessLayer.Transactions;
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
    public partial class MasterCreationList : CommonBaseClass
    {
        ReportClass reportClass = new ReportClass();
        List<ReportParameters> reportParamList = new List<ReportParameters>();
        protected void Page_Load(object sender, EventArgs e)
        {
            CheckUserLogin();
            if (Session["cache"] != null)
            {
                if (!IsPostBack)
                {
                    BindCompany();
                    BindOutlet();
                    BindReportNames();
                    if (Request.QueryString["ReportId"] != null)
                    {
                        drpReportName.SelectedValue = Request.QueryString["ReportId"].ToString();
                        drpReportName_SelectedIndexChanged(null, null);
                    }
                } 
            }
        }
        private void CheckUserLogin()
        {
            if (Request.QueryString["UserName"] != null)/*If the user accessing from outside*/
            {
                General G = new General();
                try
                {
                    UserComponent UComp = Authenticate(1, Request.QueryString["UserName"], "");

                    if (UComp.User.Errorstatus == true)
                    {
                        lblmsg.Text = G.Error(UComp.User.ErrorRemarks);
                    }
                    else
                    {
                        CreateBaseCache(UComp);
                        Response.Redirect("MasterCreationList.aspx");
                    }
                }
                catch (Exception ex)
                {
                    lblmsg.Text = G.Error(ex.Message);
                }
            }
            else
                ((MasterPageFile)this.Master).CheckSessionExpiry();
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
            reportClass.i_Type = 2;
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

        protected void drpReportName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int reportId = int.Parse(drpReportName.SelectedValue);
                if (reportId > 0)
                {
                    hlinkNew.Text = "New " + drpReportName.SelectedItem.Text;
                    hlinkNew.NavigateUrl = "MasterCreation.aspx?PrimaryKeyId=0&ReportId="+ reportId;
                    hlinkNew.Visible = true;

                    reportClass = new ReportClass();
                    reportClass.Flag = 1;
                    reportClass.pki_ReportId = reportId;
                    DataSet ds = reportClass.ReportControls_Select();
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        DynamicControls dynamicControls = new DynamicControls();
                        reportParamList = new List<ReportParameters>();
                        reportParamList.Add(new ReportParameters { ParameterText = "#PrimaryKeyId#", ParameterValue = "0" });

                        ReportDTO reportDTO = new ReportDTO();
                        reportDTO.ReportQuery = dynamicControls.GenerateDynamicQuery(dt.Rows[0]["vc_ReportQuery"].ToString(), reportParamList);
                        reportDTO.ConnectionString = dt.Rows[0]["vc_DBConnection"].ToString();
                        reportClass = new ReportClass();
                        reportClass.ReportDTO = reportDTO;
                        DataTable dtData = reportClass.ReportDynamicQuery_Execute();
                        if (dtData.Rows.Count <= 0)
                            dtData = null;
                        else
                        {
                            List<DataColumn> colTodelete = new List<DataColumn>();
                            foreach (DataColumn col in dtData.Columns)
                            {
                                if (col.ColumnName.StartsWith("#"))
                                    colTodelete.Add(col);
                            }

                            foreach (DataColumn col in colTodelete)
                            {
                                dtData.Columns.Remove(col);
                            }
                        }
                        gvDynamicMasters.DataSource = dtData;
                        gvDynamicMasters.DataBind();
                    }
                }
                else
                {
                    gvDynamicMasters.DataSource = null;
                    gvDynamicMasters.DataBind();
                    hlinkNew.Visible = false;
                }
            }
            catch (Exception ex)
            {
                ToastrCall("error", "Error: " + ex.Message);
            }
        }
        private void ToastrCall(string type, string message)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", BusinessLayer.General.Common.ToastrCall(type, message), true);
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