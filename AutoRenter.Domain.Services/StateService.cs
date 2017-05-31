using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.Api.Data;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Services
{
    public class StateService : IStateService, IDomainService, IDisposable
    {
        private bool disposed = false;
        private readonly AutoRenterContext context;
        private readonly ICommandFactory<State> commandFactory;

        public StateService(AutoRenterContext context, ICommandFactory<State> commandFactory)
        {
            this.context = context;
            this.commandFactory = commandFactory;
        }
        public async Task<Result<IEnumerable<State>>> GetAll()
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
