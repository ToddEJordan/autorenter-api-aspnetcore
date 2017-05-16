using AutoRenter.Domain.Models;
using FluentValidation;

namespace AutoRenter.Api.Validation
{
    public class VehicleInsertValidator : AbstractValidator<Vehicle>, IValidator<Vehicle>
    {
        public VehicleInsertValidator()
        {
            RuleFor(m => m.Vin).NotNull();
            RuleFor(m => m.MakeId).NotNull();
            RuleFor(m => m.ModelId).NotNull();
            RuleFor(m => m.Color).NotNull();
        }
    }

    public class VehicleUpdateValidator : AbstractValidator<Vehicle>, IValidator<Vehicle>
    {
        public VehicleUpdateValidator()
        {
            RuleFor(m => m.Id).NotNull();
            RuleFor(m => m.Vin).NotNull();
            RuleFor(m => m.MakeId).NotNull();
            RuleFor(m => m.ModelId).NotNull();
            RuleFor(m => m.Color).NotNull();
        }
    }

    public class VehicleDeleteValidator : AbstractValidator<Vehicle>, IValidator<Vehicle>
    {
        public VehicleDeleteValidator()
        {
            RuleFor(m => m.Id).NotNull();
        }
    }
}
