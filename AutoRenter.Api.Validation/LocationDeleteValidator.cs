using AutoRenter.Domain.Models;
using FluentValidation;

namespace AutoRenter.Api.Validation
{
    public class LocationDeleteValidator : AbstractValidator<Location>, IValidator<Location>
    {
        public LocationDeleteValidator()
        {
            RuleFor(m => m.Id).NotNull();
        }
    }
}
