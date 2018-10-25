using System;
using Services.Database.Connection;

namespace Services.Database.Test
{
    public class TestService : ITestService
    {
        private IConnectionService _connectionService;
        public TestService(IConnectionService connectionService)
        {
            _connectionService = connectionService;
        }

        public DateTime GetTime()
        {
            //ToDo map to string and return DateTime
           _connectionService.Execute("SELECT GetDate();");
        }
    }
}