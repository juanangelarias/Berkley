using James.Shared.EnumTypes;

namespace James.Shared.Notification;

public class NotificationPropertyValue
{
    public Guid Id { get; set; }
    public Guid NotificationId { get; set; }
    public Guid NotificationPropertyId { get; set; }
    public NotificationProperty NotificationProperty { get; set; } = null!;
    public string Type { get; set; } = "";
    public string StringTxt { get; set; } = null!;

    public object? Value => Type switch
    {
        NotificationPropertyType.String => StringTxt,
        NotificationPropertyType.Int => int.Parse(StringTxt),
        NotificationPropertyType.Decimal => decimal.Parse(StringTxt),
        NotificationPropertyType.Date => DateTime.Parse(StringTxt),
        NotificationPropertyType.Boolean => bool.Parse(StringTxt),
        NotificationPropertyType.Guid => Guid.Parse(StringTxt),
        NotificationPropertyType.Email => StringTxt,
        NotificationPropertyType.PhoneNumber => StringTxt,
        _ => null,
    };
    
    //

    public virtual Notification Notification { get; set; } = null!;
    public virtual NotificationProperty Property { get; set; } = null!;
}