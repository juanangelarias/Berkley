using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared.Model
{
    public partial class AgencyCommission:ITieredValue<double>
    {
        public double Value
        {
            get => Rate;
            set => Rate = value;
        }
    }
}
