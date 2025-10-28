using CoreOnion_Backend.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreOnion_Backend.Domain.Entities
{
    public class Endpoint : EntityBase
    {
        public Endpoint()
        {
            Roles = new HashSet<Role>();
        }
        public string ActionType { get; set; }
        public string HttpType { get; set; }
        public string Definition { get; set; }
        public string Code { get; set; }

        public Menu Menu { get; set; }
        public ICollection<Role> Roles { get; set; }
    }
}
