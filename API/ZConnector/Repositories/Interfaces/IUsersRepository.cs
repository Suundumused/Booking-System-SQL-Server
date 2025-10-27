using ZConnector.Models.Client.User;
using ZConnector.Models.Entities;
using ZConnector.Models.JWT;


namespace ZConnector.Repositories.Interfaces
{
    public interface IUsersRepository : IBaseRepository<User>
    {
        Task<UserModel?> GetUserById(int id);
        Task<User?> GetByUserNameAsync(string userName);
        Task<User?> GetByUserEmailAsync(string email);

        Task<int?> GetIdFromUserName(string userName);
        Task<int?> GetIdFromEmail(string email);

        Task Register(RegisterCredentials user);
        Task<UserModel> LoginAndGetUser(LoginCredentials credentials);
        Task UpdateUserInfo(UserModel userModel);
    }
}
