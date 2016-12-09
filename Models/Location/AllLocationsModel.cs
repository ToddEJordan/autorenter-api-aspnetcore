using System.Collections.Generic;
using AutoRenter.API.Models.Location;

namespace AutoRenter.API.Models.Locations
{
    public class AllLocationsModel : List<LocationModel>
    {
        public AllLocationsModel()
        {
        }

        public AllLocationsModel(IEnumerable<LocationModel> collection) : base(collection)
        {
        }
    }
}