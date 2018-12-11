using DataCore;
using Model.Manager;
using Model.Database;

namespace DatabaseContext.Database
{
    public class DBContext : IDBContext
    {
        public IConnector Connector => ModelManager.Connector;
        public ModelManager ModelManager { get; protected set; }

        public DBContext(ModelManager modelMapper)
        {
            ModelManager = modelMapper;
        }

        public IDBContext SetModelManager(ModelManager modelManager)
        {
            ModelManager = modelManager;
            return this;
        }

    }
}