using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoRenter.Api.Features.Sku;

namespace AutoRenter.Api.Domain
{
    public class Sku : IEntity
    {
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
    }
}