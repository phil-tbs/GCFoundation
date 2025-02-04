using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Models
{
    public class ErrorLink
    {
        public required string Href { get; set; }

        public required string Message { get; set; }
    }
}
