using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AutoRenter.API.Models;
using AutoRenter.API.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace AutoRenter.API.Controllers
{
    [Route("api/locations")]
    public class LocationsController : Controller
    {
        public LocationsController(ILocationService locationService)
        {
            LocationService = locationService;
        }

        private ILocationService LocationService { get; }

        [HttpGet]
        public IActionResult Get()
        {
            var locations = LocationService.List();
            var formattedResult = JsonConvert.SerializeObject(locations, Formatting.Indented);

            Response.Headers.Add("x-total-count", locations.Count().ToString());
            return Ok(formattedResult);
        }

        [HttpGet("{id:Guid}", Name = "GetLocation")]
        [Produces(typeof(Location))]
        public IActionResult Get(Guid id)
        {
            try
            {
                var vehicle = LocationService.Get(id);
                var formattedResult = JsonConvert.SerializeObject(vehicle, Formatting.Indented);

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
            Response.Headers.Add("x-status-reason", $"The value '{name}' is not recognize as a valid integer to uniquely identify a resource.");
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