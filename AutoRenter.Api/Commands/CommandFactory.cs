using AutoRenter.Api.Data;
using AutoRenter.Api.Domain;
using AutoRenter.Api.Commands.Interfaces;

namespace AutoRenter.Api.Commands
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
