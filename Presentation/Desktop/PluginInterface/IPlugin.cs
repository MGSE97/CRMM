using System;
using System.Windows.Forms;
using CRMM.Service.Interface;

namespace PluginInterface
{
    public interface IPlugin
    {
        void Configure(FlowLayoutPanel flp);
        void OnStart();
        void Update(Configuration configuration, ServiceFetchData data);
        void OnEnd();
    }
}
