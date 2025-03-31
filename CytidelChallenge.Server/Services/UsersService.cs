using CytidelChallenge.Server.Model;
using Microsoft.AspNetCore.Components.Forms;

namespace CytidelChallenge.Server.Services
{
    public class UsersService : IUserService
    {
        public User ValidateUser(string username, string password)
        {
            var temp = new User { 
                Id = 1,
                Username = username,
                Roles = new List<string> { "admin" }
            };

            return temp;
        }
    }
}
