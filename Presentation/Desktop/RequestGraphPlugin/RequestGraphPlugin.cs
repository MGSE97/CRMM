using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CRMM.Service.Interface;
using PluginInterface;

namespace RequestGraphPlugin
{
    public class RequestGraphPlugin : IPlugin
    {
        private PictureBox Image { get; set; }

        public RequestGraphPlugin()
        {
        }

        public void Configure(FlowLayoutPanel flp)
        {
            Image = new PictureBox();
            Image.MinimumSize = new Size(400, 200);
            Image.AutoSize = true;
            Image.SizeMode = PictureBoxSizeMode.Zoom;
            Image.Dock = DockStyle.Fill;
            flp.Controls.Add(Image);
        }

        public void OnStart()
        {
        }

        public void Update(Configuration configuration, ServiceFetchData data)
        {
            if (new FileInfo(configuration.Service.Graph).Exists)
                using (var fs = new FileStream(configuration.Service.Graph, FileMode.Open, FileAccess.ReadWrite))
                {
                    Image.Image?.Dispose();
                    Image.Image = System.Drawing.Image.FromStream(fs);
                    Trace.WriteLine($"{nameof(RequestGraphPlugin)} graph updated");
                    Image.Invoke((MethodInvoker) delegate { Image.Refresh(); });
                    GC.Collect();
                }
        }

        public void OnEnd()
        {
            Image.Dispose();
        }
    }
}
