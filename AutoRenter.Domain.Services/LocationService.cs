using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoRenter.Api.Data;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Services.Commands;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Services
{
    public class LocationService : ILocationService, IDomainService, IDisposable
    {
        private bool disposed = false;
        private readonly AutoRenterContext context;
        private readonly IVehicleService vehicleService;
        private readonly IValidationService validationService;
        public LocationService(AutoRenterContext context, IVehicleService vehicleService, IValidationService validationService)
        {
            this.context = context;
            this.vehicleService = vehicleService;
            this.validationService = validationService;
        }

        public async Task<Result<IEnumerable<Location>>> GetAll()
        {
            var command = CommandFactory<Location>.CreateGetAllCommand(context);
            var result = await command.Execute();

            if (result.ResultCode != ResultCode.Success)
            {
                return result;
            }

            foreach (var location in result.Data)
            {
                var vehiclesResult = await vehicleService.GetByLocationId(location.Id);
                if (vehiclesResult.ResultCode == ResultCode.Success)
                {
                    location.Vehicles = vehiclesResult.Data.ToList();
                }
            }

            return result;
        }

        public async Task<Result<Location>> Get(Guid id)
        {
            var command = CommandFactory<Location>.CreateGetCommand(context);
            var result = await command.Execute(id);

            if (result.ResultCode != ResultCode.Success)
            {
                return result;
            }

            var vehicleResult = await vehicleService.GetByLocationId(id);
            if (vehicleResult.ResultCode == ResultCode.Success)
            {
                result.Data.Vehicles = vehicleResult.Data.ToList();
            }

            return result;
        }

        public async Task<Result<Guid>> Insert(Location location)
        {
            if (! await validationService.IsValidInsert(location))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = CommandFactory<Location>.CreateInsertCommand(context);
            return await command.Execute(location);
        }

        public async Task<ResultCode> Delete(Guid id)
        {
            var getCommand = CommandFactory<Location>.CreateGetCommand(context);
            var getResult = await getCommand.Execute(id);

            if (getResult.ResultCode == ResultCode.NotFound)
            {
                return ResultCode.NotFound;
            }

            if (! await validationService.IsValidDelete(getResult.Data))
            {
                return ResultCode.BadRequest;
            }

            var command = CommandFactory<Location>.CreateDeleteCommand(context);
            return await command.Execute(getResult.Data);
        }

        public async Task<Result<Guid>> Update(Location location)
        {
            if (! await validationService.IsValidUpdate(location))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = CommandFactory<Location>.CreateUpdateCommand(context);
            return await command.Execute(location);
        }

        public async Task<ResultCode> AddVehicle(Guid locationId, Vehicle vehicle)
        {
            var locationResult = await Get(locationId);
            if (locationResult.ResultCode == ResultCode.NotFound)
            {
                return ResultCode.NotFound;
            }

            if (locationResult.Data.Vehicles.Contains(vehicle))
            {
                return ResultCode.Conflict;
            }

            try
            {
                locationResult.Data.Vehicles.Add(vehicle);
            }
            catch
            {
                return ResultCode.Failed;
            }

            await context.SaveChangesAsync();
            return ResultCode.Success;
        }

        public async Task<Result<IEnumerable<Vehicle>>> GetVehicles(Guid locationId)
        {
            return await vehicleService.GetByLocationId(locationId);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                    disposed = true;
                }
            }
        }
    }
}