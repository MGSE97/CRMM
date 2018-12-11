namespace Services.WorkContext
{
    public interface IWorkContext
    {
        DatabaseContext.Models.User CurrentUser { get; set; }
    }
}