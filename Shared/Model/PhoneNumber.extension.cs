using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace James.Shared.Model
{
    public partial class PhoneNumber
    {
        public string GetFormatted(bool showCountryCode = false)
        {
            switch (CountryCode)
            {
                case "1":
                    //US or Canada phone number
                    return showCountryCode
                        ? $"+{CountryCode}-({MainNumber[0..2]}){MainNumber[3..5]}-{MainNumber[6..]}"
                        : $"({MainNumber[0..2]}){MainNumber[3..5]}-{MainNumber[6..]}";
                case "52":
                    //Mexico
                    return showCountryCode
                        ? $"+{CountryCode}-{MainNumber[0..1]}-{MainNumber[2..5]}-{MainNumber[6..]}"
                        : $"{MainNumber[0..1]}-{MainNumber[2..5]}-{MainNumber[6..]}";
                default:
                    throw new NotSupportedException($"Country code of {CountryCode} is not yet supported");
            }
        }
    }
}
