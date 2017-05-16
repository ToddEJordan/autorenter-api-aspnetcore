using AutoRenter.Api.Data;
using AutoRenter.Api.Domain;
using System;
using System.Threading.Tasks;
using AutoRenter.Api.Commands.Interfaces;

namespace AutoRenter.Api.Commands
{
    public class Get<T> : IGetCommand<T>
        where T : class, IEntity
    {
        private readonly AutoRenterContext context;
        public Get(AutoRenterContext context)
        {
            this.context = context;
        }

        public async Task<Result<T>> Execute(Guid id)
        {
            var entity = await context.FindAsync<T>(id);

            if (entity == null)
            {
                return new Result<T>(ResultCode.NotFound);
            }

            return new Result<T>(ResultCode.Success, entity);
        }
    }
}
