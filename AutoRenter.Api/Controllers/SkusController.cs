using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoRenter.Api.Services;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Controllers
{
    [Route("api/skus")]
    public class SkusController : Controller
    {
        private readonly ISkuService skuService;
        private readonly IResultCodeProcessor resultCodeProcessor;
        public SkusController(ISkuService skuService, IResultCodeProcessor resultCodeProcessor)
        {
            this.skuService = skuService;
            this.resultCodeProcessor = resultCodeProcessor;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await skuService.GetAll();
            if (result.ResultCode == ResultCode.Success)
            {
                var formattedResult = new Dictionary<string, object>
                {
                    { "skus", result.Data }
                };
                Response.Headers.Add("x-total-count", result.Data.Count().ToString());
                return Ok(formattedResult);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }
    }
}