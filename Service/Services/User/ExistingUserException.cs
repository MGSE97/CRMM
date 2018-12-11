using System;

namespace Services.User
{
    public class ExistingUserException : Exception
    {
        public string User { get; }

        public ExistingUserException(string user) : base($"User {user} already exists!")
        {
            User = user;
        }
    }
}