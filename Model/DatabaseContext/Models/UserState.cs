﻿using System;
using System.Collections.Generic;
using System.Linq;
using Data.Mapping.Attributes;
using Model.Database;
using ModelCore;
using Newtonsoft.Json;

namespace DatabaseContext.Models
{
    [Table("User_has_State")]
    public class UserState : BaseModel
    {
        [Key] public ulong UserId { get; set; }
        [Key] public ulong StateId { get; set; }

        [JsonIgnore] public Lazy<User> User { get; set; }
        [JsonIgnore] public Lazy<State> State { get; set; }

        public UserState() : this(null)
        {
            
        }

        public UserState(IDBContext context) : base(context)
        {
            User = new Lazy<User>(() => new User(Context) { Id = UserId }.Find().FirstOrDefault());
            State = new Lazy<State>(() => new State(Context) { Id = StateId }.Find().FirstOrDefault());
        }

        public new UserState SetContext(IDBContext context)
        {
            base.SetContext(context);
            return this;
        }

        public UserState Save(bool insert = false)
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

        public UserState Delete()
        {
            base.Delete(this);
            return this;
        }

        public IList<UserState> Find()
        {
            return base.Find(this);
        }

        public static IList<UserState> FindAll(IDBContext context)
        {
            return BaseModel.FindAll<UserState>(context);
        }
    }
}