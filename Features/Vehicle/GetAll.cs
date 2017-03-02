using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.API.Data;
using FluentValidation;
using MediatR;

namespace AutoRenter.API.Features.Vehicle
{
    public class GetAll
    {
        public class Query : IAsyncRequest<Model>
        {
            public Guid? LocationId { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.LocationId).NotNull();
            }
        }

        public class Model
        {
            public List<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

            public class Vehicle
            {
                public Guid Id { get; set; }
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
                        Task.Run(
                            () => _locationRepository.GetSingle(s => s.Id.Equals(message.LocationId), s => s.Vehicles));
                ICollection<Domain.Vehicle> vehicles = new List<Domain.Vehicle>();

                if (location != null)
                    vehicles = location.Vehicles;

                var viewModel = new Model
                {
                    Vehicles = Mapper.Map<List<Model.Vehicle>>(vehicles)
                };

                return viewModel;
            }
        }
    }
}