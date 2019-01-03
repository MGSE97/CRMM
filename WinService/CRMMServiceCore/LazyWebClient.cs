using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CRMM.Service
{
    public class LazyWebClient : WebClient
    {
        public int Timeout { get; set; }

        public LazyWebClient(int timeout) : base()
        {
            Timeout = timeout;
        }

        protected override WebRequest GetWebRequest(Uri uri)
        {
            WebRequest w = base.GetWebRequest(uri);
            if(w != null)
                w.Timeout = Timeout * 1000;
            return w;
        }

        public async Task DownloadFileAsync(string url, string file, int range, string email, string pass)
        {
            var data = new NameValueCollection();
            data["range"] = range.ToString();
            data["email"] = email;
            data["pass"] = pass;
            //var data = $"range={range}&email={email}&pass={pass}";
            var result = UploadValues(new Uri(url), "POST", data);
            using (var fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (var writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    await writer.WriteAsync(Encoding.UTF8.GetString(result));
                }
            }

        }
    }
}