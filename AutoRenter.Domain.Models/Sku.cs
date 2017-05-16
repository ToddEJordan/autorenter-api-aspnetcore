using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutoRenter.Domain.Models
{
    public class Sku : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        //[ForeignKey("Make")]
        public string MakeId { get; set; }
        //[ForeignKey("Model")]
        public string ModelId { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }

        public virtual Make Make { get; set; }
        public virtual Model Model { get; set; }
    }
}