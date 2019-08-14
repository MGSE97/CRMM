using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Primitives;

namespace CRMM.Utils
{
    public class ApiControllerHeadersFilter : ResultFilterAttribute
    {
        public IDictionary<string, Func<HttpContext, string>> Headers { get; }

        public ApiControllerHeadersFilter(params (string key,Func<HttpContext, string> value)[] headers)
        {
            Headers = new Dictionary<string, Func<HttpContext, string>>(headers.Select(h => new KeyValuePair<string, Func<HttpContext, string>>(h.Item1, h.Item2)));
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            // Add headers
            Debug.WriteLine(context.Controller.GetType().ToString());
            if(context.Controller.GetType().GetCustomAttributes<ApiControllerAttribute>().Any())
                foreach (var header in Headers)
                    context.HttpContext.Response.Headers.Add(header.Key, header.Value(context.HttpContext));

            base.OnResultExecuting(context);
        }
    }
}