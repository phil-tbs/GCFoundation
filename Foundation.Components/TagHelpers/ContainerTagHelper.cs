using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Enums;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    /// <summary>
    /// A tag helper for rendering a container component with optional styling and layout properties.
    /// This component can be customized with attributes like border, centering, margin, padding, and size.
    /// </summary>
    [HtmlTargetElement("gcds-container")]
    public class ContainerTagHelper : BaseTagHelper
    {
        /// <summary>
        /// If set to true, adds a border around the container.
        /// </summary>
        public bool Border { get; set; }

        /// <summary>
        /// If set to true, centers the content inside the container.
        /// </summary>
        public bool Centered { get; set; }

        /// <summary>
        /// If set to true, marks the container as the main container of the page or section.
        /// </summary>
        public bool MainContainer { get; set; }

        /// <summary>
        /// Defines the margin of the container (can be a CSS unit like "px", "em", etc.).
        /// </summary>
        public string? Margin { get; set; }

        /// <summary>
        /// Defines the padding of the container. Default value is "300".
        /// </summary>
        public string? Padding { get; set; } = "300";

        /// <summary>
        /// Defines the size of the container. Default is <see cref="SizeTypeEmum.lg"/>.
        /// </summary>
        public SizeTypeEmum Size { get; set; } = SizeTypeEmum.lg;

        /// <summary>
        /// Specifies the tag name to be used for the container (e.g., div, section, etc.).
        /// </summary>
        public string? Tag {  get; set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "border", Border);
            AddAttributeIfNotNull(output, "centered", Centered);
            AddAttributeIfNotNull(output, "main-container", MainContainer);
            AddAttributeIfNotNull(output, "margin", Margin);
            AddAttributeIfNotNull(output, "padding", Padding);
            AddAttributeIfNotNull(output, "size", Size);
            AddAttributeIfNotNull(output, "tag", Tag);
            base.Process(context, output);
        }

    }
}
