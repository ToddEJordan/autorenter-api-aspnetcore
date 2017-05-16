﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Api.Domain;
using AutoRenter.Api.DomainInterfaces;
using AutoRenter.Api.Data;
using AutoRenter.Api.Commands;

namespace AutoRenter.Api.DomainServices
{
    public class SkuService : ISkuService, IDomainService, IDisposable
    {
        private bool disposed = false;
        private readonly AutoRenterContext context;
        private readonly IValidationService validationService;
        private readonly IMakeService makeService;
        private readonly IModelService modelService;

        public SkuService(AutoRenterContext context, 
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
            var command = CommandFactory<Sku>.CreateGetAllCommand(context);
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
