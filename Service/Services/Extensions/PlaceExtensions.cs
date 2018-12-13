using System.Linq;

namespace Services
{
    public static class PlaceExtensions
    {
        public static bool HasState(this DatabaseContext.Models.Place place, params string[] states)
        {
            return place.States.Value.Any(state => states.Any(r => r.Equals(state.Type) && state.DeletedOnUtc == null));
        }
    }

    public static class PlaceStates
    {
        public static string Validating => "PlaceValidating";
    }
}
