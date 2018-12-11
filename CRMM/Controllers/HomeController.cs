using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRMM.Models;
using DatabaseContext.Models;
using Services.Database;
using Services.WorkContext;

namespace CRMM.Controllers
{
    public class HomeController : Controller
    {
        private IWorkContext _workContext;
        private IDatabaseService _databaseService;

        public HomeController(IWorkContext workContext, IDatabaseService databaseService)
        {
            _workContext = workContext;
            _databaseService = databaseService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            _workContext.CurrentUser = new User(_databaseService.Context){Email = "admin",Password = "admin"}.Find().FirstOrDefault();
            return RedirectToAction("Index");
            //return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
