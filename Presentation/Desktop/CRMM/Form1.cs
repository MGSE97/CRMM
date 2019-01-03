using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.ServiceProcess;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Serialization;
using CRMM.Service.Interface;
using PluginInterface;

namespace CRMM
{
    public partial class MainForm : Form
    {
        public DirectoryInfo PluginDirectory { get; set; }
        public DirectoryInfo ServiceDirectory { get; set; }

        public IList<IPlugin> Plugins { get; }

        public ServiceController ServiceController { get; set; }

        public System.Timers.Timer UpdateTimer { get; }

        public MainForm()
        {
            InitializeComponent();

            PluginDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Plugins"));
            ServiceDirectory = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "..", "WinService", "CRMMService", "bin", "Debug"));
            Plugins = new List<IPlugin>();
            UpdateTimer = new System.Timers.Timer(1000);
            UpdateTimer.Elapsed += (s, e) => Update();

            LoadPlugins();
            StartPlugins();
            StartService();
        }

        private void StartService()
        {
            ServiceController = new ServiceController("CRMMService");
            if (ServiceController.Status == ServiceControllerStatus.Running)
            {
                ServiceController.ExecuteCommand((int) Commands.Configure);
                ServiceController.ExecuteCommand((int) Commands.StartEmail);
                ServiceController.ExecuteCommand((int) Commands.StartFetch);
                UpdateTimer.Start();
            }
        }

        private void Update()
        {
            try
            {
                Debug.WriteLine($"Update from {ServiceDirectory.FullName}");
                var cInfo = new FileInfo(Path.Combine(ServiceDirectory.FullName, "config.xml"));
                if (cInfo.Exists)
                {
                    Configuration config = null;
                    using (var fs = new FileStream(cInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (var reader = new StreamReader(fs))
                        {
                            var serializer = new XmlSerializer(typeof(Configuration));
                            config = (Configuration)serializer.Deserialize(reader);
                            config.Service.File = Path.Combine(ServiceDirectory.FullName, config.Service.File);
                            config.Service.Graph = Path.Combine(ServiceDirectory.FullName, config.Service.Graph);
                        }
                    }

                    var info = new FileInfo(Path.Combine(ServiceDirectory.FullName, config.Service.File));
                    if (info.Exists)
                    {
                        ServiceFetchData data = null;
                        using (var fs = new FileStream(info.FullName, FileMode.Open, FileAccess.Read,
                            FileShare.ReadWrite))
                        {
                            using (var reader = new StreamReader(fs))
                            {
                                var serializer = new XmlSerializer(typeof(ServiceFetchData));
                                data = (ServiceFetchData) serializer.Deserialize(reader);
                            }
                        }

                        foreach (var plugin in Plugins)
                        {
                            plugin.Update(config, data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void StartPlugins()
        {
            foreach (var plugin in Plugins)
            {
                plugin.OnStart();
                Trace.WriteLine($"Plugin stated {plugin.GetType().Name}");
            }
        }

        private void LoadPlugins()
        {
            foreach (var dll in PluginDirectory.GetFiles("*.dll", SearchOption.AllDirectories))
            {
                try
                {
                    Debug.WriteLine($"Dll found {dll.FullName}");
                    Assembly asm = Assembly.LoadFile(dll.FullName);
                    foreach (var type in asm.GetTypes().Where(t =>
                        typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract && t.IsPublic && t.IsClass))
                    {
                        var plugin = (IPlugin) Activator.CreateInstance(type);
                        plugin.Configure(FLP);
                        Plugins.Add(plugin);
                        Trace.WriteLine($"Plugin loaded {type.Name}");
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.ToString());
                }
            }
            /*foreach (RowStyle tlpRowStyle in TLP.RowStyles)
            {
                tlpRowStyle.Height = 100f/(float)TLP.RowCount;
                tlpRowStyle.SizeType = SizeType.Percent;
            }*/
            FLP.Refresh();
        }


        protected override void OnFormClosing(FormClosingEventArgs formClosingEventArgs)
        {
            StopService();
            foreach (var plugin in Plugins)
            {
                plugin.OnEnd();
                Trace.WriteLine($"Plugin stopped {plugin.GetType().Name}");
            }

            base.OnFormClosing(formClosingEventArgs);
        }

        private void StopService()
        {
            UpdateTimer.Stop();
            ServiceController.ExecuteCommand((int)Commands.StopEmail);
            ServiceController.ExecuteCommand((int)Commands.StopFetch);
        }
    }
}
