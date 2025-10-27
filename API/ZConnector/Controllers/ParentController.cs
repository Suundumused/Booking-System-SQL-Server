using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace ZConnector.Controllers
{
    public abstract class ParentController : ControllerBase
    {
        protected IActionResult AuthExpired() 
        {
            return Unauthorized("Session expired. Try Login again.");
        }
    }
}
