using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                //UNDONE: Below is sample code for Bryan to clean up an finish
                var lea = IdNavigation.LegalEntityAddresses.SingleOrDefault(l=>l.Type== "Main");
                //TODO: handle value == null
                Debug.Assert(value != null, nameof(value) + " != null");
                if (null == lea)
                {
                    if ((value?.Id??Guid.Empty) == Guid.Empty)
                    {
                        value.Id  =  Guid.NewGuid();
                    }

                    var newLea = new LegalEntityAddress
                    {
                        Type = "Main",
                        AddressId = value.Id,
                        Address = value
                    };
                    lea = newLea;
                }

                lea.Address = value;
            }
        }
    }
}
