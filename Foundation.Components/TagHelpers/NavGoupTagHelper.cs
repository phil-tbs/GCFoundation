﻿using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers
{
    /// <summary>
    /// Represents a navigation group for use in a webpage's navigation structure.
    /// This group can contain multiple links and can be dynamically opened or closed.
    /// </summary>
    [HtmlTargetElement("gcds-nav-group")]
    public class NavGoupTagHelper : BaseTagHelper
    {
        /// <summary>
        /// Gets or sets the label for the navigation menu group.
        /// </summary>
        /// <value>The label to display for the navigation group.</value>
        public required string MenuLabel { get; set; }

        /// <summary>
        /// Gets or sets the trigger element or event that opens the navigation group.
        /// </summary>
        /// <value>The trigger that opens the navigation group.</value>
        public required string OpenTrigger { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the navigation group is currently open.
        /// </summary>
        /// <value>True if the navigation group is open; otherwise, false. The default is false.</value>
        public bool Open { get; set; }

        /// <summary>
        /// Gets or sets the trigger element or event that closes the navigation group.
        /// </summary>
        /// <value>The trigger that closes the navigation group, if specified. Otherwise, the default behavior will be used.</value>
        public string? CloseTrigger { get; set; }

        /// <inheritdoc/>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            AddAttributeIfNotNull(output, "menu-label", MenuLabel);
            AddAttributeIfNotNull(output, "open-trigger", OpenTrigger);
            AddAttributeIfNotNull(output, "open", Open);
            AddAttributeIfNotNull(output, "close-trigger", CloseTrigger);
            base.Process(context, output);
        }

    }
}
