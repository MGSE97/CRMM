using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseContext.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Database;
using Services.WorkContext;

namespace CRMM.Controllers
{
    public class WorkerController : Controller
    {
        private IWorkContext _workContext;
        private readonly IDatabaseService _databaseService;

        public WorkerController(IWorkContext workContext, IDatabaseService databaseService)
        {
            _workContext = workContext;
            _databaseService = databaseService;
        }

        public IActionResult List()
        {
            var workerRole = new Role(_databaseService.Context){Name = "Worker"}.Find().FirstOrDefault();
            return View(new User(_databaseService.Context).Find().Where(u => u.Roles.Value.Any(r => r.Id == workerRole.Id)));
        }
    }
}