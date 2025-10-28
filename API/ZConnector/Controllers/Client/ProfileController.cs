using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;

using ZConnector.GlobalHanlders;
using ZConnector.Models.Client.User;
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
                string? id = User.FindFirstValue("id");
                if (id is null) return AuthExpired();

                UserModel? userDataStatus = await ApiEfCoreHandler.ExecuteWithHandlingAsync(
                    async () => await _userService.GetUserById(Convert.ToInt32(id)),
                    "User"
                );

                if (userDataStatus is null)
                {
                    return NotFound("User data not found. Try Login again.");
                }
                return Ok(userDataStatus);
            }
            catch (EfSafeException ex) 
            { 
                return StatusCode(ex.statusCode, ex.Message); 
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
                if (id is null) return AuthExpired();

                userModel.ID = Convert.ToInt32(id);

                await ApiEfCoreHandler.ExecuteWithHandlingAsync(
                    async () => await _userService.UpdateUserInfo(userModel),
                    "User"
                );
                return Ok();
            }
            catch (EfSafeException ex) 
            {
                return StatusCode(ex.statusCode, ex.Message);
            }
            catch
            {
                return BadRequest("An internal error occurred while updating user data.");
            }
        }
    }
}
