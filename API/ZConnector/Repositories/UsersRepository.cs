using Microsoft.EntityFrameworkCore;

using System.Security.Cryptography;

using ZConnector.Data;
using ZConnector.Models.Entities;
using ZConnector.Models.JWT;
using ZConnector.Repositories.Interfaces;


namespace ZConnector.Repositories
{
    public class UsersRepository : BaseRepository<User>, IUsersRepository
    {
        private readonly CommonJwtSettings _jwtSettings;


        public UsersRepository(AppDbContext context, CommonJwtSettings jwtSettings) : base(context) 
        {
            _jwtSettings = jwtSettings;
        }

        public async Task<User?> GetByUsernameAsync(string userName) 
        {
            return await _context.Users
                .Where(u => u.Username == userName)
                .FirstOrDefaultAsync();
        }

        public async Task<User> LoginAndGetUser(LoginCredentials credentials) 
        {
            string? userName = credentials.UserName;

            if (userName is not null)
            {
                User? user = await GetByUsernameAsync(userName);

                if (user is not null) 
                {
                    if (VerifyPassword(credentials.Password, user.PasswordHash, user.Salt)) 
                    {
                        user.LastLoginDate = credentials.LastLoginDate;

                        Update(user);
                        await SaveChangesAsync();

                        return user;
                    }
                }

                throw new KeyNotFoundException();
            }

            throw new NullReferenceException();
        }

        public async Task Register(RegisterCredentials user) 
        {
            var (passwordHash, salt) = HashPassword(user.Password);

            await AddAsync(new User
            {
                Username = user.UserName,
                Email = user.Email,
                PasswordHash = passwordHash,
                Salt = salt,

                Name = user.Name,
                Phone1 = user.Phone1,
                Phone2 = user.Phone2
            });

            await SaveChangesAsync();
        }

        private static (string Hash, string Salt) HashPassword(string password)
        {
            byte[] saltBytes = new byte[16]; // 1. Generate a random salt (16 bytes recommended)

            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(saltBytes);
            
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA256)) // 2. Generate hash using PBKDF2 (100,000 iterations)
            {
                byte[] hashBytes = pbkdf2.GetBytes(32); // 256-bit hash

                // 3. Return Base64 for easy DB storage
                return (Convert.ToBase64String(hashBytes), Convert.ToBase64String(saltBytes));
            }
        }

        private static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, 100000, HashAlgorithmName.SHA256))
            {
                byte[] computedHash = pbkdf2.GetBytes(32);
                string computedHashBase64 = Convert.ToBase64String(computedHash);

                // Compare stored hash with computed hash
                return computedHashBase64 == storedHash;
            }
        }
    }
}
