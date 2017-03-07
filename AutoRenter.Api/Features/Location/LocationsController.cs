using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.Api.Features.Location
{
    [Route("api/locations")]
    public class LocationsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IResponseConverter _responseConverter;

        public LocationsController(IResponseConverter responseConverter, IMediator mediator)
        {
            _responseConverter = responseConverter;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var query = new GetAll.Query();
            var model = await _mediator.SendAsync(query);
            var formattedResult = new Dictionary<string, object>();
            formattedResult.Add("locations", model.Locations);

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
                var formattedResult = new Dictionary<string, object>();
                formattedResult.Add("location", model);
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

            if ((command.Id == null) || command.Id.Equals(Guid.Empty))
                command.Id = location.Id;

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
        public async Task<IActionResult> GetAllVehicles(Guid locationId)
        {
            var model = await _mediator.SendAsync(new Vehicle.GetAll.Query {LocationId = locationId});

            if (model != null)
            {
                var totalVehicles = model.Vehicles.Count;
                var formattedResult = new Dictionary<string, object>();
                formattedResult.Add("vehicles", model.Vehicles);

                Response.Headers.Add("x-total-count", totalVehicles.ToString());
                return Ok(formattedResult);
            }

            Response.Headers.Add("x-status-reason", $"No resource was found with the unique identifier '{locationId}'.");
            return NotFound();
        }
    }
}