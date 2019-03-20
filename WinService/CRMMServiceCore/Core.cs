using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Data.Linq;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using CRMM.Service.Interface;

namespace CRMM.Service
{
    public class Core
    {
        public Configuration Configuration { get; set; }

        public SmtpClient SmtpClient { get; set; }

        public string From { get; set; }

        private Object _lockG = new Object();

        public Core()
        {
            SmtpClient = new SmtpClient("smtp.gmail.com", 587);
            From = "**Email**@gmail.com";
            SmtpClient.Credentials = new NetworkCredential(From, "**Email Password**");
            SmtpClient.EnableSsl = true;
        }
        
        public void Configure(string file)
        {
            var info = new FileInfo(file);
            if (!info.Exists)
            {
                Trace.WriteLine($"Configuration file {info.FullName} not found!");
                return;
            }

            using (var stream = new FileStream(info.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(stream))
                {
                    var serializer = new XmlSerializer(typeof(Configuration));
                    Configuration = serializer.Deserialize(reader) as Configuration;
                }
            }

            Trace.WriteLine($"Configured {Configuration.Service.Url} [{Configuration.User.Email}] => {Configuration.Service.File} => {Configuration.Service.Graph} using {info.FullName}");
        }

        public async Task<ServiceFetchData> FetchAsync(string url, string file, int range, string user, string pass)
        {
            using (var client = new LazyWebClient(60))
            {
                client.Encoding = Encoding.UTF8;
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                await client.DownloadFileAsync(url, file, range, user, pass);
            }

            ServiceFetchData model = null;

            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (TextReader reader = new StreamReader(fs, Encoding.UTF8))
                {
                    var serializer = new XmlSerializer(typeof(ServiceFetchData));
                    model = serializer.Deserialize(reader) as ServiceFetchData;
                }
            }

            return model;
        }

        public void CreateGraph(string file, Graph model)
        {
            var strength = 3;
            using (var graph = new Bitmap(400, 200))
            {
                var graphics = Graphics.FromImage(graph);
                graphics.SmoothingMode = SmoothingMode.HighQuality;

                graphics.FillRectangle(Brushes.Transparent, new Rectangle(0, 0, graph.Width, graph.Height));


                // Name
                var font = new Font(FontFamily.GenericSerif, 20);
                var size = graphics.MeasureString(model.Name, font);
                graphics.DrawString(model.Name, font, Brushes.Orange, new PointF((graph.Width - size.Width) / 2, 10));


                font = new Font(FontFamily.GenericSerif, 10);
                // Y
                size = graphics.MeasureString(model.X, font);
                graphics.DrawString(model.Y, font, Brushes.Orange, new PointF(5, 5));
                PointF y = new PointF(30, 30+size.Height);
                graphics.DrawLine(Pens.White, y, new PointF(y.X, graph.Height));

                // X
                size = graphics.MeasureString(model.Y, font);
                graphics.DrawString(model.X, font, Brushes.Orange, new PointF(graph.Width - size.Width/2 - 5, graph.Height - size.Height - 5));
                PointF x = new PointF(0, graph.Height - size.Height - 30);
                graphics.DrawLine(Pens.White, x, new PointF(graph.Width, x.Y));

                // Graph area
                var area = new RectangleF(y.X, y.Y+20, graph.Width-20 - y.X, x.Y - y.Y - 20);
                //graphics.DrawRectangle(Pens.Red, area.X, area.Y, area.Width, area.Height);

                // Values
                var max = new SizeF(Math.Max(1, model.Points.Max(p => p.X)), Math.Max(1, model.Points.Max(p => p.Y)));
                var min = new SizeF(Math.Min(1, model.Points.Min(p => p.X)), Math.Min(1, model.Points.Min(p => p.Y)));
                // min X
                size = graphics.MeasureString(min.Width.ToString(), font);
                graphics.DrawString(min.Width.ToString(), font, Brushes.Orange, area.X + 5, area.Y+area.Height + 5);
                // min Y
                size = graphics.MeasureString(min.Height.ToString(), font);
                graphics.DrawString(min.Height.ToString(), font, Brushes.Orange, area.X - size.Width - 5, area.Y + area.Height - size.Height);
                // max X
                size = graphics.MeasureString(max.Width.ToString(), font);
                graphics.DrawString(max.Width.ToString(), font, Brushes.Orange, area.X + area.Width - size.Width/2, area.Y + area.Height + 5);
                // max Y
                size = graphics.MeasureString(max.Height.ToString(), font);
                graphics.DrawString(max.Height.ToString(), font, Brushes.Orange, area.X - size.Width - 5, area.Y - size.Height/2);

                // Points
                /*var lastPoints = new Dictionary<string, PointF>();
                foreach(var point in model.Points)
                {
                    var np = GetP(ref area, ref max, ref strength, point.X, point.Y);
                    lastPoints.TryGetValue(point.Color, out PointF l);
                    if (l == default(PointF))
                        l = GetP(ref area, ref max, ref strength, model.Points.FirstOrDefault(p => p.Color.Equals(point.Color))?.X??0, model.Points.FirstOrDefault(p => p.Color.Equals(point.Color))?.Y??0);
                    //graphics.Draw
                    graphics.DrawLine(new Pen(Color.FromName(point.Color), strength), l, np);
                    lastPoints[point.Color] = np;
                    //graphics.FillPie(new SolidBrush(Color.FromName(point.Color)), p.X, p.y , 10, 10, 0, 360);
                }*/

                var colors = model.Points.Select(p => p.Color).Distinct();
                foreach (var color in colors)
                {
                    graphics.DrawCurve(new Pen(Color.FromName(color), strength), model.Points.Where(p => p.Color.Equals(color)).Select(p => GetP(ref area, ref max, ref strength, p.X, p.Y)).ToArray(), 0);
                }

                graphics.Save();
                lock (_lockG)
                using (var save = new Bitmap(graph))
                {
                    save.Save(file, ImageFormat.Png);
                }
            }
        }

        private static PointF GetP(ref RectangleF area, ref SizeF max, ref int strength, int x, int y)
        {
            return new PointF((x / max.Width) * area.Width + area.X, area.Height - (y / max.Height) * area.Height - strength/2f + area.Y);
        }

        public void CreateConfig()
        {
            using (var writer = new StreamWriter("config.xml"))
            {
                var serializer = new XmlSerializer(typeof(Configuration));
                serializer.Serialize(writer, Configuration);
            }
        }

        public async Task SendEmailAsync(string email, int minutes, params string[] attachments)
        {
            var now = DateTime.Now;
            var start = now.AddMinutes(-minutes);
            using (var mail = new MailMessage())
            {
                mail.From = new MailAddress(From);
                mail.To.Add(new MailAddress(email));
                mail.Subject = $"CRMM Report {start} - {now}";
                mail.Body = $"Users & Request activity on system CRMM from {start} to {now}.";

                foreach (var attachment in attachments)
                {
                    mail.Attachments.Add(new Attachment(attachment));
                }

                await SmtpClient.SendMailAsync(mail);
                mail.Attachments.Dispose();
            }
        }
    }
}
