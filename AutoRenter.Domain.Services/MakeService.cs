using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoRenter.Domain.Data;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Interfaces;

namespace AutoRenter.Domain.Services
{
    public class MakeService : IMakeService, IDomainService, IDisposable
    {
        private bool disposed = false;
        private readonly AutoRenterContext context;
        private readonly ICommandFactory<Make> commandFactory;
        private readonly IValidationService validationService;
        public MakeService(AutoRenterContext context, 
            ICommandFactory<Make> commandFactory, 
            IValidationService validationService)
        {
            this.context = context;
            this.commandFactory = commandFactory;
            this.validationService = validationService;
        }

        public async Task<Result<Make>> Get(Guid id)
        {
            var command = commandFactory.CreateGetCommand(context);
            return await command.Execute(id);
        }

        public async Task<Result<Make>> Get(string id)
        {
            var make = context.Makes.FirstOrDefault(x => x.ExternalId == id);
            if (make != null)
            {
                return await Task.FromResult(new Result<Make>(ResultCode.Success, make));
            }

            return await Task.FromResult(new Result<Make>(ResultCode.NotFound));
        }

        public async Task<Result<IEnumerable<Make>>> GetAll()
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
