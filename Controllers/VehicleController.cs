using AutoRenter.API.Models;
using AutoRenter.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.API.Controllers
{
    [Route("api/vehicles")]
    [Produces("application/json")]
    public class VehicleController : Controller
    {
        public VehicleController(IVehicleService vehicleService)
        {
            VehicleService = vehicleService;
        }

        private IVehicleService VehicleService { get; }

        [HttpGet]
        [Produces(typeof(Vehicle[]))]
        public IActionResult List()
        {
            return Ok(VehicleService.List());
        }

        [HttpGet("{id}", Name = "GetVehicle")]
        [Produces(typeof(Vehicle))]
        public IActionResult Get(int id)
        {
            return Ok(VehicleService.Get(id));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Vehicle model)
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
        public IActionResult Update(int id, [FromBody] Vehicle model)
        {
            VehicleService.Update(id, model);
            return Ok(model);
        }
    }
}