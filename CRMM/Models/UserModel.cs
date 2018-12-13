using DatabaseContext.Models;
using Services;

namespace CRMM.Models
{
    public class UserModel
    {
        public ulong Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool Validating { get; set; }
    }

    public static class UserExtensions
    {
        public static UserModel ToModel(this User user)
        {
            return new UserModel()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Validating = user.HasState(UserStates.Validating)
            };
        }
    }
}