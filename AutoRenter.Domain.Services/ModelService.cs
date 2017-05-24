using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoRenter.Api.Data;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Interfaces;

namespace AutoRenter.Domain.Services
{
    public class ModelService : IModelService, IDomainService, IDisposable
    {
        private bool disposed = false;
        private readonly AutoRenterContext context;
        private readonly ICommandFactory<Model> commandFactory;
        private readonly IValidationService validationService;
        public ModelService(AutoRenterContext context, 
            ICommandFactory<Model> commandFactory, 
            IValidationService validationService)
        {
            this.context = context;
            this.commandFactory = commandFactory;
            this.validationService = validationService;
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

        public async Task<Result<Model>> Get(Guid id)
        {
            var command = commandFactory.CreateGetCommand(context);
            return await command.Execute(id);
        }

        public async Task<Result<Model>> Get(string id)
        {
            var Model = context.Models.FirstOrDefault(x => x.ExternalId == id);
            if (Model != null)
            {
                return await Task.FromResult(new Result<Model>(ResultCode.Success, Model));
            }

            return await Task.FromResult(new Result<Model>(ResultCode.NotFound));
        }

        public async Task<Result<IEnumerable<Model>>> GetAll()
        {
            var command = commandFactory.CreateGetAllCommand(context);
            return await command.Execute();
        }

        public async Task<Result<Guid>> Insert(Model Model)
        {
            if (!await validationService.IsValidInsert(Model))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = commandFactory.CreateInsertCommand(context);
            return await command.Execute(Model);
        }

        public async Task<Result<Guid>> Update(Model Model)
        {
            if (!await validationService.IsValidUpdate(Model))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = commandFactory.CreateUpdateCommand(context);
            return await command.Execute(Model);
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
