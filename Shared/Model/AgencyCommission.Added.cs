using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared.Model
{
    public partial class AgencyCommission:ITieredValue<double>
    {
        [NotMapped]
        public double Value
        {
            get => Rate;
            set => Rate = value;
        }
    }
}
