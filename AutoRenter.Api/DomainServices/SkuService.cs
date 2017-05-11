using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Api.Domain;
using AutoRenter.Api.DomainInterfaces;
using AutoRenter.Api.Data;
using AutoRenter.Api.Commands;

namespace AutoRenter.Api.DomainServices
{
    public class SkuService : ISkuService, IDomainService
    {
        private readonly AutoRenterContext context;
        private readonly IValidationService validationService;
        public SkuService(AutoRenterContext context, IValidationService validationService)
        {
            this.context = context;
            this.validationService = validationService;
        }

        public async Task<ResultCode> Delete(Guid id)
        {
            var getCommand = CommandFactory<Sku>.CreateGetCommand(context);
            var getResult = await getCommand.Execute(id);

            if (getResult.ResultCode == ResultCode.NotFound)
            {
                return ResultCode.NotFound;
            }

            if (!await validationService.IsValidDelete(getResult.Data))
            {
                return ResultCode.BadRequest;
            }

            var command = CommandFactory<Sku>.CreateDeleteCommand(context);
            return await command.Execute(getResult.Data);
        }

        public async Task<Result<Sku>> Get(Guid id)
        {
            var command = CommandFactory<Sku>.CreateGetCommand(context);
            return await command.Execute(id);
        }

        public Result<IEnumerable<Sku>> GetAll()
        {
            var command = CommandFactory<Sku>.CreateGetAllCommand(context);
            return command.Execute();
        }

        public async Task<Result<Guid>> Insert(Sku sku)
        {
            if (!await validationService.IsValidInsert(sku))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = CommandFactory<Sku>.CreateInsertCommand(context);
            return await command.Execute(sku);
        }

        public async Task<Result<Guid>> Update(Sku sku)
        {
            if (!await validationService.IsValidUpdate(sku))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = CommandFactory<Sku>.CreateUpdateCommand(context);
            return await command.Execute(sku);
        }
    }
}
