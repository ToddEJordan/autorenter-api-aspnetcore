using FluentValidation;

namespace AutoRenter.Domain.Validation
{
    public interface IValidatorFactory
    {
        IValidator GetInsertInstance<T>();
        IValidator GetUpdateInstance<T>();
        IValidator GetDeleteInstance<T>();
    }
}
