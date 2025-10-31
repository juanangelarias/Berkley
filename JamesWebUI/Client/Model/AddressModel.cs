using James.Shared;
using James.Shared.Model;

namespace JamesWebUI.Client.Model;

public class AddressModel(LegalEntityAddress leAddress) : NotifyPropertyChangedBase
{
    private Guid AddressId { get; } = leAddress.AddressId;
    private Guid LegalEntityId { get; } = leAddress.LegalEntityId;

    #region Type

    private string _type = leAddress.Type;
    private string _originalType = leAddress.Type;

    public string Type
    {
        get => _type;
        set
        {
            _type = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Address1

    private string _address1 = leAddress.Address.Address1;
    private string _originalAddress1 = leAddress.Address.Address1;

    public string Address1
    {
        get => _address1;
        set
        {
            _address1 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Address2

    private string? _address2 = leAddress.Address.Address2;
    private string? _originalAddress2 = leAddress.Address.Address2;

    public string? Address2
    {
        get => _address2;
        set
        {
            _address2 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Address3

    private string? _address3 = leAddress.Address.Address3;
    private string? _originalAddress3 = leAddress.Address.Address3;

    public string? Address3
    {
        get => _address3;
        set
        {
            _address3 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region City

    private string _city = leAddress.Address.City;
    private string _originalCity = leAddress.Address.City;

    public string City
    {
        get => _city;
        set
        {
            _city = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region StateCode

    private string? _stateCode = leAddress.Address.StateCode;
    private string? _originalStateCode = leAddress.Address.StateCode;

    public string? StateCode
    {
        get => _stateCode;
        set
        {
            _stateCode = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region PostalCode

    private string? _postalCode = leAddress.Address.PostalCode;
    private string? _originalPostalCode = leAddress.Address.PostalCode;

    public string? PostalCode
    {
        get => _postalCode;
        set
        {
            _postalCode = value;
            OnPropertyChanged();
        }
    }

    #endregion

    public override bool IsChanged => _type != _originalType ||
                                      _address1 != _originalAddress1 ||
                                      _address2 != _originalAddress2 ||
                                      _address3 != _originalAddress3 ||
                                      _city != _originalCity ||
                                      _stateCode != _originalStateCode ||
                                      _postalCode != _originalPostalCode;

    public override void Reset()
    {
        _address1 = _originalAddress1;
        _address2 = _originalAddress2;
        _address3 = _originalAddress3;
        _city = _originalCity;
        _stateCode = _originalStateCode;
        _postalCode = _originalPostalCode;
    }

    public override void ApplyChanges()
    {
        _originalAddress1 = _address1;
        _originalAddress2 = _address2;
        _originalAddress3 = _address3;
        _originalCity = _city;
        _originalStateCode = _stateCode;
        _originalPostalCode = _postalCode;
    }

    public LegalEntityAddress ToLegalEntityAddress()
    {
        return new()
        {
            LegalEntityId = LegalEntityId,
            AddressId = AddressId,
            Type = Type,
            Address = new()
            {
                Id = AddressId,
                Address1 = _address1,
                Address2 = _address2,
                Address3 = _address3,
                City = _city,
                StateCode = _stateCode,
                PostalCode = _postalCode
            }
        };
    }
}