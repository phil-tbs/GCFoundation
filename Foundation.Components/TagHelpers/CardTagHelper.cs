using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-card")]
    public class CardTagHelper : BaseTagHelper
    {
        public required string CardTitle { get; set; }
        public CardTitleTagEnum CardTitleTag { get; set; } = CardTitleTagEnum.h3;

        public required string Href {  get; set; }

        public string? Badge { get; set; }

        public string? Description { get; set; }

        public string? ImgAlt { get; set; }

        public string? ImgSrc { get; set; }

        public LanguageEnum Lang { get; set; } = LanguageEnum.en;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "card-title", CardTitle);
            AddAttributeIfNotNull(output, "card-title-tag", CardTitleTag);
            AddAttributeIfNotNull(output, "href", Href);
            AddAttributeIfNotNull(output, "badge", Badge);
            AddAttributeIfNotNull(output, "description", Description);
            AddAttributeIfNotNull(output, "img-alt", ImgAlt);
            AddAttributeIfNotNull(output, "img-src", ImgSrc);
            AddAttributeIfNotNull(output, "lang", Lang);
            
            base.Process(context, output);
        }

    }
}
