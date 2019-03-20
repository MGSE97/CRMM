using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMM.Models;
using CRMM.Utils;
using DatabaseContext.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using Services.Database;
using Services.Order;
using Services.WorkContext;

namespace CRMM.Controllers
{
    public class OrderController : Controller
    {
        private readonly IWorkContext _workContext;
        private readonly IDatabaseService _databaseService;
        private readonly IOrderService _orderService;

        public OrderController(IWorkContext workContext, IDatabaseService databaseService, IOrderService orderService)
        {
            _workContext = workContext;
            _databaseService = databaseService;
            _orderService = orderService;
        }

        public IActionResult List()
        {
            return View(ListData());
        }

        private List<Order> ListData()
        {
            var orders = new List<Order>();
            if (_workContext.CurrentUser.HasRoles(UserRoles.Admin, UserRoles.Supplier))
            {
                orders.AddRange(Order.FindAll(_databaseService.Context));
            }
            else if (_workContext.CurrentUser.HasRoles(UserRoles.Worker))
            {
                orders.AddRange(Order.FindAll(_databaseService.Context).Where(o => o.HasState(OrderStates.Valid, ReclamationStates.Valid) || o.Users.Value.Any(u => u.Id.Equals(_workContext.CurrentUser.Id))));
            }
            orders.AddRange(_workContext.CurrentUser.Orders.Value);
            
            return orders.Distinct().ToList();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new OrderModel(){ Locations = _workContext.CurrentUser.Places.Value.Where(p => !p.HasState(PlaceStates.Validating)).Select(p => new SelectListItem(p.Address, p.Id.ToString())).ToArray()});
        }

        [HttpPost]
        public IActionResult Create(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var location = new Place(_databaseService.Context){Id = model.LocationId}.Find().FirstOrDefault();
                _orderService.CreateOrder(model.ToOrder(_databaseService.Context), location);
                return RedirectToAction("List");
            }

            return View(new OrderModel());
        }

        [HttpGet]
        public IActionResult Edit(ulong id)
        {
            // Get order
            var order = new Order(_databaseService.Context) {Id = id}.Find().FirstOrDefault();
            var model = order.ToModel();

            ulong dropof = 0;
            if (order?.Id > 0)
            {
                // Get dropof
                var orderStates = new OrderState(_databaseService.Context) {OrderId = order.Id}.Find().Where(o => o.State.Value.DeletedOnUtc == null);
                dropof = orderStates.FirstOrDefault(s => s.State.Value.Type.Equals(OrderStates.DropOf))?.PlaceId ?? 0;
            }

            // Set Locations
            model.Locations = _workContext.CurrentUser.Places.Value.Where(p => !p.HasState(PlaceStates.Validating)).Select(p => new SelectListItem(p.Address, p.Id.ToString(), p.Id == dropof)).ToArray();

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(OrderModel model)
        {
            if (ModelState.IsValid)
            {
                var location = new Place(_databaseService.Context) { Id = model.LocationId }.Find().FirstOrDefault();
                _orderService.UpdateOrder(model.ToOrder(_databaseService.Context), location);
                return RedirectToAction("List");
            }

            return View(new OrderModel());
        }

        public IActionResult Delete(ulong id)
        {
            try
            {
                new Order(_databaseService.Context) { Id = id }.Delete();
            }
            catch
            {
                // Ignored
            }

            return RedirectToAction("List");
        }

        public IActionResult Validate(ulong id)
        {
            if (id > 0 && _workContext.CurrentUser.HasRoles(UserRoles.Admin, UserRoles.Supplier, UserRoles.Worker))
            {
                var order = new Order(_databaseService.Context) { Id = id }.Find().FirstOrDefault();

                if (order != null)
                {
                    if (order.HasState(ReclamationStates.Validating))
                    {
                        var next = ReclamationStates.GetNextState(order) ?? ReclamationStates.Valid;
                        foreach (var state in order.States.Value.Where(s => s.Type.Equals(ReclamationStates.Validating) && s.DeletedOnUtc == null))
                        {
                            state.DeletedOnUtc = DateTime.UtcNow;
                            state.Save();
                        }

                        order.SetState(order.GetDropOfId(), next);
                    }
                    else
                    {
                        var next = OrderStates.GetNextState(order) ?? OrderStates.Valid;
                        foreach (var state in order.States.Value.Where(s => s.Type.Equals(OrderStates.Validating) && s.DeletedOnUtc == null))
                        {
                            state.DeletedOnUtc = DateTime.UtcNow;
                            state.Save();
                        }

                        order.SetState(order.GetDropOfId(), next);
                    }
                }
            }

            return RedirectToAction("Index", "Home");
        }


        [Route("[controller]/[action]/{type}")]
        public IActionResult Export(string type)
        {
            return (IActionResult)ListData().ExportData(type, "Orders") ?? RedirectToAction("Error", "Home");
        }
    }
}