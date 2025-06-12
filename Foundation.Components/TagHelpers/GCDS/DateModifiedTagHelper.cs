using Foundation.Components.Enums;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.GCDS
{
    /// <summary>
    /// A tag helper for rendering a date modified element, which can display either the date or version type.
    /// </summary>
    public class DateModifiedTagHelper : BaseTagHelper
    {
        /// <summary>
        /// The type of the date modified element, either 'date' or 'version'.
        /// </summary>
        public DateModifiedType Type { get; set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "lang", Lang);
            AddAttributeIfNotNull(output, "type", Type);
            base.Process(context, output);
        }
    }
}
