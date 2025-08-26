using James.Shared;
using James.Shared.Data;
using James.Shared.Extensions;
using James.Shared.Model;
using JamesWebUI.Client.Helpers;
using JamesWebUI.Client.Model;
using Radzen;

namespace JamesWebUI.Client.States;

public interface IAccountState: IStateBase
{
    UserType UserType { get; set; }
    bool IsUserManager { get; set; }
    List<string> Types { get; set; }
    string SelectedType { get; set; }
    string? AccountNumber { get; set; }
    Account? Account { get; set; }
    List<BsgAddress> Addresses { get; set; }
    List<BsgPhoneNumber> PhoneNumbers { get; set; }
    List<BsgEmail> Emails { get; set; }

    Task Initialize();
    Task SaveAddress(BsgAddress address, bool isNew = false);
    Task DeleteAddress(Guid addressId);
    Task SavePhoneNumber(BsgPhoneNumber phoneNumber);
    Task DeletePhoneNumber(Guid phoneNumberId);
    Task SaveEmail(BsgEmail email, bool isNew = false);
    Task DeleteEmail(Guid emailId);
}

public class AccountState(IDataAccess dataAccess, 
    IDataCache dataCache, 
    ILoggingService loggingService,
    NotificationService notificationService) 
    : StateBase(loggingService, notificationService), IAccountState
{
    private readonly IDataAccess _dataAccess = dataAccess;
    private readonly IDataCache _dataCache = dataCache;

    #region Field & Properties

    public UserType UserType { get; set; }
    public bool IsUserManager { get; set; }
    public List<string> Types { get; set; } = ["Default", "Commercial", "Contract"];
    public bool ShowDefault { get; set; }
    public bool ShowCommercial { get; set; }
    public bool ShowContract { get; set; }
    public string? AccountNumber { get; set; }
    public Account? Account { get; set; }

    #region SelectedType

    private string _selectedType = "Commercial";

    public string SelectedType
    {
        get => _selectedType;
        set
        {
            _selectedType = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Addresses

    private List<BsgAddress> _addresses = [];

    public List<BsgAddress> Addresses
    {
        get
        {
            var n = 0;
            while (!_isAddressesLoaded)
            {
                n += 1;
                if (n > 5)
                    return [];

                Task.Delay(3000).Wait();
            }

            return _addresses;
        }
        set
        {
            _addresses = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region PhoneNumbers

    private List<BsgPhoneNumber> _phoneNumbers = [];

    public List<BsgPhoneNumber> PhoneNumbers
    {
        get 
        {
            var n = 0;
            while (!_isPhoneNumbersLoaded)
            {
                n += 1;
                if (n > 5)
                    return [];

                Task.Delay(3000).Wait();
            }

            return _phoneNumbers;
        }
        set
        {
            _phoneNumbers = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Emails

    private List<BsgEmail> _emails = [];

    public List<BsgEmail> Emails
    {
        get
        {
            var n = 0;
            while (!_isEmailsLoaded)
            {
                n += 1;
                if (n > 5)
                    return [];

                Task.Delay(3000).Wait();
            }

            return _emails;
        }
        set
        {
            _emails = value;
            OnPropertyChanged();
        }
    }

    #endregion

    private bool _isAddressesLoaded;
    private bool _isPhoneNumbersLoaded;
    private bool _isEmailsLoaded;

    #endregion

    #region Loaders

    #region AddressTypes

    private IDataAccessResult<List<AddressTypeDm>> _addressTypesResult = null!;

    private LoadItem AddressTypesLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.AddressTypes,
        AsyncLoadTask = async () => _addressTypesResult = await _dataAccess.GetAddressTypes(),
        CacheLoadTask = cache => _addressTypesResult = new DataAccessResult<List<AddressTypeDm>>
        {
            Data = (List<AddressTypeDm>)cache!
        },
        ResultVariable = () => _addressTypesResult,
        AfterLoad = () => { }
    }, "address types");

    #endregion

    #region Countries

    private IDataAccessResult<List<CountryDm>> _countriesResult = null!;

    private LoadItem CountriesLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.Countries,
        AsyncLoadTask = async () => _countriesResult = await _dataAccess.GetAllCountries(),
        CacheLoadTask = cache => _countriesResult = new DataAccessResult<List<CountryDm>>
        {
            Data = (List<CountryDm>)cache!
        },
        ResultVariable = () => _countriesResult,
        AfterLoad = () => { }
    }, "countries");

    #endregion

    #region EmailTypes

    /*private IDataAccessResult<List<EmailTypeDm>> _emailTypesResult = null!;

    private LoadItem EmailTypesLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.EmailTypes,
        AsyncLoadTask = async () => _emailTypesResult = await _dataAccess.GetAllEmailTypes(),
        CacheLoadTask = cache => _emailTypesResult = new DataAccessResult<List<EmailTypeDm>>
        {
            Data = (List<EmailTypeDm>)cache!
        },
        ResultVariable = () => _emailTypesResult,
        AfterLoad = () => { }
    }, "email types");*/

    #endregion

    #region PhoneTypes

    private IDataAccessResult<List<PhoneTypeDm>> _phoneTypesResult = null!;

    private LoadItem PhoneTypesLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.PhoneTypes,
        AsyncLoadTask = async () => _phoneTypesResult = await _dataAccess.GetPhoneTypes(),
        CacheLoadTask = cache => _phoneTypesResult = new DataAccessResult<List<PhoneTypeDm>>
        {
            Data = (List<PhoneTypeDm>)cache!
        },
        ResultVariable = () => _phoneTypesResult,
        AfterLoad = () => { }
    }, "phone types");

    #endregion

    #region States

    private IDataAccessResult<List<State>> _statesResult = null!;

    private LoadItem StatesLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.States,
        AsyncLoadTask = async () => _statesResult = await _dataAccess.GetAllStates(),
        CacheLoadTask = cache => _statesResult = new DataAccessResult<List<State>>
        {
            Data = (List<State>)cache!
        },
        ResultVariable = () => _statesResult,
        AfterLoad = () => { }
    }, "states");

    #endregion
    
    #endregion

    public async Task Initialize()
    {
        // ToDo: With Greg define how to obtain the user type (Commercial or Contract)
        IsUserManager = true;
        UserType = UserType.Commercial;

        if (string.IsNullOrEmpty(AccountNumber))
            return;
        
        LoadDomainTables().Forget();
        await LoadAccount(true);
    }

    public async Task SaveAddress(BsgAddress address, bool isNew = false)
    {
        if (address.IsNew)
        {
            await _dataAccess.CreateAddress(address.Id, address.Address1, address.Address2,
                address.Address3, address.City, address.StateCode, address.PostalCode, Account!.Id,
                address.Type, "account address");
            
            Addresses.Add(address);
        }
        else
        {
            var newAddress = new Address
            {
                Id = address.Id,
                Address1 = address.Address1,
                Address2 = address.Address2,
                Address3 = address.Address3,
                City = address.City,
                StateCode = address.StateCode,
                PostalCode = address.PostalCode,
                LegalEntityAddress = new()
                {
                    LegalEntityId = Account!.Id,
                    Type = address.Type
                }
            };
            await _dataAccess.SetAddress(newAddress, "account address");
        }
        
    }

    public async Task DeleteAddress(Guid addressId)
    {
        if(addressId == Guid.Empty)
            return;

        await _dataAccess.DeleteAddress(addressId, "account address");
        
        var toRemove = Addresses.FirstOrDefault(a => a.Id == addressId);
        if (toRemove != null)
            Addresses.Remove(toRemove);
    }

    public async Task SavePhoneNumber(BsgPhoneNumber phoneNumber)
    {
        await _dataAccess.CreatePhoneNumber(phoneNumber.Id, phoneNumber.CountryCode, 
            phoneNumber.MainNumber, phoneNumber.Extension, Account!.Id, 
            phoneNumber.Type);
    }

    public async Task DeletePhoneNumber(Guid phoneNumberId)
    {
        if(phoneNumberId == Guid.Empty)
            return;
        
        await _dataAccess.DeletePhoneNumber(phoneNumberId);
        
        var toRemove = PhoneNumbers.FirstOrDefault(a => a.Id == phoneNumberId);
        if (toRemove != null)
            PhoneNumbers.Remove(toRemove);
    }

    public async Task SaveEmail(BsgEmail email, bool isNew = false)
    {
        throw new NotImplementedException();
    }
    
    public async Task DeleteEmail(Guid emailId)
    {
        throw new NotImplementedException();
    }
    
    private async Task LoadAccount(bool isMain = false)
    {
        if(string.IsNullOrEmpty(AccountNumber))
            return;

        var accountResult = await _dataAccess.GetAccountByNumber(AccountNumber!);
        if (!accountResult.Success)
        {
            NotifyLoadError(accountResult.Errors, "account", true);
        } 
        
        Account = accountResult.Data;
        
        if (isMain)
        {
            LoadAddresses().Forget();
            LoadPhoneNumbers().Forget();
            LoadEmails().Forget();
        }
    }

    private async Task LoadAddresses()
    {
        var addressesResult = await _dataAccess.GetAllLegalEntityAddresses(Account!.Id);
        if (!addressesResult.Success)
        {
            NotifyLoadError(addressesResult.Errors, "addresses", true);
        }

        Addresses = addressesResult.Data!.Select(s =>
                new BsgAddress(s.LegalEntityAddress!.Type, s))
            .ToList();
        _isAddressesLoaded = true;
    }

    private async Task LoadPhoneNumbers()
    {
        var phoneNumbersResult = await _dataAccess.GetAllLegalEntityPhoneNumbers(Account!.Id);
        if (!phoneNumbersResult.Success)
        {
            NotifyLoadError(phoneNumbersResult.Errors, "phone numbers", true);
        }

        PhoneNumbers = phoneNumbersResult.Data!
            .Select(s => new BsgPhoneNumber(s.LegalEntityPhone!.Type, s))
            .ToList();
        _isPhoneNumbersLoaded = true;
    }
    
    private async Task LoadEmails()
    {
        var emailsResult = await _dataAccess.GetAllLegalEntityEmails(Account!.Id);
        if (!emailsResult.Success)
        {
            NotifyLoadError(emailsResult.Errors, "emails", true);
        }

        Emails = emailsResult.Data!
            .Select(s=> new BsgEmail(s))
            .ToList();
        _isEmailsLoaded = true;
    }

    private async Task LoadDomainTables()
    {
        await _dataCache.ParallelGetCacheOrDataAsync(AddressTypesLoad, CountriesLoad, PhoneTypesLoad, StatesLoad);
    }
}