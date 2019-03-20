using System;
using System.Collections.Generic;
using DatabaseContext.Models;
using Microsoft.AspNetCore.Http;
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
            var request = _httpContextAccessor.HttpContext?.Request;
            new RequestLog(_databaseService.Context)
            {
                Url = $"{request?.Scheme}://{request?.Host}{request?.Path}{request?.QueryString}",
                UserId = _workContext.CurrentUser?.Id,
                CreatedOnUtc = DateTime.UtcNow
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
