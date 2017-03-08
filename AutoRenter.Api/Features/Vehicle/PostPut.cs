using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.Api.Data;
using FluentValidation;
using MediatR;

namespace AutoRenter.Api.Features.Vehicle
{
    public class PostPut
    {
        public class Query : IAsyncRequest<Command>
        {
            public Guid? Id { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public class Command : IAsyncRequest
        {
            public Guid? Id { get; set; }
            public string Vin { get; set; }
            public string MakeId { get; set; }
            public string ModelId { get; set; }
            public int Year { get; set; }
            public int Miles { get; set; }
            public string Color { get; set; }
            public bool IsRentToOwn { get; set; }
            public string Image { get; set; }
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
                Command model;
                if (message.Id == null)
                {
                    model = new Command {Id = Guid.NewGuid()};
                }
                else
                {
                    var vehicle =
                        await
                            Task.Run(() => _vehicleRepository.GetSingle(s => s.Id.Equals(message.Id), s => s.Location));
                    model = Mapper.Map<Command>(vehicle);
                }

                return model;
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.Vin).NotNull();
                RuleFor(m => m.MakeId).NotNull();
                RuleFor(m => m.ModelId).NotNull();
                RuleFor(m => m.Color).NotNull();
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
                    await Task.Run(() => _vehicleRepository.GetSingle(s => s.Id.Equals(message.Id), s => s.Location));

                if (vehicle == null)
                {
                    vehicle = new Domain.Vehicle {Id = (Guid) message.Id};
                    _vehicleRepository.Add(vehicle);
                    _vehicleRepository.Commit();
                }

                vehicle.Handle(message);

                _vehicleRepository.Update(vehicle);
                _vehicleRepository.Commit();
            }
        }
    }
}