using System.Linq;
using AutoRenter.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AutoRenter.API.Data
{
    public class AutoRenterContext : DbContext
    {
        public AutoRenterContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Location> Locations { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.Entity<Location>()
                .ToTable("Location");

            modelBuilder.Entity<Vehicle>()
                .ToTable("Vehicles");

            modelBuilder.Entity<Vehicle>()
                .HasOne(a => a.Location)
                .WithMany(s => s.Vehicles)
                .HasForeignKey(a => a.LocationId);
        }
    }
}