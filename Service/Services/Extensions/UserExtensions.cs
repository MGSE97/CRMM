using System;
using System.Linq;
using System.Reflection;
using DatabaseContext.Models;

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

        public static State GetState(this DatabaseContext.Models.User user, params string[] states)
        {
            return user.States.Value.FirstOrDefault(state => states.Any(r => r.Equals(state.Type) && state.DeletedOnUtc == null));
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

    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
    public class RoleAttribute : Attribute
    {
        public string Role { get; }

        public RoleAttribute(string role)
        {
            Role = role;
        }
    }

    public static class RoleExtensions
    {
        public static bool TypeHasRoles(this Type value, string property, params string[] role)
        {
            return value?.GetProperty(property)?.GetCustomAttributes<RoleAttribute>().Any(a => role.Any(r => r.Equals(a.Role))) ?? false;
        }
        public static string[] TypeGetRoles(this Type value, string property)
        {
            return value?.GetProperty(property)?.GetCustomAttributes<RoleAttribute>().Select(a => a.Role).ToArray();
        }

        public static bool TypeHasRoles<T>(this T value, string property, params string[] role)
        {
            return value?.GetType().TypeHasRoles(property, role)??false;
        }

        public static string[] TypeGetRoles<T>(this T value, string property)
        {
            return value?.GetType().TypeGetRoles(property);
        }
    }

    public class Wrapper<T>
    {
         public T Value { get; }

        public Wrapper()
        {
            
        }

        public Wrapper(T value)
        {
            Value = value;
        }
    }
}
