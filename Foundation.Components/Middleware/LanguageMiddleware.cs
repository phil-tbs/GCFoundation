using System.Text.RegularExpressions;
using Foundation.Common.Utilities;
using Microsoft.AspNetCore.Http;

namespace Foundation.Components.Middleware
{

    /// <summary>
    /// Middleware for handling language selection based on the request URL and storing the selected language in a cookie.
    /// </summary>
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes the middleware with the next request delegate.
        /// </summary>
        /// <param name="next">The next request delegate in the pipeline.</param>
        public LanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Processes the HTTP request to check for a language code in the URL, stores the culture in a cookie, 
        /// and forwards the request to the next middleware in the pipeline.
        /// </summary>
        /// <param name="context">The HTTP context for the request.</param>
        /// <returns>A task representing the completion of the middleware processing.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            string languages = string.Join("|", LanguageUtility.GetSupportedCulture().Select(x => x.TwoLetterISOLanguageName).ToList());
            Regex regLocalization = new Regex($"^\\/({languages})\\/*");
            var path = context.Request.Path.Value;

            if (string.IsNullOrWhiteSpace(path) || !regLocalization.IsMatch(path))
            {
                await _next(context).ConfigureAwait(false);
                return;
            }

            // Extract culture
            string culture = path.Split('/')[1];


            if (!string.IsNullOrEmpty(culture) && LanguageUtility.IsCultureSupported(culture))
            {
                string? cultureCookie = context.Request.Cookies["Culture"];

                if (!string.IsNullOrEmpty(cultureCookie) || cultureCookie != culture)
                {
                    context.Response.Cookies.Append("Culture", culture, new CookieOptions
                    {
                        Expires = DateTime.UtcNow.AddDays(1),
                        IsEssential = true,
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                    });
                }
            }

            await _next(context).ConfigureAwait(false);
        }

    }
}
