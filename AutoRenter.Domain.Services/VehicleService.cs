using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoRenter.Domain.Data;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Interfaces;

namespace AutoRenter.Domain.Services
{
    public class VehicleService : IVehicleService, IDomainService, IDisposable
    {
        private bool disposed = false;
        private readonly AutoRenterContext context;
        private readonly ICommandFactory<Vehicle> commandFactory;
        private readonly IValidationService validationService;
        private readonly IMakeService makeService;
        private readonly IModelService modelService;

        public VehicleService(AutoRenterContext context, 
            ICommandFactory<Vehicle> commandFactory,
            IValidationService validationService, 
            IMakeService makeService,
            IModelService modelService)
        {
            this.context = context;
            this.commandFactory = commandFactory;
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

            await PopulateMake(result.Data);
            await PopulateModel(result.Data);

            return result;
        }

        public async Task<Result<IEnumerable<Vehicle>>> GetByLocationId(Guid locationId)
        {
            var vehicles = context.Vehicles.Where(x => x.LocationId == locationId);
            if (vehicles == null || !vehicles.Any())
            {
                return await Task.FromResult(new Result<IEnumerable<Vehicle>>(ResultCode.NotFound));
            }

            await PopulateMakes(vehicles);
            await PopulateModels(vehicles);

            return await Task.FromResult(new Result<IEnumerable<Vehicle>>(ResultCode.Success, vehicles.OrderBy(x => x.Vin)));
        }

        public async Task<Result<IEnumerable<Vehicle>>> GetAll()
        {
            var command = commandFactory.CreateGetAllCommand(context);
            var result = await command.Execute();

            if (result.ResultCode != ResultCode.Success)
            {
                return result;
            }

            await PopulateMakes(result.Data);
            await PopulateModels(result.Data);

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

        private async Task PopulateMakes(IEnumerable<Vehicle> vehicles)
        {
            foreach (var vehicle in vehicles)
            {
                await PopulateMake(vehicle);
            }
        }

        private async Task PopulateModels(IEnumerable<Vehicle> vehicles)
        {
            foreach (var vehicle in vehicles)
            {
                await PopulateModel(vehicle);
            }
        }

        private async Task PopulateMake(Vehicle vehicle)
        {
            var result = await makeService.Get(vehicle.MakeId);
            if (result.ResultCode == ResultCode.Success)
            {
                vehicle.Make = result.Data;
            }
        }

        private async Task PopulateModel(Vehicle vehicle)
        {
            var result = await modelService.Get(vehicle.ModelId);
            if (result.ResultCode == ResultCode.Success)
            {
                vehicle.Model = result.Data;
            }
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
