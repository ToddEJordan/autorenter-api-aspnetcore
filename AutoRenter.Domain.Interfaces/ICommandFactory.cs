using AutoRenter.Domain.Data;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Interfaces
{
    public interface ICommandFactory<T>
        where T : class, IEntity
    {
        IGetCommand<T> CreateGetCommand(AutoRenterContext context);
        IGetAllCommand<T> CreateGetAllCommand(AutoRenterContext context);
        IInsertCommand<T> CreateInsertCommand(AutoRenterContext context);
        IUpdateCommand<T> CreateUpdateCommand(AutoRenterContext context);
        IDeleteCommand<T> CreateDeleteCommand(AutoRenterContext context);
    }
}   
