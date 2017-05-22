using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Api.Data;

namespace AutoRenter.Domain.Services
{
    public class SkuService : ISkuService, IDomainService, IDisposable
    {
        private bool disposed = false;
        private readonly AutoRenterContext context;
        private readonly ICommandFactory<Sku> commandFactory;
        private readonly IValidationService validationService;
        private readonly IMakeService makeService;
        private readonly IModelService modelService;

        public SkuService(AutoRenterContext context, 
            ICommandFactory<Sku> commandFactory,
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

        public async Task<Result<Sku>> Get(Guid id)
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

        public async Task<Result<IEnumerable<Sku>>> GetAll()
        {
            var command = commandFactory.CreateGetAllCommand(context);
            var result = await command.Execute();

            foreach (var sku in result.Data)
            {
                var makeResult = await makeService.Get(sku.MakeId);
                sku.Make = makeResult.Data;

                var modelResult = await modelService.Get(sku.ModelId);
                sku.Model = modelResult.Data;
            }

            return result;
        }

        public async Task<Result<Guid>> Insert(Sku sku)
        {
            if (!await validationService.IsValidInsert(sku))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = commandFactory.CreateInsertCommand(context);
            return await command.Execute(sku);
        }

        public async Task<Result<Guid>> Update(Sku sku)
        {
            if (!await validationService.IsValidUpdate(sku))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = commandFactory.CreateUpdateCommand(context);
            return await command.Execute(sku);
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
