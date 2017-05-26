using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoRenter.Api.Services;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Controllers
{
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        private readonly ILogService logService;
        private readonly IResultCodeProcessor resultCodeProcessor;

        public LogController(ILogService logService, IResultCodeProcessor resultCodeProcessor)
        {
            this.logService = logService;
            this.resultCodeProcessor = resultCodeProcessor;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] LogEntry log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await logService.Log(log);
            if (result.ResultCode == ResultCode.Success)
            {
                return Created(string.Empty, log.Message);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }
    }
}
