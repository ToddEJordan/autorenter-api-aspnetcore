using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Api.Data;
using AutoRenter.Api.Domain;
using AutoRenter.Api.DomainInterfaces;
using AutoRenter.Api.Commands;

namespace AutoRenter.Api.DomainServices
{
    public class VehicleService : IVehicleService, IDomainService
    {
        private readonly AutoRenterContext context;
        private readonly IValidationService validationService;

        public VehicleService(AutoRenterContext context, IValidationService validationService)
        {
            this.context = context;
            this.validationService = validationService;
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
            return await command.Execute(id);
        }

        public Result<IEnumerable<Vehicle>> GetAll()
        {
            var command = CommandFactory<Vehicle>.CreateGetAllCommand(context);
            return command.Execute();
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
    }
}
