using System;
using System.Collections.Generic;
using Data.Mapping.Attributes;
using Model.Database;
using Model.Manager;
using ModelCore;

namespace DatabaseContext.Models
{
    [Table("Role")]
    public class Role : BaseModel
    {
        [Key] public ulong Id { get; set; }

        public string Name { get; set; }


        public Role() : this(null)
        {
            
        }

        public Role(IDBContext context) : base(context)
        {
        }

        public new Role SetContext(IDBContext context)
        {
            base.SetContext(context);
            return this;
        }

        public Role Save()
        {
            if (Id > 0)
                return base.Save(this, false);

            return base.Save(this, true);
        }

        public Role Delete()
        {
            if(Id > 0)
                base.Delete(this);

            return this;
        }

        public IList<Role> Find()
        {
            return base.Find(this);
        }

        public static IList<Role> FindAll(IDBContext context)
        {
            return BaseModel.FindAll<Role>(context);
        }

    }
}
