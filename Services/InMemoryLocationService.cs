using System.Collections.Generic;
using AutoRenter.API.Models;

namespace AutoRenter.API.Services
{
    public class InMemoryLocationService : ILocationService
    {
        private readonly IDictionary<int, Location> _internalStorage = new Dictionary<int, Location>
        {
            {
                1, new Location
                {
                    Id = 1,
                    SiteId = "13Z",
                    Name = "Loring Seaplane Base",
                    City = "Loring",
                    StateCode = "AK"
                }
            }
        };

        public IEnumerable<Location> List()
        {
            return _internalStorage.Values;
        }

        public Location Get(int id)
        {
            return _internalStorage[id];
        }

        public void Create(Location model)
        {
            model.Id = _internalStorage.Count + 1;
            _internalStorage.Add(model.Id, model);
        }

        public void Delete(int id)
        {
            _internalStorage.Remove(id);
        }

        public void Update(int id, Location model)
        {
            _internalStorage[id] = model;
        }
    }
}