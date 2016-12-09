using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.API.Data;
using AutoRenter.API.Domain;
using AutoRenter.API.Models;
using AutoRenter.API.Models.Location;
using AutoRenter.API.Models.Locations;
using AutoRenter.API.Models.Vehicle;
using AutoRenter.API.Queries.Locations;
using AutoRenter.API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.API.Controllers
{
    [Route("api/locations")]
    public class LocationsController : Controller
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IResponseConverter _responseConverter;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMediator _mediator;

        public LocationsController(ILocationRepository locationRepository, IVehicleRepository vehicleRepository,
            IResponseConverter responseConverter, IMediator mediator)
        {
            _responseConverter = responseConverter;
            _mediator = mediator;
            _vehicleRepository = vehicleRepository;
            _locationRepository = locationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var locations = await _mediator.SendAsync(new GetAllLocationsQuery());
            var formattedResult = _responseConverter.Convert(locations);

            Response.Headers.Add("x-total-count", locations.Count.ToString());
            return Ok(formattedResult);
        }

        [HttpGet("{id:Guid}", Name = "GetLocation")]
        public IActionResult Get(Guid id)
        {
            var location = _locationRepository.GetSingle(s => s.Id == id, s => s.Vehicles);

            if (location != null)
            {
                var locationDto = Mapper.Map<Location, LocationModel>(location);
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
        public IActionResult Post([FromBody] LocationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var location = Mapper.Map<LocationModel, Location>(model);
            _locationRepository.Add(location);
            _locationRepository.Commit();

            model = Mapper.Map<Location, LocationModel>(location);

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

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] LocationModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var location = _locationRepository.GetSingle(id);

            if (location == null)
                return NotFound();
            Mapper.Map(model, location);
            _locationRepository.Update(location);
            _locationRepository.Commit();

            //TODO: Figure out route url for location header
            return NoContent();
        }

        [HttpGet("{locationId}/vehicles")]
        public IActionResult GetAllVehicles(Guid locationId)
        {
            var location = _locationRepository.GetSingle(s => s.Id == locationId, s => s.Vehicles);

            if (location != null)
            {
                var totalVehicles = location.Vehicles.Count;
                var vehicles = location.Vehicles;
                var vehicleDtos = Mapper.Map<IEnumerable<Vehicle>, IEnumerable<VehicleModel>>(vehicles);
                var formattedResult = _responseConverter.Convert(vehicleDtos);

                Response.Headers.Add("x-total-count", totalVehicles.ToString());
                return Ok(formattedResult);
            }

            Response.Headers.Add("x-status-reason", $"No resource was found with the unique identifier '{locationId}'.");
            return NotFound();
        }
    }
}