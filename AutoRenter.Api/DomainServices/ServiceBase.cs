using AutoRenter.Api.Data;
using AutoRenter.Api.Domain;
using AutoRenter.Api.DomainInterfaces;
using AutoRenter.Api.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRenter.Api.DomainServices
{
    public abstract class ServiceBase<T> : IServiceBase<T>
        where T: class, IEntity
    {
        internal readonly AutoRenterContext context;
        public ServiceBase(AutoRenterContext context)
        {
            this.context = context;
        }

        public Result<IEnumerable<T>> GetAll()
        {
            var all = context.Set<T>();

            if (all == null || !all.Any())
            {
                return new Result<IEnumerable<T>>()
                {
                    Data = null,
                    ResultCode = ResultCode.NotFound
                };
            }

            return new Result<IEnumerable<T>>()
            {
                Data = all,
                ResultCode = ResultCode.Success
            };
        }

        public async Task<Result<T>> Get(Guid id)
        {
            var entity = await context.FindAsync<T>(id);

            if (entity == null)
            {
                return new Result<T>()
                {
                    Data = null,
                    ResultCode = ResultCode.NotFound
                };
            }

            return new Result<T>
            {
                Data = entity,
                ResultCode = ResultCode.Success
            };
        }

        public async Task<ResultCode> Delete(Guid id)
        {
            var getResult = await Get(id);
            if (getResult.ResultCode == ResultCode.NotFound)
            {
                return ResultCode.NotFound;
            }

            var deleteResult = context.Remove(getResult.Data);

            if (deleteResult.State == Microsoft.EntityFrameworkCore.EntityState.Deleted)
            {
                return ResultCode.Success;
            }
            else
            {
                return ResultCode.Failed;
            }
        }

        public async Task<Result<Guid>> Insert(T entity)
        {
            if (!await ValidateInsert(entity))
            {
                return new Result<Guid>()
                {
                    Data = Guid.Empty,
                    ResultCode = ResultCode.BadRequest
                };
            }

            var existingResult = await Get(entity.Id);
            if (existingResult.ResultCode == ResultCode.Success)
            {
                return new Result<Guid>()
                {
                    Data = Guid.Empty,
                    ResultCode = ResultCode.BadRequest
                };
            }

            var insertResult = await context.AddAsync(entity);
            if (insertResult.State == Microsoft.EntityFrameworkCore.EntityState.Added)
            {
                return new Result<Guid>()
                {
                    Data = insertResult.Entity.Id,
                    ResultCode = ResultCode.Success
                };
            }

            return new Result<Guid>()
            {
                Data = Guid.Empty,
                ResultCode = ResultCode.Failed
            };
        }

        public async Task<Result<Guid>> Update(T entity)
        {
            if (!await ValidateUpdate(entity))
            {
                return new Result<Guid>()
                {
                    Data = Guid.Empty,
                    ResultCode = ResultCode.BadRequest
                };
            }
            var existingResult = await Get(entity.Id);
            if (existingResult.ResultCode == ResultCode.NotFound)
            {
                return new Result<Guid>()
                {
                    Data = Guid.Empty,
                    ResultCode = ResultCode.NotFound
                };
            }

            var updateResult = context.Update(entity);
            if (updateResult.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
            {
                return new Result<Guid>()
                {
                    Data = updateResult.Entity.Id,
                    ResultCode = ResultCode.Success,
                };
            }

            return new Result<Guid>()
            {
                Data = Guid.Empty,
                ResultCode = ResultCode.Failed
            };
        }

        public async Task<Result<Guid>> Upsert(T entity)
        {
            var existingResult = await Get(entity.Id);
            if (existingResult.ResultCode == ResultCode.Success)
            {
                return await Update(entity);
            }

            return await Insert(entity);
        }

        private async Task<bool> ValidateInsert(T entity)
        {
            var validator = ValidatorFactory.GetInsertInstance<T>();
            var validationResult = await validator.ValidateAsync(entity);
            if (validationResult.IsValid)
            {
                return true;
            }

            return false;
        }

        private async Task<bool> ValidateUpdate(T entity)
        {
            var validator = ValidatorFactory.GetUpdateInstance<T>();
            var validationResult = await validator.ValidateAsync(entity);
            if (validationResult.IsValid)
            {
                return true;
            }

            return false;
        }
    }
}
