using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Foundation.Components.Middleware
{
    public class FoundationComponentsMiddleware
    {
        private readonly RequestDelegate _next;

        public FoundationComponentsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;

            using (var newBodyStream = new MemoryStream())
            {
                context.Response.Body = newBodyStream;

                await _next(context);

                if (context.Response.ContentType?.Contains("text/html") == true)
                {
                    newBodyStream.Seek(0, SeekOrigin.Begin);
                    var html = await new StreamReader(newBodyStream).ReadToEndAsync();
                    html = html.Replace("</head>", @"
                        <link rel=""stylesheet"" href=""https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css"" crossorigin=""anonymous"">
                        <link rel=""stylesheet"" href=""https://cdn.design-system.alpha.canada.ca/@cdssnc/gcds-components@0.30.0/dist/gcds/gcds.css"">
                        <script type=""module"" src=""https://cdn.design-system.alpha.canada.ca/@cdssnc/gcds-components@0.30.0/dist/gcds/gcds.esm.js""></script>
                    </head>");
                    var modifiedHtml = Encoding.UTF8.GetBytes(html);
                    context.Response.Body = originalBodyStream;
                    await context.Response.Body.WriteAsync(modifiedHtml, 0, modifiedHtml.Length);
                }
                else
                {
                    newBodyStream.Seek(0, SeekOrigin.Begin);
                    await newBodyStream.CopyToAsync(originalBodyStream);
                }
            }
        }
    }
}
