using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using AutoRenter.API.Data;
using AutoRenter.API.Entities;
using AutoRenter.API.Models;
using AutoRenter.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.API.Controllers
{
    [Route("api/locations")]
    public class LocationsController : Controller
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IResponseConverter _responseConverter;
        private readonly IVehicleRepository _vehicleRepository;

        public LocationsController(ILocationRepository locationRepository, IVehicleRepository vehicleRepository,
            IResponseConverter responseConverter)
        {
            _responseConverter = responseConverter;
            _vehicleRepository = vehicleRepository;
            _locationRepository = locationRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var totalLocations = _locationRepository.Count();
            var locations = _locationRepository.AllIncluding(s => s.Vehicles)
                .OrderBy(s => s.Name)
                .ToList();

            var locationDtos = Mapper.Map<IEnumerable<Location>, IEnumerable<LocationDto>>(locations);
            var formattedResult = _responseConverter.Convert(locationDtos);

            Response.Headers.Add("x-total-count", totalLocations.ToString());
            return Ok(formattedResult);
        }

        [HttpGet("{id:Guid}", Name = "GetLocation")]
        public IActionResult Get(Guid id)
        {
            var location = _locationRepository.GetSingle(s => s.Id == id, s => s.Vehicles);

            if (location != null)
            {
                var locationDto = Mapper.Map<Location, LocationDto>(location);
                var formattedResult = _responseConverter.Convert(locationDto);
                return Ok(formattedResult);
            }

            Response.Headers.Add("x-status-reason", $"No resource was found with the unique identifier '{id}'.");
            return NotFound();
        }

        [HttpGet("{name}")]
        public IActionResult GetByName([Required] string name)
        {
            Response.Headers.Add("x-status-reason",
                $"The value '{name}' is not recognize as a valid integer to uniquely identify a resource.");
            return BadRequest();
        }

        [HttpPost]
        public IActionResult Post([FromBody] LocationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var location = Mapper.Map<LocationDto, Location>(model);
            _locationRepository.Add(location);
            _locationRepository.Commit();

            model = Mapper.Map<Location, LocationDto>(location);

            return CreatedAtRoute("GetLocation", new {id = model.Id}, null);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var location = _locationRepository.GetSingle(id);

            if (location == null)
                return NotFound();

            _vehicleRepository.DeleteWhere(a => a.LocationId == id);
            _locationRepository.Delete(location);
            _locationRepository.Commit();
            return NoContent();
        }

        [HttpPut]
        public IActionResult Put(Guid id, [FromBody] LocationDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var location = _locationRepository.GetSingle(id);

            if (location == null)
                return NotFound();
            Mapper.Map(model, location);
            _locationRepository.Update(location);

            //TODO: Figure out route url for location header
            return NoContent();
        }
    }
}