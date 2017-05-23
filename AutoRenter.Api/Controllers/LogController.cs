using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using AutoRenter.Api.Models;
using AutoRenter.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Post([FromBody] LogModel log)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = logService.Log(log.Message, log.Level);
            if (result.ResultCode == ResultCode.Success)
            {
                return NoContent();
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }
    }
}
