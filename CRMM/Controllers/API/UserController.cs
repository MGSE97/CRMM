using CRMM.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Database;
using Services.User;
using Services.WorkContext;

namespace CRMM.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IWorkContext _workContext;
        private readonly IUserService _userService;

        public UserController(IUserService userService, IWorkContext workContext, IDatabaseService databaseService)
        {
            _userService = userService;
            _workContext = workContext;
            _databaseService = databaseService;
        }

        // POST: api/User
        [HttpPost("login")]
        public UserModel Login([FromForm] LoginModel model)
        {
            var user = _userService.Login(model.Email, model.Password);
            if (user != null)
            {
                _workContext.CurrentUser = user;
            }

            return _workContext.CurrentUser?.ToModel();
        }

        [HttpPost("logout")]
        public void Logout()
        {
            _workContext.CurrentUser = null;
            _workContext.ClearCache();
        }

        [HttpGet]
        public UserModel Get()
        {
            return _workContext.CurrentUser?.ToModel();
        }

        [HttpPost]
        public UserModel Post()
        {
            return _workContext.CurrentUser?.ToModel();
        }
    }
}
