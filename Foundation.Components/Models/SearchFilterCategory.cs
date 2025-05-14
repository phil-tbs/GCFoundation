using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Models
{
    public class SearchFilterCategory
    {
        public string Title { get; set; }
        public string CSSId { get; set; }

        public bool IsOpen { get; set; } = true;
        public IEnumerable<SearchFilter> Filters { get; set; }
    }
}
