using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoRenter.Domain.Interfaces;
using System.Linq;

namespace AutoRenter.Api.Controllers
{
    [Route("api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService vehicleService;
        public VehiclesController(IVehicleService vehicleService)
        {
            this.vehicleService = vehicleService;
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
                Response.Headers.Add("Content-Length", result.Data.ToString().ToCharArray().Count().ToString());
                return Ok(formattedResult);
            }

            return ProcessResultCode(result.ResultCode);
        }

        [HttpGet("{id:Guid}", Name = "GetVehicle")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([Required] Guid id)
        {
            if (id == null || id == Guid.Empty)
            {
                return BadRequest(id);
            }

            var result = await vehicleService.Get(id);
            if (result.ResultCode == ResultCode.Success)
            {
                var formattedResult = new Dictionary<string, object>();
                formattedResult.Add("vehicle", result.Data);
                return Ok(formattedResult);
            }

            return ProcessResultCode(result.ResultCode);
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

            return ProcessResultCode(result.ResultCode);
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

            return ProcessResultCode(resultCode);
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

            return ProcessResultCode(result.ResultCode);
        }
    }
}