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
        public Uri? AjaxUrl { get; set; }

        /// <summary>Local data (optional). If provided, used instead of AJAX.</summary>
        public IEnumerable<object>? Data { get; set; }

        /// <summary>A list of columns to display in the Tabulator table.</summary>
        public IEnumerable<TabulatorColumn> Columns { get; set; } = Enumerable.Empty<TabulatorColumn>();

        public bool UseStaticData { get; set; }

        public int PaginationSize { get; set; } = 10;


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("id", Id);
            output.Attributes.SetAttribute("class", "tabulator-container");

            output.Content.AppendHtml(GenerateSearchHtml());
            output.Content.AppendHtml(GenerateTabulator());
        }

        private string GenerateSearchHtml()
        {
            return $@"
                <form id='{Id}-search-form'>
                    <div class='row'>
                        <div class='col'>
                            <gcds-input
                                input-id='{Id}-search=input'
                                class='tabulator-search-input'
                                data-tabulator-id='{Id}-tabulator'
                                label='Search'
                                name='{Id}-search-input'
                                type='search'
                                hint='You can search across all columns'
                            ></gcds-input>
                        </div>
                    </div>
                </form>
            ";
        }

        private string GenerateTabulator()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            var tableDiv = $@"
            <div id='{Id}-tabulator' class='tabulator-table'
                 data-layout='fitColumns'
                 data-pagination='local'
                 data-pagination-size='{PaginationSize}'
                 data-columns='{System.Text.Json.JsonSerializer.Serialize(Columns, jsonOptions)}'";

            if (UseStaticData && Data != null)
            {
                string dataJson = System.Text.Json.JsonSerializer.Serialize(Data, jsonOptions);
                tableDiv += $" data-set='{dataJson}'";
            }
            else
            {
                tableDiv += $" data-ajaxURL='{AjaxUrl}'";
            }

            tableDiv += "></div>";

            return tableDiv;

        }
    }
}
