using BuisnessLayer.Reports;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BuisnessLayer.Transactions
{
    public class DynamicControls
    {
        ReportClass reportClass = new ReportClass();
        string conString = "";
        public void BindDynamciControlsToPH(DataSet ds, PlaceHolder phDynamicControls)
        {
            int colWidth = 0;
            int colWidthThis = 0;
            int rowGroup = 0;
            int rowGroupPrev = 0;

            System.Web.UI.HtmlControls.HtmlGenericControl mainDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
            System.Web.UI.HtmlControls.HtmlGenericControl childDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
            System.Web.UI.HtmlControls.HtmlGenericControl childDiv1 = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
            Label lbl = new Label();

            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                //hdnQuery.Value = dt.Rows[0]["vc_ReportQuery"].ToString();
                conString = dt.Rows[0]["vc_DBConnection"].ToString();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow drSelected = dt.Rows[i];

                    rowGroup = int.Parse(drSelected["i_RowGroupOrder"].ToString());
                    if (rowGroup != rowGroupPrev)
                    {
                        mainDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
                        mainDiv.ID = "mainDiv_" + i;
                        mainDiv.Attributes["class"] = "row col-md-12";
                        mainDiv.Attributes["style"] = "padding-bottom:5px";
                        phDynamicControls.Controls.Add(mainDiv);
                        colWidth = 0;
                    }

                    if (colWidth == 0 || colWidth >= 12)
                    {
                        childDiv = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
                        childDiv.ID = "childDiv_" + i;
                        childDiv.Attributes["class"] = "row col-md-12";
                        mainDiv.Controls.Add(childDiv);
                    }
                    colWidthThis = int.Parse(drSelected["i_ColWidth"].ToString());
                    colWidth += colWidthThis;
                    childDiv1 = new System.Web.UI.HtmlControls.HtmlGenericControl("DIV");
                    childDiv1.ID = "childDiv1_" + i;
                    childDiv1.Attributes["class"] = "col-md-" + colWidthThis;

                    lbl = new Label();
                    lbl.ID = "lblDynamicCntrlText" + i;
                    lbl.Text = drSelected["vc_LabelName"].ToString();

                    var controlType = typeof(Control).Assembly.GetType(drSelected["vc_ControlType"].ToString(), true);
                    Control control = (Control)Activator.CreateInstance(controlType);
                    control.ID = drSelected["vc_FormControlId"].ToString();
                    SetReportControlSettings(control, drSelected, ds.Tables[1]);

                    childDiv1.Controls.Add(lbl);
                    childDiv1.Controls.Add(control);
                    childDiv.Controls.Add(childDiv1);

                    rowGroupPrev = int.Parse(drSelected["i_RowGroupOrder"].ToString());
                }
            }

            

        }
        public void SetReportControlSettings(Control control, DataRow drSelected, DataTable dtConfiguratons)
        {
            try
            {
                string cssClass = "";

                DataRow[] dataSelected = dtConfiguratons.Select("pki_ReportDetailsId=" + drSelected["pki_ReportDetailsId"]);

                if (dataSelected.Length > 0)
                {
                    var css = dataSelected.Where(r => r.Field<int>("fki_ReportSettingTypeId") == 1)
                                   .Select(r => r.Field<string>("vc_ReportSettingValue"))
                                   .AsEnumerable();
                    cssClass = String.Join(" ", css);/*Merge all CSS to string*/


                    List<DefaultValueDTO> defaultValueList = dataSelected.Where(r => r.Field<int>("fki_ReportSettingTypeId") == 2)
                                   .Select(r => new DefaultValueDTO { ReportSettingValue = r.Field<string>("vc_ReportSettingValue"), DefaultValue = r.Field<string>("vc_DefaultValue") })
                                   .ToList();
                    foreach (DefaultValueDTO item in defaultValueList)
                    {
                        if (control.GetType() == typeof(TextBox) || control.GetType() == typeof(DropDownList))
                        {
                            object[] userParameters = new object[] { control, item };
                            Type thisType = this.GetType();
                            MethodInfo theMethod = thisType.GetMethod(item.ReportSettingValue);
                            theMethod.Invoke(this, userParameters);
                        }
                    }
                }

                if (control.GetType() == typeof(TextBox))
                {
                    ((TextBox)control).CssClass = cssClass;
                    ((TextBox)control).Attributes["autocomplete"] = "off";
                }
                else if (control.GetType() == typeof(Label))
                {
                    ((Label)control).CssClass = cssClass;
                    ((Label)control).Text = drSelected["vc_LabelName"].ToString();
                }
                else if (control.GetType() == typeof(DropDownList))
                {
                    ((DropDownList)control).CssClass = cssClass;
                }

            }
            catch (Exception ex)
            {

            }
        }
        #region Callbacks for setting default values
        public void setDefault(Control control, DefaultValueDTO defaultValueDTO)/*Default value for textbox field*/
        {
            ((TextBox)control).Text = defaultValueDTO.DefaultValue;
        }
        public void setDatetime(Control control, DefaultValueDTO defaultValueDTO)/*Default value for date field*/
        {
            int daystoChange = 0;
            int.TryParse(defaultValueDTO.DefaultValue, out daystoChange);
            ((TextBox)control).Text = DateTime.Now.AddDays(daystoChange).ToString("dd-MM-yyyy");
        }
        public void setDropdown(Control control, DefaultValueDTO defaultValueDTO)/*Default value for Dropdown field*/
        {
            DropDownList drp = ((DropDownList)control);

            ReportDTO reportDTO = new ReportDTO();
            reportDTO.ReportQuery = defaultValueDTO.DefaultValue;
            reportDTO.ConnectionString = conString;

            reportClass = new ReportClass();
            reportClass.ReportDTO = reportDTO;
            DataTable dt = reportClass.ReportDynamicQuery_Execute();
            drp.DataSource = dt;
            drp.DataValueField = "ValueField";
            drp.DataTextField = "TextField";
            drp.DataBind();
        }
        #endregion

        public void FindControls(Control c, List<Control> controlList)
        {
            foreach (Control cntrl in c.Controls)
            {
                controlList.Add(cntrl);
                if (cntrl.Controls.Count > 0)
                    FindControls(cntrl, controlList);
            }
        }
        public void GenerateReportParameters(List<Control> controlList, List<ReportParameters> reportParamList)
        {
            if (controlList.Count > 0)
            {
                reportClass = new ReportClass();
                List<Control> parameterControls = controlList.Where(x => !x.ID.ToLower().Contains("div")).ToList();               
                foreach (Control cntrl in parameterControls)
                {
                    bool isAddParameter = false;
                    ReportParameters reportParameter = new ReportParameters();
                    reportParameter.ParameterText = "#" + cntrl.ID + "#";

                    if (cntrl.GetType() == typeof(TextBox))
                    {
                        TextBox txt = ((TextBox)cntrl);
                        reportParameter.ParameterValue = txt.Text;

                        if (txt.CssClass.Contains("datepicker"))
                        {
                            reportParameter.ParameterValue = reportClass.MakeDate(reportParameter.ParameterValue, "dd-MM-yyyy", "MM/dd/yyyy").ToString("MM/dd/yyyy");
                        }
                        isAddParameter = true;
                    }
                    else if (cntrl.GetType() == typeof(DropDownList))
                    {
                        DropDownList drp = ((DropDownList)cntrl);
                        reportParameter.ParameterValue = drp.SelectedValue;
                        isAddParameter = true;
                    }

                    if (isAddParameter)
                        reportParamList.Add(reportParameter);
                }
            }
        }

        public void SetDynamicControlValues(List<Control> controlList, DataTable dtValues)
        {
            try
            {
                if (dtValues.Rows.Count > 0)
                {
                    DataRow drValue = dtValues.Rows[0];

                    List<Control> parameterControls = controlList.Where(x => !x.ID.ToLower().Contains("div")).ToList();
                    foreach (Control cntrl in parameterControls)
                    {
                        ReportParameters reportParameter = new ReportParameters();
                        reportParameter.ParameterText = "#" + cntrl.ID + "#";

                        if (cntrl.GetType() == typeof(TextBox))
                        {
                            TextBox txt = ((TextBox)cntrl);
                            txt.Text = drValue["#" + cntrl.ID + "#"].ToString();

                            if (txt.CssClass.Contains("datepicker"))
                            {
                                txt.Text = DateTime.Parse(txt.Text).ToString("dd-MM-yyyy");
                            }
                        }
                        else if (cntrl.GetType() == typeof(DropDownList))
                        {
                            DropDownList drp = ((DropDownList)cntrl);
                            drp.SelectedValue = drValue["#" + cntrl.ID + "#"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
            public string GenerateDynamicQuery(string query, List<ReportParameters> ReportParamList)
        {
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.Append(query);
            foreach (ReportParameters item in ReportParamList)
            {
                queryBuilder.Replace(item.ParameterText, item.ParameterValue);
            }
            return queryBuilder.ToString();
        }
    }
}
