using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.Api.Controllers
{
    public class RaiseErrorController : Controller
    {
        [HttpGet]
        [Authorize(Policy = "RequireToken")]
        [Route("api/raise-error")]
        public IActionResult Get()
        {
            throw new Exception("An exception was intentionally raised by an api call to api/raise-error");
        }
    }
}
