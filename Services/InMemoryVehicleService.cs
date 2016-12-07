using System;
using System.Collections.Generic;
using AutoRenter.API.Models;

namespace AutoRenter.API.Services
{
    public class InMemoryVehicleService : IVehicleService
    {
        private readonly IDictionary<Guid, Vehicle> _internalStorage = new Dictionary<Guid, Vehicle>
        {
            {
                new Guid("a18f45b5-0965-4688-9869-d347427d38cc"), new Vehicle
                {
                    Id = new Guid("a18f45b5-0965-4688-9869-d347427d38cc"),
                    LocationId = new Guid("75bc8361-5c47-4c95-9340-e68e5286d2c5"),
                    Vin = "3GTP1WEC9FG166670",
                    Make = "Homebrew",
                    Model = "Bubble",
                    Year = 2000,
                    Miles = 250000,
                    Color = "Lime",
                    RentToOwn = false
                }
            }
        };

        public IEnumerable<Vehicle> List()
        {
            return _internalStorage.Values;
        }

        public Vehicle Get(Guid id)
        {
            return _internalStorage[id];
        }

        public void Create(Vehicle model)
        {
            model.Id = Guid.NewGuid();
            _internalStorage.Add(model.Id, model);
        }

        public void Delete(Guid id)
        {
            _internalStorage.Remove(id);
        }

        public void Update(Guid id, Vehicle model)
        {
            _internalStorage[id] = model;
        }
    }
}