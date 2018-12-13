using System;
using System.Linq;
using DatabaseContext.Models;
using Services.Database;
using Services.WorkContext;

namespace Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly IWorkContext _workContext;
        private readonly IDatabaseService _databaseService;

        public OrderService(IWorkContext workContext, IDatabaseService databaseService)
        {
            _workContext = workContext;
            _databaseService = databaseService;
        }

        private DatabaseContext.Models.Order SaveOrder(DatabaseContext.Models.Order order, DatabaseContext.Models.Place location)
        {
            // Update order
            order = order.SetContext(_databaseService.Context).Save();
            if (location != null)
            {
                var orderStates = new OrderState(_databaseService.Context) {OrderId = order.Id}.Find().Where(s => s.State.Value.DeletedOnUtc == null).ToList();
                if (orderStates.All(s => s.PlaceId != location.Id))
                {
                    // Set drop of
                    var state = new State(_databaseService.Context)
                    {
                        CreatedOnUtc = DateTime.UtcNow, Type = OrderStates.DropOf,
                        Description = $"Doručit do {location.Name}"
                    }.Save();
                    new OrderState(_databaseService.Context){OrderId = order.Id, StateId = state.Id, PlaceId = location.Id}.Save();

                    if (orderStates.All(s => !s.State.Value.Type.Equals(OrderStates.Validating)))
                    {
                        var created = new State(_databaseService.Context)
                        {
                            CreatedOnUtc = DateTime.UtcNow,
                            Type = OrderStates.Validating,
                            Description = $"Nová objednávka od {_workContext.CurrentUser.Name} do {location.Name}"
                        }.Save();
                        new OrderState(_databaseService.Context){OrderId = order.Id, StateId = created.Id, PlaceId = location.Id}.Save();
                    }
                }
            }

            return order;
        }

        public DatabaseContext.Models.Order CreateOrder(DatabaseContext.Models.Order order, DatabaseContext.Models.Place location)
        {
            order = SaveOrder(order, location);
            new UserOrder(_databaseService.Context) {UserId = _workContext.CurrentUser.Id, OrderId = order.Id}.Save();
            return order;
        }

        public DatabaseContext.Models.Order UpdateOrder(DatabaseContext.Models.Order order, DatabaseContext.Models.Place location)
        {
            return SaveOrder(order, location);
        }

    }
}