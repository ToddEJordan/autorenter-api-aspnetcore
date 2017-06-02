using System;
using System.Threading.Tasks;
using AutoRenter.Domain.Data;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Interfaces;

namespace AutoRenter.Domain.Services.Commands
{
    internal class Get<T> : IGetCommand<T>
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
