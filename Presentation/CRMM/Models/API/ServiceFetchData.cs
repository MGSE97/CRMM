using System.Collections.Generic;
using Services.Notification;

namespace CRMM.Models.API
{
    public class ServiceFetchData
    {
        public List<Graph> Graphs { get; set; }
        public List<NotificationModel> Notifications { get; set; }

        public ServiceFetchData()
        {
            Graphs = new List<Graph>();
            Notifications = new List<NotificationModel>();
        }
    }

    public class Graph
    {
        public string Name { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public List<Point> Points { get; set; }

        public Graph()
        {
            Points = new List<Point>();
        }
    }

    public class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Color { get; set; }

        public Point(int x, int y, string color)
        {
            X = x;
            Y = y;
            Color = color;
        }
    }
}