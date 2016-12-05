using AutoRenter.API.Models;
using AutoRenter.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AutoRenter.API.Controllers
{
    [Route("api/locations")]
    [Produces("application/json")]
    public class LocationController : Controller
    {
        public LocationController(ILocationService locationService)
        {
            LocationService = locationService;
        }

        private ILocationService LocationService { get; }

        [HttpGet]
        [Produces(typeof(Location[]))]
        public IActionResult List()
        {
            return Ok(LocationService.List());
        }

        [HttpGet("{id}", Name = "GetLocation")]
        [Produces(typeof(Location))]
        public IActionResult Get(int id)
        {
            return Ok(LocationService.Get(id));
        }

        [HttpPost]
        public IActionResult Create([FromBody] Location model)
        {
            LocationService.Create(model);
            return CreatedAtRoute("GetLocation", new {id = model.Id}, null);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            LocationService.Delete(id);
            return NoContent();
        }

        [HttpPut]
        public IActionResult Update(int id, [FromBody] Location model)
        {
            LocationService.Update(id, model);
            return Ok(model);
        }
    }
}