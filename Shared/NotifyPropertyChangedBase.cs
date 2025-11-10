using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace James.Shared;

public abstract class NotifyPropertyChangedBase: INotifyPropertyChanged
{
    public virtual bool IsChanged { get; set; }
    
    /// Occurs when a property value changes.
    /// This event is triggered whenever the value of a property is modified
    /// and change tracking or notification is required. It allows subscribers
    /// to react to updates in property values, such as updating the user interface
    /// or performing validation checks. Implementations should invoke this event
    /// with the name of the modified property to notify listeners of the change.
    public event PropertyChangedEventHandler? PropertyChanged;

    public virtual void ResetAll()
    {
    }
    
    public virtual void ApplyChangesAll()
    {
    }

    // Reset the main Level Only
    public virtual void Reset()
    {
    }
    
    // Apply Changes for the main Level Only
    public virtual void ApplyChanges()
    {
    }
    
    /// Notifies subscribers of property changes for the specified property name.
    /// This method raises the `PropertyChanged` event, which is typically used to inform
    /// data-binding clients that a property value has been updated. If no property name
    /// is provided, the name of the calling property is used by default.
    /// <param name="propertyName">
    /// The name of the property that has changed. This parameter is optional and
    /// will default to the name of the calling property if not provided.
    /// </param>
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}