using System;
using System.Collections.Generic;
using System.Linq;
using Data.Mapping.Attributes;
using Model.Database;
using ModelCore;
using Newtonsoft.Json;

namespace DatabaseContext.Models
{
    [Table("User_has_Role")]
    public class UserRole : BaseModel
    {
        [Key] public ulong UserId { get; set; }
        [Key] public ulong RoleId { get; set; }

        [JsonIgnore] public Lazy<User> User { get; set; }
        [JsonIgnore] public Lazy<Role> Role { get; set; }

        public UserRole() : this(null)
        {
            
        }

        public UserRole(IDBContext context) : base(context)
        {
            User = new Lazy<User>(() => new User(Context) { Id = UserId }.Find().FirstOrDefault());
            Role = new Lazy<Role>(() => new Role(Context) { Id = RoleId }.Find().FirstOrDefault());
        }

        public new UserRole SetContext(IDBContext context)
        {
            base.SetContext(context);
            return this;
        }

        public UserRole Save(bool insert = false)
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

        public UserRole Delete()
        {
            base.Delete(this);
            return this;
        }

        public IList<UserRole> Find()
        {
            return base.Find(this);
        }

        public static IList<UserRole> FindAll(IDBContext context)
        {
            return BaseModel.FindAll<UserRole>(context);
        }
    }
}