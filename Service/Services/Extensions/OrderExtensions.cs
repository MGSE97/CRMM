using System.Linq;

namespace Services
{
    public static class OrderExtensions
    {
        public static bool HasState(this DatabaseContext.Models.Order order, params string[] states)
        {
            return order.States.Value.Any(state => states.Any(r => r.Equals(state.Type) && state.DeletedOnUtc == null));
        }
    }
    
    public static class OrderStates
    {
        public static string Validating => "OrderValidating";
        public static string DropOf => "OrderDropOf";
    }
}
