using System.ComponentModel.DataAnnotations;

namespace AutoRenter.Api.Domain
{
    public class State
    {
        [Key]
        public string StateCode { get; set; }
        public string Name { get; set; }
    }
}