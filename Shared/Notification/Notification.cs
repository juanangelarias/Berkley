using James.Shared.EnumTypes;

namespace James.Shared.Notification;

public class Notification
{
    public Guid Id { get; set; }
    public Guid? SenderUserId { get; set; }
    public string SenderUserEmail { get; set; } = null!;
    
    public bool SendEmail { get; set; }
    public bool SendSms { get; set; }
    public string? AccountNumber { get; set; }
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public string Severity { get; set; } = null!;
    public DateTime? FollowUpDate { get; set; }
    public string Status { get; set; } = NotificationStatus.Active;

    public List<NotificationRecipient> Recipients { get; set; } = [];
    public List<NotificationPropertyValue> Properties { get; set; } = [];
}