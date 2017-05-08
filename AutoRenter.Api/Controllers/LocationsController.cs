using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoRenter.Api.DomainInterfaces;
using System.Linq;
using AutoRenter.Api.Models;

namespace AutoRenter.Api.Controllers
{
    [Route("api/locations")]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService locationService;

        public LocationsController(ILocationService locationService)
        {
            this.locationService = locationService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var result = locationService.GetAll();
            if (result.ResultCode == Domain.ResultCode.Success)
            {
                var formattedResult = new Dictionary<string, object>
                {
                    { "locations", result.Data.Select(x => x as Models.LocationModel) }
                };
                Response.Headers.Add("x-total-count", result.Data.ToString());
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
            if (result.ResultCode == Domain.ResultCode.Success)
            {
                var formattedResult = new Dictionary<string, object>
                {
                    { "location", result.Data as Models.LocationModel}
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
        public async Task<IActionResult> Post([FromBody] LocationModel location)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await locationService.Insert(location);
            if (result.ResultCode == Domain.ResultCode.Success)
            {
                return CreatedAtRoute("GetLocation", new { id = result.Data }, null);
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
            if (result == Domain.ResultCode.Success)
            {
                return NoContent();
            }

            return ProcessResultCode(result);
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Put(Guid id, [FromBody] LocationModel location)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await locationService.Update(location);

            if (result.ResultCode == Domain.ResultCode.Success)
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
            if (vehiclesResult.ResultCode == Domain.ResultCode.Success)
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
    }
}