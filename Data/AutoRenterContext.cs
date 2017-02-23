using AutoRenter.API.Domain;
using Microsoft.EntityFrameworkCore;

namespace AutoRenter.API.Data
{
    public class AutoRenterContext : DbContext
    {
        public AutoRenterContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Sku> Skus { get; set; }
    }
}