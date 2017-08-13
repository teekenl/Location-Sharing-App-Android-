using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDemo.Core.Models
{
    public class favourite_person
    {
        public string PersonName { get; set; }
        public string PersonRole { get; set; }

        public favourite_person() { }

        public favourite_person(string personName, string personRole)
        {
            this.PersonName = personName;
            this.PersonRole = personRole;

        }
    }
}
