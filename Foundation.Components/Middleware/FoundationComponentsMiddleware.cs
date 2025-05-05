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
            ArgumentNullException.ThrowIfNull(foundationComponentsSettings, nameof(foundationComponentsSettings));

            _next = next;
            _foundationComponentsSettings = foundationComponentsSettings.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            var originalBodyStream = context.Response.Body;

            using (var newBodyStream = new MemoryStream())
            {
                context.Response.Body = newBodyStream;

                await _next(context).ConfigureAwait(false);

                if (context.Response.ContentType?.Contains("text/html", StringComparison.OrdinalIgnoreCase) == true)
                {
                    // Get the right url to the CDN or the local folder
                    string bootstapCss = _foundationComponentsSettings.UsingBootstrapCDN ? _foundationComponentsSettings.BootstrapCSSCDN.ToString() : StaticResourceUtility.GetLibResourcePath("bootstrap/css/bootstrap.min.css");
                    string bootstapJs = _foundationComponentsSettings.UsingBootstrapCDN ? _foundationComponentsSettings.BootstrapJSCDN.ToString() : StaticResourceUtility.GetLibResourcePath("bootstrap/js/bootstrap.min.js");

                    string bootStrapHtml = @$"<link rel=""stylesheet"" href=""{bootstapCss}"">";

                    newBodyStream.Seek(0, SeekOrigin.Begin);
                    using (var reader = new StreamReader(newBodyStream))
                    {
                        var html = await reader.ReadToEndAsync().ConfigureAwait(false);
                        html = html.Replace("</head>", @$"
                        <link rel=""stylesheet"" href=""{_foundationComponentsSettings.FontAwesomeCDN}"" crossorigin=""anonymous"">
                        <link rel=""stylesheet"" href=""{_foundationComponentsSettings.GCDSCssCDN}"">
                        {bootStrapHtml}
                        <script type=""module"" src=""{_foundationComponentsSettings.GCDSJavaScriptCDN}""></script>
                    </head>", StringComparison.OrdinalIgnoreCase);

                        html = html.Replace("</body>", $@"
                            <script src=""{bootstapJs}""></script>
                            </body>", StringComparison.OrdinalIgnoreCase);

                        var modifiedHtml = Encoding.UTF8.GetBytes(html);
                        ReadOnlyMemory<byte> memory = new ReadOnlyMemory<byte>(modifiedHtml);
                        context.Response.Body = originalBodyStream;
                        await context.Response.Body.WriteAsync(memory, CancellationToken.None).ConfigureAwait(false);
                    }
                }
                else
                {
                    newBodyStream.Seek(0, SeekOrigin.Begin);
                    await newBodyStream.CopyToAsync(originalBodyStream).ConfigureAwait(false);
                }
            }
        }
    }
}
