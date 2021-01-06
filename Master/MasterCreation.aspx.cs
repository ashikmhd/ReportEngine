using BuisnessLayer;
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
    public partial class MasterCreation : System.Web.UI.Page
    {
        int PrimaryKeyId = 0;
        int reportId = 0;
        string ReportName = "";

        ReportClass reportClass = new ReportClass();
        List<Control> controlList = new List<Control>();
        List<ReportParameters> reportParamList = new List<ReportParameters>();
        ReportDTO reportDTO = new ReportDTO();
        DynamicControls dynamicControls = new DynamicControls();
        protected void Page_Load(object sender, EventArgs e)
        {
            ((MasterPageFile)this.Master).CheckSessionExpiry();

            if (Request.QueryString["PrimaryKeyId"] != null)
                PrimaryKeyId = int.Parse(Request.QueryString["PrimaryKeyId"].ToString());
            if (Request.QueryString["ReportId"] != null)
                reportId = int.Parse(Request.QueryString["ReportId"].ToString());
            if (Request.QueryString["ReportName"] != null)
                ReportName = Request.QueryString["ReportName"].ToString();

            if (!IsPostBack)
            {

                hlinkBack.NavigateUrl = "MasterCreationList.aspx?ReportId=" + reportId;
                lblMasterName.Text = ReportName;

                bindControls();
                bindValues();
            }
            else
            {
                recreateControls();
            }
        }
        private void bindControls()
        {
            try
            {
                if (reportId > 0)
                {
                    reportClass = new ReportClass();
                    reportClass.Flag = 1;
                    reportClass.pki_ReportId = reportId;
                    reportClass.UserId = ((BaseRequest)Session["cache"]).BaseUserId;
                    DataSet ds = reportClass.ReportControls_Select();

                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        hdnQuery.Value = dt.Rows[0]["vc_ReportQuery"].ToString();
                        hdnInsertQuery.Value = dt.Rows[0]["vc_InsertQuery"].ToString();
                        hdnUpdateQuery.Value = dt.Rows[0]["vc_UpdateQuery"].ToString();
                        hdnConnection.Value = dt.Rows[0]["vc_DBConnection"].ToString();

                        btnSave.Visible = true;
                    }
                    else
                        btnSave.Visible = false;

                    dynamicControls = new DynamicControls();
                    dynamicControls.BindDynamciControlsToPH(ds, phDynamicControls);



                    Session["phDynamicControls"] = phDynamicControls;
                }
                else
                {
                    btnSave.Visible = false;
                    Session["phDynamicControls"] = null;
                }
            }
            catch (Exception ex)
            {
                ToastrCall("error", "Error: " + ex.Message);
            }
        }
        private void recreateControls()
        {
            //phDynamicControls = (((PlaceHolder)Session["phDynamicControls"]));
            //var a = ((PlaceHolder)Session["phDynamicControls"]).Controls[0];
            //phDynamicControls.Controls.Add(((PlaceHolder)Session["phDynamicControls"]).Controls[0]);
            bindControls();
        }

        private void bindValues()
        {
            try
            {
                if (PrimaryKeyId > 0)
                {
                    dynamicControls = new DynamicControls();
                    PlaceHolder placeHolder = (PlaceHolder)Session["phDynamicControls"];

                    dynamicControls.FindControls(placeHolder, controlList);

                    bindDefaultParameters();

                    ReportDTO reportDTO = new ReportDTO();
                    reportDTO.ReportQuery = dynamicControls.GenerateDynamicQuery(hdnQuery.Value, reportParamList);
                    reportDTO.ConnectionString = hdnConnection.Value;
                    reportClass = new ReportClass();
                    reportClass.ReportDTO = reportDTO;
                    DataTable dtData = reportClass.ReportDynamicQuery_Execute();

                    if (dtData.Rows.Count > 0)
                    {
                        dynamicControls.SetDynamicControlValues(controlList, dtData);
                    }
                }
            }
            catch (Exception ex)
            {
                ToastrCall("error", "Error: " + ex.Message);
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                dynamicControls = new DynamicControls();
                PlaceHolder placeHolder = (PlaceHolder)Session["phDynamicControls"];

                bindDefaultParameters();
                dynamicControls.FindControls(placeHolder, controlList);
                dynamicControls.GenerateReportParameters(controlList, reportParamList);

                string query = hdnInsertQuery.Value;
                if (PrimaryKeyId > 0)
                {
                    query = hdnUpdateQuery.Value;
                }
                ReportDTO reportDTO = new ReportDTO();
                reportDTO.ReportQuery = dynamicControls.GenerateDynamicQuery(query, reportParamList);
                reportDTO.ConnectionString = hdnConnection.Value;
                reportClass = new ReportClass();
                reportClass.ReportDTO = reportDTO;
                DataTable dtData = reportClass.ReportDynamicQuery_Execute();

                ToastrCall("success", "Saved Successfully");
            }
            catch (Exception ex)
            {
                ToastrCall("error", "Error: " + ex.Message);
            }

        }
        private void bindDefaultParameters()
        {
            reportParamList = new List<ReportParameters>();
            reportParamList.Add(new ReportParameters { ParameterText = "#PrimaryKeyId#", ParameterValue = PrimaryKeyId.ToString() });
            reportParamList.Add(new ReportParameters { ParameterText = "#CreatedBy#", ParameterValue = ((BaseRequest)Session["cache"]).BaseUserId.ToString() });
        }
        private void ToastrCall(string type, string message)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", BusinessLayer.General.Common.ToastrCall(type, message), true);
        }
    }
}