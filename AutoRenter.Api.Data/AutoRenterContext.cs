using AutoRenter.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoRenter.Api.Data
{
    public class AutoRenterContext : DbContext
    {
        public AutoRenterContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Sku> Skus { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
    }
}