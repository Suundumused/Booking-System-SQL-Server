namespace ZConnector.Models.JWT
{
    public class LoginCredentials
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }

        public DateTime LastLoginDate { get; } = DateTime.UtcNow;
    }
}
