using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.API.Data;
using FluentValidation;
using MediatR;

namespace AutoRenter.API.Features.Location
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
                Command model;
                if (message.Id == null)
                {
                    model = new Command() {Id = Guid.NewGuid()};
                }
                else
                {
                    var location =
                    await
                        Task.Run(() => _locationRepository.GetSingle(s => s.Id.Equals(message.Id), s => s.Vehicles));
                    model = Mapper.Map<Command>(location);
                }

                return model;
            }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.SiteId).NotNull();
                RuleFor(m => m.Name).NotNull();
                RuleFor(m => m.City).NotNull();
                RuleFor(m => m.StateCode).NotNull();
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command>
        {
            private readonly ILocationRepository _locationRepository;

            public CommandHandler(ILocationRepository locationRepository)
            {
                _locationRepository = locationRepository;
            }

            protected override async Task HandleCore(Command message)
            {
                var location = await
                        Task.Run(() => _locationRepository.GetSingle(s => s.Id.Equals(message.Id), s => s.Vehicles));

                if (location == null)
                {
                    location = new Domain.Location { Id = (Guid)message.Id };
                    _locationRepository.Add(location);
                    _locationRepository.Commit();
                }

                location.Handle(message);

                _locationRepository.Update(location);
                _locationRepository.Commit();
            }
        }
    }
}