using System;
using System.Collections.Generic;
using System.Linq;
using Data.Mapping.Attributes;
using Model.Database;
using ModelCore;

namespace DatabaseContext.Models
{
    [Table("User_has_Order")]
    public class UserOrder : BaseModel
    {
        [Key] public ulong UserId { get; set; }
        [Key] public ulong OrderId { get; set; }

        public Lazy<User> User { get; set; }
        public Lazy<Order> Order { get; set; }

        public UserOrder() : this(null)
        {
            
        }

        public UserOrder(IDBContext context) : base(context)
        {
            User = new Lazy<User>(() => new User(Context) { Id = UserId }.Find().FirstOrDefault());
            Order = new Lazy<Order>(() => new Order(Context) { Id = OrderId }.Find().FirstOrDefault());
        }

        public new UserOrder SetContext(IDBContext context)
        {
            base.SetContext(context);
            return this;
        }

        public UserOrder Save(bool insert = false)
        {
            return base.Save(this, insert);
        }

        public UserOrder Delete()
        {
            base.Delete(this);
            return this;
        }

        public IList<UserOrder> Find()
        {
            return base.Find(this);
        }

        public static IList<UserOrder> FindAll(IDBContext context)
        {
            return BaseModel.FindAll<UserOrder>(context);
        }
    }
}