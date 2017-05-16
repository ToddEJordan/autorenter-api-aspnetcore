using System.ComponentModel.DataAnnotations;

namespace AutoRenter.Domain.Models
{
    public class State
    {
        [Key]
        public string StateCode { get; set; }
        public string Name { get; set; }
    }
}