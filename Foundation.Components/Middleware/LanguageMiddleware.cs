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
            string? culture = path?.Split('/').FirstOrDefault();


            if (!string.IsNullOrEmpty(culture) && LanguageUtilitiy.IsCultureSupported(culture))
            {

            }

            var cultureCookie = context.Request.Cookies["Culture"];



            if (!string.IsNullOrEmpty(cultureCookie))
            {
                
            }

            await _next(context);
        }

    }
}
