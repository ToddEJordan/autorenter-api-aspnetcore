using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoRenter.Api.Models;
using AutoRenter.Api.Authentication;
using AutoRenter.Api.Services;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Controllers
{
    [Route("api/login")]
    public class LoginController : Controller
    {
        private readonly IAuthenticateUser authenticateUser;
        private readonly IErrorCodeConverter errorCodeConverter;

        public LoginController(IAuthenticateUser authenticateUser, IErrorCodeConverter errorCodeConverter)
        {
            this.authenticateUser = authenticateUser;
            this.errorCodeConverter = errorCodeConverter;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] LoginModel loginModel)
        {
            try
            {
                var result = await authenticateUser.Execute(loginModel);
                if (result.ResultCode == ResultCode.Success)
                {
                    return Ok(result.Data);
                }
                return errorCodeConverter.Convert(result.ResultCode);
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }        
    }
}
