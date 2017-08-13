using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Glados.Core.RecentDetails
{

    public class recent_person
    {
        public string PersonName { get; set; }
        public string PersonRole { get; set; }
        public string PersonLeaveTime { get; set; }

        public recent_person() { }

        public recent_person(string personName, string personRole, string personLeaveTime)
        {
            this.PersonName = personName;
            this.PersonRole = personRole;
            this.PersonLeaveTime = personLeaveTime;
        }

    }
}
