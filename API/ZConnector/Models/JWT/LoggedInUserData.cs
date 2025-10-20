using ZConnector.Models.Client;


namespace ZConnector.Models.JWT
{
    public class LoggedInUserData
    {
        public required string Token { get; set; }
        public UserModel? UserData { get; set; }
    }
}
