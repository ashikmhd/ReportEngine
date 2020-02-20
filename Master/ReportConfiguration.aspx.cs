using BuisnessLayer.Master;
using BuisnessLayer.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ReportEngine.Master
{
    public partial class ReportConfiguration : System.Web.UI.Page
    {
        rptConfiguration rptConfiguration = new rptConfiguration();
        DataTable dtControls = new DataTable();
        DataTable dtControlProperties = new DataTable();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                IntializeComponent();
            }
        }

        private void IntializeComponent()
        {
            bindControlType();
            bindReportSettingType();
            btnSave.Visible = false;

            dtControls = new DataTable();
            dtControls.TableName = "ReportControls";
            dtControls.Columns.Add("RowId", typeof(int));
            dtControls.Columns.Add("ReportId", typeof(int));
            dtControls.Columns.Add("ReportDetailsId", typeof(int));
            dtControls.Columns.Add("ControlTypeId", typeof(int));
            dtControls.Columns.Add("ControlName", typeof(string));
            dtControls.Columns.Add("LabelName", typeof(string));
            dtControls.Columns.Add("ControlId", typeof(string));
            dtControls.Columns.Add("RowGroup", typeof(int));
            dtControls.Columns.Add("ColWidth", typeof(int));
            dtControls.Columns.Add("ControlOrder", typeof(int));
            ViewState["dtControls"] = dtControls;

            dtControlProperties = new DataTable();
            dtControlProperties.TableName = "ReportProperties";
            dtControlProperties.Columns.Add("RowId", typeof(int));
            dtControlProperties.Columns.Add("dtControlsRowId", typeof(int));
            dtControlProperties.Columns.Add("ReportConfigurationId", typeof(int));
            dtControlProperties.Columns.Add("ReportSettingTypeId", typeof(int));
            dtControlProperties.Columns.Add("ReportSettingTypeDetailsId", typeof(int));
            dtControlProperties.Columns.Add("ReportSettingName", typeof(string));
            dtControlProperties.Columns.Add("ReportDefaultValue", typeof(string));
            dtControlProperties.Columns.Add("DefaultValueEnable", typeof(bool));
            ViewState["dtControlProperties"] = dtControlProperties;

            if (Request.QueryString["ReportId"] != null)
            {
                hdnReportId.Value = Request.QueryString["ReportId"].ToString();
                LoadData();
            }

        }
        private void bindControlType()
        {
            rptConfiguration = new rptConfiguration();
            rptConfiguration.Flag = 0;
            DataTable dt = rptConfiguration.ControlType_Select();

            drpControlType.DataSource = dt;
            drpControlType.DataTextField = "TextField";
            drpControlType.DataValueField = "ValueField";
            drpControlType.DataBind();
        }
        private void bindReportSettingType()
        {
            rptConfiguration = new rptConfiguration();
            rptConfiguration.Flag = 1;
            DataTable dt = rptConfiguration.ReportSettingType_Select();

            foreach (DataRow drSelected in dt.Rows)
            {
                ListItem listItem = new ListItem();
                listItem.Text = drSelected["TextField"].ToString();
                listItem.Value = drSelected["ValueField"].ToString();
                lstControlProperties.Items.Add(listItem);
            }
        }

        private void LoadData()
        {
            try
            {
                ReportClass reportClass = new ReportClass();
                reportClass.Flag = 3;
                reportClass.pki_ReportId = int.Parse(hdnReportId.Value);
                DataSet ds = reportClass.ReportControls_Select();

                DataTable dataTable = ds.Tables[0];
                if (dataTable.Rows.Count > 0)
                {
                    DataRow drSelected = dataTable.Rows[0];
                    drpType.SelectedValue= drSelected["i_Type"].ToString();
                    txtReportName.Text = drSelected["vc_ReportName"].ToString();
                    txtReportDescription.Text = drSelected["vc_ReportDescription"].ToString();
                    txtReportQuery.Text = drSelected["vc_ReportQuery"].ToString();
                    txtInsertQuery.Text = drSelected["vc_InsertQuery"].ToString();
                    txtUpdateQuery.Text = drSelected["vc_UpdateQuery"].ToString();

                    visibilityControls();

                    DataTable dataTableProperties = ds.Tables[1];
                    foreach (DataRow drControls in dataTable.Rows)
                    {
                        int RowIdControl = Convert.ToInt32(dtControls.AsEnumerable()
                                 .Max(row => row["RowId"]));
                        RowIdControl = RowIdControl + 1;

                        DataRow drNew = dtControls.NewRow();
                        drNew["RowId"] = RowIdControl;
                        drNew["ReportId"] = drControls["pki_ReportId"];
                        drNew["ReportDetailsId"] = drControls["pki_ReportDetailsId"];
                        drNew["ControlTypeId"] = drControls["pki_ControlTypeId"];
                        drNew["ControlName"] = drControls["vc_ControlName"];
                        drNew["LabelName"] = drControls["vc_LabelName"];
                        drNew["ControlId"] = drControls["vc_FormControlId"];
                        drNew["RowGroup"] = drControls["i_RowGroupOrder"];
                        drNew["ColWidth"] = drControls["i_ColWidth"];
                        drNew["ControlOrder"] = drControls["i_DisplayOrder"];
                        dtControls.Rows.Add(drNew);

                        DataRow[] drProperties = dataTableProperties.Select("fki_ReportDetailsId=" + drControls["pki_ReportDetailsId"]);
                        foreach (DataRow drProperty in drProperties)
                        {
                            int RowIdProp = Convert.ToInt32(dtControlProperties.AsEnumerable().Max(row => row["RowId"]));
                            RowIdProp = RowIdProp + 1;

                            DataRow drPropertiesNew = dtControlProperties.NewRow();                         

                            drPropertiesNew["RowId"] = RowIdProp;
                            drPropertiesNew["dtControlsRowId"] = RowIdControl;
                            drPropertiesNew["ReportConfigurationId"] = drProperty["pki_ReportConfigurationId"];
                            drPropertiesNew["ReportSettingTypeId"] = drProperty["pki_ReportSettingTypeId"];
                            drPropertiesNew["ReportSettingTypeDetailsId"] = drProperty["pki_ReportSettingTypeDetailsId"];
                            drPropertiesNew["ReportSettingName"] = drProperty["SettingText"];
                            drPropertiesNew["ReportDefaultValue"] = drProperty["vc_DefaultValue"];
                            string DefaultDefaultValueEnable = "false";
                            if (drPropertiesNew["ReportSettingTypeId"].ToString() == "2")
                                DefaultDefaultValueEnable = "true";
                            drPropertiesNew["DefaultValueEnable"] = DefaultDefaultValueEnable;
                            dtControlProperties.Rows.Add(drPropertiesNew);

                        }
                    }
                }

                btnSave.Text = "Update";
                BindControlToGrid();
            }
            catch (Exception ex)
            {
                ToastrCall("error", "Error: " + ex.Message);
            }
        }
        private void BindControlToGrid()
        {
            gvDynamicControls.DataSource = dtControls;
            gvDynamicControls.DataBind();

            if (gvDynamicControls.Rows.Count > 0 && dtControlProperties.Rows.Count > 0)
            {
                btnSave.Visible = true;
                foreach (GridViewRow gvRow in gvDynamicControls.Rows)
                {
                    HiddenField hdnRowId = (HiddenField)gvRow.FindControl("hdnRowId");
                    GridView gvControlProperties = (GridView)gvRow.FindControl("gvControlProperties");
                    DataRow[] drSelected = dtControlProperties.Select("dtControlsRowId=" + hdnRowId.Value);
                    if (drSelected.Length > 0)
                    {
                        gvControlProperties.DataSource = drSelected.CopyToDataTable();
                        gvControlProperties.DataBind();
                    }
                }
            }
            else
            {
                btnSave.Visible = false;
            }
        }
        private void RetailGridValues()
        {
            dtControls = (DataTable)ViewState["dtControls"];
            dtControlProperties = (DataTable)ViewState["dtControlProperties"];

            int i = 0;
            int j = 0;
            foreach (GridViewRow gvRow in gvDynamicControls.Rows)
            {
                DataRow drSelected = dtControls.Rows[i];

                TextBox txtLabelName = (TextBox)gvRow.FindControl("txtLabelName");
                TextBox txtControlId = (TextBox)gvRow.FindControl("txtControlId");
                TextBox txtRowGroup = (TextBox)gvRow.FindControl("txtRowGroup");
                TextBox txtColWidth = (TextBox)gvRow.FindControl("txtColWidth");
                TextBox txtControlOrder = (TextBox)gvRow.FindControl("txtControlOrder");

                drSelected["LabelName"] = txtLabelName.Text;
                drSelected["ControlId"] = txtControlId.Text;
                drSelected["RowGroup"] = txtRowGroup.Text;
                drSelected["ColWidth"] = txtColWidth.Text;
                drSelected["ControlOrder"] = txtControlOrder.Text;

                GridView gvControlProperties = (GridView)gvRow.FindControl("gvControlProperties");
                foreach (GridViewRow gvRowprop in gvControlProperties.Rows)
                {
                    DataRow drSelectedProperties = dtControlProperties.Rows[j];
                    TextBox txtDefaultValue = (TextBox)gvRowprop.FindControl("txtDefaultValue");

                    drSelectedProperties["ReportDefaultValue"] = txtDefaultValue.Text;

                    j++;
                }
                i++;
            }
        }
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                RetailGridValues();

                DataRow[] drSelected = dtControls.Select("ControlId='" + txtControlId.Text + "'");


                if (drSelected.Length == 0)
                {
                    int RowIdControl = Convert.ToInt32(dtControls.AsEnumerable()
                                   .Max(row => row["RowId"]));
                    RowIdControl = RowIdControl + 1;

                    DataRow drNew = dtControls.NewRow();
                    drNew["RowId"] = RowIdControl;
                    drNew["ReportId"] = 0;
                    drNew["ReportDetailsId"] = 0;
                    drNew["ControlTypeId"] = drpControlType.SelectedValue;
                    drNew["ControlName"] = drpControlType.SelectedItem.Text;
                    drNew["LabelName"] = txtLabelName.Text;
                    drNew["ControlId"] = txtControlId.Text;
                    drNew["RowGroup"] = txtRowGroup.Text;
                    drNew["ColWidth"] = txtColWidth.Text;
                    drNew["ControlOrder"] = txtDisplayOrder.Text;
                    dtControls.Rows.Add(drNew);


                    foreach (ListItem item in lstControlProperties.Items)
                    {
                        if (item.Selected == true)
                        {
                            int RowIdProp = Convert.ToInt32(dtControlProperties.AsEnumerable()
                                .Max(row => row["RowId"]));
                            RowIdProp = RowIdProp + 1;

                            DataRow drPropertiesNew = dtControlProperties.NewRow();
                            string[] valueSplit = item.Value.Split('-');

                            drPropertiesNew["RowId"] = RowIdProp;
                            drPropertiesNew["dtControlsRowId"] = RowIdControl;
                            drPropertiesNew["ReportConfigurationId"] = 0;
                            drPropertiesNew["ReportSettingTypeId"] = valueSplit[1];
                            drPropertiesNew["ReportSettingTypeDetailsId"] = valueSplit[0];
                            drPropertiesNew["ReportSettingName"] = item.Text;
                            drPropertiesNew["ReportDefaultValue"] = ""; 
                            string DefaultDefaultValueEnable = "false";
                            if (drPropertiesNew["ReportSettingTypeId"].ToString() == "2")
                                DefaultDefaultValueEnable = "true";
                            drPropertiesNew["DefaultValueEnable"] = DefaultDefaultValueEnable;
                            dtControlProperties.Rows.Add(drPropertiesNew);
                        }
                    }
                }

                ToastrCall("success", "Control Added Successfully");
                BindControlToGrid();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            SaveReport();
        }
        private void SaveReport()
        {
            try
            {
                RetailGridValues();

                StringWriter swReportControls = new StringWriter();
                StringWriter swReportProperties = new StringWriter();
                dtControls.WriteXml(swReportControls);
                dtControlProperties.WriteXml(swReportProperties);

                rptConfiguration = new rptConfiguration();
                rptConfiguration.Flag = 0;
                rptConfiguration.pki_ReportId = int.Parse(hdnReportId.Value);
                rptConfiguration.i_Type = int.Parse(drpType.SelectedValue);
                rptConfiguration.vc_ReportName = txtReportName.Text;
                rptConfiguration.vc_ReportDescription = txtReportDescription.Text;
                rptConfiguration.vc_ReportQuery = txtReportQuery.Text;
                rptConfiguration.vc_InsertQuery = txtInsertQuery.Text;
                rptConfiguration.vc_UpdateQuery = txtUpdateQuery.Text;
                rptConfiguration.ReportControls = swReportControls.ToString();
                rptConfiguration.ReportProperties = swReportProperties.ToString();
                rptConfiguration.fki_CreatedBy = 1;
                rptConfiguration.ReportConfiguration_Save();


                if (rptConfiguration.pki_ReportId > 0)
                {
                    ToastrCall("success", "Saved Successfully");
                    if (int.Parse(hdnReportId.Value) == 0)
                        Response.Redirect("ReportConfiguration.aspx?ReportId=" + rptConfiguration.pki_ReportId.ToString());
                }
                else
                {
                    throw new Exception(rptConfiguration.OutputMessage);
                }
            }
            catch (Exception ex)
            {
                ToastrCall("error", "Error: " + ex.Message);
            }
        }

        protected void drpType_SelectedIndexChanged(object sender, EventArgs e)
        {
            visibilityControls();
        }
        private void visibilityControls()
        {
            if (drpType.SelectedValue == "1")
            {
                divInsertQuery.Visible = false;
                divUpdatetQuery.Visible = false;
            }
            else
            {
                divInsertQuery.Visible = true;
                divUpdatetQuery.Visible = true;
            }
        }
    }
}