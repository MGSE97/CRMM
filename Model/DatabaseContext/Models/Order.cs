﻿using System;
using System.Collections.Generic;
using Data.Mapping.Attributes;
using Model.Database;
using ModelCore;

namespace DatabaseContext.Models
{
    [Table("Order")]
    public class Order : BaseModel
    {
        [Key] public ulong Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public Lazy<IList<State>> States { get; set; }

        public Order()
        {
            States = new Lazy<IList<State>>(() => { return State.FindAll(Context); });
        }

        public new Order SetContext(IDBContext context)
        {
            base.SetContext(context);
            return this;
        }

        public Order Save()
        {
            if (Id > 0)
                return base.Save(this, false);

            return base.Save(this, true);
        }

        public Order Delete()
        {
            if (Id > 0)
                base.Delete(this);

            return this;
        }

        public IList<Order> Find()
        {
            return base.Find(this);
        }

        public static IList<Order> FindAll(IDBContext context)
        {
            return BaseModel.FindAll<Order>(context);
        }

    }
}