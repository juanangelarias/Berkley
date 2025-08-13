using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace James.Shared;

public class NotifyPropertyChanged: INotifyPropertyChanged
{
    public bool IsChanged { get; set; }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    public virtual void ResetAll()
    {
        IsChanged = false;
    }
    
    public virtual void ApplyChangesAll()
    {
        IsChanged = false;
    }

    // Reset the main Level Only
    public virtual void Reset()
    {
        IsChanged = false;
    }
    
    // Apply Changes for the main Level Only
    public virtual void ApplyChanges()
    {
        IsChanged = false;
    }
    
    protected virtual void CheckIsChanged()
    {
    }
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}