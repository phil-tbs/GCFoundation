using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Foundation.Components.Models
{
    public class TabulatorRequest
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;
        public IEnumerable<TabulatorSorter> Sort { get; set; } = Enumerable.Empty<TabulatorSorter>();
        public IEnumerable<TabulatorFilter> Filter { get; set; } = Enumerable.Empty<TabulatorFilter>();
    }
}
