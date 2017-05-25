using AutoRenter.Domain.Models;
using FluentValidation;

namespace AutoRenter.Api.Validation
{
    public class LocationUpdateValidator : AbstractValidator<Location>, IValidator<Location>
    {
        public LocationUpdateValidator()
        {
            RuleFor(m => m.Id).NotNull();
            RuleFor(m => m.SiteId).NotNull();
            RuleFor(m => m.Name).NotNull();
        }
    }
}
