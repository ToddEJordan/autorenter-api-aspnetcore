using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.API.Data;
using FluentValidation;
using MediatR;

namespace AutoRenter.API.Features.Location
{
    public class Get
    {
        public class Query : IAsyncRequest<Model>
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

        public class Model
        {
            public Guid? Id { get; set; }
            public string SiteId { get; set; }
            public string Name { get; set; }
            public string City { get; set; }
            public string StateCode { get; set; }

            public int VehicleCount { get; set; }
        }

        public class Hanlder : IAsyncRequestHandler<Query, Model>
        {
            private readonly ILocationRepository _locationRepository;

            public Hanlder(ILocationRepository locationRepository)
            {
                _locationRepository = locationRepository;
            }

            public async Task<Model> Handle(Query message)
            {
                var location =
                    await
                        Task.Run(() => _locationRepository.GetSingle(s => s.Id.Equals(message.Id), s => s.Vehicles));
                var viewModel = Mapper.Map<Model>(location);

                return viewModel;
            }
        }
    }
}