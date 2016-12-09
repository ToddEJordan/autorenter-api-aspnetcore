using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoRenter.API.Data;
using AutoRenter.API.Models.Vehicle;
using MediatR;
using System.Linq;
using AutoMapper;

namespace AutoRenter.API.Features.Location
{
    public class GetAll
    {
        public class Query : IAsyncRequest<Model>
        { }

        public class Model
        {
            public List<Location> Locations { get; set; }

            public class Location
            {
                public Guid Id { get; set; }
                public string SiteId { get; set; }
                public string Name { get; set; }
                public string City { get; set; }
                public string StateCode { get; set; }
                public int VehicleCount { get; set; }
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
                var locations = await Task.Run(() => _locationRepository.AllIncluding(s => s.Vehicles)
                .OrderBy(s => s.Name)
                .ToList());

                var viewModel = new Model
                {
                    Locations = Mapper.Map<List<Model.Location>>(locations)
                };

                return viewModel;
            }
        }
    }
}
