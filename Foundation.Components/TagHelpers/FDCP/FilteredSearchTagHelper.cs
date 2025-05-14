using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Foundation.Components.Models;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.FDCP
{
    [HtmlTargetElement("fdcp-filtered-search")]
    public class FilteredSearchTagHelper: TagHelper
    {

        public string Title { get; set; }

        public string SearchInputText { get; set; }

        public string EndPoint { get; set; }

        public IEnumerable<SearchFilterCategory> Filters { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "filtered-search");


            StringBuilder sb = new StringBuilder();

            sb.Append("<div class='search-component'>");
            sb.Append(CultureInfo.InvariantCulture, $@"<h2>{Title}</h2>");

            sb.Append("<gcds-grid columns='1fr 2fr'>");

            sb.Append(GenerateFilters());
            sb.Append("<div>");
            sb.Append(GenerateSearchBox());
            sb.Append("<div class='filtered-search-result'>test</div>");
            sb.Append("</div>");
            sb.Append("</gcds-grid>");
            sb.Append("</div>");

            output.Content.SetHtmlContent(sb.ToString());

        }

        private string GenerateFilters()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class='filter-panel'>");
            sb.Append(CultureInfo.InvariantCulture, $@"<h3>Filter</h3>");

            foreach (SearchFilterCategory category in Filters)
            {
                sb.Append(CultureInfo.InvariantCulture, $@"<div class='filter-section'>");

                sb.Append(CultureInfo.InvariantCulture, $@"<button 
                    class='fdcp-collapse-button'
                    data-fdcp-collapse-toggle='collapse-{category.CSSId}' 
                    aria-expanded='{category.IsOpen.ToString().ToLower(CultureInfo.CurrentCulture)}'
                    aria-controls='collapse-{category.CSSId}'>
                    {category.Title}
                </button>");

                sb.Append(CultureInfo.InvariantCulture, $@"<div class='fdcp-collapse {((category.IsOpen) ? "fdcp-show" : "")}' id='collapse-{category.CSSId}'>");

                foreach (SearchFilter filter in category.Filters)
                {
                    sb.Append("<div class='filter-option'>");
                    sb.Append(CultureInfo.InvariantCulture, $@"<input type='checkbox' name='{filter.Name}' id='{filter.Name}' />");
                    sb.Append(CultureInfo.InvariantCulture, $@"<label for='{filter.Name}' class=''>{filter.Title}</label>");
                    sb.Append(CultureInfo.InvariantCulture, $@"<span class='filter-count'>{filter.Count}</span>");
                    sb.Append("</div>");
                }
                sb.Append($@"</div>");
                sb.Append($@"</div>");
                

                
            }
            sb.Append($@"</div>");
            return sb.ToString();
        }

        private string GenerateSearchBox()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("<div class='search-box-wrapper'>");

            sb.Append("<label for=''>{}</label>");
            sb.Append("<input type='search' id='search-input' name='search' class='form-control' placeholder='Search for program’s name, funding opportunity, key words...' />");
            sb.Append("<div>");


            return sb.ToString();
        }


    }
}
