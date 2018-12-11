using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services.WorkContext;

namespace CRMM.Controllers
{
    public class OrderController : Controller
    {
        private IWorkContext _workContext;

        public OrderController(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        public IActionResult List()
        {
            return View(_workContext.CurrentUser.Orders.Value);
        }
    }
}