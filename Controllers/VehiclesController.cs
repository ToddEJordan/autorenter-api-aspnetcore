using System;
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
        public VehiclesController(IVehicleService vehicleService, IResponseConverter responseConverter)
        {
            VehicleService = vehicleService;
            ResponseConverter = responseConverter;
        }

        private IVehicleService VehicleService { get; }
        private IResponseConverter ResponseConverter { get; }

        [HttpGet]
        public IActionResult Get()
        {
            var vehicles = VehicleService.List();
            var formattedResult = ResponseConverter.Convert(vehicles);

            Response.Headers.Add("x-total-count", vehicles.Count().ToString());
            return Ok(formattedResult);
        }

        [HttpGet("{id:Guid}", Name = "GetVehicle")]
        public IActionResult Get([Required] Guid id)
        {
            try
            {
                var vehicle = VehicleService.Get(id);
                var formattedResult = ResponseConverter.Convert(vehicle);

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
        public IActionResult Delete(Guid id)
        {
            VehicleService.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult Put(Guid id, [FromBody] Vehicle model)
        {
            VehicleService.Update(id, model);
            return Ok(model);
        }
    }
}