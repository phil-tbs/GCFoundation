using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-header")]
    public class HeaderTagHelper : BaseTagHelper
    {
        public required string LangHref { get; set; }

        public required string SkipToHerf { get; set; }

        public Language Lang { get; set; } = Language.en;

        public bool SignatureHasLink { get; set; } = true;

        public HeaderSignatureVariant SignatureVariant { get; set; } = HeaderSignatureVariant.colour;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "lang-href", LangHref);
            AddAttributeIfNotNull(output, "skip-to-href", SkipToHerf);
            AddAttributeIfNotNull(output, "lang", Lang);
            AddAttributeIfNotNull(output, "signature-has-link", SignatureHasLink);
            AddAttributeIfNotNull(output, "signature-variant", SignatureVariant);

            base.Process(context, output);
        }
    }
}
