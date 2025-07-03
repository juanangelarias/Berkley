using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared.Model
{
    //TODO:Replace this with generated code next time it is generated
    public class SecurityRole
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public string Role { get; set; }

        public int Ord { get; set; }
        public string Description { get; set; }
    }

    //TODO:Replace this with generated code next time it is generated
    public class Security
    {
        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

        public Guid PrincipalId { get; set; }

        public string Role { get; set; }
    }
}
