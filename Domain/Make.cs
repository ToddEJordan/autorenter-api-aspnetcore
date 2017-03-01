using System.ComponentModel.DataAnnotations;

namespace AutoRenter.API.Domain
{
    public class Make
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}