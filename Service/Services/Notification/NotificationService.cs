using System.Collections.Generic;
using System.Linq;
using Services.Database;

namespace Services.Notification
{
    public class NotificationService : INotificationService
    {
        private readonly IDatabaseService _databaseService;

        public NotificationService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public IList<NotificationModel> GetUserNotifications(DatabaseContext.Models.User user)
        {
            var notifications = new List<NotificationModel>();

            if (user.HasRoles(UserRoles.Admin, UserRoles.Supplier))
            {

                // Add validation pending accounts
                var users = DatabaseContext.Models.User.FindAll(_databaseService.Context)?.Where(u => u.HasState(UserStates.Validating)).ToList();
                if(users != null && users.Any())
                    notifications.AddRange(
                        users.Select(u => new NotificationModel(
                            u.Id, 
                            UserStates.Validating, 
                            u.States.Value?.FirstOrDefault(s => s.Type.Equals(UserStates.Validating))?.Description, 
                            "fas fa-user-plus",
                            "danger"
                        )));

                // Add validation pending orders
                var orders = DatabaseContext.Models.Order.FindAll(_databaseService.Context)?.Where(p => p.HasState(OrderStates.Validating)).ToList();
                if (orders != null && orders.Any())
                    notifications.AddRange(
                        orders.Select(u => new NotificationModel(
                            u.Id,
                            OrderStates.Validating,
                            u.States.Value?.FirstOrDefault(s => s.Type.Equals(OrderStates.Validating))?.Description,
                            "fas fa-box",
                            "info"
                        )));
            }

            if (user.HasRoles(UserRoles.Admin, UserRoles.Supplier, UserRoles.Worker))
            {
                // Add validation pending places
                var places = DatabaseContext.Models.Place.FindAll(_databaseService.Context)?.Where(p => p.HasState(PlaceStates.Validating)).ToList();
                if(places != null && places.Any())
                    notifications.AddRange(
                        places.Select(u => new NotificationModel(
                            u.Id,
                            PlaceStates.Validating,
                            u.States.Value?.FirstOrDefault(s => s.Type.Equals(PlaceStates.Validating))?.Description,
                            "fas fa-building",
                            "warning"
                        )));
            }

            return notifications;
        }
    }
}