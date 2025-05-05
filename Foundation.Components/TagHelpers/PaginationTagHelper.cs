using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-pagination")]
    public class PaginationTagHelper : BaseTagHelper
    {
        public required string Label { get; set; }

        public int CurrentPage { get; set; }

        public PaginationDisplay Display { get; set; } = PaginationDisplay.List;

        public Language Lang { get; set; } = Language.en;

        public string NextHref { get; set; } = "#next";

        public string? NextLabel { get; set; }

        public string PreviousHref { get; set; } = "#previous";

        public string? PreviousLabel { get; set; }

        public int TotalPages { get; set; }

        public Uri? Url { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "label", Label);
            AddAttributeIfNotNull(output, "current-page", CurrentPage);
            AddAttributeIfNotNull(output, "display", Display);
            AddAttributeIfNotNull(output, "lan", Lang);
            AddAttributeIfNotNull(output, "next-href", NextHref);
            AddAttributeIfNotNull(output, "next-label", NextLabel);
            AddAttributeIfNotNull(output, "previous-href", PreviousHref);
            AddAttributeIfNotNull(output, "previous-label", PreviousLabel);
            AddAttributeIfNotNull(output, "total-pages", TotalPages);
            AddAttributeIfNotNull(output, "url", Url);

            base.Process(context, output);
        }
    }
}
