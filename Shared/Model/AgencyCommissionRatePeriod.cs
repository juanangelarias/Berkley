namespace James.Shared.Model;

public class AgencyCommissionRatePeriod : NotifyPropertyChangedBase
{
    #region Fields & Properties

    public Guid AgencyId { get; set; }
    public BondType BondType { get; set; }
    public CommissionType CommissionType { get; set; }

    #region Start

    private DateTime? _start;
    private DateTime? _originalStart;

    public DateTime? Start
    {
        get => _start;
        set
        {
            _start = value;
            CheckIsChanged();
            OnPropertyChanged();
        }
    }

    #endregion

    #region Finish

    private DateTime? _finish;
    private DateTime? _originalFinish;

    public DateTime? Finish
    {
        get => _finish;
        set
        {
            _finish = value;
            CheckIsChanged();
            OnPropertyChanged();
        }
    }

    #endregion

    #region Ranges

    private List<AgencyCommissionRateRange> _ranges = [];
    private List<AgencyCommissionRateRange> _originalRanges;

    public List<AgencyCommissionRateRange> Ranges
    {
        get => _ranges;
        set
        {
            _ranges = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #endregion

    public AgencyCommissionRatePeriod(Guid agencyId, BondType bondType, CommissionType commissionType, DateTime start,
        DateTime? finish, List<AgencyCommissionRateRange> ranges)
    {
        AgencyId = agencyId;
        BondType = bondType;
        CommissionType = commissionType;
        Start = start;
        Finish = finish;

        _originalStart = start;
        _originalFinish = finish;

        Ranges = ranges;

        _originalRanges = Ranges;
        IsChanged = false;
    }

    public override void ResetAll()
    {
        Start = _originalStart;
        Finish = _originalFinish;
        Ranges = _originalRanges;

        IsChanged = false;
    }

    public override void ApplyChangesAll()
    {
        _originalStart = Start;
        _originalFinish = Finish;
        foreach (var range in _ranges)
        {
            range.ApplyChanges();
        }

        _originalRanges = Ranges;

        IsChanged = false;
    }

    protected override void CheckIsChanged()
    {
        IsChanged = _start != _originalStart ||
                    _finish != _originalFinish ||
                    _ranges.Any(r => r.IsChanged) ||
                    _ranges.Count != _originalRanges.Count;
    }
}