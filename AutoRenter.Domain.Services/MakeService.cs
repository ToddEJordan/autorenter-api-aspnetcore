using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoRenter.Api.Data;
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
                return new Result<Make>(ResultCode.Success, make);
            }

            return new Result<Make>(ResultCode.NotFound);
        }

        public async Task<Result<IEnumerable<Make>>> GetAll()
        {
            var command = commandFactory.CreateGetAllCommand(context);
            return await command.Execute();
        }

        public async Task<Result<Guid>> Insert(Make make)
        {
            if (!await validationService.IsValidInsert(make))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = commandFactory.CreateInsertCommand(context);
            return await command.Execute(make);
        }

        public async Task<Result<Guid>> Update(Make make)
        {
            if (!await validationService.IsValidUpdate(make))
            {
                return new Result<Guid>(ResultCode.BadRequest);
            }

            var command = commandFactory.CreateUpdateCommand(context);
            return await command.Execute(make);
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
