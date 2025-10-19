using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

using ZConnector.Models.Entities;
using ZConnector.Models.JWT;
using ZConnector.Services.JWT;


namespace ZConnector.Controllers.JWT
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthManagerService _authManagerService;


        public AuthController(IAuthManagerService authManagerService) 
        {
            _authManagerService = authManagerService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentials user)
        {
            (string, User) result = await _authManagerService.TestCredentialsAndGetUser(user);

            User? userData = result.Item2;

            if (userData is not null) 
            {
                userData.PasswordHash = string.Empty;
                userData.Salt = string.Empty;

                return Ok(new LoggedInUserData { Token = result.Item1, UserData = userData });
            }

            return Unauthorized(result.Item1);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCredentials user) 
        {
            await _authManagerService.Register(user);

            return Ok();
        }
    }
}
