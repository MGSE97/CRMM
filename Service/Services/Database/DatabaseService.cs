using Model.Database;

namespace Services.Database
{
    public class DatabaseService : IDatabaseService
    {
        public IDBContext Context { get; protected set; }

        public DatabaseService(IDBContext context)
        {
            Context = context;
        }
    }
}
