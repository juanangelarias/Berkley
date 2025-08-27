using James.Shared;
using James.Shared.Data;
using James.Shared.Extensions;
using James.Shared.Model;
using JamesWebUI.Client.Helpers;
using JamesWebUI.Client.Model;
using Radzen;

namespace JamesWebUI.Client.States;

public interface IAccountState : IStateBase
{
    UserType UserType { get; set; }
    bool IsUserManager { get; set; }
    List<string> Types { get; set; }
    AccountLayout? SelectedType { get; set; }
    string? AccountNumber { get; set; }
    Account? Account { get; set; }
    List<BsgAddress> Addresses { get; set; }
    List<BsgPhoneNumber> PhoneNumbers { get; set; }
    List<BsgEmail> Emails { get; set; }
    List<AddressTypeDm> AddressTypes { get; set; }
    List<CountryDm> Countries { get; set; }
    List<EmailTypeDm> EmailTypes { get; set; }
    List<PhoneTypeDm> PhoneNumberTypes { get; set; }
    List<State> States { get; set; }
    bool IsAccountLoaded { get; set; }
    List<WatchStatusDm> WatchStatuses { get; set; }
    List<AccountWatch> AccountWatches { get; set; }
    AccountWatch? LastAccountWatch { get; set; }

    Task<bool> Initialize();
    Task SaveAddress(BsgAddress address, bool isNew = false);
    Task DeleteAddress(Guid addressId);
    Task SavePhoneNumber(BsgPhoneNumber phoneNumber);
    Task DeletePhoneNumber(Guid phoneNumberId);
    Task SaveEmail(BsgEmail email, bool isNew = false);
    Task DeleteEmail(Guid emailId);
}

