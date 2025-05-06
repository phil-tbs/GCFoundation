using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enum;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    /// <summary>
    /// A tag helper for rendering a date input field component with a specified format and optional legend.
    /// </summary>
    [HtmlTargetElement("gcds-date-input")]
    public class DateInputTagHelper : BaseFormComponentTagHelper
    {
        /// <summary>
        /// The date format to use for the input field.
        /// </summary>
        public required DateInputFormatType Format { get; set; }

        /// <summary>
        /// The legend or label to display above the date input field.
        /// </summary>
        public required string Legend { get; set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "format", Format);
            AddAttributeIfNotNull(output, "legend", Legend);
            base.Process(context, output);
        }
    }
}
