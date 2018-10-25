using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;

namespace Data.Mapping
{
    public class SqlMapper
    {
        public static TModel Map<TModel>(SqlDataReader reader, TModel baseModel = default(TModel), AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            // Create new model, or edit old one
            var model = baseModel.Equals(default(TModel)) ? new TModel() : baseModel;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                // Parse & Set
                foreach (var propertyInfo in model.GetType().GetProperties())
                {
                    if (reader.GetName(i).Equals(propertyInfo.Name) && propertyInfo.CanWrite(accessProtection))
                        propertyInfo.SetValue(model, reader[i]);
                }
            }

            return model;
        }

        public static IEnumerable<TModel> MapAll<TModel>(SqlDataReader reader, IEnumerable<TModel> baseModels = null, bool removeUnReaded = false, AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            var models = baseModels?.ToList() ?? new List<TModel>();

            if (baseModels == null)
            {
                while (reader.Read())
                    models.Add(Map<TModel>(reader, accessProtection: accessProtection));
                return models;
            }

            var updated = new List<TModel>();
            while (reader.Read())
            {
                var model = models.FirstOrDefault(m => m.CompareKeys(reader));
                if (model == null)
                    updated.Add(Map<TModel>(reader, accessProtection: accessProtection));
                else
                    updated.Add(Map(reader, model, accessProtection));
            }

            if (removeUnReaded)
                return updated;

            updated.AddRange(models.Where(model => updated.All(m => !m.CompareKeys(model))));
            return updated;
        }
    }
}