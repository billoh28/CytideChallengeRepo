namespace CytidelChallenge.Server.Services
{
    public interface ITokenService
    {
        string GenerateToken(string userId, string username, IEnumerable<string> roles);
    }
}
