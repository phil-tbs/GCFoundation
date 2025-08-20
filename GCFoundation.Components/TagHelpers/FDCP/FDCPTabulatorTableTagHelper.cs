using GCFoundation.Components.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace GCFoundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// Renders a Tabulator table with support for dynamic data from either a static data source or an AJAX endpoint.
    /// </summary>
    [HtmlTargetElement("fdcp-tabulator-table")]
    public class FDCPTabulatorTableTagHelper : TagHelper
    {
        /// <summary>
        /// ViewContext for accessing the current view context.
        /// </summary>
        [ViewContext]
        public ViewContext ViewContext { get; set; } = null!;

        /// <summary>
        /// The HTML element ID to assign to the Tabulator container (must be unique).
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// The URL of the AJAX endpoint that returns paginated JSON data.
        /// </summary>
#pragma warning disable CA1056 // URI-like properties should not be strings
        public string? AjaxUrl { get; set; }
#pragma warning restore CA1056 // URI-like properties should not be strings

        /// <summary>
        /// Local data (optional). If provided, used instead of AJAX.
        /// </summary>
        public IEnumerable<object>? Data { get; set; }

        /// <summary>
        /// A list of columns to display in the Tabulator table.
        /// </summary>
        public IEnumerable<TabulatorColumn> Columns { get; set; } = Enumerable.Empty<TabulatorColumn>();

        /// <summary>
        /// Flag indicating whether to use static data or AJAX. Defaults to false (use AJAX).
        /// </summary>
        public bool UseStaticData { get; set; }

        /// <summary>
        /// The number of records per page for pagination. Defaults to 10.
        /// </summary>
        public int PaginationSize { get; set; } = 10;

        /// <inheritdoc/>
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

        /// <summary>
        /// Generates the HTML for the search input form.
        /// </summary>
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

        /// <summary>
        /// Generates the HTML for the Tabulator table element, including columns and data.
        /// </summary>
        private string GenerateTabulator()
        {
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };

            // Get anti-forgery token
            var htmlHelper = (IHtmlHelper)ViewContext.HttpContext.RequestServices.GetService(typeof(IHtmlHelper))!;
            ((IViewContextAware)htmlHelper).Contextualize(ViewContext);
            
            string antiForgeryToken = "";
            var tokenHtml = htmlHelper.AntiForgeryToken();
            using (var writer = new StringWriter())
            {
                tokenHtml.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
                antiForgeryToken = writer.ToString().Replace("\"", "&quot;");
            }

            var tableDiv = $@"
            <div id='{Id}-tabulator' class='tabulator-table'
                 data-layout='fitColumns'
                 data-pagination='local'
                 data-pagination-size='{PaginationSize}'
                 data-columns='{JsonSerializer.Serialize(Columns, jsonOptions)}'
                 data-antiforgery-token='{antiForgeryToken}'";

            if (UseStaticData && Data != null)
            {
                string dataJson = JsonSerializer.Serialize(Data, jsonOptions);
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
