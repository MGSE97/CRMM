using DatabaseContext.Models;
using Newtonsoft.Json;
using Services;

namespace CRMM.Models
{
    public class UserModel
    {
        [JsonProperty("id")]
        public ulong Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("validating")]
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