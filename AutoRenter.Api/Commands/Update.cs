using AutoRenter.Api.Data;
using AutoRenter.Api.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRenter.Api.Commands
{
    public class Update<T>
        where T : class, IEntity
    {
        private readonly AutoRenterContext context;
        public Update(AutoRenterContext context)
        {
            this.context = context;
        }

        public async Task<Result<Guid>> Execute(T entity)
        {
            var existingResult = await context.FindAsync<T>(entity.Id);
            if (existingResult == null)
            {
                return new Result<Guid>(ResultCode.NotFound);
            }

            var updateResult = context.Update(entity);
            if (updateResult.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
            {
                return new Result<Guid>(ResultCode.Success, updateResult.Entity.Id);
            }

            return new Result<Guid>(ResultCode.Failed);
        }
    }
}
