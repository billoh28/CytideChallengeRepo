using CytidelChallenge.Server.Model;

namespace CytidelChallenge.Server.Services
{
    public interface IUserService
    {
        public User ValidateUser(string username, string password);
    }
}