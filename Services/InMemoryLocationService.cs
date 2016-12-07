using System;
using System.Collections.Generic;
using AutoRenter.API.Models;

namespace AutoRenter.API.Services
{
    public class InMemoryLocationService : ILocationService
    {
        private readonly IDictionary<Guid, Location> _internalStorage = new Dictionary<Guid, Location>
        {
            {
                new Guid("75bc8361-5c47-4c95-9340-e68e5286d2c5"), new Location
                {
                    Id = new Guid("75bc8361-5c47-4c95-9340-e68e5286d2c5"),
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

        public Location Get(Guid id)
        {
            return _internalStorage[id];
        }

        public void Create(Location model)
        {
            model.Id = Guid.NewGuid();
            _internalStorage.Add(model.Id, model);
        }

        public void Delete(Guid id)
        {
            _internalStorage.Remove(id);
        }

        public void Update(Guid id, Location model)
        {
            _internalStorage[id] = model;
        }
    }
}