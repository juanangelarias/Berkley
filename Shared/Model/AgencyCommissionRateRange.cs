namespace James.Shared.Model;

public class AgencyCommissionRateRange : NotifyPropertyChangedBase
{
    #region Fields & Properties

    public Guid RecordId { get; set; }
    public int Id { get; set; }

    #region From

    private int _from;
    private int _originalFrom;

    public int From
    {
        get => _from;
        set
        {
            _from = value;
            CheckIsChanged();
            OnPropertyChanged();
        }
    }

    #endregion

    #region To

    private int? _to;
    private int? _originalTo;

    public int? To
    {
        get => _to;
        set
        {
            _to = value;
            CheckIsChanged();
            OnPropertyChanged();
        }
    }

    #endregion

    #region Rate

    private double _rate;
    private double _originalRate;

    public double Rate
    {
        get => _rate;
        set
        {
            _rate = value;
            CheckIsChanged();
            OnPropertyChanged();
        }
    }

    #endregion
    
    public string FromTxt => From.ToString("C0");
    public string ToTxt => To?.ToString("C0") ?? "0";
    public string RateTxt => Rate.ToString("P2");

    #endregion

    public AgencyCommissionRateRange()
    {
        
    }
    
    public AgencyCommissionRateRange(Guid recordId, int id, int from, int? to, double rate, bool isNew = true)
    {
        RecordId = recordId;
        Id = id;
        From = from;
        To = to;
        Rate = rate;

        if (isNew)
        {
            _originalFrom = 0;
            _originalTo = 0;
            _originalRate = 0;
        }
        else
        {
            _originalFrom = from;
            _originalTo = to;
            _originalRate = rate;
        }

        IsChanged = false;
    }

    public override void Reset()
    {
        From = _originalFrom;
        To = _originalTo;
        Rate = _originalRate;
        
        IsChanged = false;
    }

    public override void ApplyChanges()
    {
        _originalFrom = From;
        _originalTo = To;
        _originalRate = Rate;
        
        IsChanged = false;
    }

    protected override void CheckIsChanged()
    {
        IsChanged = _from != _originalFrom ||
                    _to != _originalTo ||
                    Math.Abs(_rate - _originalRate) > 0.00001;
    }
}