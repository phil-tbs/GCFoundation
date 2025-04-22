using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Models
{
    public class TabulatorRequest
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public List<TabulatorSorter> Sort { get; set; } = new();
        public List<TabulatorFilter> Filter { get; set; } = new();
    }
}
