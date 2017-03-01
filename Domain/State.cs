using System.ComponentModel.DataAnnotations;

namespace AutoRenter.API.Domain
{
    public class State
    {
        [Key]
        public string StateCode { get; set; }
        public string Name { get; set; }
    }
}