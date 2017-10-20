using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IErrorCodeConverter errorCodeConverter;
        private readonly IDataStructureConverter dataStructureConverter;

        public VehiclesController(IVehicleService vehicleService, 
            IErrorCodeConverter errorCodeConverter, 
            IDataStructureConverter dataStructureConverter)
        {
            this.vehicleService = vehicleService;
            this.errorCodeConverter = errorCodeConverter;
            this.dataStructureConverter = dataStructureConverter;
        }

        [HttpGet]
        [Authorize(Policy = "RequireToken")]
        public async Task<IActionResult> GetAll()
        {
            var result = await vehicleService.GetAll();
            if (result.ResultCode == ResultCode.Success)
            {
                Response.Headers.Add("x-total-count", result.Data.Count().ToString());
                var formattedResult = dataStructureConverter
                    .ConvertAndMap<IEnumerable<VehicleModel>, IEnumerable<Vehicle>>("vehicles", result.Data);
                return Ok(formattedResult);
            }

            return errorCodeConverter.Convert(result.ResultCode);
        }

        [HttpGet("{id:Guid}", Name = "GetVehicle")]
        [Authorize(Policy = "RequireToken")]
        public async Task<IActionResult> Get([Required] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(id);
            }

            var result = await vehicleService.Get(id);
            if (result.ResultCode == ResultCode.Success)
            {
                var formattedResult = dataStructureConverter
                    .ConvertAndMap<VehicleModel, Vehicle>("vehicle", result.Data);
                return Ok(formattedResult);
            }

            return errorCodeConverter.Convert(result.ResultCode);
        }

        [HttpPost]
        [Authorize(Policy = "RequireToken")]
        public async Task<IActionResult> Post([FromBody] VehicleModel vehicleModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = dataStructureConverter.Map<Vehicle, VehicleModel>(vehicleModel);

            var result = await vehicleService.Insert(vehicle);
            if (result.ResultCode == ResultCode.Success)
            {
                return CreatedAtRoute("GetVehicle", new { id = result.Data }, result.Data);
            }

            return errorCodeConverter.Convert(result.ResultCode);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "RequireToken")]
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

            return errorCodeConverter.Convert(resultCode);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "RequireToken")]
        public async Task<IActionResult> Put(Guid id, [FromBody] VehicleModel vehicleModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = dataStructureConverter.Map<Vehicle, VehicleModel>(vehicleModel);

            var result = await vehicleService.Update(vehicle);
            if (result.ResultCode == ResultCode.Success)
            {
                return Ok(result.Data);
            }

            return errorCodeConverter.Convert(result.ResultCode);
        }
    }
}