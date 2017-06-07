using System.Threading.Tasks;
using FluentValidation;
using AutoRenter.Domain.Interfaces;
using IValidatorFactory = AutoRenter.Domain.Validation.IValidatorFactory;

namespace AutoRenter.Domain.Services
{
    public class ValidationService : IValidationService, IDomainService
    {
        private readonly IValidatorFactory validatorFactory;

        public ValidationService(IValidatorFactory validatorFactory)
        {
            this.validatorFactory = validatorFactory;
        }

        public async Task<bool> IsValidInsert<T>(T entity)
        {
            var validator = validatorFactory.GetInsertInstance<T>();
            return await IsValid(validator, entity);
        }

        public async Task<bool> IsValidUpdate<T>(T entity)
        {
            var validator = validatorFactory.GetUpdateInstance<T>();
            return await IsValid(validator, entity);
        }

        public async Task<bool> IsValidDelete<T>(T entity)
        {
            var validator = validatorFactory.GetDeleteInstance<T>();
            return await IsValid(validator, entity);
        }

        private async Task<bool> IsValid<T>(IValidator validator, T entity)
        {
            var result = await validator.ValidateAsync(entity);
            return result.IsValid;
        }
    }
}
