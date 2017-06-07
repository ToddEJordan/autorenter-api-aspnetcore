using System;
using System.Threading.Tasks;
using AutoRenter.Domain.Data;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Interfaces;

namespace AutoRenter.Domain.Services.Commands
{
    internal class Update<T> : IUpdateCommand<T>
        where T : class, IEntity
    {
        private readonly AutoRenterContext context;
        public Update(AutoRenterContext context)
        {
            this.context = context;
        }

        public async Task<Result<Guid>> Execute(T entity)
        {
            var existingEntity = await context.FindAsync<T>(entity.Id);
            if (existingEntity == null)
            {
                return new Result<Guid>(ResultCode.NotFound);
            }

            try
            {
                context.Entry(existingEntity).CurrentValues.SetValues(entity);
            }
            catch
            {
                return new Result<Guid>(ResultCode.Failed);
            }

            await context.SaveChangesAsync();
            return new Result<Guid>(ResultCode.Success, entity.Id);
        }
    }
}
