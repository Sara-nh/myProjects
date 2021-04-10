using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clic_Works
{
    class LoginStatus
    {
        private static string userStat="";

        public static string Users
        {
            get { return userStat; }
            set { userStat = value; }
        }
     

    }
}
