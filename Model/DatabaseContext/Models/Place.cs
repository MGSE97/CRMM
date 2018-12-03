using System;
using System.Collections.Generic;
using Data.Mapping.Attributes;
using Model.Database;
using ModelCore;

namespace DatabaseContext.Models
{
    [Table("Place")]
    public class Place : BaseModel
    {
        [Key]
        public ulong Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string Address { get; set; }

        public double X { get; set; }

        public double Y { get; set; }

        public Lazy<IList<Order>> Orders { get; set; }

        public Lazy<IList<State>> States { get; set; }

        public new Place SetContext(IDBContext context)
        {
            base.SetContext(context);
            return this;
        }

        public Place Save()
        {
            if (Id > 0)
                return base.Save(this, false);

            return base.Save(this, true);
        }

        public Place Delete()
        {
            if (Id > 0)
                base.Delete(this);

            return this;
        }

        public IList<Place> Find()
        {
            return base.Find(this);
        }

        public static IList<Place> FindAll(IDBContext context)
        {
            return BaseModel.FindAll<Place>(context);
        }

    }
}
