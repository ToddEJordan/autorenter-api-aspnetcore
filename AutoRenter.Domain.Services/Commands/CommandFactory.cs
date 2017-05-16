using AutoRenter.Api.Data;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Services.Commands;
using AutoRenter.Domain.Services.Commands.Interfaces;

namespace AutoRenter.Domain.Services.Commands
{
    public static class CommandFactory<T>
        where T : class, IEntity
    {
        public static IGetCommand<T> CreateGetCommand(AutoRenterContext context)
        {
            return new Get<T>(context);
        }

        public static IGetAllCommand<T> CreateGetAllCommand(AutoRenterContext context)
        {
            return new GetAll<T>(context);
        }

        public static IInsertCommand<T> CreateInsertCommand(AutoRenterContext context)
        {
            return new Insert<T>(context);
        }

        public static IUpdateCommand<T> CreateUpdateCommand(AutoRenterContext context)
        {
            return new Update<T>(context);
        }

        public static IDeleteCommand<T> CreateDeleteCommand(AutoRenterContext context)
        {
            return new Delete<T>(context);
        }
    }
}
