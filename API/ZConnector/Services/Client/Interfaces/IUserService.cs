using ZConnector.Models.Client.User;


namespace ZConnector.Services.Client.Interfaces
{
    public interface IUserService
    {
        Task<UserModel?> GetUserById(int id);
        Task<UserModel?> GetByUserNameAsync(string userName);
        Task<UserModel?> GetByUserEmailAsync(string email);

        Task<int?> GetIdFromUserName(string userName);

        Task UpdateUserInfo(UserModel user);
    }
}