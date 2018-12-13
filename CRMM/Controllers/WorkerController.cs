using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMM.Models;
using DatabaseContext.Models;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Database;
using Services.User;
using Services.WorkContext;

namespace CRMM.Controllers
{
    public class WorkerController : Controller
    {
        private readonly IWorkContext _workContext;
        private readonly IDatabaseService _databaseService;
        private readonly IUserService _userService;

        public WorkerController(IWorkContext workContext, IDatabaseService databaseService, IUserService userService)
        {
            _workContext = workContext;
            _databaseService = databaseService;
            _userService = userService;
        }

        public IActionResult List()
        {
            if (_workContext.CurrentUser.HasRoles(UserRoles.Admin, UserRoles.Supplier))
                return View(new User(_databaseService.Context).Find().Where(u => u.HasRoles(UserRoles.Worker)));
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (_workContext.CurrentUser.HasRoles(UserRoles.Admin, UserRoles.Supplier))
                return View(new RegisterModel());
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Create(RegisterModel model)
        {
            if (ModelState.IsValid && _workContext.CurrentUser.HasRoles(UserRoles.Admin, UserRoles.Supplier))
            {
                _userService.Register(model.Name, model.Email, model.Password, false);
                return RedirectToAction("List");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(ulong id)
        {
            if (id > 0 && _workContext.CurrentUser.HasRoles(UserRoles.Admin, UserRoles.Supplier))
            {
                var user = new User(_databaseService.Context) {Id = id}.Find().FirstOrDefault();
                return View(new RegisterModel(){Email = user.Email, Id = user.Id, Name = user.Name, Password = user.Password});
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult Edit(RegisterModel model)
        {
            if (ModelState.IsValid && _workContext.CurrentUser.HasRoles(UserRoles.Admin, UserRoles.Supplier))
            {
                var user = new User(_databaseService.Context) { Id = model.Id }.Find().FirstOrDefault();
                user.Email = model.Email;
                user.Password = model.Password;
                user.Name = model.Name;
                user.Save();
                return RedirectToAction("List");
            }

            return View(model);
        }

        public IActionResult Delete(ulong id)
        {
            if (id > 0 && _workContext.CurrentUser.HasRoles(UserRoles.Admin, UserRoles.Supplier))
            {
                try
                {
                    new User(_databaseService.Context) {Id = id}.Find().FirstOrDefault()?.Delete();
                }
                catch
                {
                    // Ignore
                }
            }

            return RedirectToAction("List");
        }
    }
}