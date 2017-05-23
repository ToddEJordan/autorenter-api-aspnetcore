using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoRenter.Api.Data;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Services
{
    public class StateService : IStateService, IDomainService
    {
        private readonly AutoRenterContext context;
        private readonly ICommandFactory<State> commandFactory;

        public StateService(AutoRenterContext context, ICommandFactory<State> commandFactory)
        {
            this.context = context;
            this.commandFactory = commandFactory;
        }

        public async Task<Result<State>> Get(Guid id)
        {
            var command = commandFactory.CreateGetCommand(context);
            return await command.Execute(id);
        }

        public async Task<Result<IEnumerable<State>>> GetAll()
        {
            var command = commandFactory.CreateGetAllCommand(context);
            return await command.Execute();
        }
    }
}
