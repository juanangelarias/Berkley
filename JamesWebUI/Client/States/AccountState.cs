using James.Shared;
using James.Shared.Constants;
using James.Shared.Data;
using James.Shared.Dto;
using James.Shared.Model;
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
    string AccountStatus { get; set; }
    bool IsAccountActive { get; set; }
    Agency? Agency { get; set; }
    string FirstIndemnityDate { get; set; }
    IconStyle AccountStatusStyle { get; set; }
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
    List<AgencyStatusDm> AgencyStatuses { get; set; }
    AccountWatch? ActiveWatch { get; set; }
    List<BsgLookup> BranchesLookup { get; set; }
    List<BsgLookup> DivisionsLookup { get; set; }
    List<BsgLookup> UnderwritersLookup { get; set; }
    List<AgencyDto> AllAgencies { get; set; }
    List<Address> AccountAddresses { get; set; }
    List<Agent> AgencyAgents { get; set; }
    List<BsgLookup> AgencyAgentsLookup { get; set; }
    AlertPeriod AlertPeriod { get; set; }
    AccountAlertPackageDto Alerts { get; set; }

    Task CreateAccountWatch(AccountWatch accountWatch);
    Task UpdateAccountWatch(AccountWatch accountWatch);
    Task DeleteAccountWatch(Guid accountWatchId);

    Task SetLayout();
    Task<bool> Initialize();
    void ClearAccount();
    IconStyle AgencyStatusStyle { get; set; }
    Task<bool> SaveAgencyAndAgent(string agencyNumber, Guid agentId);
    Task SaveAddress(BsgAddress address, bool isNew = false);
    Task DeleteAddress(Guid addressId);
    Task SavePhoneNumber(BsgPhoneNumber phoneNumber);
    Task DeletePhoneNumber(Guid phoneNumberId);
    Task SaveEmail(BsgEmail email, bool isNew = false);
    Task DeleteEmail(Guid emailId);
    Task<SaveDataResult> SaveAccountBasicInfo();
    Task LoadAlerts();

    Task<Agency?> AgencyChanged(string agencyNumber);
}

