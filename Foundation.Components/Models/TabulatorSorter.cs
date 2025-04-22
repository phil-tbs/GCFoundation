using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Models
{
    public class TabulatorSorter
    {
        public string Field { get; set; } = "";
        public string Dir { get; set; } = "asc"; // or "desc"
    }
}
