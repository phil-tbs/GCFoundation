﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Components.TagHelpers.FDCP
{
    /// <summary>
    /// A TagHelper that renders a session modal for managing session timeouts and reminders.
    /// It extends the <see cref="FDCPModalTagHelper"/> and adds additional attributes for session handling.
    /// </summary>
    [HtmlTargetElement("fdcp-session-modal")]
    public class FDCPSessionModalTagHelper: FDCPModalTagHelper
    {
        /// <summary>
        /// The session timeout in seconds. This is the time after which the session will expire.
        /// </summary>
        public int SessionTimeout { get; set; }

        /// <summary>
        /// The time in seconds before the session timeout, at which a reminder should be shown to the user.
        /// </summary>
        public int ReminderTime { get; set; }

        /// <summary>
        /// The URL to refresh the session. This is called when the session is about to expire.
        /// </summary>
        public Uri RefreshURL { get; set; } = default!;

        /// <summary>
        /// The URL to log out the user. This is called when the session expires or the user chooses to log out.
        /// </summary>
        public Uri LogoutURL { get; set; } = default!;

        /// <inheritdoc/>
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            ArgumentNullException.ThrowIfNull(output, nameof(output));

            output.Attributes.SetAttribute("data-session-timeout", SessionTimeout);
            output.Attributes.SetAttribute("data-reminder-time", ReminderTime);
            output.Attributes.SetAttribute("data-refresh", RefreshURL);
            output.Attributes.SetAttribute("data-logout", LogoutURL);


            return base.ProcessAsync(context, output);
        }
    }
}
