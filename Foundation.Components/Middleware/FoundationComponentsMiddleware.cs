using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Common.Settings;
using Foundation.Components.Setttings;
using Foundation.Components.Utilities;
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
                    // Get the right url to the CDN or the local folder
                    string bootstapCss = _foundationComponentsSettings.UsingBootstrapCDN ? _foundationComponentsSettings.BootstrapCSSCDN.ToString() : StaticResourceUtility.GetLibResourcePath("bootstrap/css/bootstrap.min.css");
                    string bootstapJs = _foundationComponentsSettings.UsingBootstrapCDN ? _foundationComponentsSettings.BootstrapJSCDN.ToString() : StaticResourceUtility.GetLibResourcePath("bootstrap/js/bootstrap.min.js");

                    string bootStrapHtml = @$"<link rel=""stylesheet"" href=""{bootstapCss}"">";

                    newBodyStream.Seek(0, SeekOrigin.Begin);
                    var html = await new StreamReader(newBodyStream).ReadToEndAsync();
                    html = html.Replace("</head>", @$"
                        <link rel=""stylesheet"" href=""{_foundationComponentsSettings.FontAwesomeCDN}"" crossorigin=""anonymous"">
                        <link rel=""stylesheet"" href=""{_foundationComponentsSettings.GCDSCssCDN}"">
                        {bootStrapHtml}
                        <script type=""module"" src=""{_foundationComponentsSettings.GCDSJavaScriptCDN}""></script>
                    </head>");

                    html = html.Replace("</body>", $@"
                            <script src=""{bootstapJs}""></script>
                            </body>");

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
