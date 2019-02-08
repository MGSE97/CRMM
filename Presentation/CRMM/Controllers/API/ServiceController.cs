using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using CRMM.Models.API;
using CRMM.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Notification;
using Services.Statistics;
using Services.User;
using Point = CRMM.Models.API.Point;

namespace CRMM.Controllers.API
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IRequestLogService _requestLogService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public ServiceController(IRequestLogService requestLogService, IUserService userService, INotificationService notificationService)
        {
            _requestLogService = requestLogService;
            _userService = userService;
            _notificationService = notificationService;
        }

        // 1 min
        public IActionResult Fetch([FromForm] string email, [FromForm] string pass, [FromForm] int range)
        {
            if (range <= 0)
                range = 1;
            var model = new ServiceFetchData();
            var now = DateTime.UtcNow;
            var start = now.AddMinutes(-range);

            // Get requests
            var logs = _requestLogService.GetLogs(start, DateTime.UtcNow);

            // Get notifications
            bool logged = false;
            if (!string.IsNullOrWhiteSpace(email))
            {
                var user = _userService.Login(email, pass);
                if (user != null)
                {
                    model.Notifications.AddRange(_notificationService.GetUserNotifications(user));
                    logged = true;
                }
            }

            // Build graph model for xml
            var graph = new Graph();
            graph.Name = "Provoz";
            graph.X = "Čas [s]";
            graph.Y = "Požadavků/Uživatelů";
            for (int i = 0; i < 60; i++)
            {
                var logsInRange = logs.Where(l => Between(l.CreatedOnUtc, start, range)).ToList();

                // Requests
                var point = new Point(i, logsInRange.Count, Color.DeepSkyBlue.Name);
                /*if (i == 59 && point.Y > 0)
                    point.Y--;*/
                graph.Points.Add(point);

                // Users
                point = new Point(i, logsInRange.Count(l => l.UserId > 0), Color.LimeGreen.Name);
                /*if (i == 59 && point.Y > 0 && logged)
                    point.Y--;*/
                graph.Points.Add(point);

                start = start.AddSeconds(1);
            }
            model.Graphs.Add(graph);


            return model.ExportData("xml", "ServiceData");
        }

        private bool Between(DateTime value, DateTime start, int interval)
        {
            return Math.Abs((value-start).TotalSeconds) <= interval;
        }
    }
}