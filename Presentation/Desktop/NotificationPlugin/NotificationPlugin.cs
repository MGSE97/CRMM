using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CRMM.Service.Interface;
using PluginInterface;
using System.Threading;
using System.Globalization;
using System.Resources;

namespace NotificationPlugin
{
    public class NotificationPlugin : IPlugin
    {
        private Label Text { get; set; }

        public void Configure(FlowLayoutPanel flp)
        {
            Text = new Label();
            Text.MinimumSize = new Size(400, 0);
            Text.AutoSize = true;
            Text.Dock = DockStyle.Fill;
            Text.Text = $"{Locale.Notifications}";
            flp.Controls.Add(Text);
        }

        public void OnStart()
        {
        }

        public void OnEnd()
        {
            Text.Dispose();
        }

        public void Update(Configuration configuration, ServiceFetchData data)
        {
            Trace.WriteLine($"{nameof(NotificationPlugin)} notifications updated");

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("cs-CZ");
            var text = $"\n{Locale.Notifications}:\n\r{ string.Join("\n\r", data.Notifications.Select(n => n.Message).ToArray())}";

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            text += $"\n\n{Locale.Notifications}:\n\r{ string.Join("\n\r", data.Notifications.Select(n => n.Message).ToArray())}\n\n";

            Text.Invoke((MethodInvoker) delegate
            {
                Text.Text = text;
                Text.Refresh();
            });
        }
    }
}
