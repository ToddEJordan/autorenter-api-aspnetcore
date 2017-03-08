using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.Api.Data;
using FluentValidation;
using MediatR;

namespace AutoRenter.Api.Features.Location
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
            public string SiteId { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public string StateCode { get; set; }
        }

        public class QueryHandler : IAsyncRequestHandler<Query, Command>
        {
            private readonly ILocationRepository _locationRepository;

            public QueryHandler(ILocationRepository locationRepository)
            {
                _locationRepository = locationRepository;
            }

            public async Task<Command> Handle(Query message)
            {
                var location =
                    await
                        Task.Run(() => _locationRepository.GetSingle(s => s.Id.Equals(message.Id), s => s.Vehicles));
                var viewModel = Mapper.Map<Command>(location);

                return viewModel;
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly ILocationRepository _locationRepository;
            private readonly IVehicleRepository _vehicleRepository;

            public CommandHandler(ILocationRepository locationRepository, IVehicleRepository vehicleRepository)
            {
                _locationRepository = locationRepository;
                _vehicleRepository = vehicleRepository;
            }

            protected override async Task HandleCore(Command message)
            {
                var location = await
                    Task.Run(() => _locationRepository.GetSingle(s => s.Id.Equals(message.Id)));

                _vehicleRepository.DeleteWhere(v => v.LocationId.Equals(message.Id));
                _locationRepository.Delete(location);
                _locationRepository.Commit();
            }
        }
    }
}