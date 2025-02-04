using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-link")]
    public class LinkTagHelper : BaseTagHelper
    {
        public string? Display { get; set; }

        public string? Download { get; set; }

        public bool External { get; set; } = false;

        public required string Href { get; set; } = "";

        public string? Rel { get; set; }

        public LinkSizeEnum Size { get; set; } = LinkSizeEnum.Inherit;

        /// <summary>
        /// Target of the link (_blank, _self, _parent, framename)
        /// </summary>
        public required string Target { get; set; } = "_self";

        public string? Type {  get; set; }

        public LinkVariantEnum Variant { get; set; } = LinkVariantEnum.Default;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "display", Display);
            AddAttributeIfNotNull(output, "download", Download);
            AddAttributeIfNotNull(output, "external", External);
            AddAttributeIfNotNull(output, "href", Href);
            AddAttributeIfNotNull(output, "rel", Rel);
            AddAttributeIfNotNull(output, "size", Size);
            AddAttributeIfNotNull(output, "target", Target);
            AddAttributeIfNotNull(output, "type", Type);
            AddAttributeIfNotNull(output, "variant", Variant);

            base.Process(context, output);
        }


    }
}
