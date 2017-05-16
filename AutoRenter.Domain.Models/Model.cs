using System;
using System.ComponentModel.DataAnnotations;

namespace AutoRenter.Domain.Models
{
    public class Model : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string ExternalId { get; set; }
        public string Name { get; set; }
    }
}