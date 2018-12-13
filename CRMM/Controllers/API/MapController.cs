using System.Collections.Generic;
using System.Linq;
using CRMM.Models;
using CRMM.Models.API;
using DatabaseContext.Models;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Database;
using Services.WorkContext;

namespace CRMM.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IDatabaseService _databaseService;
        private readonly IWorkContext _workContext;

        public MapController(IDatabaseService databaseService, IWorkContext workContext)
        {
            _databaseService = databaseService;
            _workContext = workContext;
        }

        // GET: api/Map
        [HttpGet]
        public IEnumerable<MapPlaceModel> GetMap()
        {
            return GetPlaces();
        }

        // POST: api/Map/location/{id}
        [HttpGet("location/{id}")]
        public MapPlaceModel GetLocation(ulong id)
        {
            return GetPlace(id);
        }

        // POST: api/Map
        [HttpPost]
        public IEnumerable<MapPlaceModel> PostMap()
        {
            return GetPlaces();
        }

        // POST: api/Map/location/{id}
        [HttpPost("location")]
        public MapPlaceModel PostLocation([FromBody] ulong id)
        {
            return GetPlace(id);
        }

        private IEnumerable<MapPlaceModel> GetPlaces()
        {
            var locations = new List<MapPlaceModel>();

            if (_workContext.CurrentUser == null)
                return locations;

            if (_workContext.CurrentUser.HasRoles(UserRoles.Admin, UserRoles.Supplier))
            {
                // Show all
                locations.AddRange(Place.FindAll(_databaseService.Context).Select(p => new MapPlaceModel() { Place = p.ToModel() }));
            }
            else if (_workContext.CurrentUser.HasRoles(UserRoles.Worker))
            {
                // Show delivery, order validated waiting
                locations.AddRange(Place.FindAll(_databaseService.Context).Where(p => p.Orders.Value.Any(o => o.HasState(OrderStates.Valid) || o.Users.Value.Any(u => u.Id.Equals(_workContext.CurrentUser.Id)))).Select(p => new MapPlaceModel() { Place = p.ToModel() }));
            }
            else if (_workContext.CurrentUser.HasRoles(UserRoles.Customer))
            {
                // Show owned
                locations.AddRange(_workContext.CurrentUser.Places.Value.Select(p => new MapPlaceModel(){ Place = p.ToModel() }));
            }

            return locations;
        }

        private MapPlaceModel GetPlace(ulong id)
        {
            var model = new MapPlaceModel();
            if (_workContext.CurrentUser == null)
                return model;

            // Add place
            var place = new Place(_databaseService.Context) {Id = id}.Find().FirstOrDefault();
            model.Place = place?.ToModel();
            model.User = place.Users.Value.FirstOrDefault(u => u.HasRoles(UserRoles.Customer))?.ToModel();

            // Add workers
            if (_workContext.CurrentUser.HasRoles(UserRoles.Admin, UserRoles.Supplier))
            {
                model.Workers = place.Users.Value.Where(u => u.HasRoles(UserRoles.Worker)).Select(u => u.ToModel()).ToList();
            }

            if (_workContext.CurrentUser.HasRoles(UserRoles.Worker))
            {
                // Add only valid orders
                model.Orders = place.Orders.Value.Where(o => o.HasState(OrderStates.Valid) || o.Users.Value.Any(u => u.Id.Equals(_workContext.CurrentUser.Id))).Select(o => o.ToModel()).ToList();
            }
            else
            {
                // Add all
                model.Orders = place.Orders.Value.Select(o => o.ToModel()).ToList();
            }

            return model;
        }
    }
}
