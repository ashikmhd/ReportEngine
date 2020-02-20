using BuisnessLayer.Master;
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
    public partial class ReportOutletMap : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ReportId"] != null)
                hdnReportId.Value = Request.QueryString["ReportId"];

            if (!IsPostBack)
            {
                if (hdnReportId.Value != "0")
                    bindDetails();
            }
        }
        private void bindDetails()
        {
            try
            {
                MapReport mapReport = new MapReport();
                mapReport.Flag = 0;
                mapReport.fki_ReportId = int.Parse(hdnReportId.Value);
                DataTable dt = mapReport.MapReportOutlet_Select();
                if (dt.Rows.Count > 0)
                {
                    btnSaveMapping.Visible = true;

                    chkListOutlets.DataSource = dt;
                    chkListOutlets.DataValueField = "ValueField";
                    chkListOutlets.DataTextField = "vc_OutletName";
                    chkListOutlets.DataBind();
                    int i = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        chkListOutlets.Items[i].Selected = bool.Parse(dr["Active"].ToString());
                        i++;
                    }
                }
                else
                {
                    btnSaveMapping.Visible = false;
                    chkListOutlets.DataSource = null;
                    chkListOutlets.DataBind();
                }
            }
            catch (Exception ex)
            {
                ToastrCall("error", "Error: " + ex.Message);
            }
        }

        protected void btnSaveMapping_Click(object sender, EventArgs e)
        {
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                foreach (ListItem li in chkListOutlets.Items)
                {
                    if (li.Selected)
                    {
                        string[] valueSplt = li.Value.Split('-');
                        sb.Append("<Outlet Id=\"" + valueSplt[0] + "\"  PrimaryKey=\"" + valueSplt[1] + "\" />");
                    }
                }
                //if (sb.Length > 0)
                //{
                MapReport mapReport = new MapReport();
                mapReport.Flag = 0;
                mapReport.fki_ReportId = int.Parse(hdnReportId.Value);
                mapReport.OutletList = sb.ToString();
                mapReport.UserId = ((BaseRequest)Session["cache"]).BaseUserId;
                mapReport.MapReportOutlet_Save();

                ToastrCall("success", "Saved Successfully");
                bindDetails();
                //}
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
    }
}