using AutoRenter.Api.Data;
using AutoRenter.Api.Domain;

namespace AutoRenter.Api.Commands
{
    public static class CommandFactory<T>
        where T : class, IEntity
    {
        public static Get<T> CreateGetCommand(AutoRenterContext context)
        {
            return new Get<T>(context);
        }

        public static GetAll<T> CreateGetAllCommand(AutoRenterContext context)
        {
            return new GetAll<T>(context);
        }

        public static Insert<T> CreateInsertCommand(AutoRenterContext context)
        {
            return new Insert<T>(context);
        }

        public static Update<T> CreateUpdateCommand(AutoRenterContext context)
        {
            return new Update<T>(context);
        }

        public static Delete<T> CreateDeleteCommand(AutoRenterContext context)
        {
            return new Delete<T>(context);
        }
    }
}
