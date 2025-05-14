using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Models
{
    public class SearchFilter
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public int Count { get; set; }

        public bool IsActive { get; set; }
    }
}
