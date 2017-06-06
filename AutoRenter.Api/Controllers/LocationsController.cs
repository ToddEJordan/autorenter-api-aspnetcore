using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IResultCodeProcessor resultCodeProcessor;
        private readonly IDataStructureConverter dataStructureConverter;

        public LocationsController(ILocationService locationService, IResultCodeProcessor resultCodeProcessor, IDataStructureConverter dataStructureConverter)
        {
            this.locationService = locationService;
            this.resultCodeProcessor = resultCodeProcessor;
            this.dataStructureConverter = dataStructureConverter;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await locationService.GetAll();
            if (result.ResultCode == ResultCode.Success)
            {
                Response.Headers.Add("x-total-count", result.Data.Count().ToString());
                var formattedResult = dataStructureConverter
                    .FormatAndMap<IEnumerable<LocationModel>, IEnumerable<Location>>("locations", result.Data);
                return Ok(formattedResult);
            }

            return resultCodeProcessor.Process(result.ResultCode);
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
                var formattedResult = dataStructureConverter
                    .FormatAndMap<LocationModel, Location>("location", result.Data);
                return Ok(formattedResult);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }

        [HttpPost]
        [AllowAnonymous]
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

            var result = await locationService.Delete(id);
            if (result == ResultCode.Success)
            {
                return NoContent();
            }

            return resultCodeProcessor.Process(result);
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
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

            return resultCodeProcessor.Process(result.ResultCode);
        }

        [HttpGet("{locationId}/vehicles")]
        [AllowAnonymous]
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
                    dataStructureConverter.FormatAndMap<IEnumerable<VehicleModel>, IEnumerable<Vehicle>>("vehicles", result.Data);
                return Ok(formattedResult);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }

        [HttpPost("{locationId}/vehicles")]
        [AllowAnonymous]
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

            return resultCodeProcessor.Process(result.ResultCode);
        }
    }
}