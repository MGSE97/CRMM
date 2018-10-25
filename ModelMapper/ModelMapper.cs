using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;

namespace Data.Mapping
{
    public static class ModelMapper
    {
        public static TModel Map<TModel, TValues>(TValues values, Func<TValues, IEnumerable<string>> keyBind, Func<TValues, IEnumerable<object>> valueBind, TModel baseModel = default(TModel), AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            var data = new Dictionary<string, object>();
            var keys = keyBind(values).ToList();
            var vals = valueBind(values).ToList();

            for(int i = 0; i < keys.Count && i < vals.Count; i++)
                data.Add(keys[i], vals[i]);

            return Map(data, baseModel, accessProtection);
        }

        public static TModel Map<TModel, TValues>(TValues values, Func<TValues, IDictionary<string,object>> dataBind, TModel baseModel = default(TModel), AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            return Map(dataBind(values), baseModel, accessProtection);
        }

        public static TModel Map<TModel>(IEnumerable<KeyValuePair<string, object>> values, TModel baseModel = default(TModel), AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            return Map(values, data => data.Key, data => data.Value, baseModel, accessProtection);
        }

        public static TModel Map<TModel>(IEnumerable<object> values, TModel baseModel = default(TModel), AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            return Map(values, data => data.GetType().GetProperty("key").GetValue(data) as string,
                               data => data.GetType().GetProperty("value").GetValue(data)
                    , baseModel, accessProtection);
        }

        public static TModel Map<TModel>(object values, TModel baseModel = default(TModel), AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            return Map(values, data => data.GetType().GetProperties()
                                           .Where(p => p.CanRead && p.CanWrite(accessProtection))
                                           .Select(p => new {key = p.Name, value = p.GetValue(data)})
                                           .ToDictionary(t => t.key, t => t.value)
                    , baseModel, accessProtection);
        }

        public static TModel Map<TModel, TValue>(IEnumerable<TValue> values, Func<TValue, string> keyBind, Func<TValue, object> valueBind, TModel baseModel = default(TModel), AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            return Map(values.Select(value => new { key = keyBind(value), value = valueBind(value)}).ToDictionary(t => t.key, t => t.value), baseModel, accessProtection);
        }

        public static TModel Map<TModel, TValue>(IEnumerable<TValue> values, Func<TValue, (string, object)> dataBind, TModel baseModel = default(TModel), AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            return Map(values.Select(dataBind).ToDictionary(t => t.Item1, t => t.Item2), baseModel, accessProtection);
        }

        public static TModel Map<TModel>(IDictionary<string, object> values, TModel baseModel = default(TModel), AccessProtection accessProtection = AccessProtection.Public) where TModel : new()
        {
            // Create new model, or edit old one
            var model = baseModel.Equals(default(TModel)) ? new TModel() : baseModel;

            // Get model properties
            var properties = model.GetType().GetProperties();

            // Find and set values
            foreach (var value in values)
            {
                var property = properties.FirstOrDefault(p => p.Name.Equals(value.Key) && p.CanWrite(accessProtection));
                if(property != null)
                    property.SetValue(model, value.Value);
            }

            return model;
        }
    }
}
