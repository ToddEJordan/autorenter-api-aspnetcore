using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoRenter.Api.Models;
using AutoRenter.Api.Authentication;

namespace AutoRenter.Api.Controllers
{
    [Route("api/login")]
    public class LoginController : Controller
    {
        private readonly IAuthenticateUser authenticateUser;

        public LoginController(IAuthenticateUser authenticateUser)
        {
            this.authenticateUser = authenticateUser;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] LoginModel loginModel)
        {
            try
            {
                var result = authenticateUser.Execute(loginModel);
                return Ok(result);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
    }
}
