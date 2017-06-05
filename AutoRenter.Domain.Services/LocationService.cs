using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoRenter.Domain.Data;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Services
{
    public class LocationService : ILocationService, IDomainService, IDisposable
    {
        private bool disposed = false;
        private readonly AutoRenterContext context;
        private readonly ICommandFactory<Location> commandFactory;
        private readonly IVehicleService vehicleService;
        private readonly IValidationService validationService;
        public LocationService(AutoRenterContext context, 
            ICommandFactory<Location> commandFactory, 
            IVehicleService vehicleService, 
            IValidationService validationService)
        {
            this.context = context;
            this.commandFactory = commandFactory;
            this.vehicleService = vehicleService;
            this.validationService = validationService;
        }

        public async Task<Result<IEnumerable<Location>>> GetAll()
        {
            var command = commandFactory.CreateGetAllCommand(context);
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
            var command = commandFactory.CreateGetCommand(context);
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

            var command = commandFactory.CreateInsertCommand(context);
            return await command.Execute(location);
        }

        public async Task<ResultCode> Delete(Guid id)
        {
            var getCommand = commandFactory.CreateGetCommand(context);
            var getResult = await getCommand.Execute(id);

            if (getResult.ResultCode == ResultCode.NotFound)
            {
                return ResultCode.NotFound;
            }

            if (! await validationService.IsValidDelete(getResult.Data))
            {
                return ResultCode.BadRequest;
            }

            var command = commandFactory.CreateDeleteCommand(context);
            return await command.Execute(getResult.Data);
        }

        public async Task<Result<Guid>> Update(Location location)
        {
            if (! await validationService.IsValidUpdate(location))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = commandFactory.CreateUpdateCommand(context);
            return await command.Execute(location);
        }

        public async Task<Result<Guid>> AddVehicle(Guid locationId, Vehicle vehicle)
        {
            var locationResult = await Get(locationId);
            if (locationResult.ResultCode == ResultCode.NotFound)
            {
                return new Result<Guid>(ResultCode.NotFound);
            }

            var vehiclesResult = await vehicleService.GetByLocationId(locationId);
            if (vehiclesResult.Data != null
                && vehiclesResult.Data.Any(x => x.Id == vehicle.Id))
            {
                return new Result<Guid>(ResultCode.Conflict);
            }

            var insertResult = await vehicleService.Insert(vehicle);
            if (insertResult.ResultCode != ResultCode.Success)
            {
                return insertResult;
            }

            var addedVehicle = await vehicleService.Get(insertResult.Data);
            if (addedVehicle.ResultCode != ResultCode.Success)
            {
                return new Result<Guid>(ResultCode.Failed);
            }

            var existingVehicles = vehiclesResult.Data == null
                ? new List<Vehicle>()
                : vehiclesResult.Data.ToList();
                
            existingVehicles.Add(addedVehicle.Data);
            locationResult.Data.Vehicles = existingVehicles;
            await context.SaveChangesAsync();

            return new Result<Guid>(ResultCode.Success, addedVehicle.Data.Id);
        }

        public async Task<Result<IEnumerable<Vehicle>>> GetVehicles(Guid locationId)
        {
            var locationResult = await Get(locationId);

            if (locationResult.ResultCode != ResultCode.Success
                || locationResult.Data == null)
            {
                return new Result<IEnumerable<Vehicle>>(ResultCode.NotFound);
            }

            return await vehicleService.GetByLocationId(locationId);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
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