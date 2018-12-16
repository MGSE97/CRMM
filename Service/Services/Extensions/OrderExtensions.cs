using System;
using System.Linq;
using System.Runtime.CompilerServices;
using DatabaseContext.Models;

namespace Services
{
    public static class OrderExtensions
    {
        public static bool HasState(this DatabaseContext.Models.Order order, params string[] states)
        {
            return order.States.Value.Any(state => states.Any(r => r.Equals(state.Type) && state.DeletedOnUtc == null));
        }

        public static State GetState(this DatabaseContext.Models.Order order, params string[] states)
        {
            if (states == null || states.Length == 0)
                return null;
            return order.States.Value.FirstOrDefault(state => states.Any(r => r.Equals(state.Type) && state.DeletedOnUtc == null));
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

        public static DatabaseContext.Models.Order AddUser(this DatabaseContext.Models.Order order, DatabaseContext.Models.User user)
        {
            if (order.Users.Value.All(u => u.Id != user.Id))
                new UserOrder(order.Context) {OrderId = order.Id, UserId = user.Id}.Save();
            return order;
        }
        public static DatabaseContext.Models.Order RemoveUser(this DatabaseContext.Models.Order order, DatabaseContext.Models.User user)
        {
            if (!user.HasRoles(UserRoles.Customer) && order.Users.Value.Any(u => u.Id == user.Id))
                new UserOrder(order.Context) {OrderId = order.Id, UserId = user.Id}.Delete();

            return order;
        }

        public static ulong GetDropOfId(this DatabaseContext.Models.Order order)
        {
            return new OrderState(order.Context) {OrderId = order.Id, StateId = order.GetState(OrderStates.DropOf).Id}.Find().FirstOrDefault()?.PlaceId??0;
        }

        public static DatabaseContext.Models.Place GetDropOf(this DatabaseContext.Models.Order order)
        {
            return new OrderState(order.Context) {OrderId = order.Id, StateId = order.GetState(OrderStates.DropOf).Id}.Find().FirstOrDefault()?.Place.Value;
        }

        public static DatabaseContext.Models.OrderState GetDropOfRaw(this DatabaseContext.Models.Order order)
        {
            return new OrderState(order.Context) { OrderId = order.Id, StateId = order.GetState(OrderStates.DropOf).Id }.Find().FirstOrDefault();
        }
    }
    
    public static class OrderStates
    {
        [Role("Admin")]
        [Role("Supplier")]
        public static string Validating => "OrderValidating";
        public static string DropOf => "OrderDropOf";
        [Role("Admin")]
        [Role("Supplier")]
        public static string Valid => "OrderValid";
        [Role("Admin")]
        [Role("Supplier")]
        [Role("Worker")]
        public static string Delivering => "OrderDelivering";
        [Role("Admin")]
        [Role("Supplier")]
        [Role("Worker")]
        public static string Delivered => "OrderDelivered";

        public static string GetNextState(string state, DatabaseContext.Models.User user)
        {
            var s = GetNextState(state);
            if (user == null || s == null)
                return s;
            bool valid;
            if (state.StartsWith("Reclamation"))
            {
                var trim = "Reclamation";
                valid = typeof(ReclamationStates).TypeHasRoles(s.Substring(trim.Length, s.Length - trim.Length), user.Roles.Value.Select(r => r.Name).ToArray());
            }
            else
            {
                var trim = "Order";
                valid = typeof(OrderStates).TypeHasRoles(s.Substring(trim.Length, s.Length - trim.Length), user.Roles.Value.Select(r => r.Name).ToArray());
            }
            return valid ? s : null;
        }

        public static string GetNextState(DatabaseContext.Models.Order order, DatabaseContext.Models.User user)
        {
            var state = GetNextState(order);
            if (user == null || state == null)
                return state;
            bool valid;
            if (state.StartsWith("Reclamation"))
            {
                var trim = "Reclamation";
                valid = typeof(ReclamationStates).TypeHasRoles(state.Substring(trim.Length, state.Length-trim.Length), user.Roles.Value.Select(r => r.Name).ToArray());
            }
            else
            {
                var trim = "Order";
                valid = typeof(OrderStates).TypeHasRoles(state.Substring(trim.Length, state.Length-trim.Length), user.Roles.Value.Select(r => r.Name).ToArray());
            }
                
            return valid ? state : null;
        }

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


        public static string GetState(DatabaseContext.Models.Order order)
        {
            if (order.HasState(Validating))
                return Validating;
            if (order.HasState(Valid))
                return Valid;
            if (order.HasState(Delivering))
                return Delivering;
            if (order.HasState(Delivered))
                return Delivered;

            return ReclamationStates.GetState(order);
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
        [Role("Admin")]
        [Role("Supplier")]
        [Role("Customer")]
        public static string Validating => "ReclamationValidating";
        public static string PickUp => "ReclamationPickUp";
        [Role("Admin")]
        [Role("Supplier")]
        public static string Valid => "ReclamationValid";
        [Role("Admin")]
        [Role("Supplier")]
        [Role("Worker")]
        public static string PickedUp => "ReclamationPickedUp";
        [Role("Admin")]
        [Role("Supplier")]
        [Role("Worker")]
        public static string Running => "ReclamationRunning";
        [Role("Admin")]
        [Role("Supplier")]
        public static string Handled => "ReclamationHandled";
        [Role("Admin")]
        [Role("Supplier")]
        [Role("Worker")]
        public static string Delivering => "ReclamationDelivering";
        [Role("Admin")]
        [Role("Supplier")]
        [Role("Worker")]
        public static string Delivered => "ReclamationDelivered";

        public static string GetNextState(string state, DatabaseContext.Models.User user)
        {
            var s = GetNextState(state);
            if (user == null || s == null)
                return s;
            var trim = "Reclamation";
            var valid = typeof(ReclamationStates).TypeHasRoles(s.Substring(trim.Length,s.Length-trim.Length), user.Roles.Value.Select(r => r.Name).ToArray());
            return valid ? s : null;
        }

        public static string GetNextState(DatabaseContext.Models.Order order, DatabaseContext.Models.User user)
        {
            var state = GetNextState(order);
            if (user == null || state == null)
                return state;
            var trim = "Reclamation";
            var valid = typeof(ReclamationStates).TypeHasRoles(state.Substring(trim.Length,state.Length-trim.Length), user.Roles.Value.Select(r => r.Name).ToArray());
            return valid ? state : null;
        }

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

        public static string GetState(DatabaseContext.Models.Order order)
        {
            if (order.HasState(Validating))
                return Validating;
            if (order.HasState(Valid))
                return Valid;
            if (order.HasState(PickedUp))
                return PickedUp;
            if (order.HasState(Running))
                return Running;
            if (order.HasState(Handled))
                return Handled;
            if (order.HasState(Delivering))
                return Delivering;
            if (order.HasState(Delivered))
                return Delivered;

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
