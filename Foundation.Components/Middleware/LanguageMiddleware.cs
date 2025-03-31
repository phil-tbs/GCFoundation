using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var path = context.Request.Path.Value;

            if (string.IsNullOrEmpty(path) || path.StartsWith("/_content"))
            {
                await _next(context);
                return;
            }

            string? culture = path?.Split('/')[1];


            if (!string.IsNullOrEmpty(culture) && LanguageUtilitiy.IsCultureSupported(culture))
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
