using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseContext.Models;
using Services.Database;

namespace Services.User
{
    public class UserService : IUserService
    {
        private IDatabaseService _databaseService;

        public UserService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public DatabaseContext.Models.User Login(string email, string password)
        {
            return new DatabaseContext.Models.User(_databaseService.Context) {Email = email, Password = password}.Find().FirstOrDefault();
        }

        public DatabaseContext.Models.User Register(string name, string email, string password, bool? customer = null)
        {
            // Check if exists
            if (new DatabaseContext.Models.User(_databaseService.Context) {Email = email}.Find().Any())
                throw new ExistingUserException(email);

            // Register user
            var user = new DatabaseContext.Models.User(_databaseService.Context)
                {Name = name, Email = email, Password = password}.Save();

            // Add limits
            if (customer == true)
            {
                // Map role
                new UserRole(_databaseService.Context)
                {
                    UserId = user.Id,
                    RoleId = new Role(_databaseService.Context) {Name = UserRoles.Customer}.Find().FirstOrDefault().Id
                }.Save();

                // Require account validation
                var confirm = new State(_databaseService.Context)
                {
                    Type = UserStates.Validating,
                    Description = $"Zákazník {user.Name} čeká na ověření"
                }.Save();

                new UserState(_databaseService.Context)
                {
                    StateId = confirm.Id,
                    UserId = user.Id
                }.Save();
            }
            else if(customer == false)
            {
                // Map role
                new UserRole(_databaseService.Context)
                {
                    UserId = user.Id,
                    RoleId = new Role(_databaseService.Context) { Name = UserRoles.Worker }.Find().FirstOrDefault().Id
                }.Save();
            }

            return user;
        }
    }
}
