using System;
using System.Collections.Generic;
using System.Linq;
using Data.Mapping.Attributes;
using Model.Database;
using ModelCore;
using Newtonsoft.Json;

namespace DatabaseContext.Models
{
    [Table("User")]
    public class User : BaseModel
    {
        [Key] public ulong Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [JsonIgnore] public Lazy<IList<Role>> Roles { get; set; }
        [JsonIgnore] public Lazy<IList<State>> States { get; set; }
        [JsonIgnore] public Lazy<IList<Place>> Places { get; set; }
        [JsonIgnore] public Lazy<IList<Order>> Orders { get; set; }

        public User() : this(null)
        {
        }

        public User(IDBContext context) : base(context)
        {
            Roles = new Lazy<IList<Role>>(() => new UserRole(Context) { UserId = Id }.Find().Select(x => x.Role.Value).ToList());
            States = new Lazy<IList<State>>(() => new UserState(Context) { UserId = Id }.Find().Select(x => x.State.Value).ToList());
            Places = new Lazy<IList<Place>>(() => new UserPlace(Context) { UserId = Id }.Find().Select(x => x.Place.Value).ToList());
            Orders = new Lazy<IList<Order>>(() => new UserOrder(Context) { UserId = Id }.Find().Select(x => x.Order.Value).ToList());
        }

        public new User SetContext(IDBContext context)
        {
            base.SetContext(context);
            return this;
        }

        public User Save()
        {
            if (Id > 0)
                return base.Save(this, false);

            return base.Save(this, true);
        }

        public User Delete()
        {
            if (Id > 0)
            {
                foreach (var state in States.Value)
                {
                    new UserState(Context) {UserId = Id, StateId = state.Id}.Delete();
                    state.Delete();
                }

                foreach (var order in Orders.Value)
                {
                    new UserOrder(Context) {UserId = Id, OrderId = order.Id}.Delete();
                    order.Delete();
                }

                foreach (var place in Places.Value)
                {
                    new UserPlace(Context) {UserId = Id, PlaceId = place.Id}.Delete();
                    place.Delete();
                }

                foreach (var role in Roles.Value)
                {
                    new UserRole(Context) {UserId = Id, RoleId = role.Id}.Delete();
                }

                base.Delete(this);
            }

            return this;
        }

        public IList<User> Find()
        {
            return base.Find(this);
        }

        public static IList<User> FindAll(IDBContext context)
        {
            return BaseModel.FindAll<User>(context);
        }

    }
}