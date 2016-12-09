using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.API.Data;
using AutoRenter.API.Models.Vehicle;
using AutoRenter.API.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.API.Features.Location
{
    [Route("api/locations")]
    public class LocationsController : Controller
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMediator _mediator;
        private readonly IResponseConverter _responseConverter;

        public LocationsController(ILocationRepository locationRepository, IResponseConverter responseConverter,
            IMediator mediator)
        {
            _responseConverter = responseConverter;
            _mediator = mediator;
            _locationRepository = locationRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetAll.Query();
            var model = await _mediator.SendAsync(query);
            var formattedResult = _responseConverter.Convert(model);

            Response.Headers.Add("x-total-count", model.Locations.ToString());
            return Ok(formattedResult);
        }

        [HttpGet("{id:Guid}", Name = "GetLocation")]
        public async Task<IActionResult> Get(Guid id)
        {
            var query = new Get.Query {Id = id};
            var model = await _mediator.SendAsync(query);

            if (model != null)
            {
                var formattedResult = _responseConverter.Convert(model);
                return Ok(formattedResult);
            }

            Response.Headers.Add("x-status-reason", $"No resource was found with the unique identifier '{id}'.");
            return NotFound();
        }

        [HttpGet("{name}")]
        public IActionResult GetByName([Required] string name)
        {
            Response.Headers.Add("x-status-reason",
                $"The value '{name}' is not recognize as a valid guid to uniquely identify a resource.");
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PostPut.Command command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var location = await _mediator.SendAsync(new PostPut.Query {Id = command.Id});

            if (command.Id == null || command.Id.Equals(Guid.Empty))
            {
                command.Id = location.Id;
            }
            
            await _mediator.SendAsync(command);

            return CreatedAtRoute("GetLocation", new {id = command.Id}, null);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var query = new Delete.Query {Id = id};
            var command = await _mediator.SendAsync(query);

            if (command == null)
                return NotFound();

            await _mediator.SendAsync(command);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] PostPut.Command command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var location = await _mediator.SendAsync(new PostPut.Query {Id = id});

            if (location == null)
                return NotFound();

            Mapper.Map(location, command);

            await _mediator.SendAsync(command);

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
                var vehicleDtos = Mapper.Map<IEnumerable<Domain.Vehicle>, IEnumerable<VehicleModel>>(vehicles);
                var formattedResult = _responseConverter.Convert(vehicleDtos);

                Response.Headers.Add("x-total-count", totalVehicles.ToString());
                return Ok(formattedResult);
            }

            Response.Headers.Add("x-status-reason", $"No resource was found with the unique identifier '{locationId}'.");
            return NotFound();
        }
    }
}