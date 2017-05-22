using AutoRenter.Domain.Models;
using FluentValidation;

namespace AutoRenter.Api.Validation
{
    public class SkuUpdateValidator : AbstractValidator<Sku>, IValidator
    {
        public SkuUpdateValidator()
        {
            RuleFor(m => m.Id).NotNull();
            RuleFor(m => m.MakeId).NotNull();
            RuleFor(m => m.ModelId).NotNull();
            RuleFor(m => m.Year).NotNull();
            RuleFor(m => m.Color).NotNull();
        }
    }
}
