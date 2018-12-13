using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Data.Mapping.Extensions;

namespace Data.Mapping
{
    public static class SqlMapper
    {
        public static TModel Map<TModel>(this IDataReader reader, TModel baseModel = default(TModel), AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            // Create new model, or edit old one
            var model = EqualityComparer<TModel>.Default.Equals(baseModel, default(TModel)) ? new TModel() : baseModel;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                // Parse & Set
                foreach (var propertyInfo in model.GetType().GetProperties().Where(p => !p.IsLazy()))
                {
                    if (reader.GetName(i).Equals(propertyInfo.Name) && propertyInfo.CanWrite(accessProtection))
                    {
                        object value = reader[i];
                        if (value == DBNull.Value)
                            value = null;
                        propertyInfo.SetValue(model, value);
                    }
                }
            }

            return model;
        }

        public static IEnumerable<TModel> MapAll<TModel>(this IDataReader reader, IEnumerable<TModel> baseModels = null, bool removeUnReaded = false, AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            var models = baseModels?.ToList() ?? new List<TModel>();

            // Just read new models
            if (baseModels == null)
            {
                while (reader.Read())
                    models.Add(Map<TModel>(reader, accessProtection: AccessProtection.Private));

                reader.Close();

                return models;
            }

            // Update old models, add new one
            var updated = new List<TModel>();
            while (reader.Read())
            {
                var model = models.FirstOrDefault(m => m.CompareKeys(reader));
                if (model == null)
                    updated.Add(Map<TModel>(reader, accessProtection: AccessProtection.Private));
                else
                    updated.Add(Map(reader, model, accessProtection));
            }

            reader.Close();

            if (removeUnReaded)
                return updated;

            // Remove unused old models
            updated.AddRange(models.Where(model => updated.All(m => !m.CompareKeys(model))));
            return updated;
        }
    }
}