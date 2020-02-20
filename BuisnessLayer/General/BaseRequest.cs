using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
namespace BusinessLayer.General
{
    public class BaseRequest
    {
        public int BaseUserId;
        public string BaseConnectionString;
        public string Action = "";
        public string BaseUserGrade = "U";
        public DateTime LogindateTime;
        public String BaseUserName;
        public String BaseUserfirstName = "";
        public long LoggedID = 0;
        public String sessionID = "";
        public DataTable PermissedMenus = null;
        public String ScrollText = "";
        public int RoleID;
        public String BaseCompany = "";
        public String flg;
        public String flgOut;
        public String BaseEmirates = "";
        public int BaseCompanyID;
        public String BasePassword = "";
        public String BaseEmail = "";
        public String BaseRoleName = "";
        public string FirstLogin = "2";

        public int BaseOutletId;
        public string BaseOutletName;
        public string BaseOutletAddress;
        public string BaseOutletPOBox = "";
        public string BaseOutletEmail = "";
        public string BaseOutletPhone = "";
        public string BaseOutletMobile = "";
        public string BaseOutletWebSite = "";
        public string BaseOutletHeaderImage = "";
        public string OutletOracleOrgCode = "";
        public string OutletOracleInvCode = "";
        public int BaseOutletRenewMin;
        public int BaseOutletRenewMax;

        public DataTable dtAccessableOutlets = null;

    }
}
