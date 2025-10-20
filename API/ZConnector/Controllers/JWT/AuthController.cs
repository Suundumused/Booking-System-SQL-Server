using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ZConnector.Models.JWT;
using ZConnector.Services.JWT;


namespace ZConnector.Controllers.JWT
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ParentController
    {
        private readonly IAuthManagerService _authManagerService;


        public AuthController(IAuthManagerService authManagerService) 
        {
            _authManagerService = authManagerService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentials user)
        {
            try 
            {
                return Ok(await _authManagerService.TestCredentialsAndGetUser(user));
            }
            catch (KeyNotFoundException) 
            {
                return BadRequest("Missing Credentials.");
            }
            catch (FileNotFoundException) 
            {
                return NotFound("User not found");
            }
            catch (UnauthorizedAccessException) 
            {
                return Unauthorized("Wrong Credentials.");
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while logging in the user.");
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCredentials user) 
        {
            try 
            {
                await _authManagerService.Register(user);
                return Ok();
            }
            catch 
            {
                return BadRequest("An error occurred while registering the user.");
            }
        }
    }
}
