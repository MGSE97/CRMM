using System.Linq;
using Microsoft.AspNetCore.Http;
using Services.Database;

namespace Services.WorkContext
{
    public class WorkContext : IWorkContext
    {
        private readonly ISession _session;
        private readonly IDatabaseService _databaseService;

        private DatabaseContext.Models.User _currentUser = null;
        public DatabaseContext.Models.User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    //_currentUser = _session.GetObjectFromJson<DatabaseContext.Models.User>("CurrentUser")?.SetContext(_databaseService.Context);
                {
                    var id = _session.GetObjectFromJson<ulong>("CurrentUserId");
                    if(id > 0)
                        _currentUser = new DatabaseContext.Models.User(_databaseService.Context) { Id = id }.Find().FirstOrDefault();
                }
                    
                return _currentUser;
            }
            set
            {
                _session.SetObjectAsJson("CurrentUserId", value?.Id);
                _currentUser = value;
            }
        }

        public WorkContext(IHttpContextAccessor httpContext, IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            _session = httpContext.HttpContext.Session;
        }

        public IWorkContext ClearCache()
        {
            _currentUser = null;
            return this;
        }
    }
}