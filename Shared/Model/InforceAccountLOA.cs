using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared.Model
{
    public partial class InforceAccountLOA
    {
        public string AccountNum { get; set; } = null!;
        public LineOfAuthorityLog? ContractLOA { get; set; }
        public LineOfAuthorityLog? CommercialLOA { get; set; }
    }
}
