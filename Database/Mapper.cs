using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;

namespace Database
{
    public static class Mapper
    {
        public static T Map<T>(SqlDataReader reader, T baseModel = default(T)) where T : INullable, new()
        {
            // ToDo poslat gist ple0049 pokud bude fungovat
            var model = baseModel.Equals(default(T)) ? new T() : baseModel;
            //T model = baseModel ?? new T();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                // Parse
                dynamic fieldMetaData = new ExpandoObject();
                fieldMetaData.columnName = reader.GetName(i);
                fieldMetaData.value = reader[i];
                fieldMetaData.dotNetType = reader.GetFieldType(i);
                fieldMetaData.sqlType = reader.GetDataTypeName(i);
                fieldMetaData.specificType = reader.GetProviderSpecificFieldType(i);

                // Set
                foreach (var propertyInfo in model.GetType().GetProperties())
                {
                    if(fieldMetaData.columnName.Equals(propertyInfo.Name) && propertyInfo.CanWrite)
                        propertyInfo.SetValue(model, fieldMetaData.value);
                }
            }

            return model;
        }

        public static List<T> MapAll<T>(SqlDataReader reader) where T : INullable, new()
        {
            var list = new List<T>();
            while (reader.Read())
            {
                // identify baseModel
                list.Add(Map<T>(reader));
            }
            // not here
            reader.Close();
            return list;
        }
    }
}