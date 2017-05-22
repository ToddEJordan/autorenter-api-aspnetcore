using AutoRenter.Domain.Models;
using FluentValidation;

namespace AutoRenter.Api.Validation
{
    public class VehicleDeleteValidator : AbstractValidator<Vehicle>, IValidator<Vehicle>
    {
        public VehicleDeleteValidator()
        {
            RuleFor(m => m.Id).NotNull();
        }
    }
}
