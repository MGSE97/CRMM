using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Services.Statistics
{
    public class RequestLogMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IRequestLogService requestLogService)
        {
            requestLogService.Log();
            await _next(context);
        }
    }
}