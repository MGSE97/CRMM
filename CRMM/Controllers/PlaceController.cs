using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMM.Models;
using DatabaseContext.Models;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Database;
using Services.Place;
using Services.WorkContext;

namespace CRMM.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IWorkContext _workContext;
        private readonly IDatabaseService _databaseService;
        private readonly IPlaceService _placeService;

        public PlaceController(IWorkContext workContext, IDatabaseService databaseService, IPlaceService placeService)
        {
            _workContext = workContext;
            _databaseService = databaseService;
            _placeService = placeService;
        }

        public IActionResult List()
        {
            return View(_workContext.CurrentUser.Places.Value);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new PlaceModel());
        }

        [HttpPost]
        public IActionResult Create(PlaceModel model)
        {
            if (ModelState.IsValid)
            {
                _placeService.CreatePlace(model.ToPlace(_databaseService.Context));
                return RedirectToAction("List");
            }

            return View(new PlaceModel());
        }

        [HttpGet]
        public IActionResult Edit(ulong id)
        {
            return View(new Place(_databaseService.Context){Id = id}.Find().FirstOrDefault().ToModel());
        }

        [HttpPost]
        public IActionResult Edit(PlaceModel model)
        {
            if (ModelState.IsValid)
            {
                _placeService.UpdatePlace(model.ToPlace(_databaseService.Context));
                return RedirectToAction("List");
            }

            return View(new PlaceModel());
        }

        public IActionResult Delete(ulong id)
        {
            try
            {
                new Place(_databaseService.Context) {Id = id}.Delete();
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
                var place = new Place(_databaseService.Context) { Id = id }.Find().FirstOrDefault();

                if (place != null)
                    foreach (var state in place.States.Value.Where(s => s.Type.Equals(PlaceStates.Validating) && s.DeletedOnUtc == null))
                    {
                        state.DeletedOnUtc = DateTime.UtcNow;
                        state.Save();
                    }
            }

            return RedirectToAction("Index", "Home");
        }
    }
}