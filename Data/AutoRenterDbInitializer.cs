using System;
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
            InitializeSkus();
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

        private static void InitializeSkus()
        {
            if (!_context.Skus.Any())
            {
                ICollection<Sku> skus = new List<Sku> {
                    new Sku{Id = new Guid("b19a9a7a-3631-427e-90ad-777f3d2b2535"), MakeId = "tsl", ModelId = "tms", Year = 2016, Color = "Black"},
                    new Sku{Id = new Guid("f90f3ce2-b9a9-48f1-b38a-8101f75b414b"), MakeId = "tsl", ModelId = "tmx", Year = 2016, Color = "Black"},
                    new Sku{Id = new Guid("72c88d7e-238d-47db-9185-d80828c9b008"), MakeId = "tsl", ModelId = "tms", Year = 2017, Color = "Black"},
                    new Sku{Id = new Guid("2dcb89f6-c71c-4651-9cb4-bc120d877248"), MakeId = "tsl", ModelId = "tms", Year = 2017, Color = "Silver"},
                    new Sku{Id = new Guid("5a0431d7-93a3-4d8a-9d87-7f8a0c7c2829"), MakeId = "tsl", ModelId = "tmx", Year = 2017, Color = "Black"},
                    new Sku{Id = new Guid("fb240f9e-7e48-43cd-90a9-43a6433f1d07"), MakeId = "tsl", ModelId = "tmx", Year = 2017, Color = "Silver"},
                    new Sku{Id = new Guid("1370471a-cc1e-4b37-98a4-73f5e344bbe9"), MakeId = "che", ModelId = "cvt", Year = 2016, Color = "Black"},
                    new Sku{Id = new Guid("3b7844c2-be06-4843-b413-3360895a02ea"), MakeId = "che", ModelId = "cvt", Year = 2016, Color = "Red"},
                    new Sku{Id = new Guid("a931c512-749a-4181-b212-e5bcd30ac731"), MakeId = "che", ModelId = "cvt", Year = 2017, Color = "Black"},
                    new Sku{Id = new Guid("950588cc-5b8c-4be0-ac4d-e0207e20bb79"), MakeId = "che", ModelId = "cvt", Year = 2017, Color = "Red"},
                    new Sku{Id = new Guid("a05f8bf7-c028-4f4f-8e5f-cddb378cc86a"), MakeId = "frd", ModelId = "fxp", Year = 2016, Color = "Black"},
                    new Sku{Id = new Guid("0f40b060-201f-46e9-8557-b168292859ae"), MakeId = "frd", ModelId = "fta", Year = 2016, Color = "Black"},
                    new Sku{Id = new Guid("f52a9405-f57d-4e35-836d-8ebc14f6dc37"), MakeId = "frd", ModelId = "fta", Year = 2016, Color = "Red"},
                    new Sku{Id = new Guid("02208a1d-62ef-4298-95c0-ef6e8d2fa7a5"), MakeId = "frd", ModelId = "fta", Year = 2016, Color = "Silver"},
                    new Sku{Id = new Guid("4d281090-360c-4e0c-a63e-0fe981d17bcc"), MakeId = "frd", ModelId = "fxp", Year = 2017, Color = "Black"},
                    new Sku{Id = new Guid("c33ac900-4e23-4361-9806-975447794364"), MakeId = "frd", ModelId = "fxp", Year = 2017, Color = "Silver"},
                    new Sku{Id = new Guid("80b2ff7c-f551-4505-bd86-a96863443075"), MakeId = "frd", ModelId = "fta", Year = 2017, Color = "Black"},
                    new Sku{Id = new Guid("2252e340-7ac2-4543-abb6-46d067a7836e"), MakeId = "frd", ModelId = "fta", Year = 2017, Color = "Red"},
                    new Sku{Id = new Guid("ddec89e9-eb22-4b44-acff-d45e5600596e"), MakeId = "frd", ModelId = "fta", Year = 2017, Color = "Silver"}
                };

                foreach (var sku in skus)
                    _context.Skus.Add(sku);
            }

            _context.SaveChanges();
        }
    }
}