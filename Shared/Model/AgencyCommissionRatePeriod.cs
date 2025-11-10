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

    public override bool IsChanged => _start != _originalStart || 
                                      _finish != _originalFinish || 
                                      _ranges.Any(r => r.IsChanged);

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
    }

    public override void ResetAll()
    {
        Start = _originalStart;
        Finish = _originalFinish;
        Ranges = _originalRanges;
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
    }
}