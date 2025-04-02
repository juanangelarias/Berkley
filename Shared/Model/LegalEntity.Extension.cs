using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared.Model
{
    public partial class LegalEntity
    {
        public string Get2LetterInitialsOfIndividual() =>
            $"{((GivenName?? string.Empty).Length > 0 ? GivenName!.ToUpper()[0] : " ")}{((FamilyName?? string.Empty).Length > 0 ? FamilyName!.ToUpper()[0] : " ")}";

        public Address? GetMainAddress() => LegalEntityAddresses.FirstOrDefault(lea => lea.Type == "Main")?.Address;
        public PhoneNumber? GetMainPhoneNumber() => LegalEntityPhones.FirstOrDefault(lep => lep.Type == "Main")?.PhoneNumber;
        public string? GetMainEmail() => LegalEntityEmails.FirstOrDefault(lee => lee.Type == "Main")?.EmailAddress;
    }
}
