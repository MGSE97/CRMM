using DatabaseContext.Models;

namespace Services.Place
{
    public interface IPlaceService
    {
        DatabaseContext.Models.Place CreatePlace(DatabaseContext.Models.Place place);
        DatabaseContext.Models.Place UpdatePlace(DatabaseContext.Models.Place place);
    }
}