using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared.Model
{
    public partial class Account
    {
        public Address? MainAddress
        {
            get
            {
                return this.IdNavigation?.LegalEntityAddresses?.Where(a => a.Type == "Main")?.FirstOrDefault()?.Address;
            }
            set
            {
                this.MainAddress = value;
            }
        }
    }
}
