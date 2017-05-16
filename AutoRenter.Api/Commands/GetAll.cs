using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoRenter.Api.Data;
using AutoRenter.Api.Domain;
using AutoRenter.Api.Commands.Interfaces;

namespace AutoRenter.Api.Commands
{
    public class GetAll<T> : IGetAllCommand<T>
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
                return new Result<IEnumerable<T>>(ResultCode.NotFound);
            }

            return new Result<IEnumerable<T>>(ResultCode.Success, all);
        }
    }
}
