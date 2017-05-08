using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoRenter.Api.DomainInterfaces;
using AutoRenter.Api.Domain;

namespace AutoRenter.Api.Controllers
{
    [Route("api/skus")]
    public class SkusController : ControllerBase
    {
        private readonly ISkuService skuService;
        public SkusController(ISkuService skuService)
        {
            this.skuService = skuService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var result = skuService.GetAll();
            if (result.ResultCode == ResultCode.Success)
            {
                var formattedResult = new Dictionary<string, object>
                {
                    { "skus", result.Data }
                };
                Response.Headers.Add("x-total-count", result.Data.ToString());
                return Ok(formattedResult);
            }

            return ProcessResultCode(result.ResultCode);
        }
    }
}