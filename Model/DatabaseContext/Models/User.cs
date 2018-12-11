using System;
using System.Collections.Generic;
using System.Linq;
using Data.Mapping.Attributes;
using Model.Database;
using ModelCore;

namespace DatabaseContext.Models
{
    [Table("User")]
    public class User : BaseModel
    {
        [Key] public ulong Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public Lazy<IList<Role>> Roles { get; set; }
        public Lazy<IList<State>> States { get; set; }
        public Lazy<IList<Place>> Places { get; set; }
        public Lazy<IList<Order>> Orders { get; set; }

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
                base.Delete(this);

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