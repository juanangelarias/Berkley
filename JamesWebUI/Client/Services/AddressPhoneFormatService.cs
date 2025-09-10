using Blazored.LocalStorage;
using James.Shared.Data;
using James.Shared.Model;
using JamesWebUI.Client.Shared;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace JamesWebUI.Client.Services
{
    public partial class AddressPhoneFormatService(
        IDataAccess dataAccess,
        ILocalStorageService localStorageService)
    {

        private IDataAccess DataAccess { get; init; } = dataAccess;
        private ILocalStorageService LocalStorageService { get; init; } = localStorageService;


        private IDataAccessResult<List<CountryDm>> _loadResult = null!;
        private List<CountryDm> _countries = null!;
        private Task? _countryLoadTask;
        private async Task<List<CountryDm>> GetCountryList()
        {
            {
                if (_countries == null!)
                    await (_countryLoadTask ??= DataAccess.GetCacheOrLoadDataAsync(CountriesLoadItem()));
                return _countries!;
            }
        }
        private LoadItem CountriesLoadItem() =>
            new()
            {
                Key = "Countries",
                AsyncLoadTask = async () => _loadResult = await DataAccess.GetAllCountries(),
                CacheLoadTask = cache => _loadResult = new DataAccessResult<List<CountryDm>> { Data = (List<CountryDm>)cache! },
                ResultVariable = () => _loadResult,
                CacheDuration = TimeSpan.FromDays(1),
                AfterLoad = () =>
                {
                    Debug.Assert(_loadResult.Data != null);
                    //Add after load code here
                    _countries = _loadResult.Data;
                    //Save to local storage asynchronously and don't wait for the save to finish
                    Task.Factory.StartNew(data =>
                        LocalStorageService.SetItemAsyncWithExpiry("Countries", TimeSpan.FromDays(1), data), _countries);
                }
            };

        public async ValueTask<string> FormatPhoneNumberForCountryAsync(PhoneNumber phoneNumber)
        {
            var countryList = await GetCountryList();
            var country = countryList.FirstOrDefault(c => c.PhoneCode == phoneNumber.CountryCode)
                          ?? countryList.First(c => c.Code == "US");
            var phoneMask = country.PhoneMask;
            var mainNumberStripped = NonDigitPattern().Replace(phoneNumber.MainNumber, "");
            var sourceCharIndex = 0;
            var formattedChars = new char[phoneMask.Length];
            for (var maskIndex = 0; maskIndex < phoneMask.Length; maskIndex++)
            {
                if (phoneMask[maskIndex] == 'D')
                {
                    //Must check that the format doesn't ask for more numbers than are contained in mainNumberStripped
                    if (sourceCharIndex >= mainNumberStripped.Length)
                    {
                        sourceCharIndex = -1;//Set to trigger garbage in/garbage out below
                        break;
                    }
                    formattedChars[maskIndex] = mainNumberStripped[sourceCharIndex++];
                }
                //NOTE: Displaying a '-' in the phone mask as a space is a formatting choice by Stefanie, our UX designer
                else if (phoneMask[maskIndex] == '-')
                    formattedChars[maskIndex] = ' ';
                else
                    formattedChars[maskIndex] = phoneMask[maskIndex];
            }
            var preExtensionNumber = sourceCharIndex == mainNumberStripped.Length ? new string(formattedChars) :
                    phoneNumber.MainNumber;
            var withExtensionNumber = string.IsNullOrWhiteSpace(phoneNumber.Extension) ? preExtensionNumber : $"{preExtensionNumber} ext. {phoneNumber.Extension}";
            //If not US/Canada, include country code
            return country.Code == "US" || phoneNumber.CountryCode == "1" ? withExtensionNumber : $"+{phoneNumber.CountryCode} {withExtensionNumber}";
        }

        /// <summary>
        /// Generates a properly formatted postal code for a country
        /// </summary>
        /// <param name="address">address that contains the postal code and country.  If country is not include the US is assumed</param>
        /// <returns>Properly formatted postal code</returns>
        public async ValueTask<string> FormatPostalCodeForCountryAsync(Address address)
        {
            // ReSharper disable once ConditionalAccessQualifierIsNonNullableAccordingToAPIContract
            if (null == address?.PostalCode) return string.Empty;
            var countryList = await GetCountryList();
            var country = countryList.FirstOrDefault(c => c.Code == address.StateCodeNavigation?.CountryCode)
                          ?? countryList.First(c => c.Code == "US");
            var postalCodeMasks = country.PostalCodeMask.Split(" or ");
            var mainCodeStripped = NonAlphaNumericPattern().Replace(address.PostalCode, "").ToUpperInvariant();
            var postalCodeMask = postalCodeMasks.Length == 1 ? postalCodeMasks[0] :
                postalCodeMasks.FirstOrDefault(pcm => GetRegexForPostalCodeMask(pcm).IsMatch(address.PostalCode));
            if (null == postalCodeMask)
                //Postal code does not match what should be there, so garbage in, garbage out
                return address.PostalCode;

            var sourceCharIndex = 0;
            var formattedChars = new char[postalCodeMask.Length];
            for (var maskIndex = 0; maskIndex < postalCodeMask.Length && sourceCharIndex < mainCodeStripped.Length; maskIndex++)
            {
                if (postalCodeMask[maskIndex] == 'A')
                {
                    Debug.Assert(mainCodeStripped[sourceCharIndex] is < '0' or > '9', "Source should be a letter");
                    formattedChars[maskIndex] = mainCodeStripped[sourceCharIndex++];
                }
                else if (postalCodeMask[maskIndex] == 'N')
                {
                    Debug.Assert(mainCodeStripped[sourceCharIndex] is >= '0' and <= '9', "Source should be a digit");
                    formattedChars[maskIndex] = mainCodeStripped[sourceCharIndex++];
                }
                else
                    formattedChars[maskIndex] = postalCodeMask[sourceCharIndex];
            }

            return sourceCharIndex == mainCodeStripped.Length ? new string(formattedChars) :
                //Garbage in, garbage out
                address.PostalCode;
        }

        /// <summary>
        /// Get a string of the standard final line of an address based on the country
        /// </summary>
        /// <returns>Final address line for the country</returns>
        /// <remarks>If the country is unknown, US is assumed.
        /// If the country has an unsupported format, it will be formatted as a US address with "|Address formatting not supported for this country" appended to the result</remarks>
        public async ValueTask<string> GetAddressFinalLineAsync(Address address)
        {
            var countryCode = address.StateCodeNavigation?.CountryCode ?? "US";
            var country = address.StateCodeNavigation?.CountryCodeNavigation ??
                          (await GetCountryFromCodeAsync(countryCode));
            //HACK: Hardcoding db values isn't typically good practice, but is good enough here
            switch (country.AddressFinalLineFormat)
            {
                case "City, State PostalCode":
                    return $"{address.City}, {address.StateCode??"xx"} {FormatPostalCodeForCountryAsync(address).Result}";
                case "City, PostalCode":
                    return $"{address.City}, {(await FormatPostalCodeForCountryAsync(address))}";
                case "City PostalCode":
                    return $"{address.City} {(await FormatPostalCodeForCountryAsync(address))}";
                case "PostalCode City":
                    return $"{(await FormatPostalCodeForCountryAsync(address))} {address.City}";
                case "PostalCode City, State":
                    return $"{(await FormatPostalCodeForCountryAsync(address))} {address.City}, {address.StateCode}";
                default:
                    return $"{address.City}, {address.StateCode}  {FormatPostalCodeForCountryAsync(address).Result}|Address formatting not supported for this country";
            }
        }

        /// <summary>
        /// Get full CountryDM object from country code
        /// </summary>
        /// <param name="countryCode"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public async ValueTask<CountryDm> GetCountryFromCodeAsync(string countryCode)
        {
            if (countryCode.Length != 2)
                throw new FormatException("countryCode must be 2 characters.");
            //await LoadCountryData();
            //Debug.Assert(CountryList != null, nameof(CountryList) + " != null");
            var countryList = await GetCountryList();
            return countryList.FirstOrDefault(c => c.Code == countryCode)
                   ?? countryList.First(c => c.Code == "US") ;
        }

        //private bool _loadingCountries = false;
        //private readonly ILoggingService _logger = logger;


        public async Task<string> GetFullCountryNameFromCode(string countryCode)
        {
            if (countryCode.Length != 2)
                throw new FormatException("countryCode must be 2 characters.");
            return (await GetCountryFromCodeAsync(countryCode)).Name;
        }

        private static readonly Dictionary<string, Regex> _postalCodeRegexCache = new();
        private static Regex GetRegexForPostalCodeMask(string mask)
        {
            if (_postalCodeRegexCache.TryGetValue(mask, out var postalCodeMask))
                return postalCodeMask;
            var sb = new StringBuilder("^");
            foreach (char c in mask.ToUpperInvariant())
                sb.Append(c switch
                {
                    'A' => @"[A-Z]",
                    'N' => @"[0-9]",
                    ' ' => " ?",
                    '-' => @"\-?",
                    _ => c.ToString() + "?"
                });
            sb.Append("$");
            return _postalCodeRegexCache[mask] = new Regex(sb.ToString());
        }

        [GeneratedRegex(@"[^A-Za-z0-9]")]
        private static partial Regex NonAlphaNumericPattern();
        [GeneratedRegex(@"\D")]
        private static partial Regex NonDigitPattern();
    }
}
