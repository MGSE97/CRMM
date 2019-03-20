using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRMM.Models;
using Data.Mapping.Extensions;
using DatabaseContext.Models;
using Services.Database;
using Services.User;
using Services.WorkContext;

namespace CRMM.Controllers
{
    public class HomeController : Controller
    {
        private IWorkContext _workContext;
        private IDatabaseService _databaseService;
        private IUserService _userService;

        public HomeController(IWorkContext workContext, IDatabaseService databaseService, IUserService userService)
        {
            _workContext = workContext;
            _databaseService = databaseService;
            _userService = userService;
        }

        public IActionResult Index()
        {
            if (_workContext.CurrentUser == null)
                return RedirectToAction("Login");
            //_workContext.CurrentUser = new User(_databaseService.Context) { Email = "admin", Password = "admin" }.Find().FirstOrDefault();
            return View();
        }

        [Route("login")]
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginModel());
        }

        [Route("login")]
        [HttpPost]
        public IActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.Login(model.Email, model.Password);
                if (user != null)
                {
                    _workContext.CurrentUser = user;
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }


        [Route("register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterModel());
        }

        [Route("register")]
        [HttpPost]
        public IActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = _userService.Register(model.Name, model.Email, model.Password, true);
                    if (user != null)
                    {
                        _workContext.CurrentUser = user;
                        return RedirectToAction("Index");
                    }
                }
                catch (ExistingUserException)
                {
                    model.Email = "";
                    return View(model);
                }
                
            }

            return View(model);
        }

        [Route("logout")]
        public IActionResult Logout()
        {
            _workContext.CurrentUser = null;
            _workContext.ClearCache();
            return RedirectToAction("Login");
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
