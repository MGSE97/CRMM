using DataCore;
using Model.Manager;

namespace Model.Database
{
    public interface IDBContext
    {
        IConnector Connector { get; }

        ModelManager ModelManager { get; }

        IDBContext SetModelManager(ModelManager modelManager);
    }
}