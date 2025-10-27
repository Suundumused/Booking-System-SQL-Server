using System.ComponentModel.DataAnnotations;


namespace ZConnector.Models.Client.User 
{
    public partial class UserModel
    {
        public int ID { get; set; }

        public string? Username { get; set; }
        public string? Email { get; set; }

        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Only alphabetic characters are allowed.")]
        public string? Name { get; set; }

        public int? Phone1 { get; set; }
        public int? Phone2 { get; set; }

        public DateTime? LastLoginDate { get; set; }
    }
}