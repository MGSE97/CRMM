using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DatabaseContext.Models;
using Microsoft.AspNetCore.Mvc;
using ModelCore;
using Services;
using Services.Database;
using Services.User;

namespace CRMM.Controllers
{
    public class CmdController : Controller
    {
        private IDatabaseService _databaseService;
        private IUserService _userService;

        public CmdController(IDatabaseService databaseService, IUserService userService)
        {
            _databaseService = databaseService;
            _userService = userService;
        }

        [Route("cmd/user")]
        public IActionResult UsersList()
        {
            return Json(DatabaseContext.Models.User.FindAll(_databaseService.Context));
        }

        [Route("cmd/role")]
        public IActionResult RolesList()
        {
            return Json(Role.FindAll(_databaseService.Context));
        }

        [Route("cmd/user/roles")]
        public IActionResult UsersRolesList()
        {
            return Json(UserRole.FindAll(_databaseService.Context));
        }

        [Route("cmd/place")]
        public IActionResult PlacesList()
        {
            return Json(Place.FindAll(_databaseService.Context));
        }

        [Route("cmd/order")]
        public IActionResult OrdersList()
        {
            return Json(Order.FindAll(_databaseService.Context));
        }

        [Route("cmd/state")]
        public IActionResult StatesList()
        {
            return Json(State.FindAll(_databaseService.Context));
        }

        [Route("cmd/user/{id}")]
        public IActionResult UsersList(ulong id)
        {
            dynamic model = new ExpandoObject();
            var user = new DatabaseContext.Models.User(_databaseService.Context) {Id = id}.Find().FirstOrDefault();
            model.Id = user.Id;
            model.Name = user.Name;
            model.Email = user.Email;
            model.Password = user.Password;
            model.Roles = user.Roles.Value;
            model.Places = user.Places.Value;
            model.Orders = user.Orders.Value;
            model.States = user.States.Value;
            return Json(model);
        }

        [Route("cmd/user/login/{email}/{password}")]
        public IActionResult Login(string email, string password)
        {
            dynamic model = new ExpandoObject();
            var user = _userService.Login(email, password);
            model.Id = user.Id;
            model.Name = user.Name;
            model.Email = user.Email;
            model.Password = user.Password;
            model.Roles = user.Roles.Value;
            model.Places = user.Places.Value;
            model.Orders = user.Orders.Value;
            model.States = user.States.Value;
            return Json(model);
        }

        [Route("cmd/seedup/{code}")]
        public IActionResult SeedUp(string code)
        {
            if (code == "007")
            {
                var data = new List<BaseModel>
                {
                    new Role {Name = "Admin"},
                    new Role {Name = "Supplier"},
                    new Role {Name = "Worker"},
                    new Role {Name = "Customer"},
                    new DatabaseContext.Models.User {Name = "Admin", Email = "admin", Password = "admin"},
                };

                var exceptions = new List<Exception>();

                data.ForEach(item =>
                {
                    item.SetContext(_databaseService.Context);
                    item.Try(i => i.Save(i, true), (i, exception) => exceptions.Add(exception));
                });

                if (!exceptions.Any())
                {

                    var mapping = new List<BaseModel>
                    {
                        new UserRole {UserId = (data[4] as DatabaseContext.Models.User).Id, RoleId = (data[0] as Role).Id}
                    };

                    mapping.ForEach(item =>
                    {
                        item.SetContext(_databaseService.Context);
                        item.Try(i => i.Save(i, true), (i, exception) => exceptions.Add(exception));
                    });
                }

                return Json(exceptions);
            }
            return Json(false);
        }

        [Route("cmd/seeddown/{code}")]
        [Route("cmd/dropdb/{code}")]
        [Route("cmd/cleardb/{code}")]
        public IActionResult DropDB(string code)
        {
            if (code == "007")
            {
                var exceptions = new List<Exception>();
                UserState.FindAll(_databaseService.Context).ForEach(m => m.Try(i => i.Delete(), (i, exception) => exceptions.Add(exception)));
                UserRole.FindAll(_databaseService.Context).ForEach(m => m.Try(i => i.Delete(), (i, exception) => exceptions.Add(exception)));
                UserPlace.FindAll(_databaseService.Context).ForEach(m => m.Try(i => i.Delete(), (i, exception) => exceptions.Add(exception)));
                UserOrder.FindAll(_databaseService.Context).ForEach(m => m.Try(i => i.Delete(), (i, exception) => exceptions.Add(exception)));
                PlaceState.FindAll(_databaseService.Context).ForEach(m => m.Try(i => i.Delete(), (i, exception) => exceptions.Add(exception)));
                OrderState.FindAll(_databaseService.Context).ForEach(m => m.Try(i => i.Delete(), (i, exception) => exceptions.Add(exception)));
                DatabaseContext.Models.User.FindAll(_databaseService.Context).ForEach(m => m.Try(i => i.Delete(), (i, exception) => exceptions.Add(exception)));
                Order.FindAll(_databaseService.Context).ForEach(m => m.Try(i => i.Delete(), (i, exception) => exceptions.Add(exception)));
                Place.FindAll(_databaseService.Context).ForEach(m => m.Try(i => i.Delete(), (i, exception) => exceptions.Add(exception)));
                State.FindAll(_databaseService.Context).ForEach(m => m.Try(i => i.Delete(), (i, exception) => exceptions.Add(exception)));
                Role.FindAll(_databaseService.Context).ForEach(m => m.Try(i => i.Delete(), (i, exception) => exceptions.Add(exception)));
                return Json(exceptions);
            }
            return Json(false);
        }
    }
}