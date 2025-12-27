using System.ComponentModel.DataAnnotations;


namespace ZConnector.Models.JWT
{
    public class RegisterCredentials
    {
        [MinLength(4, ErrorMessage = "The username must contain at least four characters.")]
        public required string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email.")]
        public required string Email { get; set; }

        [MinLength(6, ErrorMessage = "The password must contain at least six characters.")]
        public required string Password { get; set; }

        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Only alphabetic characters are allowed.")]
        public required string Name { get; set; }

        public int? Phone1 { get; set; }
        public int? Phone2 { get; set; }
    }
}
