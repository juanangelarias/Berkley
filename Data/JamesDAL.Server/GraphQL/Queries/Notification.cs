using HotChocolate.Authorization;
using James.Shared.Notification;
using Newtonsoft.Json;
using Notification = James.Shared.Notification.Notification;

namespace James.Data.Server.GraphQL.Queries;

public partial class Query
{
    [Authorize]
    public List<NotificationProperty> GetNotificationProperties()
    {
        var json = File.ReadAllText(@"C:\temp\BsgNotifications\NotificationProperties.json");
        var data = JsonConvert.DeserializeObject<List<NotificationProperty>>(json);
        return data ?? [];
    }

    [Authorize]
    public List<Notification> GetNotifications()
    {
        var json = File.ReadAllText(@"C:\temp\BsgNotifications\Notifications.json");
        var data = JsonConvert.DeserializeObject<List<Notification>>(json);
        return data ?? [];
    }

    [Authorize]
    public List<Notification> GetNotificationsByUser(string userEmail, string? accountNumber = null)
    {
        var json = File.ReadAllText(@"C:\temp\BsgNotifications\Notifications.json");
        var notifications = JsonConvert.DeserializeObject<List<Notification>>(json) ?? [];

        var data = notifications
            .Where(r => r.SenderUserEmail == userEmail ||
                        r.Recipients.Any(a => a.RecipientEmail == userEmail)
                        || (accountNumber != null && r.AccountNumber == accountNumber))
            .ToList();

        return data;
    }
}