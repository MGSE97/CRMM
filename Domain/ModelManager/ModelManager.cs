using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Data.Mapping;
using Data.Mapping.Extensions;
using DataCore;

namespace Model.Manager
{
    public class ModelManager
    {
        public IConnector Connector { get; protected set; }

        public ModelManager(IConnector connector)
        {
            Connector = connector;
        }

        public TModel Insert<TModel>(TModel model) where TModel : new()
        {
            var pairs = model.ToDictionary(AccessProtection.Private, MapMode.Both|MapMode.NotNull);
            var sql = Connector.SqlBuilder.Insert(model.GetTableName(), pairs.Keys.ToList());
            var id = Connector.ExecuteSqlScalar<ulong>(sql, pairs);

            // Set received key
            var key = model.GetKeys(AccessProtection.Private).FirstOrDefault().Key;
            if (!string.IsNullOrWhiteSpace(key))
                model.GetType().GetProperty(key).SetValue(model, id);

            return model;
        }

        public TModel Update<TModel>(TModel model) where TModel : new()
        {
            var values = model.ToDictionary(AccessProtection.Private, MapMode.Both|MapMode.NotNull);
            var keys = model.ToDictionary(AccessProtection.Private, MapMode.Keys);
            var sql = Connector.SqlBuilder.Update(model.GetTableName(), values.Keys.ToList(), keys.Keys.ToList());
            return Connector.ExecuteSqlReader<TModel>(sql, values.Union(keys).ToDictionary(p => p.Key, p => p.Value)).FirstOrDefault();
        }

        public TModel Delete<TModel>(TModel model) where TModel : new()
        {
            var keys = model.ToDictionary(AccessProtection.Private, MapMode.Keys);
            var sql = Connector.SqlBuilder.Delete(model.GetTableName(), keys.Keys.ToList());
            Connector.ExecuteSql(sql, keys);
            return model;
        }

        public IList<TModel> Search<TModel>(TModel model = default(TModel)) where TModel : new()
        {
            if (EqualityComparer<TModel>.Default.Equals(model, default(TModel)))
            {
                // use Type
                var properties = typeof(TModel).GetProperties().Where(p => !p.IsLazy() && !p.IsIgnored());
                var sql = Connector.SqlBuilder.Search(typeof(TModel).GetTableName(), properties.Select(p => p.Name).ToList());
                return Connector.ExecuteSqlReader<TModel>(sql, null).ToList();
            }
            else
            {
                // use Model
                var both = model.ToDictionary(AccessProtection.Private, MapMode.Both|MapMode.NotNull);
                var columns = model.ToDictionary(AccessProtection.Private);
                var sql = Connector.SqlBuilder.Search(model.GetTableName(), columns.Keys.ToList(), both.Keys.ToList());
                return Connector.ExecuteSqlReader<TModel>(sql, both).ToList();
            }
            
        }
    }
}
