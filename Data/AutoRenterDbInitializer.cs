﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoRenter.API.Domain;

namespace AutoRenter.API.Data
{
    public static class AutoRenterDbInitializer
    {
        private static AutoRenterContext _context;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _context = (AutoRenterContext) serviceProvider.GetService(typeof(AutoRenterContext));
            InitializeLocations();
        }

        private static void InitializeLocations()
        {
            if (!_context.Locations.Any())
            {
                ICollection<Location> locations = new List<Location>
                {
                    new Location
                    {
                        Id = new Guid("75bc8361-5c47-4c95-9340-e68e5286d2c5"),
                        SiteId = "13Z",
                        Name = "Loring Seaplane Base",
                        City = "Loring",
                        StateCode = "AK",
                        Vehicles = new List<Vehicle>
                        {
                            new Vehicle
                            {
                                Id = new Guid("a18f45b5-0965-4688-9869-d347427d38cc"),
                                LocationId = new Guid("75bc8361-5c47-4c95-9340-e68e5286d2c5"),
                                Vin = "3GTP1WEC9FG166670",
                                Make = "Homebrew",
                                Model = "Bubble",
                                Year = 2000,
                                Miles = 250000,
                                Color = "Lime",
                                IsRentToOwn = false
                            }
                        }
                    }
                };

                foreach (var location in locations)
                    _context.Locations.Add(location);
            }

            _context.SaveChanges();
        }
    }
}