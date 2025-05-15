using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Models
{
    public class SearchFilterRequest
    {
        public int Page {  get; set; }

        public string? SearchTerm { get; set; }

        public int NumberByPage { get; set; }
        public IEnumerable<SearchFilterCategory> Categories { get; set; } = Enumerable.Empty<SearchFilterCategory>();
    }
}
