using System;
using System.Collections.Generic;
using DatabaseContext.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Services.Database;
using Services.WorkContext;

namespace Services.Statistics
{
    public class RequestLogService : IRequestLogService
    {
        private readonly IDatabaseService _databaseService;
        private readonly IWorkContext _workContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RequestLogService(IDatabaseService databaseService, IWorkContext workContext, IHttpContextAccessor httpContextAccessor)
        {
            _databaseService = databaseService;
            _workContext = workContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public IRequestLogService Log()
        {
            var context = _httpContextAccessor.HttpContext;
            var request = context?.Request;

            string userAgent = null;
            if (request?.Headers.ContainsKey("User-Agent") ?? false)
                userAgent = request.Headers["User-Agent"].ToString();

            new RequestLog(_databaseService.Context)
            {
                Url = $"{request?.Scheme}://{request?.Host}{request?.Path}{request?.QueryString}",
                UserId = _workContext.CurrentUser?.Id,
                CreatedOnUtc = DateTime.UtcNow,
                IpAddress = context?.Connection.RemoteIpAddress.ToString(),
                UserAgent = userAgent,
                Request = request == null ? null : JsonConvert.SerializeObject(new
                {
                    Https = request.IsHttps,
                    Protocol = request.Protocol,
                    Host = request.Host,
                    ClientPort = context.Connection.RemotePort,
                    Content = new {
                        Length = request.ContentLength,
                        Type = request.ContentType,
                        Headers = request.Headers,
                        Body = request.Body.ToString(),
                        Cookies = request.Cookies
                    }
                })
            }.Save();

            return this;
        }

        public IList<RequestLog> GetLogs(DateTime @from, DateTime to)
        {
            return new RequestLog(_databaseService.Context)
            {
                CreatedOnUtc = @from
            }.FindRange(new RequestLog(_databaseService.Context)
            {
                CreatedOnUtc = to
            });
        }
    }
}
