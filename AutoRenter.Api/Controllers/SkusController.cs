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
                Response.Headers.Add("Content-Length", result.Data.ToString().ToCharArray().Count().ToString());
                return Ok(formattedResult);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }

        [HttpGet("{id:Guid}", Name = "GetSku")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(id);
            }

            var result = await skuService.Get(id);
            if (result.ResultCode == ResultCode.Success)
            {
                var formattedResult = new Dictionary<string, object>
                {
                    { "sku", result.Data }
                };
                return Ok(formattedResult);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Put(Guid id, [FromBody] Sku sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await skuService.Update(sku);
            if (result.ResultCode == ResultCode.Success)
            {
                return Ok(result.Data);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] Sku sku)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await skuService.Insert(sku);
            if (result.ResultCode == ResultCode.Success)
            {
                return CreatedAtRoute("GetSku", new { id = result.Data }, result.Data);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await skuService.Delete(id);
            if (result == ResultCode.Success)
            {
                return NoContent();
            }

            return resultCodeProcessor.Process(result);
        }
    }
}