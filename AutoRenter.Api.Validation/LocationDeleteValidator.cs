using FluentValidation;
using AutoRenter.Domain.Models;

namespace AutoRenter.Domain.Validation
{
    public class LocationDeleteValidator : AbstractValidator<Location>, IValidator<Location>
    {
        public LocationDeleteValidator()
        {
            RuleFor(m => m.Id).NotNull();
        }
    }
}
