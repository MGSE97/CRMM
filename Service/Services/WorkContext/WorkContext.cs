namespace Services.WorkContext
{
    public class WorkContext : IWorkContext
    {
        public DatabaseContext.Models.User CurrentUser { get; set; }

        public WorkContext()
        {
            
        }
    }
}