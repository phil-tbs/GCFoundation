﻿using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GCFoundation.Components.TagHelpers.GCDS
{
    /// <summary>
    /// 
    /// </summary>
    [HtmlTargetElement("gcds-icon")]
    public class IconTagHelper : BaseTagHelper
    {
        /// <summary>
        /// Gets or sets to determine the icon Image to display.
        /// info-circle,warning-triangle,exclamation-circle,checkmark-circle,
        /// chevron-(left,right, up,down), close, download, email, external,
        /// phone, search
        /// </summary>
        public required string Name { get; set; }
        /// <summary>
        /// Gets or sets the label property for an icon with no accompanying text.
        /// </summary>
        public string? Label { get; set; }
        /// <summary>
        /// Add a margin to the lefft of an icon by setting the margin-left 
        /// attribute to a spacing value.
        /// ex) 0,25,50, 75,100 ,..., 1250
        /// </summary>
        public string? MarginLeft { get; set; }
        /// <summary>
        /// Add a margin to the right of an icon by setting the margin-right 
        /// attribute to a spacing value.
        /// ex) 0,25,50, 75,100 ,..., 1250
        /// </summary>
        public string? MarginRight { get; set; }
        /// <summary>
        /// Change the size of an icon by setting the size attribute to a specific font size, 
        /// like text-small | text |  h1 | h2 | h3 | h4 | h5 | h6.
        /// </summary>
        public string? Size { get; set; }

        /// <summary>
        /// Processes the <c>gcds-icon</c> element by adding the <c>name</c>, <c>label</c>, <c>margin-left</c>, <c>margin_right</c>, 
        /// <c>size</c> attributes to the rendered output.
        /// </summary>
        /// <param name="context">The context for the tag helper.</param>
        /// <param name="output">The HTML element output generated by the tag helper.</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "name", Name);
            AddAttributeIfNotNull(output, "label", Label);
            AddAttributeIfNotNull(output, "margin-left", MarginLeft);
            AddAttributeIfNotNull(output, "margin-right", MarginRight);
            AddAttributeIfNotNull(output, "size", Size);

            base.Process(context, output);
        }
    }
}
