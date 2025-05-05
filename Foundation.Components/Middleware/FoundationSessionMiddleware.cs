using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Setttings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Foundation.Components.Middleware
{
    public class FoundationSessionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly FoundationSessionSetting _settings;

        public FoundationSessionMiddleware(RequestDelegate next, IOptions<FoundationSessionSetting> options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            _next = next;
            _settings = options.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            if (_settings.UseSession)
            {
                context.Session.SetString("SessionActive", "true");
            }

            // Optionally expose timing info in headers for JS to read
            if (_settings.UseReminder)
            {
                context.Response.Headers["X-Session-Timeout"] = _settings.SessionTimeout.ToString(CultureInfo.InvariantCulture);
                context.Response.Headers["X-Session-Reminder"] = _settings.ReminderTime.ToString(CultureInfo.InvariantCulture);
            }

            await _next(context).ConfigureAwait(false);
        }
    }
}
