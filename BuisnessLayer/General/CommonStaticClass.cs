using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace BuisnessLayer.General
{
    public static class CommonStaticClass
    {
        public static void enableUserAccessibleGrid(this GridView gv)/*For enabling data grid property of bootstrap for gridview*/
        {
            if (gv.Rows.Count > 0)
            {
                gv.UseAccessibleHeader = true;
                gv.HeaderRow.TableSection = TableRowSection.TableHeader;
                gv.FooterRow.TableSection = TableRowSection.TableFooter;
            }
            //return gv;
        }
    }
}
