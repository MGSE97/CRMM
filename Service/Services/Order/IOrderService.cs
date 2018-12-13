using DatabaseContext.Models;

namespace Services.Order
{
    public interface IOrderService
    {
        DatabaseContext.Models.Order CreateOrder(DatabaseContext.Models.Order order, DatabaseContext.Models.Place location);
        DatabaseContext.Models.Order UpdateOrder(DatabaseContext.Models.Order order, DatabaseContext.Models.Place location);
    }
}