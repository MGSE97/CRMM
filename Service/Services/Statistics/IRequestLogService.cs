using System;
using System.Collections.Generic;
using System.Text;
using DatabaseContext.Models;

namespace Services.Statistics
{
    public interface IRequestLogService
    {
        IRequestLogService Log();
        IList<RequestLog> GetLogs(DateTime from, DateTime to);
    }
}
