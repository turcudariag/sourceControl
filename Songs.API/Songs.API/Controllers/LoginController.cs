using Songs.API.Models;
using Songs.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Songs.API.Controllers
{
    /// <summary>
    /// Authenticate users
    /// </summary>
    [Route("api/v2/login")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v2")]
    public class LoginController : ControllerBase
    {
        private readonly IUsersService _userService;

        public LoginController(IUsersService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Authenticate the user.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Produces("application/json")]
        public IActionResult Login([FromBody] AuthenticateRequest login)
        {
            IActionResult response = Unauthorized();
            var token = _userService.Authenticate(login.Username, login.Password);
            if (token != null)
                response = Ok(token);

            return response;
        }
    }
}
