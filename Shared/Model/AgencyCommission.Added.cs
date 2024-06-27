using HotChocolate;
using System.ComponentModel.DataAnnotations.Schema;

namespace James.Shared.Model
{
    public partial class AgencyCommission:ITieredValue<double>
    {
        [NotMapped]
        [GraphQLIgnore]
        public double Value
        {
            get => Rate;
            set => Rate = value;
        }
    }
}
