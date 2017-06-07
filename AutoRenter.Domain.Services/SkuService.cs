using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Data;

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

        public async Task<Result<IEnumerable<Sku>>> GetAll()
        {
            var command = commandFactory.CreateGetAllCommand(context);
            var result = await command.Execute();

            if (result.ResultCode != ResultCode.Success)
            {
                return result;
            }

            foreach (var sku in result.Data)
            {
                var makeResult = await makeService.Get(sku.MakeId);
                sku.Make = makeResult.Data;

                var modelResult = await modelService.Get(sku.ModelId);
                sku.Model = modelResult.Data;
            }

            return result;
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
