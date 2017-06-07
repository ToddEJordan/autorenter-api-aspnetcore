using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoRenter.Api.Models;
using AutoRenter.Api.Services;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Controllers
{
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        private readonly ILogService logService;
        private readonly IErrorCodeConverter errorCodeConverter;
        private readonly IDataStructureConverter dataStructureConverter;

        public LogController(ILogService logService, IErrorCodeConverter errorCodeConverter, IDataStructureConverter dataStructureConverter)
        {
            this.logService = logService;
            this.errorCodeConverter = errorCodeConverter;
            this.dataStructureConverter = dataStructureConverter;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] LogEntryModel logEntryModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var log = dataStructureConverter.Map<LogEntry, LogEntryModel>(logEntryModel);

            var result = await logService.Log(log);
            if (result.ResultCode == ResultCode.Success)
            {
                return Created(string.Empty, log.Message);
            }

            return errorCodeConverter.Convert(result.ResultCode);
        }
    }
}
