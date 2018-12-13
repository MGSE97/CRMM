using System;
using DatabaseContext.Models;
using Services.Database;
using Services.WorkContext;

namespace Services.Place
{
    public class PlaceService : IPlaceService
    {
        private IDatabaseService _databaseService;
        private IWorkContext _workContext;
        public PlaceService(IDatabaseService databaseService, IWorkContext workContext)
        {
            _databaseService = databaseService;
            _workContext = workContext;
        }

        private DatabaseContext.Models.Place SavePlace(DatabaseContext.Models.Place place)
        {
            place = place.SetContext(_databaseService.Context).Save();
            var state = new State(_databaseService.Context) { Type = PlaceStates.Validating, Description = $"Lokaci {place.Name} je nutné prověřit", CreatedOnUtc = DateTime.UtcNow }.Save();
            new PlaceState(_databaseService.Context) { PlaceId = place.Id, StateId = state.Id }.Save();
            return place;
        }

        public DatabaseContext.Models.Place CreatePlace(DatabaseContext.Models.Place place)
        {
            place = SavePlace(place);
            new UserPlace(_databaseService.Context) {UserId = _workContext.CurrentUser.Id, PlaceId = place.Id}.Save();
            return place;
        }

        public DatabaseContext.Models.Place UpdatePlace(DatabaseContext.Models.Place place)
        {
            return SavePlace(place);
        }
    }
}