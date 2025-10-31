using James.Shared;

namespace JamesWebUI.Client.Model;

public class AccountAgencyAgentModel(string? agencyNumber, string? agencyName, string? agencyCity, string? agencyStateCode,
    string? agencyStatus, string? agencyEmail,  Guid? agentId, string? agentName, string? agentEmail) 
    : NotifyPropertyChangedBase
{
    #region AgencyNumber

    private string? _agencyNumber = agencyNumber;
    private string? _originalAgencyNumber = agencyNumber;

    public string? AgencyNumber
    {
        get => _agencyNumber;
        set
        {
            _agencyNumber = value;
            _agentId = null;
            OnPropertyChanged();
        }
    }

    #endregion

    #region AgencyName

    private string? _agencyName = agencyName;
    private string? _originalAgencyName = agencyName;

    public string? AgencyName
    {
        get => _agencyName;
        set
        {
            _agencyName = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region AgentId

    private Guid? _agentId = agentId;
    private Guid? _originalAgentId = agentId;

    public Guid? AgentId
    {
        get => _agentId;
        set
        {
            _agentId = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region AgencyCity

    private string? _agencyCity = agencyCity;
    private string? _originalAgencyCity = agencyCity;

    public string? AgencyCity
    {
        get => _agencyCity;
        set
        {
            _agencyCity = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region AgencyStateCode

    private string? _agencyStateCode = agencyStateCode;
    private string? _originalAgencyStateCode = agencyStateCode;

    public string? AgencyStateCode
    {
        get => _agencyStateCode;
        set
        {
            _agencyStateCode = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region AgencyStatus

    private string? _agencyStatus = agencyStatus;
    private string? _originalAgencyStatus = agencyStatus;

    public string? AgencyStatus
    {
        get => _agencyStatus;
        set
        {
            _agencyStatus = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region AgencyEmail

    private string? _agencyEmail = agencyEmail;
    private string? _originalAgencyEmail = agencyEmail;

    public string? AgencyEmail
    {
        get => _agencyEmail;
        set
        {
            _agencyEmail = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region AgentName

    private string? _agentName = agentName;
    private string? _originalAgentName = agentName;

    public string? AgentName
    {
        get => _agentName;
        set
        {
            _agentName = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region AgentEmail

    private string? _agentEmail = agentEmail;
    private string? _originalAgentEmail = agentEmail;

    public string? AgentEmail
    {
        get => _agentEmail;
        set
        {
            _agentEmail = value;
            OnPropertyChanged();
        }
    }

    #endregion

    public override bool IsChanged => _agencyNumber != _originalAgencyNumber ||
                                      _agencyName != _originalAgencyName ||
                                      _agencyCity != _originalAgencyCity ||
                                      _agencyStateCode != _originalAgencyStateCode ||
                                      _agencyStatus != _originalAgencyStatus ||
                                      _agencyEmail != _originalAgencyEmail ||
                                      _agentId != _originalAgentId ||
                                      _agentName != _originalAgentName ||
                                      _agentEmail != _originalAgentEmail;

    public override void ApplyChanges()
    {
        _originalAgencyNumber = _agencyNumber;
        _originalAgencyName = _agencyName;
        _originalAgentId = _agentId;
        _originalAgencyCity = _agencyCity;
        _originalAgencyStateCode = _agencyStateCode;
        _originalAgencyStatus = _agencyStatus;
        _originalAgencyEmail = _agencyEmail;
        _originalAgentName = _agentName;
        _originalAgentEmail = _agentEmail;
    }

    public override void Reset()
    {
        AgencyNumber = _originalAgencyNumber;
        AgencyName = _originalAgencyName;
        AgentId = _originalAgentId;
        AgencyCity = _originalAgencyCity;
        AgencyStateCode = _originalAgencyStateCode;
        AgencyStatus = _originalAgencyStatus;
        AgencyEmail = _originalAgencyEmail;
        AgentName = _originalAgentName;
        AgentEmail = _originalAgentEmail;
    }
}