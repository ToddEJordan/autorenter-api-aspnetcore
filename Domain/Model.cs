using System.ComponentModel.DataAnnotations;

namespace AutoRenter.API.Domain
{
    public class Model
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}