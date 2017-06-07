using System.Collections.Generic;

namespace AutoRenter.Domain.Models
{
    public class LookupData
    {
        public virtual ICollection<State> States { get; set; } = new List<State>();
        public virtual ICollection<Make> Makes { get; set; } = new List<Make>();
        public virtual ICollection<Model> Models { get; set; } = new List<Model>();
    }
}