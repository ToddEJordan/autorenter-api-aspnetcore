using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutoRenter.API.Models
{
    public class LogDto
    {
        public string Message { get; set; }
        // TODO: Switch to enum?
        public string Level { get; set; }
    }
}
