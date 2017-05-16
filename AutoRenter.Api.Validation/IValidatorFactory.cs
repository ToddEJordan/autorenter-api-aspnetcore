using System;
using FluentValidation;

namespace AutoRenter.Api.Validation
{
    public interface IValidatorFactory
    {
        IValidator<T> GetValidator<T>();
        IValidator GetValidator(Type type);
    }
}
