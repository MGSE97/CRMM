using Model.Manager;

namespace Model.Database
{
    public interface IDBContext
    {
        ModelManager ModelManager { get; }

        IDBContext SetModelManager(ModelManager modelManager);
    }
}