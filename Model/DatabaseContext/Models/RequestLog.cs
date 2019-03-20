using Model.Database;
using ModelCore;
using System;
using System.Collections.Generic;
using Data.Mapping.Attributes;

namespace DatabaseContext.Models
{
    [Table("RequestLog")]
    public class RequestLog : BaseModel
    {
        [Key] public ulong Id { get; set; }

        public string Url { get; set; }

        public ulong? UserId { get; set; }

        public DateTime CreatedOnUtc { get; set; }


        public RequestLog() : this(null)
        {

        }

        public RequestLog(IDBContext context) : base(context)
        {
        }

        public new RequestLog SetContext(IDBContext context)
        {
            base.SetContext(context);
            return this;
        }

        public RequestLog Save()
        {
            if (Id > 0)
                return base.Save(this, false);

            return base.Save(this, true);
        }

        public RequestLog Delete()
        {
            if (Id > 0)
                base.Delete(this);

            return this;
        }

        public IList<RequestLog> Find()
        {
            return base.Find(this);
        }

        public static IList<RequestLog> FindAll(IDBContext context)
        {
            return BaseModel.FindAll<RequestLog>(context);
        }

        public IList<RequestLog> FindRange(RequestLog end)
        {
            return base.FindBetween(this, end);
        }
    }
}