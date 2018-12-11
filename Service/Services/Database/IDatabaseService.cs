using Model.Database;

namespace Services.Database
{
    public interface IDatabaseService
    {
        IDBContext Context { get; }
    }
}