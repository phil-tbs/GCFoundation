using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    public class SearchTagHelper: BaseTagHelper
    {
        public required string Action { get; set; }

        public LanguageEnum Lang { get; set; } = LanguageEnum.en;

        public SearchMethodEnum Method { get; set; } = SearchMethodEnum.Get;

        public required string Name { get; set; }

        public string? Placeholder { get; set; } = "Canada.ca";

        public required string SearchId { get; set; } = "search";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "action", Action);
            AddAttributeIfNotNull(output, "lang", Lang);
            AddAttributeIfNotNull(output, "method", Method);
            AddAttributeIfNotNull(output, "name", Name);
            AddAttributeIfNotNull(output, "placeholder", Placeholder);
            AddAttributeIfNotNull(output, "search-id", SearchId);
            base.Process(context, output);
        }

    }
}
