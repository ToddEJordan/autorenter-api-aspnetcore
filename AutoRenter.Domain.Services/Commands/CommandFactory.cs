using AutoRenter.Api.Data;
using AutoRenter.Domain.Interfaces;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Services.Commands
{
    public class CommandFactory<T> : ICommandFactory<T>
        where T : class, IEntity
    {
        public IGetCommand<T> CreateGetCommand(AutoRenterContext context)
        {
            return new Get<T>(context);
        }

        public IGetAllCommand<T> CreateGetAllCommand(AutoRenterContext context)
        {
            return new GetAll<T>(context);
        }

        public IInsertCommand<T> CreateInsertCommand(AutoRenterContext context)
        {
            return new Insert<T>(context);
        }

        public IUpdateCommand<T> CreateUpdateCommand(AutoRenterContext context)
        {
            return new Update<T>(context);
        }

        public IDeleteCommand<T> CreateDeleteCommand(AutoRenterContext context)
        {
            return new Delete<T>(context);
        }
    }
}
