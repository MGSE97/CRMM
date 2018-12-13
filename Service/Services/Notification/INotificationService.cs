using System.Collections.Generic;

namespace Services.Notification
{
    public interface INotificationService
    {
        IList<NotificationModel> GetUserNotifications(DatabaseContext.Models.User user);
    }
}