namespace CytidelChallenge.Server.Model
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required List<String> Roles { get; set; }
    }


    public class LoginModel
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
