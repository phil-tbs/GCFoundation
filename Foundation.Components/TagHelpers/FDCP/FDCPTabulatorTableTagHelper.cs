using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Foundation.Components.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Foundation.Components.TagHelpers.FDCP
{
    [HtmlTargetElement("fdcp-tabulator-table")]
    public class FDCPTabulatorTableTagHelper : TagHelper
    {

        /// <summary>The HTML element ID to assign to the Tabulator container (must be unique).</summary>
        public string Id { get; set; } = "";

        /// <summary>The URL of the AJAX endpoint that returns paginated JSON data.</summary>
        public string? AjaxUrl { get; set; }

        /// <summary>Local data (optional). If provided, used instead of AJAX.</summary>
        public IEnumerable<object>? Data { get; set; }

        /// <summary>A list of columns to display in the Tabulator table.</summary>
        public List<TabulatorColumn> Columns { get; set; } = new();

        public bool UseStaticData { get; set; } = false;

        public int PaginationSize { get; set; } = 10;


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("id", Id);
            output.Attributes.SetAttribute("class", "tabulator-table");
            output.Attributes.SetAttribute("data-layout", "fitColumns");
            output.Attributes.SetAttribute("data-pagination", "local");
            output.Attributes.SetAttribute("data-pagination-size", PaginationSize);
            output.Attributes.SetAttribute("data-columns", System.Text.Json.JsonSerializer.Serialize(Columns, jsonOptions));
            if (UseStaticData && Data != null)
            {
                
                string dataJson = System.Text.Json.JsonSerializer.Serialize(Data, jsonOptions);
                output.Attributes.SetAttribute("data-set", dataJson);
            }
            else
            {
                output.Attributes.SetAttribute("data-ajaxURL", AjaxUrl);
            }
                
        }
    }
}
