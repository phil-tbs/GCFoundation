using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-grid")]
    public class GridTagHelper : BaseTagHelper
    {
        public AlignContent? AlignContent { get; set; }

        public AlignItem AlingItem { get; set; }

        public string? Columns { get; set; }

        public string? ColumnsDesktop { get; set; }

        public string? ColumnsTablet { get; set; }
        public SizeTypeEmum Container { get; set; } = SizeTypeEmum.full;

        public GridDisplay Display { get; set; } = GridDisplay.grid;

        public bool EqualRowHeight { get; set; }

        public string? Gap { get; set; } = "300";
        public string? GapDesktop { get; set; }

        public string? GapTablet { get; set; }

        public AlignContent? JustifyContent { get; set; }

        public AlignItem? JustifyItems { get; set; }

        public AlignContent? PlaceContent { get; set; }

        public AlignItem? PlaceItems { get; set; }

        public string? Tag { get; set; } = "div";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNullWithCaseConversion(output, "align-content", AlignContent);
            AddAttributeIfNotNullWithCaseConversion(output, "align-items", AlingItem);
            AddAttributeIfNotNull(output, "columns", Columns);
            AddAttributeIfNotNull(output, "columns-desktop", ColumnsDesktop);
            AddAttributeIfNotNull(output, "columns-tablet", ColumnsTablet);
            AddAttributeIfNotNull(output, "container", Container);
            AddAttributeIfNotNull(output, "display", Display);
            AddAttributeIfNotNull(output, "equal-row-height", EqualRowHeight);
            AddAttributeIfNotNull(output, "gab", Gap);
            AddAttributeIfNotNull(output, "gapDesktop", GapDesktop);
            AddAttributeIfNotNull(output, "gapTablet", GapTablet);

            AddAttributeIfNotNullWithCaseConversion(output, "justify-content", JustifyContent);
            AddAttributeIfNotNullWithCaseConversion(output, "justify-items", JustifyItems);
            AddAttributeIfNotNullWithCaseConversion(output, "place-content", PlaceContent);
            AddAttributeIfNotNullWithCaseConversion(output, "place-items", PlaceItems);
            AddAttributeIfNotNull(output, "tag", Tag);

            base.Process(context, output);
        }

    }
}
