using BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Master
{
    public class UserComponent:Base
    {
        private UserInfo objUser;
        public UserInfo User
        {
            get { return objUser; }
            set { objUser = value; }
        }
    }
}
