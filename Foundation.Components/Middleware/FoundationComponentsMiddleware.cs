using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Setttings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Foundation.Components.Middleware
{
    public class FoundationComponentsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly FoundationComponentsSettings _foundationComponentsSettings;

        public FoundationComponentsMiddleware(RequestDelegate next, IOptions<FoundationComponentsSettings> foundationComponentsSettings)
        {
            _next = next;
            _foundationComponentsSettings = foundationComponentsSettings.Value;
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
                    html = html.Replace("</head>", @$"
                        <link rel=""stylesheet"" href=""{_foundationComponentsSettings.FontAwesomeCDN}"" crossorigin=""anonymous"">
                        <link rel=""stylesheet"" href=""{_foundationComponentsSettings.GCDSCssCDN}"">
                        <script type=""module"" src=""{_foundationComponentsSettings.GCDSJavaScriptCDN}""></script>
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
