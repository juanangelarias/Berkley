using James.Shared;

namespace JamesWebUI.Client.Model;

public class AccountBasicInfoModel(
    Guid id,
    string accountNum,
    string fullName,
    Guid? underwriterId,
    string branch,
    string hoLead,
    string division,
    string? sicCode)
    : NotifyPropertyChangedBase
{
    public Guid Id { get; set; } = id;
    
    #region AccountNum

    private string _accountNum = accountNum;
    private string _originalAccountNum = accountNum;

    public string AccountNum
    {
        get => _accountNum;
        set
        {
            _accountNum = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region FullName

    private string _fullName = fullName;
    private string _originalFullName = fullName;

    public string FullName
    {
        get => _fullName;
        set
        {
            _fullName = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region UnderwriterId

    private Guid? _underwriterId = underwriterId;
    private Guid? _originalUnderwriterId = underwriterId;

    public Guid? UnderwriterId
    {
        get => _underwriterId;
        set
        {
            _underwriterId = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Branch

    private string _branch = branch;
    private string _originalBranch = branch;

    public string Branch
    {
        get => _branch;
        set
        {
            _branch = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region HoLead

    private string _hoLead = hoLead;
    private string _originalHoLead = hoLead;

    public string HoLead
    {
        get => _hoLead;
        set
        {
            _hoLead = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region DivisionId

    private string _division = division;
    private string _originalDivision = division;

    public string Division
    {
        get => _division;
        set
        {
            _division = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region SicCode

    private string? _sicCode = sicCode;
    private string? _originalSicCode = sicCode;

    public string? SicCode
    {
        get => _sicCode;
        set
        {
            _sicCode = value;
            OnPropertyChanged();
        }
    }

    #endregion

    public override bool IsChanged => _accountNum != _originalAccountNum ||
                                      _fullName != _originalFullName ||
                                      _underwriterId != _originalUnderwriterId ||
                                      _branch != _originalBranch ||
                                      _hoLead != _originalHoLead ||
                                      _division != _originalDivision ||
                                      _sicCode != _originalSicCode;

    public override void ApplyChanges()
    {
        _originalAccountNum = _accountNum;
        _originalFullName = _fullName;
        _originalUnderwriterId = _underwriterId;
        _originalBranch = _branch;
        _originalHoLead = _hoLead;
        _originalDivision = _division;
        _originalSicCode = _sicCode;
    }

    public override void Reset()
    {
        _accountNum = _originalAccountNum;
        _fullName = _originalFullName;
        _underwriterId = _originalUnderwriterId;
        _branch = _originalBranch;
        _hoLead = _originalHoLead;
        _division = _originalDivision;
        _sicCode = _originalSicCode;
    }
}