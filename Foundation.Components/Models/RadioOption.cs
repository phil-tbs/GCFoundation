using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Models
{
    public class RadioOption
    {
        public required string Id { get; set; }
        public required string Label { get; set; }

        public required string Value { get; set; }

        public bool Required { get; set; }

        public bool Disabled { get; set; }

        public bool Checked { get; set; }

        public string? Hint { get; set; }

    }
}
