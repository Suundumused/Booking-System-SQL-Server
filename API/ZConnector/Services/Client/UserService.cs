using AutoMapper;

using ZConnector.Models.Client;
using ZConnector.Repositories.Interfaces;
using ZConnector.Services.Client.Interfaces;


namespace ZConnector.Services.Client
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;


        public UserService(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<UserModel?> GetUserById(int id) 
        {
            return await _usersRepository.GetUserById(id);            
        }

        public async Task<UserModel?> GetByUserNameAsync(string userName) 
        {
            return _mapper.Map<UserModel?>(await _usersRepository.GetByUserNameAsync(userName));
        }

        public async Task<UserModel?> GetByUserEmailAsync(string email) 
        {
            return _mapper.Map<UserModel?>(await _usersRepository.GetByUserEmailAsync(email));
        }
    }
}