public class AccountState(
    IDataAccess dataAccess,
    IDataCache dataCache,
    ILoggingService loggingService,
    NotificationService notificationService)
    : StateBase(loggingService, notificationService), IAccountState
{
    #region Field & Properties

    public UserType UserType { get; set; }
    public bool IsUserManager { get; set; }
    public List<string> Types { get; set; } = ["Default", "Commercial", "Contract"];
    public string? AccountNumber { get; set; }
    public Account? Account { get; set; }
    public List<AddressTypeDm> AddressTypes { get; set; } = [];
    public List<CountryDm> Countries { get; set; } = [];
    public List<EmailTypeDm> EmailTypes { get; set; } = [];
    public List<PhoneTypeDm> PhoneNumberTypes { get; set; } = [];
    public List<State> States { get; set; } = [];
    public List<WatchStatusDm> WatchStatuses { get; set; } = [];
    public AccountLayout? SelectedType { get; set; }

    #region IsAccountLoaded

    private bool _isAccountLoaded;

    public bool IsAccountLoaded
    {
        get => _isAccountLoaded;
        set
        {
            _isAccountLoaded = value;
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

    #region AccountWatches

    private List<AccountWatch> _accountWatches = [];

    public List<AccountWatch> AccountWatches
    {
        get
        {
            var n = 0;
            while (!_isAccountWatchesLoaded)
            {
                n += 1;
                if (n > 5)
                    return [];

                Task.Delay(3000).Wait();
            }

            return _accountWatches;
        }
        set
        {
            _accountWatches = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region LastAccountWatch

    private AccountWatch? _lastAccountWatch;

    public AccountWatch? LastAccountWatch
    {
        get => _lastAccountWatch;
        set
        {
            _lastAccountWatch = value;
            OnPropertyChanged();
        }
    }

    #endregion

    private bool _isAddressesLoaded;
    private bool _isPhoneNumbersLoaded;
    private bool _isEmailsLoaded;
    private bool _isAccountWatchesLoaded;

    #endregion

    #region Loaders

    #region AddressTypes

    private IDataAccessResult<List<AddressTypeDm>> _addressTypesResult = null!;

    private LoadItem AddressTypesLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.AddressTypes,
        AsyncLoadTask = async () => _addressTypesResult = await dataAccess.GetAddressTypes(),
        CacheLoadTask = cache => _addressTypesResult = new DataAccessResult<List<AddressTypeDm>>
        {
            Data = (List<AddressTypeDm>)cache!
        },
        ResultVariable = () => _addressTypesResult,
        AfterLoad = () => { AddressTypes = _addressTypesResult.Data!; }
    }, "address types");

    #endregion

    #region Countries

    private IDataAccessResult<List<CountryDm>> _countriesResult = null!;

    private LoadItem CountriesLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.Countries,
        AsyncLoadTask = async () => _countriesResult = await dataAccess.GetAllCountries(),
        CacheLoadTask = cache => _countriesResult = new DataAccessResult<List<CountryDm>>
        {
            Data = (List<CountryDm>)cache!
        },
        ResultVariable = () => _countriesResult,
        AfterLoad = () => { Countries = _countriesResult.Data!; }
    }, "countries");

    #endregion

    #region EmailTypes

    private IDataAccessResult<List<EmailTypeDm>> _emailTypesResult = null!;

    private LoadItem EmailTypesLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.EmailTypes,
        AsyncLoadTask = async () => _emailTypesResult = await dataAccess.GetAllEmailTypes(),
        CacheLoadTask = cache => _emailTypesResult = new DataAccessResult<List<EmailTypeDm>>
        {
            Data = (List<EmailTypeDm>)cache!
        },
        ResultVariable = () => _emailTypesResult,
        AfterLoad = () => { EmailTypes = _emailTypesResult.Data!; }
    }, "email types");

    #endregion

    #region PhoneTypes

    private IDataAccessResult<List<PhoneTypeDm>> _phoneTypesResult = null!;

    private LoadItem PhoneTypesLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.PhoneTypes,
        AsyncLoadTask = async () => _phoneTypesResult = await dataAccess.GetPhoneTypes(),
        CacheLoadTask = cache => _phoneTypesResult = new DataAccessResult<List<PhoneTypeDm>>
        {
            Data = (List<PhoneTypeDm>)cache!
        },
        ResultVariable = () => _phoneTypesResult,
        AfterLoad = () => { PhoneNumberTypes = _phoneTypesResult.Data!; }
    }, "phone types");

    #endregion

    #region States

    private IDataAccessResult<List<State>> _statesResult = null!;

    private LoadItem StatesLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.States,
        AsyncLoadTask = async () => _statesResult = await dataAccess.GetAllStates(),
        CacheLoadTask = cache => _statesResult = new DataAccessResult<List<State>>
        {
            Data = (List<State>)cache!
        },
        ResultVariable = () => _statesResult,
        AfterLoad = () => { States = _statesResult.Data!; }
    }, "states");

    #endregion

    #region WatchStatuses

    private IDataAccessResult<List<WatchStatusDm>> _watchStatusesResult = null!;

    private LoadItem WatchStatusesLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.WatchStatuses,
        AsyncLoadTask = async () => _watchStatusesResult = await dataAccess.GetWatchStatusDms(),
        CacheLoadTask = cache => _watchStatusesResult = new DataAccessResult<List<WatchStatusDm>>
        {
            Data = (List<WatchStatusDm>)cache!
        },
        ResultVariable = () => _watchStatusesResult,
        AfterLoad = () => { WatchStatuses = _watchStatusesResult.Data!; }
    }, "watch statuses");

    #endregion

    #endregion

    public async Task<bool> Initialize()
    {
        // ToDo: With Greg define how to obtain the user type (Commercial or Contract)
        IsUserManager = true;
        UserType = UserType.Commercial;

        SelectedType = UserType switch
        {
            UserType.Commercial => AccountLayout.Commercial,
            UserType.Contract => AccountLayout.Contract,
            UserType.Default => AccountLayout.Default,
            UserType.Undefined => null,
            _ => throw new ArgumentOutOfRangeException()
        };
        

        if (string.IsNullOrEmpty(AccountNumber))
            return false;

        LoadDomainTables().Forget();
        return await LoadAccount(true);
    }

    public async Task SaveAddress(BsgAddress address, bool isNew = false)
    {
        if (address.IsNew)
        {
            await dataAccess.CreateAddress(address.Id, address.Address1, address.Address2,
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
            await dataAccess.SetAddress(newAddress, "account address");
        }
    }

    public async Task DeleteAddress(Guid addressId)
    {
        if (addressId == Guid.Empty)
            return;

        await dataAccess.DeleteAddress(addressId, "account address");

        var toRemove = Addresses.FirstOrDefault(a => a.Id == addressId);
        if (toRemove != null)
            Addresses.Remove(toRemove);
    }

    public async Task SavePhoneNumber(BsgPhoneNumber phoneNumber)
    {
        await dataAccess.CreatePhoneNumber(phoneNumber.Id, phoneNumber.CountryCode,
            phoneNumber.MainNumber, phoneNumber.Extension, Account!.Id,
            phoneNumber.Type);
    }

    public async Task DeletePhoneNumber(Guid phoneNumberId)
    {
        if (phoneNumberId == Guid.Empty)
            return;

        await dataAccess.DeletePhoneNumber(phoneNumberId);

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

    private async Task<bool> LoadAccount(bool isMain = false)
    {
        if (string.IsNullOrEmpty(AccountNumber))
            return false;

        var accountResult = await dataAccess.GetAccountByNumber(AccountNumber!);
        if (!accountResult.Success)
        {
            NotifyLoadError(accountResult.Errors, "account", true);
        }

        Account = accountResult.Data;
        IsAccountLoaded = true;

        if (!isMain) 
            return true;
        
        LoadAddresses().Forget();
        LoadPhoneNumbers().Forget();
        LoadEmails().Forget();
        LoadAccountWatches().Forget();

        return true;
    }

    private async Task LoadAddresses()
    {
        var addressesResult = await dataAccess.GetAllLegalEntityAddresses(Account!.Id);
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
        var phoneNumbersResult = await dataAccess.GetAllLegalEntityPhoneNumbers(Account!.Id);
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
        var emailsResult = await dataAccess.GetAllLegalEntityEmails(Account!.Id);
        if (!emailsResult.Success)
        {
            NotifyLoadError(emailsResult.Errors, "emails", true);
        }

        Emails = emailsResult.Data!
            .Select(s => new BsgEmail(s))
            .ToList();
        _isEmailsLoaded = true;
    }

    private async Task LoadAccountWatches()
    {
        var result = await dataAccess.GetAccountWatches(Account!.Id);
        if (!result.Success)
        {
            NotifyLoadError(result.Errors, "account watches", true);
        }
        
        AccountWatches = result.Data!;
        _isAccountWatchesLoaded = true;
    }

    private async Task LoadDomainTables()
    {
        await dataCache.ParallelGetCacheOrDataAsync(AddressTypesLoad, CountriesLoad, PhoneTypesLoad,
            StatesLoad, WatchStatusesLoad);
    }
}