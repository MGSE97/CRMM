using System;
using System.Collections.Generic;
using System.Linq;
using Data.Mapping.Attributes;
using Model.Database;
using ModelCore;

namespace DatabaseContext.Models
{
    [Table("User_has_Place")]
    public class UserPlace : BaseModel
    {
        [Key] public ulong UserId { get; set; }
        [Key] public ulong PlaceId { get; set; }

        public Lazy<User> User { get; set; }
        public Lazy<Place> Place { get; set; }

        public UserPlace() : this(null)
        {
            
        }

        public UserPlace(IDBContext context) : base(context)
        {
            User = new Lazy<User>(() => new User(Context) { Id = UserId }.Find().FirstOrDefault());
            Place = new Lazy<Place>(() => new Place(Context) { Id = PlaceId }.Find().FirstOrDefault());
        }

        public new UserPlace SetContext(IDBContext context)
        {
            base.SetContext(context);
            return this;
        }

        public UserPlace Save(bool insert = false)
        {
            return base.Save(this, insert);
        }

        public UserPlace Delete()
        {
            base.Delete(this);
            return this;
        }

        public IList<UserPlace> Find()
        {
            return base.Find(this);
        }

        public static IList<UserPlace> FindAll(IDBContext context)
        {
            return BaseModel.FindAll<UserPlace>(context);
        }
    }
}