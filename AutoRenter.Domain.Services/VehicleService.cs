using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoRenter.Api.Data;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Interfaces;

namespace AutoRenter.Domain.Services
{
    public class VehicleService : IVehicleService, IDomainService, IDisposable
    {
        private bool disposed = false;
        private readonly AutoRenterContext context;
        private readonly ICommandFactory<Vehicle> commandFactory;
        private readonly ICommandFactory<Location> locationCommandFactory;
        private readonly IValidationService validationService;
        private readonly IMakeService makeService;
        private readonly IModelService modelService;

        public VehicleService(AutoRenterContext context, 
            ICommandFactory<Vehicle> commandFactory,
            ICommandFactory<Location> locationCommandFactory,
            IValidationService validationService, 
            IMakeService makeService,
            IModelService modelService)
        {
            this.context = context;
            this.commandFactory = commandFactory;
            this.locationCommandFactory = locationCommandFactory;
            this.validationService = validationService;
            this.makeService = makeService;
            this.modelService = modelService;
        }

        public async Task<ResultCode> Delete(Guid id)
        {
            var getCommand = commandFactory.CreateGetCommand(context);
            var getResult = await getCommand.Execute(id);

            if (getResult.ResultCode == ResultCode.NotFound)
            {
                return ResultCode.NotFound;
            }

            if (!await validationService.IsValidDelete(getResult.Data))
            {
                return ResultCode.BadRequest;
            }

            var command = commandFactory.CreateDeleteCommand(context);
            return await command.Execute(getResult.Data);
        }

        public async Task<Result<Vehicle>> Get(Guid id)
        {
            var command = commandFactory.CreateGetCommand(context);
            var result = await command.Execute(id);

            if (result.ResultCode != ResultCode.Success)
            {
                return result;
            }

            var makeResult = await makeService.Get(result.Data.MakeId);
            result.Data.Make = makeResult.Data;

            var modelResult = await modelService.Get(result.Data.ModelId);
            result.Data.Model = modelResult.Data;

            return result;
        }

        public async Task<Result<IEnumerable<Vehicle>>> GetByLocationId(Guid locationId)
        {
            var command = locationCommandFactory.CreateGetCommand(context);
            var locationResult = await command.Execute(locationId);

            if (locationResult.ResultCode != ResultCode.Success
                || locationResult.Data == null)
            {
                return new Result<IEnumerable<Vehicle>>(ResultCode.NotFound);
            }

            var vehicles = context.Vehicles.Where(x => x.LocationId == locationId);
            if (vehicles == null || !vehicles.Any())
            {
                return new Result<IEnumerable<Vehicle>>(ResultCode.NotFound);
            }

            return new Result<IEnumerable<Vehicle>>(ResultCode.Success, vehicles.OrderBy(x => x.Vin).ToList());
        }

        public async Task<Result<IEnumerable<Vehicle>>> GetAll()
        {
            var command = commandFactory.CreateGetAllCommand(context);
            var result = await command.Execute();

            if (result.ResultCode != ResultCode.Success)
            {
                return result;
            }

            foreach (var vehicle in result.Data)
            {
                var makeResult = await makeService.Get(vehicle.MakeId);
                vehicle.Make = makeResult.Data;

                var modelResult = await modelService.Get(vehicle.ModelId);
                vehicle.Model = modelResult.Data;
            }
            return result;
        }

        public async Task<Result<Guid>> Insert(Vehicle vehicle)
        {
            if (! await validationService.IsValidInsert(vehicle))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = commandFactory.CreateInsertCommand(context);
            return await command.Execute(vehicle);
        }

        public async  Task<Result<Guid>> Update(Vehicle vehicle)
        {
            if (! await validationService.IsValidUpdate(vehicle))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = commandFactory.CreateUpdateCommand(context);
            return await command.Execute(vehicle);
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
