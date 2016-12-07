using System.Collections.Generic;
using System.Linq;
using AutoRenter.API.Models;
using AutoRenter.API.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AutoRenter.API.Controllers
{
    [Route("api/vehicles")]
    public class VehiclesController : Controller
    {
        public VehiclesController(IVehicleService vehicleService)
        {
            VehicleService = vehicleService;
        }

        private IVehicleService VehicleService { get; }

        [HttpGet]
        public IActionResult Get()
        {
            var vehicles = VehicleService.List();
            var formattedResult = JsonConvert.SerializeObject(vehicles, Formatting.Indented);

            Response.Headers.Add("x-total-count", vehicles.Count().ToString());
            return Ok(formattedResult);
        }

        [HttpGet("{id:int:min(1)}", Name = "GetVehicle")]
        public IActionResult Get([Required] int id)
        {
            try
            {
                var vehicle = VehicleService.Get(id);
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
        public IActionResult Post([FromBody] Vehicle model)
        {
            VehicleService.Create(model);
            return CreatedAtRoute("GetVehicle", new {id = model.Id}, null);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            VehicleService.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult Put(int id, [FromBody] Vehicle model)
        {
            VehicleService.Update(id, model);
            return Ok(model);
        }
    }
}