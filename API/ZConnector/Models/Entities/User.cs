using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ZConnector.Models.Entities 
{
    public partial class User
    {
        [Key]
        public int ID { get; set; }

        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string Salt { get; set; } = null!;

        public string? Name { get; set; } = null!;

        public int? Phone1 { get; set; }
        public int? Phone2 { get; set; }

        public DateTime? LastLoginDate { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public bool IsAdmin { get; set; }
    }
}