using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMM.Utils;
using DatabaseContext.Models;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Database;
using Services.WorkContext;

namespace CRMM.Controllers
{
    public class CustomerController : Controller
    {
        private IWorkContext _workContext;
        private readonly IDatabaseService _databaseService;

        public CustomerController(IWorkContext workContext, IDatabaseService databaseService)
        {
            _workContext = workContext;
            _databaseService = databaseService;
        }

        public IActionResult List()
        {
            if (_workContext.CurrentUser.HasRoles(UserRoles.Admin, UserRoles.Supplier))
                return base.View(ListData(true));
            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<User> ListData(bool valid = false)
        {
            if (valid || _workContext.CurrentUser.HasRoles(UserRoles.Admin, UserRoles.Supplier))
                return new User(_databaseService.Context).Find().Where(u => u.HasRoles(UserRoles.Customer));
            return new List<User>();
        }

        public IActionResult Validate(ulong id)
        {
            if (id > 0 && _workContext.CurrentUser.HasRoles(UserRoles.Admin, UserRoles.Supplier))
            {
                var user = new User(_databaseService.Context) {Id = id}.Find().FirstOrDefault();

                if(user != null)
                foreach (var state in user.States.Value.Where(s => s.Type.Equals(UserStates.Validating) && s.DeletedOnUtc == null))
                {
                    state.DeletedOnUtc = DateTime.UtcNow;
                    state.Save();
                }
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Delete(ulong id)
        {
            if (id > 0 && _workContext.CurrentUser.HasRoles(UserRoles.Admin, UserRoles.Supplier))
            {
                new User(_databaseService.Context) { Id = id }.Find().FirstOrDefault()?.Delete();
            }

            return RedirectToAction("Index", "Home");
        }

        [Route("[controller]/[action]/{type}")]
        public IActionResult Export(string type)
        {
            return (IActionResult)ListData().ExportData(type, "Customers") ?? RedirectToAction("Error", "Home");
        }
    }
}