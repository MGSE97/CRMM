using System;
using System.Linq;
using DatabaseContext.Models;

namespace Services
{
    public static class OrderExtensions
    {
        public static bool HasState(this DatabaseContext.Models.Order order, params string[] states)
        {
            return order.States.Value.Any(state => states.Any(r => r.Equals(state.Type) && state.DeletedOnUtc == null));
        }

        public static DatabaseContext.Models.Order SetState(this DatabaseContext.Models.Order order, ulong placeId, string state, string description = null, params State[] dropStates)
        {
            dropStates.ForEach(s =>
            {
                s.SetContext(order.Context);
                s.DeletedOnUtc = DateTime.UtcNow;
                s.Save();
            });

            var st = new State(order.Context){CreatedOnUtc = DateTime.UtcNow, Description = description, Type = state}.Save();
            new OrderState(order.Context) {OrderId = order.Id, StateId = st.Id, PlaceId = placeId}.Save();

            return order;
        }
    }
    
    public static class OrderStates
    {
        public static string Validating => "OrderValidating";
        public static string DropOf => "OrderDropOf";
        public static string Valid => "OrderValid";
        public static string Delivering => "OrderDelivering";
        public static string Delivered => "OrderDelivered";

        public static string GetNextState(string state)
        {
            if (state.Equals(Valid))
                return Delivering;
            if (state.Equals(Delivering))
                return Delivered;
            if (state.Equals(Delivered))
                return ReclamationStates.Validating;

            return null;
        }

        public static string GetNextState(DatabaseContext.Models.Order order)
        {
            if (order.HasState(Valid))
                return Delivering;
            if (order.HasState(Delivering))
                return Delivered;
            if (order.HasState(Delivered))
                return ReclamationStates.Validating;

            return ReclamationStates.GetNextState(order);
        }
    }

    public static class ReclamationStates
    {
        public static string Validating => "ReclamationValidating";
        public static string PickUp => "ReclamationPickUp";
        public static string Valid => "ReclamationValid";
        public static string PickedUp => "ReclamationPickedUp";
        public static string Running => "ReclamationRunning";
        public static string Handled => "ReclamationHandled";
        public static string Delivering => "ReclamationDelivering";
        public static string Delivered => "ReclamationDelivered";

        public static string GetNextState(string state)
        {
            if (state.Equals(Valid))
                return PickedUp;
            if (state.Equals(PickedUp))
                return Running;
            if (state.Equals(Running))
                return Handled;
            if (state.Equals(Handled))
                return Delivering;
            if (state.Equals(Delivering))
                return Delivered;
            if (state.Equals(Delivered))
                return Validating;

            return null;
        }
        public static string GetNextState(DatabaseContext.Models.Order order)
        {
            if (order.HasState(Valid))
                return PickedUp;
            if (order.HasState(PickedUp))
                return Running;
            if (order.HasState(Running))
                return Handled;
            if (order.HasState(Handled))
                return Delivering;
            if (order.HasState(Delivering))
                return Delivered;
            if (order.HasState(Delivered))
                return Validating;

            return null;
        }
    }
}
