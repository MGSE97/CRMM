namespace Services.User
{
    public interface IUserService
    {
        DatabaseContext.Models.User Login(string email, string password);

        DatabaseContext.Models.User Register(string name, string email, string password, bool? customer = null);
    }
}