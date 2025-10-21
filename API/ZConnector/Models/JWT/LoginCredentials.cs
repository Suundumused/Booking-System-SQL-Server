using System.ComponentModel.DataAnnotations;


namespace ZConnector.Models.JWT
{
    public class LoginCredentials
    {
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email.")]
        public string? Email { get; set; }
        public required string Password { get; set; }

        public DateTime LastLoginDate { get; } = DateTime.Now;
    }
}
