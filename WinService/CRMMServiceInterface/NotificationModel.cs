namespace CRMM.Service.Interface
{
    public class NotificationModel
    {
        public ulong Data { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Icon { get; set; }
        public string Level { get; set; }

        public NotificationModel()
        {
            
        }

        public NotificationModel(ulong data, string type, string message, string icon, string level)
        {
            Data = data;
            Type = type;
            Message = message;
            Icon = icon;
            Level = level;
        }
    }
}