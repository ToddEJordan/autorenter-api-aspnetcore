using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoRenter.Api.Data;
using AutoRenter.Domain.Models;
using AutoRenter.Domain.Interfaces;

namespace AutoRenter.Domain.Services.Commands
{
    internal class GetAll<T> : IGetAllCommand<T>
        where T : class, IEntity
    {
        private readonly AutoRenterContext context;
        public GetAll(AutoRenterContext context)
        {
            this.context = context;
        }

        public async Task<Result<IEnumerable<T>>> Execute()
        {
            var all = context.Set<T>();

            if (all == null || !all.Any())
            {
                return await Task.FromResult(new Result<IEnumerable<T>>(ResultCode.NotFound));
            }

            return await Task.FromResult(new Result<IEnumerable<T>>(ResultCode.Success, all));
        }
    }
}
