using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoRenter.API.Features.Vehicle;

namespace AutoRenter.API.Domain
{
    public class Vehicle : IEntity
    {
        public string Vin { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int Miles { get; set; }
        public string Color { get; set; }
        public bool IsRentToOwn { get; set; }

        [ForeignKey("Location")]
        public Guid LocationId { get; set; }

        public virtual Location Location { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public void Handle(PostPut.Command message)
        {
            UpdateDetails(message);
        }

        private void UpdateDetails(PostPut.Command message)
        {
            Vin = message.Vin;
            Make = message.Make;
            Model = message.Model;
            Year = message.Year;
            Miles = message.Miles;
            Color = message.Color;
            IsRentToOwn = message.IsRentToOwn;
            LocationId = message.LocationId;
        }
    }
}