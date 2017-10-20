using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoRenter.Api.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoRenter.Api.Models;
using AutoRenter.Api.Services;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Controllers
{
    [Route("api/locations")]
    public class LocationsController : Controller
    {
        private readonly ILocationService locationService;
        private readonly IErrorCodeConverter errorCodeConverter;
        private readonly IDataStructureConverter dataStructureConverter;

        public LocationsController(ILocationService locationService, IErrorCodeConverter errorCodeConverter, IDataStructureConverter dataStructureConverter)
        {
            this.locationService = locationService;
            this.errorCodeConverter = errorCodeConverter;
            this.dataStructureConverter = dataStructureConverter;
        }

        [HttpGet]
        [Authorize(Policy = "RequireToken")]
        public async Task<IActionResult> GetAll()
        {
            var result = await locationService.GetAll();
            if (result.ResultCode == ResultCode.Success)
            {
                Response.Headers.Add("x-total-count", result.Data.Count().ToString());
                var formattedResult = dataStructureConverter
                    .ConvertAndMap<IEnumerable<LocationModel>, IEnumerable<Location>>("locations", result.Data);
                return Ok(formattedResult);
            }

            return errorCodeConverter.Convert(result.ResultCode);
        }

        [HttpGet("{id:Guid}", Name = "GetLocation")]
        [Authorize(Policy = "RequireToken")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(id);
            }

            var result = await locationService.Get(id);
            if (result.ResultCode == ResultCode.Success)
            {
                var formattedResult = dataStructureConverter
                    .ConvertAndMap<LocationModel, Location>("location", result.Data);
                return Ok(formattedResult);
            }

            return errorCodeConverter.Convert(result.ResultCode);
        }

        [HttpPost]
        [Authorize(Policy = "RequireToken")]
        public async Task<IActionResult> Post([FromBody] LocationModel locationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var location = dataStructureConverter.Map<Location, LocationModel>(locationModel);

            var result = await locationService.Insert(location);
            if (result.ResultCode == ResultCode.Success)
            {
                return CreatedAtRoute("GetLocation", new { id = result.Data }, result.Data);
            }

            return errorCodeConverter.Convert(result.ResultCode);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireToken")]
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

            return errorCodeConverter.Convert(result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireToken")]
        public async Task<IActionResult> Put(Guid id, [FromBody] LocationModel locationModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var location = dataStructureConverter.Map<Location, LocationModel>(locationModel);

            var result = await locationService.Update(location);
            if (result.ResultCode == ResultCode.Success)
            {
                return Ok(result.Data);
            }

            return errorCodeConverter.Convert(result.ResultCode);
        }

        [HttpGet("{locationId}/vehicles")]
        [Authorize(Policy = "RequireToken")]
        public async Task<IActionResult> GetAllVehicles(Guid locationId)
        {
            if (locationId == Guid.Empty)
            {
                return BadRequest(locationId);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await locationService.GetVehicles(locationId);
            if (result.ResultCode == ResultCode.Success 
                || result.ResultCode == ResultCode.NotFound)
            {
                Response.Headers.Add("x-total-count", 
                    result.Data == null ? "0" : result.Data.Count().ToString());

                var formattedResult =
                    dataStructureConverter.ConvertAndMap<IEnumerable<VehicleModel>, IEnumerable<Vehicle>>("vehicles", result.Data);
                return Ok(formattedResult);
            }

            return errorCodeConverter.Convert(result.ResultCode);
        }

        [HttpPost("{locationId}/vehicles")]
        [Authorize(Policy = "RequireToken")]
        public async Task<IActionResult> AddVehicleToLocation(Guid locationId, [FromBody] VehicleModel vehicleModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = dataStructureConverter.Map<Vehicle, VehicleModel>(vehicleModel);

            var result = await locationService.AddVehicle(locationId, vehicle);
            if (result.ResultCode == ResultCode.Success)
            {
                return Created("GetVehicle", result.Data);
            }

            return errorCodeConverter.Convert(result.ResultCode);
        }
    }
}