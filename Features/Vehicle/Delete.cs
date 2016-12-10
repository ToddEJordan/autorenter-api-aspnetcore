using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.API.Data;
using FluentValidation;
using MediatR;

namespace AutoRenter.API.Features.Vehicle
{
    public class Delete
    {
        public class Query : IAsyncRequest<Command>
        {
            public Guid? Id { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public class Command : IAsyncRequest
        {
            public Guid? Id { get; set; }
            public string Vin { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
            public int Year { get; set; }
            public int Miles { get; set; }
            public string Color { get; set; }
            public bool IsRentToOwn { get; set; }
            public Guid LocationId { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Command>
        {
            private readonly IVehicleRepository _vehicleRepository;

            public QueryHandler(IVehicleRepository vehicleRepository)
            {
                _vehicleRepository = vehicleRepository;
            }

            public async Task<Command> Handle(Query message)
            {
                var vehicle =
                    await Task.Run(() => _vehicleRepository.GetSingle(s => s.Id.Equals(message.Id), s => s.Location));
                var viewModel = Mapper.Map<Command>(vehicle);

                return viewModel;
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly IVehicleRepository _vehicleRepository;

            public CommandHandler(IVehicleRepository vehicleRepository)
            {
                _vehicleRepository = vehicleRepository;
            }

            protected override async Task HandleCore(Command message)
            {
                var vehicle =
                    await Task.Run(() => _vehicleRepository.GetSingle(s => s.Id.Equals(message.Id)));

                _vehicleRepository.Delete(vehicle);
                _vehicleRepository.Commit();
            }
        }
    }
}