using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using ZConnector.Models.Client;
using ZConnector.Services.Client.Interfaces;


namespace ZConnector.Controllers.Client
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ParentController
    {
        private readonly IUserService _userService;


        public ProfileController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpGet("data")]
        public async Task<IActionResult> GetUserData()
        {
            string? userName = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string? Email;

            UserModel? userDataStatus;

            try 
            {
                if (userName is not null)
                {
                    userDataStatus = await _userService.GetByUserNameAsync(userName);
                }
                else
                {
                    Email = User.FindFirstValue(ClaimTypes.Email);

                    if (Email is not null)
                    {
                        userDataStatus = await _userService.GetByUserEmailAsync(Email);
                    }
                    else
                    {
                        return Unauthorized("Username and Email are missing. Try Login again.");
                    }
                }

                if (userDataStatus is null)
                {
                    return NotFound("User data not found. Try Login again.");
                }

                return Ok(userDataStatus);
            }
            catch 
            {
                return BadRequest("An internal error occurred while retrieving user data.");
            }
        }
    }
}
