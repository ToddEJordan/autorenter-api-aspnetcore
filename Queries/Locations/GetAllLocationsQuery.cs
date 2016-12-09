using AutoRenter.API.Models;
using AutoRenter.API.Models.Locations;
using MediatR;

namespace AutoRenter.API.Queries.Locations
{
    public class GetAllLocationsQuery : IAsyncRequest<AllLocationsModel>
    {
    }
}