using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoRenter.Api.Commands;
using AutoRenter.Api.Data;
using AutoRenter.Api.Domain;
using AutoRenter.Api.DomainInterfaces;

namespace AutoRenter.Api.DomainServices
{
    public class LocationService : ILocationService, IDomainService
    {
        private readonly AutoRenterContext context;
        private readonly IVehicleService vehicleService;
        private readonly IValidationService validationService;
        public LocationService(AutoRenterContext context, IVehicleService vehicleService, IValidationService validationService)
        {
            this.context = context;
            this.vehicleService = vehicleService;
            this.validationService = validationService;
        }

        public Result<IEnumerable<Location>> GetAll()
        {
            var command = CommandFactory<Location>.CreateGetAllCommand(context);
            return command.Execute();
        }

        public async Task<Result<Location>> Get(Guid id)
        {
            var command = CommandFactory<Location>.CreateGetCommand(context);
            return await command.Execute(id);
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
            var command = CommandFactory<Location>.CreateGetCommand(context);
            var locationResult = await command.Execute(locationId);

            if (locationResult.ResultCode != ResultCode.Success
                || locationResult.Data == null || !locationResult.Data.Vehicles.Any())
            {
                return new Result<IEnumerable<Vehicle>>(ResultCode.NotFound);
            }

            return new Result<IEnumerable<Vehicle>>(ResultCode.Success, locationResult.Data.Vehicles);
        }
    }
}