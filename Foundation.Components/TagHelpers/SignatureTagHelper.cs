using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-signature")]
    public class SignatureTagHelper: BaseTagHelper
    {
        public bool HasLink { get; set; } = false;

        public LanguageEnum Lang { get; set; } = LanguageEnum.en;

        public SignatureTypeEnum Type { get; set; } = SignatureTypeEnum.Signature;
        public SignatureVariantEnum Variant { get; set; } = SignatureVariantEnum.Colour;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "has-link", HasLink);
            AddAttributeIfNotNull(output, "lang", Lang);
            AddAttributeIfNotNull(output, "type", Type);
            AddAttributeIfNotNull(output, "variant", Variant);

            base.Process(context, output);
        }
    }
}
