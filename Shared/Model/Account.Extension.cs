using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared.Model
{
    public partial class Account
    {
        [ForeignKey("Id")]
        public Address? MainAddress
        {
            get
            {
                return this.IdNavigation?.LegalEntityAddresses?.Where(a => a.Type == "Main")?.FirstOrDefault()?.Address;
            }
            set
            {
                if (value != null)
                {
                    var lea = IdNavigation?.LegalEntityAddresses?.SingleOrDefault(l => l.Type == "Main");
                    //TODO: handle value == null
                    Debug.Assert(value != null, nameof(value) + " != null");
                    if (null == lea)
                    {
                        if ((value?.Id ?? Guid.Empty) == Guid.Empty)
                        {
                            value!.Id = Guid.NewGuid();
                        }

                        var newLea = new LegalEntityAddress
                        {
                            Type = "Main",
                            AddressId = value!.Id,
                            Address = value
                        };
                        lea = newLea;
                        if (IdNavigation == null)
                        {
                            IdNavigation = new LegalEntity();
                        }
                        IdNavigation.LegalEntityAddresses?.Add(lea);
                    }

                    lea.Address = value;
                }
            }
        }
    }
}
