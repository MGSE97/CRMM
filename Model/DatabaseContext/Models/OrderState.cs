using System;
using System.Collections.Generic;
using System.Linq;
using Data.Mapping.Attributes;
using Model.Database;
using ModelCore;
using Newtonsoft.Json;

namespace DatabaseContext.Models
{
    [Table("OrderState")]
    public class OrderState : BaseModel
    {
        [Key] public ulong StateId { get; set; }
        [Key] public ulong PlaceId { get; set; }
        [Key] public ulong OrderId { get; set; }

        [JsonIgnore] public Lazy<State> State { get; set; }
        [JsonIgnore] public Lazy<Place> Place { get; set; }
        [JsonIgnore] public Lazy<Order> Order { get; set; }

        public OrderState() : this(null)
        {
            
        }

        public OrderState(IDBContext context) : base(context)
        {
            State = new Lazy<State>(() => new State(Context) { Id = StateId }.Find().FirstOrDefault());
            Place = new Lazy<Place>(() => new Place(Context) { Id = PlaceId }.Find().FirstOrDefault());
            Order = new Lazy<Order>(() => new Order(Context) { Id = OrderId }.Find().FirstOrDefault());
        }

        public new OrderState SetContext(IDBContext context)
        {
            base.SetContext(context);
            return this;
        }

        public OrderState Save(bool insert = false)
        {
            if (insert == false)
            {
                try
                {
                    Find()?.FirstOrDefault()?.Delete();
                }
                catch
                {
                    // Ignored
                }
            }

            return base.Save(this, true);
        }

        public OrderState Delete()
        {
            base.Delete(this);
            return this;
        }

        public IList<OrderState> Find()
        {
            return base.Find(this);
        }

        public static IList<OrderState> FindAll(IDBContext context)
        {
            return BaseModel.FindAll<OrderState>(context);
        }
    }
}