using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.Api.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.Api.Features.Vehicle
{
    [Route("api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IResponseConverter _responseConverter;

        public VehiclesController(IResponseConverter responseConverter, IMediator mediator)
        {
            _responseConverter = responseConverter;
            _mediator = mediator;
        }

        [HttpGet("{id:Guid}", Name = "GetVehicle")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([Required] Guid id)
        {
            var query = new Get.Query {Id = id};
            var model = await _mediator.SendAsync(query);

            if (model != null)
            {
                var formattedResult = new Dictionary<string, object>();
                formattedResult.Add("vehicle", model);
                return Ok(formattedResult);
            }

            Response.Headers.Add("x-status-reason", $"No resource was found with the unique identifier '{id}'.");
            return NotFound();
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
        public async Task<IActionResult> Post([FromBody] PostPut.Command command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = await _mediator.SendAsync(new PostPut.Query {Id = command.Id});

            if ((command.Id == null) || command.Id.Equals(Guid.Empty))
                command.Id = vehicle.Id;

            await _mediator.SendAsync(command);

            return CreatedAtRoute("GetVehicle", new {id = command.Id}, null);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
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
        [AllowAnonymous]
        public async Task<IActionResult> Put(Guid id, [FromBody] PostPut.Command command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = await _mediator.SendAsync(new PostPut.Query {Id = id});

            if (vehicle == null)
                return NotFound();

            Mapper.Map(command, vehicle);

            await _mediator.SendAsync(command);

            //TODO: Figure out route url for location header
            return NoContent();
        }
    }
}