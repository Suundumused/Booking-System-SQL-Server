namespace ZConnector.Models.Entities;

public partial class User
{
    public int ID { get; set; }

    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;

    public string? Name { get; set; } = null!;

    public int? Phone1 { get; set; }
    public int? Phone2 { get; set; }

    public DateTime? LastLoginDate { get; set; }
}
