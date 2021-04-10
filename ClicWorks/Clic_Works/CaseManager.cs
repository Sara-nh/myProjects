using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clic_Works
{
    public class CaseManager
    {
        public CaseManager() { }

        public string CMFirstName { get; set; }

        public string CMLastName { get; set; }

        public int AgencyId { get; set; }

        public CaseManager (string FirstName, string LastName,int agnID )
        {
            CMFirstName = FirstName;
            CMLastName = LastName;
            AgencyId = agnID;
        }

        public override string ToString()
        {
            return CMFirstName + " " + CMLastName;
        }
     
    }
}
