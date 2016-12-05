using System.Collections.Generic;
using AutoRenter.API.Models;

namespace AutoRenter.API.Services
{
    public class InMemoryVehicleService : IVehicleService
    {
        private readonly IDictionary<int, Vehicle> _internalStorage = new Dictionary<int, Vehicle>
        {
            {
                1, new Vehicle
                {
                    Id = 1,
                    LocationId = 1,
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

        public Vehicle Get(int id)
        {
            return _internalStorage[id];
        }

        public void Create(Vehicle model)
        {
            model.Id = _internalStorage.Count + 1;
            _internalStorage.Add(model.Id, model);
        }

        public void Delete(int id)
        {
            _internalStorage.Remove(id);
        }

        public void Update(int id, Vehicle model)
        {
            _internalStorage[id] = model;
        }
    }
}