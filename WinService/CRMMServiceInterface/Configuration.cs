using System.Xml.Serialization;

namespace CRMM.Service.Interface
{
    [XmlRoot]
    public class Configuration
    {
        [XmlElement(typeof(User))]
        public User User { get; set; }

        [XmlElement(typeof(Service))]
        public Service Service { get; set; }

        public Configuration()
        {
            User = new User();
            Service = new Service();
        }
    }

    public class User
    {
        public string Email { get; set; }
        public string Pass { get; set; }
    }

    public class Service
    {
        public string Url { get; set; }
        public string File { get; set; }

        public string Graph { get; set; }
    }
}