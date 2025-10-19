using ZConnector.Models.Entities;

namespace ZConnector.Models.JWT
{
    public class LoggedInUserData
    {
        public required string Token { get; set; }
        public User? UserData { get; set; }
    }
}
