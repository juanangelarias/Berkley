using HotChocolate.Authorization;
using James.Shared.Notification;
using Newtonsoft.Json;
using Notification = James.Shared.Notification.Notification;

namespace James.Data.Server.GraphQL.Mutations;

[MutationType]
public class NotificationMutations
{
    // Notification Properties
    [Authorize]
    public bool UpdateNotificationProperty(Guid id, string name, string type)
    {
        var dbNotificationProperties = GetNotificationProperties();
        var notificationProperty = dbNotificationProperties
            .FirstOrDefault(f => f.Id == id);

        if (notificationProperty != null)
        {
            notificationProperty.Name = name;
            notificationProperty.Type = type;
        }
        else
        {
            dbNotificationProperties.Add(new NotificationProperty
            {
                Id = id,
                Name = name,
                Type = type
            });
        }

        var json = JsonConvert.SerializeObject(dbNotificationProperties);
        File.WriteAllText(@"C:\temp\BsgNotifications\NotificationProperties.json", json);

        return true;
    }

    [Authorize]
    public bool DeleteNotificationProperty(Guid id)
    {
        var dbNotificationProperties = GetNotificationProperties();
        var notificationProperty = dbNotificationProperties
            .FirstOrDefault(f => f.Id == id);
        if (notificationProperty != null)
        {
            dbNotificationProperties.Remove(notificationProperty);
        }
        else
        {
            return false;
        }

        var json = JsonConvert.SerializeObject(dbNotificationProperties);
        File.WriteAllText(@"C:\temp\BsgNotifications\NotificationProperties.json", json);

        return true;
    }

    // Notifications
    [Authorize]
    public bool UpdateNotification(Guid id, Guid? senderUserId, string senderUserEmail, bool sendEmail, bool sendSms,
        Guid? accountId, Guid? agencyId, string title, string body, DateTime? followUpDate, string status,
        List<NotificationPropertyValue> properties, List<NotificationRecipient> recipients)
    {
        var dbNotifications = GetNotifications();
        var dbNotificationProperties = GetNotificationProperties();

        foreach (var property in properties)
        {
            if (dbNotificationProperties.All(a => a.Id != property.NotificationProperty.Id))
                return false;
        }

        var notification = dbNotifications
            .FirstOrDefault(f => f.Id == id);

        if (notification != null)
        {
            notification.SenderUserEmail = senderUserEmail;
            notification.SenderUserId = senderUserId;
            notification.SendEmail = sendEmail;
            notification.SendSms = sendSms;
            notification.AccountId = accountId;
            notification.AgencyId = agencyId;
            notification.Title = title;
            notification.Body = body;
            notification.FollowUpDate = followUpDate;
            notification.Status = status;
            notification.Recipients = recipients;
            notification.Properties = properties;
        }
        else
        {
            notification = new Notification
            {
                Id = id,
                SenderUserEmail = senderUserEmail,
                SenderUserId = senderUserId,
                SendEmail = sendEmail,
                SendSms = sendSms,
                AccountId = accountId,
                AgencyId = agencyId,
                Title = title,
                Body = body,
                FollowUpDate = followUpDate,
                Status = status,
                Recipients = recipients,
                Properties = properties
            };
            dbNotifications.Add(notification);
        }

        var json = JsonConvert.SerializeObject(dbNotifications);
        File.WriteAllText(@"C:\temp\BsgNotifications\Notifications.json", json);

        return true;
    }

    [Authorize]
    public bool DeleteNotification(Guid id)
    {
        var dbNotifications = GetNotifications();
        var notification = dbNotifications
            .FirstOrDefault(f => f.Id == id);
        if (notification != null)
        {
            dbNotifications.Remove(notification);
        }
        else
        {
            return false;
        }
        
        var json = JsonConvert.SerializeObject(dbNotifications);
        File.WriteAllText(@"C:\temp\BsgNotifications\Notifications.json", json);
        
        return true;   
    }

    private List<NotificationProperty> GetNotificationProperties()
    {
        var json = File.ReadAllText(@"C:\temp\BsgNotifications\NotificationProperties.json");
        var data = JsonConvert.DeserializeObject<List<NotificationProperty>>(json);
        return data ?? [];
    }

    private List<Notification> GetNotifications()
    {
        var json = File.ReadAllText(@"C:\temp\BsgNotifications\Notifications.json");
        var data = JsonConvert.DeserializeObject<List<Notification>>(json);
        return data ?? [];
    }
}