using James.Shared;
using James.Shared.Data;
using James.Shared.Notification;
using Radzen;

namespace JamesWebUI.Client.States;

public interface INotificationGeneralState
{
    List<NotificationProperty> NotificationPropertyList { get; set; }
    //Task LoadProperties();
    Task<SaveDataResult> SetNotificationProperty(NotificationProperty notificationProperty);
    Task<SaveDataResult> DeleteNotificationProperty(Guid id);
}

public class NotificationGeneralState(
    IDataAccess dataAccess,
    ILoggingService loggingService,
    NotificationService notificationService)
    : StateBase(loggingService, notificationService), INotificationGeneralState
{
    #region Fields & Properties

    #region NotificationPropertyList

    private List<NotificationProperty> _notificationPropertyList = [];

    public List<NotificationProperty> NotificationPropertyList
    {
        get => _notificationPropertyList;
        set
        {
            _notificationPropertyList = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #endregion

    /*#region Loaders

    private IDataAccessResult<List<NotificationProperty>> _notificationResult = null!;

    private LoadItem NotificationLoad => AddEventNotify(new LoadItem
    {
        Key = CacheKeys.NotificationPriorities,
        AsyncLoadTask = async () => _notificationResult = await dataAccess.GetAllNotificationProperties(),
        CacheLoadTask = cache => _notificationResult = new DataAccessResult<List<NotificationProperty>>
        {
            Data = (List<NotificationProperty>)cache!
        },
        ResultVariable = () => _notificationResult,
        AfterLoad = () => { NotificationPropertyList = _notificationResult.Data!; },
        CacheDuration = new TimeSpan(0,0,1)
    }, "notification properties");

    #endregion

    public async Task LoadProperties()
    {
        var response = await dataAccess.GetAllNotificationProperties();
        if(!response.Success)
        {
            NotifyLoadError(response.Errors, "notification properties", true);
            return;
        }
        
        NotificationPropertyList = response.Data!;
    }*/

    public async Task<SaveDataResult> SetNotificationProperty(NotificationProperty notificationProperty)
    {
        throw new NotImplementedException();
        /*var response = await dataAccess.UpdateNotificationProperty(notificationProperty.Id, notificationProperty.Name,
            notificationProperty.Type);
        
        return (SaveDataResult)response;*/
    }

    public async Task<SaveDataResult> DeleteNotificationProperty(Guid id)
    {
        throw new NotImplementedException();
        /*var response = await dataAccess.DeleteNotificationProperty(id);
        return (SaveDataResult)response;*/
    }
}