using ZConnector.Models.Entities;
using ZConnector.Models.JWT;


namespace ZConnector.Repositories.Interfaces
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        Task<User?> GetByUsernameAsync(string userName);
        Task Register(RegisterCredentials user);
        Task<User> LoginAndGetUser(LoginCredentials credentials);
    }
}
