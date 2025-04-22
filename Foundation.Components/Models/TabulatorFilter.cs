using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Models
{
    public class TabulatorFilter
    {
        public string Field { get; set; } = "";
        public string Type { get; set; } = "like";
        public string? Value { get; set; }
    }
}
