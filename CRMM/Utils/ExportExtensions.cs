using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Xml;
using Newtonsoft.Json;

namespace CRMM.Utils
{
    public static class ExportExtensions
    {
        public static FileContentResult ExportData<T>(this T data, string type, string prefix)
        {
            string text;
            FileContentResult result = null;
            switch (type.ToLowerInvariant())
            {
                case "json":
                    text = JsonConvert.SerializeObject(data);
                    result = new FileContentResult(Encoding.UTF8.GetBytes(text), "text/json");
                    result.FileDownloadName = $"{prefix}_Export_{DateTime.Now.Ticks}.json";
                    break;
                case "xml":
                    text = data.Serialize().OuterXml;
                    result = new FileContentResult(Encoding.UTF8.GetBytes(text), "text/xml");
                    result.FileDownloadName = $"{prefix}_Export_{DateTime.Now.Ticks}.xml";
                    break;
            }

            return result;
        }

        public static XmlDocument Serialize<TData>(this TData data)
        {
            var doc = new XmlDocument();
            var dec = doc.CreateXmlDeclaration("1.0", "utf-8", "yes");
            var root = doc.CreateElement(XmlConvert.EncodeName(data.GetType().Name));
            doc.AppendChild(root);
            doc.InsertBefore(dec, root);
            if (data is IEnumerable enumerable)
            {
                foreach (var item in enumerable)
                {
                    var parent = doc.CreateElement(XmlConvert.EncodeName(item.GetType().Name));
                    foreach (var propertyInfo in item.GetType().GetProperties())
                    {
                        var el = doc.CreateElement(propertyInfo.Name);
                        el.InnerText = propertyInfo.GetValue(item).ToString();
                        parent.AppendChild(el);
                    }

                    root.AppendChild(parent);
                }
            }
            else
                foreach (var propertyInfo in data.GetType().GetProperties())
                {
                    var el = doc.CreateElement(propertyInfo.Name);
                    el.InnerText = propertyInfo.GetValue(data).ToString();
                    root.AppendChild(el);
                }
            return doc;
        }
    }
}