using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Web;
namespace BusinessLayer.General
{
    public class Common : Base
    {
        public List<string> lstheaderToBeHide;

        public void gridViewHideColumns(GridView gv)
        {
            try
            {

                List<string> lstheaderNames = new List<string>();
                List<int> lstcolumnToBeHide = new List<int>();
                for (int i = 0; i < gv.Columns.Count; i++)
                {
                    lstheaderNames.Add(gv.HeaderRow.Cells[i].Text.ToLower());
                }
                foreach (string headerToBeHide in lstheaderToBeHide)
                {
                    int position = lstheaderNames.IndexOf(headerToBeHide.ToLower());
                    if (position >= 0)
                        lstcolumnToBeHide.Add(position);
                }
                foreach (int columnPosition in lstcolumnToBeHide)
                {
                    //gv.HeaderRow.Cells[columnPosition].Visible = false;
                    gv.Columns[columnPosition].Visible = false;
                }
            }
            catch (Exception ex) { throw ex; }
        }


        public string DataTableToCSV(DataTable datatable, char seperator, bool isHeadingRequired)
        {
            StringBuilder sb = new StringBuilder();
            if (isHeadingRequired)
            {
                for (int i = 0; i < datatable.Columns.Count; i++)
                {
                    sb.Append(datatable.Columns[i]);
                    if (i < datatable.Columns.Count - 1)
                        sb.Append(seperator);
                }
                sb.AppendLine();
            }
            foreach (DataRow dr in datatable.Rows)
            {
                for (int i = 0; i < datatable.Columns.Count; i++)
                {
                    sb.Append(dr[i].ToString());

                    if (i < datatable.Columns.Count - 1)
                        sb.Append(seperator);
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }
        public static void enableUserAccessibleGrid(GridView gv)/*For enabling data grid property of bootstrap for gridview*/
        {
            if (gv.Rows.Count > 0)
            {
                gv.UseAccessibleHeader = true;
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        public DateTime DateRoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }

        /*Retain page search*/
        public class SearchCriteria
        {
            public string pagename;
            public string id;
            public string value;
        }
        public List<SearchCriteria> generateSearchCriteriaIds(string pagename, string ids)
        {
            string[] idArray = ids.Split(',');
            List<SearchCriteria> objSearchCriteriaCol = new List<SearchCriteria>();
            SearchCriteria objSearchCriteria = new SearchCriteria();
            for (int i = 0; i < idArray.Length; i++)
            {
                objSearchCriteria = new SearchCriteria();
                objSearchCriteria.pagename = pagename;
                objSearchCriteria.id = idArray[i];
                objSearchCriteriaCol.Add(objSearchCriteria);
            }
            return objSearchCriteriaCol;
        }
        public void updateSearchCriteria(ContentPlaceHolder cph, string pagename, List<SearchCriteria> objSearchCriteriaCol)
        {
            if (objSearchCriteriaCol.Count > 0)
            {
                if (pagename == objSearchCriteriaCol[0].pagename)
                {
                    foreach (SearchCriteria searchCriteria in objSearchCriteriaCol)
                    {
                        var obj = cph.FindControl(searchCriteria.id);
                        if (obj is TextBox)
                            searchCriteria.value = ((TextBox)obj).Text;
                        else if (obj is CheckBox)
                            searchCriteria.value = ((CheckBox)obj).Checked.ToString();
                        else if (obj is DropDownList)
                            searchCriteria.value = ((DropDownList)obj).SelectedValue;
                    }
                }
            }
        }
        public void setSearchCriteria(ContentPlaceHolder cph, string pagename, List<SearchCriteria> objSearchCriteriaCol)
        {
            if (objSearchCriteriaCol.Count > 0)
            {
                if (pagename == objSearchCriteriaCol[0].pagename)
                {
                    foreach (SearchCriteria searchCriteria in objSearchCriteriaCol)
                    {
                        var obj = cph.FindControl(searchCriteria.id);
                        if (obj is TextBox)
                            ((TextBox)obj).Text = searchCriteria.value;
                        else if (obj is CheckBox)
                            ((CheckBox)obj).Checked = bool.Parse(searchCriteria.value);
                        else if (obj is DropDownList)
                            ((DropDownList)obj).SelectedValue = searchCriteria.value;
                    }
                }
            }
        }

        public void bindOldSearch(ContentPlaceHolder cph, string pagename, string ids)
        {
            if (HttpContext.Current.Session["SearchCriteria"] == null)
            {
                List<SearchCriteria> objSearchCriteriaCol = new List<Common.SearchCriteria>();
                objSearchCriteriaCol = generateSearchCriteriaIds(pagename, ids);
                updateSearchCriteria(cph, pagename, objSearchCriteriaCol);
                HttpContext.Current.Session["SearchCriteria"] = objSearchCriteriaCol;
            }
            else
            {
                List<SearchCriteria> objSearchCriteriaCol = (List<SearchCriteria>)HttpContext.Current.Session["SearchCriteria"];
                if (objSearchCriteriaCol[0].pagename != pagename)
                {
                    HttpContext.Current.Session["SearchCriteria"] = null;
                    bindOldSearch(cph, pagename, ids);
                }
                setSearchCriteria(cph, pagename, objSearchCriteriaCol);
            }
        }
        public void updateOldSearch(ContentPlaceHolder cph, string pagename)
        {
            List<BusinessLayer.General.Common.SearchCriteria> objSearchCriteriaCol = (List<BusinessLayer.General.Common.SearchCriteria>)HttpContext.Current.Session["SearchCriteria"];
            updateSearchCriteria(cph, pagename, objSearchCriteriaCol);
            HttpContext.Current.Session["SearchCriteria"] = objSearchCriteriaCol;
        }
        /*Retain page search*/

        public List<DataTable> CloneTable(DataTable tableToClone, int countLimit)
        {
            List<DataTable> tables = new List<DataTable>();
            int count = 0;
            DataTable copyTable = null;
            foreach (DataRow dr in tableToClone.Rows)
            {
                if ((count++ % countLimit) == 0)
                {
                    copyTable = new DataTable();
                    // Clone the structure of the table.
                    copyTable = tableToClone.Clone();
                    // Add the new DataTable to the list.
                    tables.Add(copyTable);
                }
                // Import the current row.
                copyTable.ImportRow(dr);
            }
            return tables;
        }


        public int Rounding(int decimaValue)
        {
            if (decimaValue <= 12 && decimaValue > 0)
                return 0;
            else if (decimaValue > 12 && decimaValue <= 37)
                return 25;
            else if (decimaValue > 37 && decimaValue <= 62)
                return 50;
            else if (decimaValue > 62 && decimaValue <= 87)
                return 75;
            else if (decimaValue > 87)
                return 1;
            else
                return 0;
        }

        public string generateRandomValue(int length = 8, bool isSimple = false)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789@";
            if (isSimple)
                chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(
                Enumerable.Repeat(chars, length)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
        }
        public static string ToastrCall(string type, string message)
        {
            //if (message.Length > 50)
            //    message = message.Replace("'", "").Substring(0, 50);

            message = message.Replace("'", "").Replace("\r", " ").Replace("\n", " ");
            return "toastr.options = {'closeButton': true};toastr." + type + "('" + message + "');";
        }
    }


}
