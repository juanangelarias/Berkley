using System.Text.RegularExpressions;

namespace James.Shared.Model
{
    public partial class Address
    {
        /// <summary>
        /// Get a string of the standard final line of an address
        /// </summary>
        /// <returns>City, ST FormattedPostalCode</returns>
        [Obsolete("UseAddressPhoneFormatService instead.")]
        public string GetCityZipLine() => $"{City}, {StateCode}, {GetFormattedPostalCode()}";//HACK: Not internationalized

        /// <summary>
        /// Appropriately formats postal codes according to country standards
        /// </summary>
        /// <returns>Formatted Postal Code</returns>
        [Obsolete("UseAddressPhoneFormatService instead.")]
        private string GetFormattedPostalCode()
        {
            //HACK: Currently only have logic for US zip codes.  Can add logic below
            if (null != PostalCode && (StateCodeNavigation?.CountryCode ?? "US")=="US" && 
                NineDigitPostalCodePattern().IsMatch(PostalCode))
                //Nine digit US zipcode needs a hyphen added.
                return PostalCode.Insert(5, "-");
            return PostalCode??"";
        }

        [GeneratedRegex(@"^\d{9}$")]
        private static partial Regex NineDigitPostalCodePattern();
    }
}
