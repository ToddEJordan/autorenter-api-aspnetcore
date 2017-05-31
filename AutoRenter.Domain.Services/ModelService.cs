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
