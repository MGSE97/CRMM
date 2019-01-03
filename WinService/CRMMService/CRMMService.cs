using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceProcess;
using System.Timers;
using CRMM.Service;
using CRMM.Service.Interface;

namespace CRMMService
{
    public partial class CRMMService : ServiceBase
    {
        private Core Core { get; set; }

        private Timer FetchUpdate { get; set; }
        private Timer EmailUpdate { get; set; }

        public CRMMService()
        {
            InitializeComponent();
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
            Trace.Listeners.Add(new EventLogTraceListener("CRMMService"));
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            Core = new Core();
            FetchUpdate = new Timer(1000);
            FetchUpdate.Elapsed += async (sender, args) =>
            {
                try
                {
                    if (Core?.Configuration?.Service?.Url != null)
                    {
                        var result = await Core.FetchAsync(Core.Configuration.Service.Url,
                            Core.Configuration.Service.File, 1, Core.Configuration.User.Email,
                            Core.Configuration.User.Pass);
                        Core.CreateGraph(Core.Configuration.Service.Graph, result.Graphs.FirstOrDefault());
                    }
                    Trace.WriteLine("Fetched data from server");
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
            };
            EmailUpdate = new Timer(5*60*1000);
            EmailUpdate.Elapsed += async (sender, args) =>
            {
                var success = false;
                do
                {
                    try
                    {
                        var result = await Core.FetchAsync(Core.Configuration.Service.Url,
                            $"mailtemp_{Core.Configuration.Service.File}", 5, null, null);
                        Core.CreateGraph($"mailtemp_{Core.Configuration.Service.Graph}",
                            result.Graphs.FirstOrDefault());
                        await Core.SendEmailAsync(Core.Configuration.User.Email, 5,
                            $"mailtemp_{Core.Configuration.Service.Graph}");
                        success = true;
                        Trace.WriteLine("Send reports email");
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(ex.ToString());
                    }
                } while (!success);
            };
        }

        protected override void OnCustomCommand(int command)
        {
            Trace.WriteLine($"Executing command {(Commands)command}");
            switch ((Commands)command)
            {
                case Commands.Configure:
                    Core.Configure("config.xml");
                    break;
                case Commands.StartFetch:
                    FetchUpdate.Start();
                    break;
                case Commands.StopFetch:
                    FetchUpdate.Stop();
                    break;
                case Commands.StartEmail:
                    EmailUpdate.Start();
                    break;
                case Commands.StopEmail:
                    EmailUpdate.Stop();
                    break;
            }
        }

        protected override void OnStart(string[] args)
        {
            Trace.WriteLine($"CRMM Service started in {Directory.GetCurrentDirectory()}");
        }

        protected override void OnStop()
        {
            FetchUpdate.Dispose();
            EmailUpdate.Dispose();
        }
    }
}
