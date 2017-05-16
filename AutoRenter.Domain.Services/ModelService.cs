using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoRenter.Domain.Services.Commands;
using AutoRenter.Api.Data;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Interfaces;

namespace AutoRenter.Domain.Services
{
    public class ModelService : IModelService, IDomainService, IDisposable
    {
        private bool disposed = false;
        private readonly AutoRenterContext context;
        private readonly IValidationService validationService;
        public ModelService(AutoRenterContext context, IValidationService validationService)
        {
            this.context = context;
            this.validationService = validationService;
        }

        public async Task<ResultCode> Delete(Guid id)
        {
            var getCommand = CommandFactory<Model>.CreateGetCommand(context);
            var getResult = await getCommand.Execute(id);

            if (getResult.ResultCode == ResultCode.NotFound)
            {
                return ResultCode.NotFound;
            }

            if (!await validationService.IsValidDelete(getResult.Data))
            {
                return ResultCode.BadRequest;
            }

            var command = CommandFactory<Model>.CreateDeleteCommand(context);
            return await command.Execute(getResult.Data);
        }

        public async Task<Result<Model>> Get(Guid id)
        {
            var command = CommandFactory<Model>.CreateGetCommand(context);
            return await command.Execute(id);
        }

        public async Task<Result<Model>> Get(string id)
        {
            var Model = context.Models.FirstOrDefault(x => x.ExternalId == id);
            if (Model != null)
            {
                return new Result<Model>(ResultCode.Success, Model);
            }

            return new Result<Model>(ResultCode.NotFound);
        }

        public async Task<Result<IEnumerable<Model>>> GetAll()
        {
            var command = CommandFactory<Model>.CreateGetAllCommand(context);
            return await command.Execute();
        }

        public async Task<Result<Guid>> Insert(Model Model)
        {
            if (!await validationService.IsValidInsert(Model))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = CommandFactory<Model>.CreateInsertCommand(context);
            return await command.Execute(Model);
        }

        public async Task<Result<Guid>> Update(Model Model)
        {
            if (!await validationService.IsValidUpdate(Model))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = CommandFactory<Model>.CreateUpdateCommand(context);
            return await command.Execute(Model);
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
