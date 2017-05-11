using System;
using System.Collections.Generic;
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
        public IActionResult GetAll()
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

        [HttpGet("{id:Guid}", Name = "GetSku")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await skuService.Get(id);
            if (result.ResultCode == ResultCode.Success)
            {
                var formattedResult = new Dictionary<string, object>
                {
                    { "sku", result.Data }
                };
                return Ok(formattedResult);
            }

            return ProcessResultCode(result.ResultCode);
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

            return ProcessResultCode(result.ResultCode);
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

            return ProcessResultCode(result.ResultCode);
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

            return ProcessResultCode(result);
        }
    }
}