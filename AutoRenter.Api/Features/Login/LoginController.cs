using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.Api.Features.Login
{
    [Route("api/login")]
    public class LoginController : Controller
    {
        private readonly IAuthenticateUser _authenticateUser;

        public LoginController(IAuthenticateUser authenticateUser)
        {
            _authenticateUser = authenticateUser;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] LoginModel loginModel)
        {
            try
            {
                var result = _authenticateUser.Execute(loginModel);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpGet]
        //[Authorize(Policy="IsAdmin")]
        [Authorize]
        public IActionResult Get()
        {
            try
            {
                return Ok(true);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
