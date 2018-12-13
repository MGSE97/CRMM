using Microsoft.AspNetCore.Mvc;

namespace CRMM.Controllers.API
{
    [Route("api")]
    public class ApiController : Controller
    { 
        public IActionResult Index()
        {
            return View();
        }
    }
}