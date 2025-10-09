namespace James.Shared.Notification;

public class NotificationRecipient
{
    public Guid Id { get; set; }
    public DateTime? Snoozed { get; set; }
    public Guid NotificationId { get; set; }
    public string Name { get; set; } = "";
    public string RecipientEmail { get; set; } = "";
    public string RecipientPhoneNumber { get; set; } = "";
    
    // 
    
    public virtual Notification Notification { get; set; } = null!;
}