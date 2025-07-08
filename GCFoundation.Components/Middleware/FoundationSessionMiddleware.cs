using GCFoundation.Components.Setttings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace GCFoundation.Components.Middleware
{
    /// <summary>
    /// Middleware for managing session states and adding session-related headers to HTTP responses.
    /// </summary>
    public class FoundationSessionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly GCFoundationSessionSetting _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoundationSessionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next request delegate in the middleware pipeline.</param>
        /// <param name="options">The options containing session settings.</param>
        /// <exception cref="ArgumentNullException">Thrown when the options parameter is null.</exception>
        public FoundationSessionMiddleware(RequestDelegate next, IOptions<GCFoundationSessionSetting> options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            _next = next;
            _settings = options.Value;
        }

        /// <summary>
        /// Invokes the middleware to manage session states and add session-related headers to the response.
        /// </summary>
        /// <param name="context">The HTTP context for the current request.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the context parameter is null.</exception>
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
