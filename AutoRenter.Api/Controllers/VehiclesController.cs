using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using AutoRenter.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoRenter.Domain.Interfaces;

using AutoRenter.Api.Services;

namespace AutoRenter.Api.Controllers
{
    [Route("api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IVehicleService vehicleService;
        private readonly IResultCodeProcessor resultCodeProcessor;
        public VehiclesController(IVehicleService vehicleService, IResultCodeProcessor resultCodeProcessor)
        {
            this.vehicleService = vehicleService;
            this.resultCodeProcessor = resultCodeProcessor;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await vehicleService.GetAll();
            if (result.ResultCode == ResultCode.Success)
            {
                var formattedResult = new Dictionary<string, object>
                {
                    { "vehicles", result.Data }
                };
                Response.Headers.Add("x-total-count", result.Data.Count().ToString());
                return Ok(formattedResult);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }

        [HttpGet("{id:Guid}", Name = "GetVehicle")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([Required] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(id);
            }

            var result = await vehicleService.Get(id);
            if (result.ResultCode == ResultCode.Success)
            {
                var formattedResult = new Dictionary<string, object>
                {
                    {"vehicle", result.Data}
                };
                return Ok(formattedResult);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }

        [HttpGet("{name}")]
        [AllowAnonymous]
        public IActionResult GetByName([Required] string name)
        {
            Response.Headers.Add("x-status-reason",
                $"The value '{name}' is not recognize as a valid guid to uniquely identify a resource.");
            return BadRequest();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await vehicleService.Insert(vehicle);
            if (result.ResultCode == ResultCode.Success)
            {
                return CreatedAtRoute("GetVehicle", new { id = result.Data }, result.Data);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(id);
            }

            var resultCode = await vehicleService.Delete(id);
            if (resultCode == ResultCode.Success)
            {
                return NoContent();
            }

            return resultCodeProcessor.Process(resultCode);
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Put(Guid id, [FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await vehicleService.Update(vehicle);
            if (result.ResultCode == ResultCode.Success)
            {
                return Ok(result.Data);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }
    }
}