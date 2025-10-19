using Microsoft.IdentityModel.Tokens;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using ZConnector.Models.Entities;
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

        private string GenerateJwtToken(string username)
        {
            return new JwtSecurityTokenHandler().WriteToken(
                new JwtSecurityToken(
                    issuer: _settings.Issuer,
                    audience: _settings.Audience,

                    claims: new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, username),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                    },

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

        public async Task<(string, User)> TestCredentialsAndGetUser(LoginCredentials credentials)
        {
            try 
            {
                return (GenerateJwtToken(credentials.UserName), await _usersRepository.LoginAndGetUser(credentials));
            }
            catch (KeyNotFoundException) 
            {
                return ("Wrong credentials.", default!);
            }
            catch (NullReferenceException)
            {
                return ("No Username provided.", default!);
            }
            catch (Exception ex) 
            {
                return (ex.Message, default!);
            }
        }

        public async Task Register(RegisterCredentials user) 
        {
            await _usersRepository.Register(user);
        }
    }
}
