using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using AutoRenter.Api.Models;
using AutoRenter.Api.Services;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Api.Controllers
{
    [Route("api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IVehicleService vehicleService;
        private readonly IResultCodeProcessor resultCodeProcessor;
        private readonly IMapper mapper;

        public VehiclesController(IVehicleService vehicleService, IResultCodeProcessor resultCodeProcessor, IMapper mapper)
        {
            this.vehicleService = vehicleService;
            this.resultCodeProcessor = resultCodeProcessor;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var result = await vehicleService.GetAll();
            if (result.ResultCode == ResultCode.Success)
            {
                var response = result.Data
                    .Select(vehicle => mapper.Map<VehicleModel>(vehicle))
                    .ToList();

                var formattedResult = new Dictionary<string, object>
                {
                    { "vehicles", response }
                };
                Response.Headers.Add("x-total-count", response?.Count().ToString());
                return Ok(formattedResult);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }

        [HttpGet("{id:Guid}", Name = "GetVehicle")]
        [AllowAnonymous]
        public async Task<IActionResult> Get([Required] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(id);
            }

            var result = await vehicleService.Get(id);
            if (result.ResultCode == ResultCode.Success)
            {
                var response = mapper.Map<VehicleModel>(result.Data);
                var formattedResult = new Dictionary<string, object>
                {
                    {"vehicle", response}
                };
                return Ok(formattedResult);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromBody] VehicleModel vehicleModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = mapper.Map<Vehicle>(vehicleModel);

            var result = await vehicleService.Insert(vehicle);
            if (result.ResultCode == ResultCode.Success)
            {
                return CreatedAtRoute("GetVehicle", new { id = result.Data }, result.Data);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(id);
            }

            var resultCode = await vehicleService.Delete(id);
            if (resultCode == ResultCode.Success)
            {
                return NoContent();
            }

            return resultCodeProcessor.Process(resultCode);
        }

        [HttpPut("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Put(Guid id, [FromBody] VehicleModel vehicleModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = mapper.Map<Vehicle>(vehicleModel);

            var result = await vehicleService.Update(vehicle);
            if (result.ResultCode == ResultCode.Success)
            {
                return Ok(result.Data);
            }

            return resultCodeProcessor.Process(result.ResultCode);
        }
    }
}