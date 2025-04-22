using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Foundation.Components.Models
{
    public class TabulatorResponse<T>
    {
        [JsonPropertyName("last_page")]
        public int LastPage { get; set; }

        [JsonPropertyName("data")]
        public List<T> Data { get; set; } = new();
    }
}
