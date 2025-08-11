using James.Shared;

namespace JamesWebUI.Client.Model;

public class RatePeriod: NotifyPropertyChanged
{
    #region Start

    private DateOnly _start;
    private DateOnly? _originalStart;

    public DateOnly Start
    {
        get => _start;
        set
        {
            _start = value;
            OnPropertyChanged();
        }
    }

    #endregion
    
    public DateOnly Finish { get; set; }
    public List<RateRange> Rates { get; set; } = [];
}