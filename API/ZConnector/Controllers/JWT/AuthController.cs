using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ZConnector.GlobalHanlders;
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
                return Ok(await ApiEfCoreHandler.ExecuteWithHandlingAsync(
                        async () => await _authManagerService.TestCredentialsAndGetUser(user),
                        "User"
                    )
                );
            }
            catch (EfSafeException ex)
            {
                return StatusCode(ex.statusCode, ex.Message);
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
                await ApiEfCoreHandler.ExecuteWithHandlingAsync(
                    async () => await _authManagerService.Register(user),
                    "Registration"
                );
                return Ok();
            }
            catch (EfSafeException ex)
            {
                return StatusCode(ex.statusCode, ex.Message);
            }
            catch 
            {
                return BadRequest("An error occurred while registering the user.");
            }
        }
    }
}
