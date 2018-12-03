using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Data.Mapping.Extensions;

namespace Data.Mapping
{
    public static class ModelMapper
    {
        public static IDictionary<string, object> GetKeys<TModel>(this TModel model, AccessProtection accessProtection = AccessProtection.Public)
        {
            return model.GetType().GetProperties()
                .Where(p => !p.IsIgnored() && !p.IsReadOnly() && p.CanWrite(accessProtection))
                .GetTypeKeys()
                .ToDictionary(k => k.Name, k => k.GetValue(model));
        }

        public static IDictionary<string, object> ToDictionary<TModel>(this TModel model, AccessProtection accessProtection = AccessProtection.Public, MapMode mapMode = MapMode.Both, params string[] selected)
        {
            if (model == null)
                //throw new ArgumentNullException(nameof(model));
                return new Dictionary<string, object>();
            return model.GetType().GetProperties()
                .Where(p => 
                    (!selected.Any() || selected.Any(s => s.Equals(p.Name))) &&
                    !p.IsIgnored() && 
                    p.CanRead(accessProtection) && 
                    (
                        mapMode == MapMode.Both || !(mapMode.HasFlag(MapMode.Keys) ^ p.IsKey())
                    ))
                .ToDictionary(p => p.Name, p => p.GetValue(model));
        }

        public static TModel Map<TModel>(this TModel values, TModel baseModel = default(TModel), AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            if (values == null)
                //throw new ArgumentNullException(nameof(values));
                return baseModel;

            return Map(
                values.ToDictionary(accessProtection),
                baseModel,
                accessProtection
            );
        }

        /*public static IDictionary<string, object> Map<TModel>(this TModel values, TModel baseModel = default(TModel), AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            if (values == null)
                throw new ArgumentNullException(nameof(values));

            return Map(
                values.ToDictionary(accessProtection),
                baseModel, 
                accessProtection
            ).ToDictionary(accessProtection);
        }*/

        public static TModel Map<TModel>(this IDictionary<string, object> values, TModel baseModel = default(TModel), AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            if (values == null)
                //throw new ArgumentNullException(nameof(values));
                return baseModel;

            // Create new model, or edit old one
            var create = EqualityComparer<TModel>.Default.Equals(baseModel, default(TModel));
            var model = create ? new TModel() : baseModel;

            // Get model properties
            var properties = model.GetType().GetProperties().Where(p => !p.IsIgnored() && !p.IsReadOnly() && p.CanWrite(accessProtection)).ToArray();

            // Find and set values
            foreach (var value in values)
            {
                if (create || value.Value != null)
                {
                    var property = properties.FirstOrDefault(p => p.Name.Equals(value.Key));
                    if (property != null)
                        property.SetValue(model, value.Value);
                }
            }

            return model;
        }
    }
}
