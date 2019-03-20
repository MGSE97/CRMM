using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRMM.Service;
using CRMM.Service.Interface;

namespace CRMMServiceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var core = new Core();
            //CreateConfig(core);
            FetchData(core);
        }

        private static void FetchData(Core core)
        {
            core.Configure(Path.Combine(Directory.GetCurrentDirectory(), "config.xml"));

            Task.Run(async () =>
            {
                var result = await core.FetchAsync(
                    core.Configuration.Service.Url,
                    core.Configuration.Service.File,
                    60,
                    core.Configuration.User.Email,
                    core.Configuration.User.Pass);

                core.CreateGraph(core.Configuration.Service.Graph, result.Graphs.FirstOrDefault());

                result = await core.FetchAsync(core.Configuration.Service.Url, $"mailtemp_{core.Configuration.Service.File}", 60, null, null);
                core.CreateGraph($"mailtemp_{core.Configuration.Service.Graph}", result.Graphs.FirstOrDefault());
                await core.SendEmailAsync(core.Configuration.User.Email, 60, $"mailtemp_{core.Configuration.Service.Graph}");
            }).GetAwaiter().GetResult();

        }

        private static void CreateConfig(Core core)
        {
            core.Configuration = new Configuration();
            core.Configuration.User.Email = "admin";
            core.Configuration.User.Pass = "admin";
            core.Configuration.Service.File = "data.xml";
            core.Configuration.Service.Url = "http://localhost:8080/api/service/fetch";
            core.Configuration.Service.Graph = "graph.png";
            core.CreateConfig();
        }
    }
}
