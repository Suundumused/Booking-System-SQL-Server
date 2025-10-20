using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using ZConnector.Models.JWT;
using ZConnector.Repositories.Interfaces;


namespace ZConnector.Services.JWT
{
    public class AuthManagerService : IAuthManagerService
    {
        private readonly CommonJwtSettings _settings;
        private readonly IUsersRepository _usersRepository;


        public AuthManagerService(IUsersRepository usersRepository, CommonJwtSettings jwtSettings) 
        {
            _settings = jwtSettings;
            _usersRepository = usersRepository;
        }

        private string GenerateJwtToken(LoginCredentials credentials)
        {
            Claim[] claims;

            if (credentials.UserName is not null)
            {
                claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, credentials.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            }
            else 
            {
                claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, credentials.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
            }

            return new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                    issuer: _settings.Issuer,
                    audience: _settings.Audience,

                    claims: claims,

                    expires: DateTime.Now.AddMinutes(Convert.ToDouble(_settings.ExpirationDate)),

                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(
                            _settings.Secret
                        ),
                        SecurityAlgorithms.HmacSha256
                    )
                )
            );
        }

        public async Task<LoggedInUserData> TestCredentialsAndGetUser(LoginCredentials credentials)
        {
            return new LoggedInUserData { Token = GenerateJwtToken(credentials), UserData = await _usersRepository.LoginAndGetUser(credentials) };
        }

        public async Task Register(RegisterCredentials user) 
        {
            await _usersRepository.Register(user);
        }
    }
}
