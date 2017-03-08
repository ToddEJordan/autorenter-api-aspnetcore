using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoRenter.Api.Features.Location;

namespace AutoRenter.Api.Domain
{
    public class Location : IEntity
    {
        public string SiteId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        private void UpdateDetails(PostPut.Command message)
        {
            SiteId = message.SiteId;
            Name = message.Name;
            City = message.City;
            StateCode = message.StateCode;
        }

        public void Handle(PostPut.Command message)
        {
            UpdateDetails(message);
        }
    }
}