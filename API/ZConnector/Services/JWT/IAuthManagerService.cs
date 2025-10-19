using ZConnector.Models.Entities;
using ZConnector.Models.JWT;


namespace ZConnector.Services.JWT
{
    public interface IAuthManagerService
    {
        Task Register(RegisterCredentials user);
        Task<(string, User)> TestCredentialsAndGetUser(LoginCredentials user);
    }
}
