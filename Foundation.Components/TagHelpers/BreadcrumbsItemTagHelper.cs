using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    [HtmlTargetElement("gcds-breadcrumbs-item")]
    public class BreadcrumbsItemTagHelper : BaseTagHelper
    {
        public string? Href { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "href", Href);

            base.Process(context, output);
        }
    }
}
