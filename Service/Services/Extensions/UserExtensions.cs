using System.Linq;

namespace Services
{
    public static class UserExtensions
    {
        public static bool HasRoles(this DatabaseContext.Models.User user, params string[] roles)
        {
            return user.Roles.Value.Any(role => roles.Any(r => r.Equals(role.Name)));
        }

        public static bool HasState(this DatabaseContext.Models.User user, params string[] states)
        {
            return user.States.Value.Any(state => states.Any(r => r.Equals(state.Type) && state.DeletedOnUtc == null));
        }
    }

    public static class UserRoles
    {
        public static string Admin => "Admin";
        public static string Customer => "Customer";
        public static string Worker => "Worker";
        public static string Supplier => "Supplier";
    }

    public static class UserStates
    {
        public static string Validating => "UserValidating";
    }
}
