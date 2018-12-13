using System;
using System.Collections.Generic;
using System.Linq;
using Data.Mapping.Attributes;
using Model.Database;
using ModelCore;
using Newtonsoft.Json;

namespace DatabaseContext.Models
{
    [Table("Place_has_State")]
    public class PlaceState : BaseModel
    {
        [Key] public ulong PlaceId { get; set; }
        [Key] public ulong StateId { get; set; }

        [JsonIgnore] public Lazy<Place> Place { get; set; }
        [JsonIgnore] public Lazy<State> State { get; set; }

        public PlaceState() : this(null)
        {
            
        }

        public PlaceState(IDBContext context) : base(context)
        {
            Place = new Lazy<Place>(() => new Place(Context) { Id = PlaceId }.Find().FirstOrDefault());
            State = new Lazy<State>(() => new State(Context) { Id = StateId }.Find().FirstOrDefault());
        }

        public new PlaceState SetContext(IDBContext context)
        {
            base.SetContext(context);
            return this;
        }

        public PlaceState Save(bool insert = false)
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

        public PlaceState Delete()
        {
            base.Delete(this);
            return this;
        }

        public IList<PlaceState> Find()
        {
            return base.Find(this);
        }

        public static IList<PlaceState> FindAll(IDBContext context)
        {
            return BaseModel.FindAll<PlaceState>(context);
        }
    }
}