public class AccountState(
    IDataAccess dataAccess,
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
    public string AccountStatus { get; set; } = "";
    public IconStyle AccountStatusStyle { get; set; }
    public IconStyle AgencyStatusStyle { get; set; }
    public bool IsAccountActive { get; set; }
    public string FirstIndemnityDate { get; set; } = "";
    public AccountLayout? SelectedType { get; set; }
    public List<AgencyStatusDm> AgencyStatuses { get; set; } = [];

    private bool _accountLoadResult;

    #region AddressTypes

    private List<AddressTypeDm> _addressTypes = [];

    public List<AddressTypeDm> AddressTypes
    {
        get
        {
            var n = 0;
            while (!_isAddressTypesLoaded)
            {
                n += 1;
                if (n > 5)
                    return [];

                Task.Delay(2000);
            }

            return _addressTypes;
        }
        set
        {
            _addressTypes = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Countries

    private List<CountryDm> _countries = [];

    public List<CountryDm> Countries
    {
        get
        {
            var n = 0;
            while (!_isCountriesLoaded)
            {
                n += 1;
                if (n > 5)
                    return [];

                Task.Delay(2000);
            }

            return _countries;
        }
        set
        {
            _countries = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region EmailTypes

    private List<EmailTypeDm> _emailTypes = [];

    public List<EmailTypeDm> EmailTypes
    {
        get
        {
            var n = 0;
            while (!_isEmailTypesLoaded)
            {
                n += 1;
                if (n > 5)
                    return [];

                Task.Delay(2000);
            }

            return _emailTypes;
        }
        set
        {
            _emailTypes = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region PhoneNumberTypes

    private List<PhoneTypeDm> _phoneNumberTypes = [];

    public List<PhoneTypeDm> PhoneNumberTypes
    {
        get
        {
            var n = 0;
            while (!_isPhoneNumberTypesLoaded)
            {
                n += 1;
                if (n > 5)
                    return [];

                Task.Delay(2000);
            }

            return _phoneNumberTypes;
        }
        set
        {
            _phoneNumberTypes = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region States

    private List<State> _states = [];

    public List<State> States
    {
        get
        {
            var n = 0;
            while (!_isStatesLoaded)
            {
                n += 1;
                if (n > 5)
                    return [];

                Task.Delay(2000);
            }

            return _states;
        }
        set
        {
            _states = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region WatchStatuses

    private List<WatchStatusDm> _watchStatuses = [];

    public List<WatchStatusDm> WatchStatuses
    {
        get
        {
            var n = 0;
            while (!_isWatchStatusesLoaded)
            {
                n += 1;
                if (n > 5)
                    return [];

                Task.Delay(2000);
            }

            return _watchStatuses;
        }
        set
        {
            _watchStatuses = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region BranchesLookup

    private List<BsgLookup> _branchesLookup = [];

    public List<BsgLookup> BranchesLookup
    {
        get
        {
            var n = 0;
            while (!_isBranchesLoaded)
            {
                n += 1;
                if (n > 5)
                {
                    return [];
                }

                Task.Delay(2000);
            }

            return _branchesLookup;
        }
        set
        {
            _branchesLookup = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region DivisionsLookup

    private List<BsgLookup> _divisionsLookup = [];

    public List<BsgLookup> DivisionsLookup
    {
        get
        {
            var n = 0;
            while (!_isDivisionsLoaded)
            {
                n += 1;
                if (n > 5)
                {
                    return [];
                }

                Task.Delay(2000);
            }

            return _divisionsLookup;
        }
        set
        {
            _divisionsLookup = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region UnderwritersLookup

    private List<BsgLookup> _underwritersLookup = [];

    public List<BsgLookup> UnderwritersLookup
    {
        get
        {
            var n = 0;
            while (!_isUnderwritersLoaded)
            {
                n += 1;
                if (n > 5)
                    return [];

                Task.Delay(2000);
            }

            return _underwritersLookup;
        }
        set
        {
            _underwritersLookup = value;
            OnPropertyChanged();
        }
    }

    #endregion

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
        get => _addresses;
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
        get => _phoneNumbers;
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
        get => _emails;
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
        get => _accountWatches;
        set
        {
            _accountWatches = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region ActiveWatch

    private AccountWatch? _activeWatch;

    public AccountWatch? ActiveWatch
    {
        get => _activeWatch;
        set
        {
            _activeWatch = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region AgencyAgents

    private List<Agent> _agencyAgents = [];

    public List<Agent> AgencyAgents
    {
        get => _agencyAgents;
        set
        {
            _agencyAgents = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region AgencyAgentsLookup

    private List<BsgLookup> _agencyAgentsLookup = [];

    public List<BsgLookup> AgencyAgentsLookup
    {
        get => _agencyAgentsLookup;
        set
        {
            _agencyAgentsLookup = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region AccountAddresses

    private List<Address> _accountAddresses = [];

    public List<Address> AccountAddresses
    {
        get => _accountAddresses;
        set
        {
            _accountAddresses = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region AllAgencies

    private List<AgencyDto> _allAgencies = [];

    public List<AgencyDto> AllAgencies
    {
        get => _allAgencies;
        set
        {
            _allAgencies = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Agency

    private Agency? _agency;

    public Agency? Agency
    {
        get => _agency;
        set
        {
            _agency = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region NotificationPeriod

    private AlertPeriod _alertPeriod = AlertPeriod.Last30Days;

    public AlertPeriod AlertPeriod
    {
        get => _alertPeriod;
        set
        {
            _alertPeriod = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Notifications

    private AccountAlertPackageDto _alerts = null!;

    public AccountAlertPackageDto Alerts
    {
        get => _alerts;
        set
        {
            _alerts = value;
            OnPropertyChanged();
        }
    }

    #endregion

    private bool _isBranchesLoaded;
    private bool _isDivisionsLoaded;
    private bool _isUnderwritersLoaded;
    private bool _isWatchStatusesLoaded;
    private bool _isCountriesLoaded;
    private bool _isPhoneNumberTypesLoaded;
    private bool _isEmailTypesLoaded;
    private bool _isAddressTypesLoaded;
    private bool _isStatesLoaded;

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
        AfterLoad = () =>
        {
            AddressTypes = _addressTypesResult.Data!;
            _isAddressTypesLoaded = true;
        }
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
        AfterLoad = () =>
        {
            Countries = _countriesResult.Data!;
            _isCountriesLoaded = true;
        }
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
        AfterLoad = () =>
        {
            EmailTypes = _emailTypesResult.Data!;
            _isEmailTypesLoaded = true;
        }
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
        AfterLoad = () =>
        {
            PhoneNumberTypes = _phoneTypesResult.Data!;
            _isPhoneNumberTypesLoaded = true;
        }
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
        AfterLoad = () =>
        {
            States = _statesResult.Data!;
            _isStatesLoaded = true;
        }
    }, "states");

    #endregion

    #region WatchStatuses

    private IDataAccessResult<List<WatchStatusDm>> _watchStatusesResult = null!;

    private LoadItem WatchStatusesLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.WatchStatuses,
        AsyncLoadTask = async () => _watchStatusesResult = await dataAccess.GetWatchStatuses(),
        CacheLoadTask = cache => _watchStatusesResult = new DataAccessResult<List<WatchStatusDm>>
        {
            Data = (List<WatchStatusDm>)cache!
        },
        ResultVariable = () => _watchStatusesResult,
        AfterLoad = () =>
        {
            WatchStatuses = _watchStatusesResult.Data!;
            _isWatchStatusesLoaded = true;
        }
    }, "watch statuses");

    #endregion

    #region Branches

    private IDataAccessResult<List<Branch>> _branchesResult = null!;

    private LoadItem BranchesLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.Branches,
        AsyncLoadTask = async () => _branchesResult = await dataAccess.GetAllBranches(),
        CacheLoadTask = cache => _branchesResult = new DataAccessResult<List<Branch>>
        {
            Data = (List<Branch>)cache!
        },
        ResultVariable = () => _branchesResult,
        AfterLoad = () =>
        {
            BranchesLookup = _branchesResult.Data!
                .Select(s => new BsgLookup
                {
                    Id = s.Id,
                    Code = s.BranchKey,
                    Name = s.Name,
                })
                .ToList();
            _isBranchesLoaded = true;
        }
    }, "branches");

    #endregion

    #region Divisions

    private IDataAccessResult<List<DivisionDm>> _divisionsResult = null!;

    private LoadItem DivisionsLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.Divisions,
        AsyncLoadTask = async () => _divisionsResult = await dataAccess.GetDivisions(),
        CacheLoadTask = cache => _divisionsResult = new DataAccessResult<List<DivisionDm>>
        {
            Data = (List<DivisionDm>)cache!
        },
        ResultVariable = () => _divisionsResult,
        AfterLoad = () =>
        {
            DivisionsLookup = _divisionsResult.Data!
                .Select(s => new BsgLookup
                {
                    Id = s.Id,
                    Code = s.DivisionCode,
                    Name = s.Division
                })
                .ToList();
            _isDivisionsLoaded = true;
        }
    }, "divisions");

    #endregion

    #region Underwriters

    private IDataAccessResult<List<Underwriter>> _underwritersResult = null!;

    private LoadItem UnderwritersLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.Underwriters,
        AsyncLoadTask = async () => _underwritersResult = await dataAccess.GetUnderwriters(),
        CacheLoadTask = cache => _underwritersResult = new DataAccessResult<List<Underwriter>>
        {
            Data = (List<Underwriter>)cache!
        },
        ResultVariable = () => _underwritersResult,
        AfterLoad = () =>
        {
            UnderwritersLookup = _underwritersResult.Data!
                .Select(s => new BsgLookup
                {
                    Id = s.Id,
                    Code = s.IdNavigation.Initials,
                    Name = s.IdNavigation.FullName,
                })
                .ToList();
            _isUnderwritersLoaded = true;
        }
    }, "underwriters");

    #endregion

    #region Agency Agents

    private IDataAccessResult<List<Agent>> _agencyAgentsResult = null!;

    private LoadItem AgencyAgentsLoad => AddEventNotify(new()
    {
        Key = CacheKeys.AgencyAgents(Account!.AgencyNumber ?? ""),
        AsyncLoadTask = async () =>
            _agencyAgentsResult = await dataAccess.GetAgencyAgents(Account?.AgencyNumberNavigation?.Id ?? Guid.Empty),
        CacheLoadTask = cache => _agencyAgentsResult = new DataAccessResult<List<Agent>>
        {
            Data = (List<Agent>)cache!
        },
        ResultVariable = () => _agencyAgentsResult,
        AfterLoad = () =>
        {
            AgencyAgents = _agencyAgentsResult.Data!;
            AgencyAgentsLookup = AgencyAgents
                .Select(s => new BsgLookup
                {
                    Id = s.Id,
                    Code = "",
                    Name = s.IdNavigation.FullName
                })
                .ToList();
        }
    }, "agency agents");

    #endregion

    #region Agencies

    private IDataAccessResult<List<AgencyDto>> _agenciesResult = null!;

    private LoadItem AgenciesLoad => AddEventNotify(new()
    {
        Key = CacheKeys.AgenciesDto,
        AsyncLoadTask = async () => _agenciesResult = await dataAccess.GetAllActiveAgencies(),
        CacheLoadTask = cache => _agenciesResult = new DataAccessResult<List<AgencyDto>>
        {
            Data = (List<AgencyDto>)cache!
        },
        ResultVariable = () => _agenciesResult,
        AfterLoad = () => { AllAgencies = _agenciesResult.Data!; }
    }, "agencies");

    #endregion

    #region Agency Statuses

    private IDataAccessResult<List<AgencyStatusDm>> _agencyStatusesResult = null!;

    private LoadItem AgencyStatusesLoad => AddEventNotify(new()
    {
        Key = CacheKeys.AgencyStatuses,
        AsyncLoadTask = async () => _agencyStatusesResult = await dataAccess.GetAgencyStatuses(),
        CacheLoadTask = cache => _agencyStatusesResult = new DataAccessResult<List<AgencyStatusDm>>
        {
            Data = (List<AgencyStatusDm>)cache!
        },
        ResultVariable = () => _agencyStatusesResult,
        AfterLoad = () => { AgencyStatuses = _agencyStatusesResult.Data!; }
    }, "agency statuses");

    #endregion

    #region Alerts

    private IDataAccessResult<AccountAlertPackageDto> _accountAlerts = null!;

    private LoadItem AccountAlertsLoad => AddEventNotify(new()
    {
        Key = CacheKeys.AccountAlerts,
        AsyncLoadTask = async () =>
            _accountAlerts = await dataAccess.GetAccountAlerts((int)AlertPeriod, AccountNumber??""),
        CacheLoadTask = cache => _accountAlerts = new DataAccessResult<AccountAlertPackageDto>
        {
            Data = (AccountAlertPackageDto)cache!
        },
        ResultVariable = () => _accountAlerts,
        CacheDuration = new TimeSpan(0, 0, 0, 0, 1000),
        AfterLoad = () => { Alerts = _accountAlerts.Data!; }
    }, "account notifications");

    #endregion

    #endregion

    public async Task SetLayout()
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
    }

    public async Task<bool> Initialize()
    {
        if (string.IsNullOrEmpty(AccountNumber))
            return false;

        await LoadAccount();
        await LoadDomainTables();

        return _accountLoadResult;
    }

    public void ClearAccount()
    {
        Account = null;
        Agency = null;
        Addresses = [];
        PhoneNumbers = [];
        Emails = [];
        AccountWatches = [];
        ActiveWatch = null;
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

    public async Task<SaveDataResult> SaveAccountBasicInfo()
    {
        var response = await dataAccess
            .SetAccountCommercialInfo(Account!.Id, Account.IdNavigation.FullName, Account!.UnderwriterId!.Value,
                Account.Branch, Account.Division, Guid.Empty, Guid.Empty);

        return (SaveDataResult)response;
    }

    public async Task LoadAlerts()
    {
        await dataAccess.GetCacheOrLoadDataAsync(AccountAlertsLoad);
    }

    private async Task LoadAccount()
    {
        IsAccountLoaded = false;
        if (string.IsNullOrEmpty(AccountNumber))
        {
            _accountLoadResult = false;
            return;
        }

        var accountResult = await dataAccess.GetAccountByNumber(AccountNumber!);
        if (!accountResult.Success)
        {
            NotifyLoadError(accountResult.Errors, "account", true);
        }

        Account = accountResult.Data;

        Agency = Account?.AgencyNumberNavigation;

        Addresses = Account?.IdNavigation.LegalEntityAddresses
            .Select(s => new BsgAddress(s.Type, s.Address))
            .ToList() ?? [];
        PhoneNumbers = Account?.IdNavigation.LegalEntityPhones
            .Select(s => new BsgPhoneNumber(s.Type, s.PhoneNumber))
            .ToList() ?? [];
        Emails = Account?.IdNavigation.LegalEntityEmails
            .Select(s => new BsgEmail(s))
            .ToList() ?? [];
        AccountWatches = Account?.AccountWatches
            .OrderByDescending(o => o.WatchDate)
            .ToList() ?? [];

        ActiveWatch = AccountWatches
            .FirstOrDefault();

        var statusLog = Account?.AccountStatusLogs
            .OrderByDescending(o => o.Effective)
            .FirstOrDefault();

        var accountStatus = statusLog?.AccountStatus ?? "";
        AccountStatus = accountStatus.ToLower() switch
        {
            "active" => "Active",
            "term. agent" => "Terminated",
            "term. comp." => "Terminated",
            "declined" => "Terminated",
            "lost" => "Terminated",
            "prospect" => "Prospect",
            "pending" => "Prospect",
            "on hold" => "Prospect",
            _ => "Not Defined"
        };

        AccountStatusStyle = AccountStatus switch
        {
            "Active" => IconStyle.Success,
            "Prospect" => IconStyle.Warning,
            "Terminated" => IconStyle.Danger,
            _ => IconStyle.Base
        };

        var agencyStatus = Agency?.AgencyStatusLogs
            .OrderByDescending(o => o.Effective)
            .FirstOrDefault()?
            .NewStatus ?? "";

        AgencyStatusStyle = agencyStatus.ToLower() switch
        {
            "active" => IconStyle.Success,
            "pending" or "on hold" => IconStyle.Warning,
            "lost" or "terminated" => IconStyle.Danger,
            _ => IconStyle.Base
        };

        IsAccountActive = statusLog?.AccountStatusNavigation?.Active ?? false;

        var firstIndemnity = Account?.Indemnitors
            .OrderBy(o => o.AgreementDate)
            .FirstOrDefault()?
            .AgreementDate;

        FirstIndemnityDate = firstIndemnity == null
            ? "First indemnity: None"
            : $"First indemnity: {firstIndemnity:MM/dd/yyyy}";

        await LoadAgencyAgents();

        IsAccountLoaded = true;

        _accountLoadResult = true;
    }

    private async Task LoadDomainTables()
    {
        var start = DateTime.Now;
        await dataAccess.ParallelGetCacheOrDataAsync(AddressTypesLoad, CountriesLoad, PhoneTypesLoad,
            StatesLoad, WatchStatusesLoad, BranchesLoad, DivisionsLoad, UnderwritersLoad, AgenciesLoad,
            AgencyStatusesLoad, AccountAlertsLoad);

        var elapsed = DateTime.Now - start;
        var elapsedTxt = elapsed.ToString(@"mm\:ss\.fff");
        Console.WriteLine($"Load domain tables took {elapsedTxt} seconds");
    }

    private async Task LoadAgencyAgents()
    {
        Console.WriteLine($"Load agency agents - {Account?.AgencyNumberNavigation?.Id.ToString() ?? "NO AgencyId"}");
        if (Account?.AgencyNumberNavigation?.Id != null && Account?.AgencyNumberNavigation?.Id != Guid.Empty)
            await dataAccess.GetCacheOrLoadDataAsync(AgencyAgentsLoad);
    }

    #region Agency

    public async Task<bool> SaveAgencyAndAgent(string agencyNumber, Guid agentId)
    {
        var response = await dataAccess.SetAccountAgencyAndAgent(Account!.Id, agencyNumber, agentId);
        if (response.Success)
        {
            var agencyResponse = await dataAccess.GetAgencyByAgencyNumber(agencyNumber);
            var agentResponse = await dataAccess.GetAgent(agentId);

            Account.AgencyNumber = agencyNumber;
            Account.AgencyNumberNavigation = agencyResponse.Data;
            Account.AgentId = agentId;
            Account.Agent = agentResponse.Data;

            return true;
        }

        NotifyLoadError(response.Errors, "account", true);
        return false;
    }

    public async Task<Agency?> AgencyChanged(string agencyNumber)
    {
        var agencyData = await dataAccess.GetAgencyByAgencyNumber(agencyNumber);
        if (!agencyData.Success)
        {
            NotifyLoadError(agencyData.Errors, "agency", true);
            return null;
        }

        var agencyAgents = await dataAccess.GetAgencyAgents(agencyData.Data!.Id);
        if (agencyAgents.Success)
        {
            AgencyAgents = agencyAgents.Data!;
            AgencyAgentsLookup = AgencyAgents
                .Select(s => new BsgLookup
                {
                    Id = s.Id,
                    Code = "",
                    Name = s.IdNavigation.FullName
                })
                .ToList();

            return agencyData.Data;
        }

        NotifyLoadError(agencyAgents.Errors, "agency agents", true);
        return null;
    }

    #endregion

    #region Account Watches

    public async Task CreateAccountWatch(AccountWatch accountWatch)
    {
        await dataAccess.CreateAccountWatch(accountWatch.Id, accountWatch.AccountId, accountWatch.WatchDate,
            accountWatch.WatchStatus, accountWatch.Reason, accountWatch.ActionPlan);
    }
    public async Task UpdateAccountWatch(AccountWatch accountWatch)
    {
        await dataAccess.UpdateAccountWatch(accountWatch.Id, accountWatch.WatchStatus, accountWatch.Reason,
            accountWatch.ActionPlan);
    }
    public async Task DeleteAccountWatch(Guid accountWatchId)
    {
        await dataAccess.DeleteAccountWatch(accountWatchId);
    }

    #endregion
}