namespace ZConnector.Models.JWT
{
    public class RegisterCredentials
    {
        public required string UserName { get; set; }

        public required string Email { get; set; }
        public required string Password { get; set; }

        public string? Name { get; set; }
        public int? Phone1 { get; set; }
        public int? Phone2 { get; set; } 
    }
}
