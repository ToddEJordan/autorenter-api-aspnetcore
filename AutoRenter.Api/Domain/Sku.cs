using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoRenter.Api.Domain
{
    public class Sku : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
    }
}