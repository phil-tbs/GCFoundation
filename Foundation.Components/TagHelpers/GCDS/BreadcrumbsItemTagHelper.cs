using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.GCDS
{
    /// <summary>
    /// A tag helper for rendering individual breadcrumb items in the breadcrumbs navigation.
    /// </summary>
    [HtmlTargetElement("gcds-breadcrumbs-item")]
    public class BreadcrumbsItemTagHelper : BaseTagHelper
    {
        /// <summary>
        /// The href (link) for the breadcrumb item.
        /// </summary>
        public string? Href { get; set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "href", Href);

            base.Process(context, output);
        }
    }
}
