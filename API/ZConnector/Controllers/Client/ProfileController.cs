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
            try 
            {
                UserModel? userDataStatus;
                
                string? id = User.FindFirstValue("id");
                if (id is not null)
                {
                    userDataStatus = await _userService.GetUserById(Convert.ToInt32(id));
                }
                else
                {
                    return Unauthorized("User Id not found. Try Login again.");
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

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateUserData([FromBody] UserModel userModel) 
        {
            try 
            {
                string? id = User.FindFirstValue("id");
                if (id is not null)
                {
                    userModel.ID = Convert.ToInt32(id);

                    await _userService.UpdateUserInfo(userModel);
                    return Ok();
                }
                return Unauthorized("Session expired. Try Login again.");
            }
            catch 
            {
                return BadRequest("An internal error occurred while updating user data.");
            }
        }
    }
}
