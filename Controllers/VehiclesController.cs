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
    [Route("api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IResponseConverter _responseConverter;
        private readonly IVehicleRepository _vehicleRepository;

        public VehiclesController(IVehicleRepository vehicleRepository, IResponseConverter responseConverter)
        {
            _responseConverter = responseConverter;
            _vehicleRepository = vehicleRepository;
        }

        [HttpGet("{id:Guid}", Name = "GetVehicle")]
        public IActionResult Get([Required] Guid id)
        {
            var vehicle = _vehicleRepository.GetSingle(s => s.Id == id, s => s.Location);

            if (vehicle != null)
            {
                var vehicleDto = Mapper.Map<Vehicle, VehicleDto>(vehicle);
                var formattedResult = _responseConverter.Convert(vehicleDto);
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
        public IActionResult Post([FromBody] VehicleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = Mapper.Map<VehicleDto, Vehicle>(model);
            _vehicleRepository.Add(vehicle);
            _vehicleRepository.Commit();

            model = Mapper.Map<Vehicle, VehicleDto>(vehicle);

            return CreatedAtRoute("GetVehicle", new {id = model.Id}, null);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var vehicle = _vehicleRepository.GetSingle(id);

            if (vehicle == null)
                return NotFound();

            _vehicleRepository.Delete(vehicle);
            _vehicleRepository.Commit();
            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] VehicleDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var vehicle = _vehicleRepository.GetSingle(id);

            if (vehicle == null)
                return NotFound();

            Mapper.Map(model, vehicle);
            _vehicleRepository.Update(vehicle);
            _vehicleRepository.Commit();

            //TODO: Figure out route url for location header
            return NoContent();
        }
    }
}