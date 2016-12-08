using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoRenter.API.Models;
using AutoRenter.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.API.Controllers
{
    [Route("api/locations")]
    public class LocationsController : Controller
    {
        public LocationsController(ILocationService locationService, IResponseConverter responseConverter)
        {
            LocationService = locationService;
            ResponseConverter = responseConverter;
        }

        private ILocationService LocationService { get; }
        private IResponseConverter ResponseConverter { get; }

        [HttpGet]
        public IActionResult Get()
        {
            var locations = LocationService.List();
            var formattedResult = ResponseConverter.Convert(locations);

            Response.Headers.Add("x-total-count", locations.Count().ToString());
            return Ok(formattedResult);
        }

        [HttpGet("{id:Guid}", Name = "GetLocation")]
        [Produces(typeof(Location))]
        public IActionResult Get(Guid id)
        {
            try
            {
                var location = LocationService.Get(id);
                var formattedResult = ResponseConverter.Convert(location);

                return Ok(formattedResult);
            }
            catch (KeyNotFoundException)
            {
                Response.Headers.Add("x-status-reason", $"No resource was found with the unique identifier '{id}'.");
                return NotFound();
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetByName([Required] string name)
        {
            Response.Headers.Add("x-status-reason",
                $"The value '{name}' is not recognize as a valid integer to uniquely identify a resource.");
            return BadRequest();
        }

        [HttpPost]
        public IActionResult Post([FromBody] Location model)
        {
            LocationService.Create(model);
            return CreatedAtRoute("GetLocation", new {id = model.Id}, null);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            LocationService.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult Update(Guid id, [FromBody] Location model)
        {
            LocationService.Update(id, model);
            return Ok(model);
        }
    }
}