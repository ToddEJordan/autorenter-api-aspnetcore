using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoRenter.API.Data;
using AutoRenter.API.Models;
using AutoRenter.API.Models.Location;
using AutoRenter.API.Models.Locations;
using MediatR;

namespace AutoRenter.API.Queries.Locations
{
    public class GetAllLocationsQueryHandler : IAsyncRequestHandler<GetAllLocationsQuery, AllLocationsModel>
    {
        private readonly ILocationRepository _locationRepository;

        public GetAllLocationsQueryHandler(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public async Task<AllLocationsModel> Handle(GetAllLocationsQuery message)
        {
            var entities = await Task.Run(() => _locationRepository.AllIncluding(s => s.Vehicles)
                .OrderBy(s => s.Name)
                .ToList());
            return new AllLocationsModel(Mapper.Map<IEnumerable<LocationModel>>(entities));
        }
    }
}