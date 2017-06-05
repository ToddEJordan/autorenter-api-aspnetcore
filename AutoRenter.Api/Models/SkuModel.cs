using System;

namespace AutoRenter.Api.Models
{
    public class SkuModel
    {
        public Guid Id { get; set; }
        public string MakeId { get; set; }
        public string ModelId { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }

        public MakeModel Make { get; set; }
        public ModelModel Model { get; set; }
    }
}
