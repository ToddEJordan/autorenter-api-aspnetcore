using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoRenter.Api.Domain
{
    public class Vehicle : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string Vin { get; set; }
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public int Year { get; set; }
        public int Miles { get; set; }
        public string Color { get; set; }
        public bool IsRentToOwn { get; set; }
        public string Image { get; set;}

        [ForeignKey("Location")]
        public Guid LocationId { get; set; }

        public virtual Location Location { get; set; }
    }
}