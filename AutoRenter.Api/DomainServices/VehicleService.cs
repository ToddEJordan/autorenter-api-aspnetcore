using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Api.Data;
using AutoRenter.Api.Domain;
using AutoRenter.Api.DomainInterfaces;
using AutoRenter.Api.Commands;

namespace AutoRenter.Api.DomainServices
{
    public class VehicleService : IVehicleService, IDomainService, IDisposable
    {
        private bool disposed = false;
        private readonly AutoRenterContext context;
        private readonly IValidationService validationService;
        private readonly IMakeService makeService;
        private readonly IModelService modelService;

        public VehicleService(AutoRenterContext context, 
            IValidationService validationService, 
            IMakeService makeService,
            IModelService modelService)
        {
            this.context = context;
            this.validationService = validationService;
            this.makeService = makeService;
            this.modelService = modelService;
        }

        public async Task<ResultCode> Delete(Guid id)
        {
            var getCommand = CommandFactory<Vehicle>.CreateGetCommand(context);
            var getResult = await getCommand.Execute(id);

            if (getResult.ResultCode == ResultCode.NotFound)
            {
                return ResultCode.NotFound;
            }

            if (!await validationService.IsValidDelete(getResult.Data))
            {
                return ResultCode.BadRequest;
            }

            var command = CommandFactory<Vehicle>.CreateDeleteCommand(context);
            return await command.Execute(getResult.Data);
        }

        public async Task<Result<Vehicle>> Get(Guid id)
        {
            var command = CommandFactory<Vehicle>.CreateGetCommand(context);
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

        public async Task<Result<IEnumerable<Vehicle>>> GetAll()
        {
            var command = CommandFactory<Vehicle>.CreateGetAllCommand(context);
            var result = await command.Execute();

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

            var command = CommandFactory<Vehicle>.CreateInsertCommand(context);
            return await command.Execute(vehicle);
        }

        public async  Task<Result<Guid>> Update(Vehicle vehicle)
        {
            if (! await validationService.IsValidUpdate(vehicle))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = CommandFactory<Vehicle>.CreateUpdateCommand(context);
            return await command.Execute(vehicle);
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
