using System.Text.RegularExpressions;
using Foundation.Components.Utilities;
using Microsoft.AspNetCore.Http;

namespace Foundation.Components.Middleware
{
    

    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;

        public LanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            string languages = string.Join("|", LanguageUtility.GetSupportedCulture().Select(x => x.TwoLetterISOLanguageName).ToList());
            Regex regLocalization = new Regex($"^\\/({languages})\\/*");
            var path = context.Request.Path.Value;

            if(string.IsNullOrEmpty(path) || !regLocalization.IsMatch(path))
            {
                await _next(context);
                return;
            }

            // Extract culture
            string? culture = path?.Split('/')[1];


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

            await _next(context);
        }

    }
}
