using ZConnector.Models.JWT;


namespace ZConnector.Services.JWT
{
    public interface IAuthManagerService
    {
        Task Register(RegisterCredentials user);
        Task<LoggedInUserData> TestCredentialsAndGetUser(LoginCredentials user);
    }
}
