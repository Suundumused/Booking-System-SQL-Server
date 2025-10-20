using ZConnector.Models.Client;


namespace ZConnector.Services.Client.Interfaces
{
    public interface IUserService
    {
        Task<UserModel?> GetUserById(int id);
        Task<UserModel?> GetByUserNameAsync(string userName);
        Task<UserModel?> GetByUserEmailAsync(string email);
    }
}