using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Controllers
{
    [Route("api/locations")]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService locationService;
        private readonly IValidationService validationService;

        public LocationsController(ILocationService locationService, IValidationService validationService)
        {
            this.locationService = locationService;
            this.validationService = validationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await locationService.GetAll();
            if (result.ResultCode == ResultCode.Success)
            {
                var formattedResult = new Dictionary<string, object>
                {
                    { "locations", result.Data }
                };
                Response.Headers.Add("x-total-count", result.Data.Count().ToString());
                Response.Headers.Add("Content-Length", result.Data.ToString().ToCharArray().Count().ToString());
                return Ok(formattedResult);
            }

            return ProcessResultCode(result.ResultCode);
        }

        [HttpGet("{id:Guid}", Name = "GetLocation")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(id);
            }

            var result = await locationService.Get(id);
            if (result.ResultCode == ResultCode.Success)
            {
                var formattedResult = new Dictionary<string, object>
                {
                    { "location", result.Data }
                };
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
        public async Task<IActionResult> Post([FromBody] Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await locationService.Insert(location);
            if (result.ResultCode == ResultCode.Success)
            {
                return CreatedAtRoute("GetLocation", new { id = result.Data }, result.Data);
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

            var result = await locationService.Delete(id);
            if (result == ResultCode.Success)
            {
                return NoContent();
            }

            return ProcessResultCode(result);
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Put(Guid id, [FromBody] Location location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await locationService.Update(location);
            if (result.ResultCode == ResultCode.Success)
            {
                return Ok(result.Data);
            }

            return ProcessResultCode(result.ResultCode);
        }

        [HttpGet("{locationId}/vehicles")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllVehicles(Guid locationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehiclesResult = await locationService.GetVehicles(locationId);
            if (vehiclesResult.ResultCode == ResultCode.Success)
            {
                var totalVehicles = vehiclesResult.Data.Count();
                var formattedResult = new Dictionary<string, object>
                {
                    { "vehicles", vehiclesResult.Data }
                };
                Response.Headers.Add("x-total-count", totalVehicles.ToString());
                return Ok(formattedResult);
            }

            return ProcessResultCode(vehiclesResult.ResultCode);
        }

        [HttpPost("{locationId}/vehicles")]
        [AllowAnonymous]
        public async Task<IActionResult> AddVehicleToLocation(Guid locationId, [FromBody] Vehicle vehicle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await locationService.AddVehicle(locationId, vehicle);
            if (result == ResultCode.Success)
            {
                return NoContent();
            }

            return ProcessResultCode(result);
        }
    }
}