using System;
using System.Collections.Generic;
using Data.Mapping.Attributes;
using Model.Database;
using ModelCore;

namespace DatabaseContext.Models
{
    [Table("State")]
    public class State : BaseModel
    {
        [Key] public ulong Id { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOnUtc { get; set; }

        public DateTime DeletedOnUtc { get; set; }

        public State() : this(null)
        {

        }

        public State(IDBContext context) :base(context)
        {
        }

        public new State SetContext(IDBContext context)
        {
            base.SetContext(context);
            return this;
        }

        public State Save()
        {
            if (Id > 0)
                return base.Save(this, false);

            return base.Save(this, true);
        }

        public State Delete()
        {
            if (Id > 0)
                base.Delete(this);

            return this;
        }

        public IList<State> Find()
        {
            return base.Find(this);
        }

        public static IList<State> FindAll(IDBContext context)
        {
            return BaseModel.FindAll<State>(context);
        }

    }
}