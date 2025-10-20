namespace ZConnector.Models.Client;

public partial class UserModel
{
    public int ID { get; set; }

    public string Username { get; set; } = null!;
    public string Email { get; set; } = null!;

    public string? Name { get; set; } = null!;

    public int? Phone1 { get; set; }
    public int? Phone2 { get; set; }

    public DateTime? LastLoginDate { get; set; }
}