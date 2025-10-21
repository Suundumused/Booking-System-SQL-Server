using AutoMapper;

using Microsoft.EntityFrameworkCore;

using System.Security.Cryptography;

using ZConnector.Data;
using ZConnector.Models.Client;
using ZConnector.Models.Entities;
using ZConnector.Models.JWT;
using ZConnector.Repositories.Interfaces;


namespace ZConnector.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        private readonly IMapper _mapper;


        public UsersRepository(AppDbContext context, CommonJwtSettings jwtSettings, IMapper mapper) : base(context) 
        {
            _mapper = mapper;
        }

        public async Task<UserModel?> GetUserById(int id) 
        {
            return _mapper.Map<UserModel?>(await GetByIdAsync(id));
        }

        public async Task<User?> GetByUserNameAsync(string userName) 
        {
            return await _context.Users
                .Where(u => u.Username == userName)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetByUserEmailAsync(string email) 
        {
            return await _context.Users
                .Where(e => e.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetIdFromUserName(string userName)
        {
            return await _context.Users
                .Where(u => u.Username == userName)
                .Select(u => u.ID)
                .FirstOrDefaultAsync();
        }

        public async Task<int?> GetIdFromEmail(string email) 
        {
            return await _context.Users
                .Where(u => u.Email == email)
                .Select(u => u.ID)
                .FirstOrDefaultAsync();
        }

        public async Task<UserModel> LoginAndGetUser(LoginCredentials credentials) 
        {
            string? userName = credentials.UserName;
            string? email = credentials.Email;

            User? user;

            if (userName is not null)
            {
                user = await GetByUserNameAsync(userName);
            }
            else if (email is not null)
            {
                user = await GetByUserEmailAsync(email);
            }
            else 
            {
                throw new KeyNotFoundException();
            }

            if (user is null) 
            {
                throw new FileNotFoundException();
            }

            if (VerifyPassword(credentials.Password, user.PasswordHash, user.Salt))
            {
                user.LastLoginDate = credentials.LastLoginDate;

                Update(_mapper.Map<User>(user));
                await SaveChangesAsync();

                return _mapper.Map<UserModel>(user);
            }

            throw new UnauthorizedAccessException();
        }

        public async Task Register(RegisterCredentials user) 
        {
            var asUser = _mapper.Map<User>(user);
            (asUser.PasswordHash, asUser.Salt) = HashPassword(user.Password);

            await AddAsync(asUser);
            await SaveChangesAsync();
        }

        public async Task UpdateUserInfo(UserModel userModel) 
        {
            var user = new User { ID = userModel.ID };

            _context.Users.Attach(user);

            if (userModel.Username != null)
            {  
                user.Username = userModel.Username;
                _context.Entry(user).Property(u => u.Username).IsModified = true;
            }

            if (userModel.Email != null)
            {
                user.Email = userModel.Email;
                _context.Entry(user).Property(u => u.Email).IsModified = true;
            }

            if (userModel.Name != null)
            {
                user.Name = userModel.Name;
                _context.Entry(user).Property(u => u.Name).IsModified = true;
            }

            if (userModel.Phone1 != null) 
            {
                user.Phone1 = userModel.Phone1;
                _context.Entry(user).Property(u => u.Phone1).IsModified = true;
            }

            if (userModel.Phone2 != null)
            {
                user.Phone2 = userModel.Phone2;
                _context.Entry(user).Property(u => u.Phone2).IsModified = true;
            }

            await SaveChangesAsync();
        }

        private static (string Hash, string Salt) HashPassword(string password)
        {
            byte[] saltBytes = new byte[16]; // 1. Generate a random salt (16 bytes recommended)

            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(saltBytes);
            
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA256)) // 2. Generate hash using PBKDF2 (100,000 iterations)
            {   
                return (Convert.ToBase64String(pbkdf2.GetBytes(32)), Convert.ToBase64String(saltBytes)); // 256-bit hash // 3. Return Base64 for easy DB storage
            }
        }

        private static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(storedSalt), 100000, HashAlgorithmName.SHA256))
            {
                return Convert.ToBase64String(pbkdf2.GetBytes(32)) == storedHash; // Compare stored hash with computed hash
            }
        }
    }
}
