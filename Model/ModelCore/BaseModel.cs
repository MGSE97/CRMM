using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using Data.Mapping;
using Data.Mapping.Attributes;
using Model.Database;
using Newtonsoft.Json;

namespace ModelCore
{
    public class BaseModel
    {
        [Ignore]
        [JsonIgnore]
        public IDBContext Context { get; protected set; }

        public BaseModel()
        {
            
        }

        public BaseModel(IDBContext context)
        {
            Context = context;
        }

        public virtual void SetContext(IDBContext context)
        {
            Context = context;
        }

        public virtual TModel Save<TModel>(TModel model, bool? insert = null) where TModel : new()
        {
            if (insert.HasValue)
            {
                if (insert.Value)
                    return Context.ModelManager.Insert(model);

                return Context.ModelManager.Update(model);
            }

            if (this.GetKeys().All(key => key.Value != null))
                return Context.ModelManager.Update(model);

            return Context.ModelManager.Insert(model);
        }

        public virtual void Delete<TModel>(TModel model) where TModel : new()
        {
            Context.ModelManager.Delete(model);
        }

        public virtual IList<TModel> Find<TModel>(TModel model = default(TModel)) where TModel : BaseModel, new()
        {
            var found = Context.ModelManager.Search(model);
            foreach (var f in found)
                f.SetContext(Context);
            return found;
        }

        public static IList<TModel> FindAll<TModel>(IDBContext context, TModel model = default(TModel)) where TModel : BaseModel, new()
        {
            var found = context.ModelManager.Search(model);
            foreach (var f in found)
                f.SetContext(context);
            return found;
        }
    }
}
