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
    public class UserController : Controller
    {
        private IWorkContext _workContext;
        private readonly IDatabaseService _databaseService;

        public UserController(IWorkContext workContext, IDatabaseService databaseService)
        {
            _workContext = workContext;
            _databaseService = databaseService;
        }

        public IActionResult List()
        {
            return View(new User(_databaseService.Context).Find());
        }
    }
}