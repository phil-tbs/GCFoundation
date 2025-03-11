using System.Security.Cryptography;
using Foundation.Security.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Foundation.Security.Middlewares
{
    public class FoundationContentPoliciesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ContentPolicySettings _settings;

        public FoundationContentPoliciesMiddleware(RequestDelegate next, IOptions<ContentPolicySettings> settings)
        {
            _next = next;
            _settings = settings.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Generate a nonce for inline styles/scripts (if needed)
            string nonce = GenerateNonce();
            context.Items["CspNonce"] = nonce; // Store for use in views (if required)

            // Convert lists to space-separated strings
            string cssCDN = string.Join(" ", _settings.CssCDN ?? Enumerable.Empty<string>());
            string jsCDN = string.Join(" ", _settings.JavascriptCDN ?? Enumerable.Empty<string>());
            string fontCDN = string.Join(" ", _settings.FontCDN ?? Enumerable.Empty<string>());
            string cssHash = string.Join(" ", _settings.CssCDNHash ?? Enumerable.Empty<string>());

            // Build Content Security Policy (CSP)
            string contentSecurityPolicy = $"default-src 'self'; " +
                               $"script-src 'self' {jsCDN} 'nonce-{nonce}'; " +
                               $"object-src 'none'; " +
                               $"style-src 'self' {cssCDN} {cssHash} 'nonce-{nonce}'; " +
                               $"font-src 'self' {fontCDN}; " +
                               $"connect-src 'self' http://localhost:* https://localhost:* ws://localhost:* wss://localhost:*; " +
                               $"img-src 'self' data:; " + 
                               $"frame-ancestors 'none'; " +
                               $"upgrade-insecure-requests;";

            // Set security headers
            context.Response.Headers.Append("Content-Security-Policy", contentSecurityPolicy);
            context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains"); // 1 year HSTS
            context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
            context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
            context.Response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=(), camera=()");
            context.Response.Headers.Append("Expect-CT", "max-age=86400, enforce");
            context.Response.Headers.Append("Cache-Control", "no-store, no-cache, must-revalidate, proxy-revalidate");
            context.Response.Headers.Append("Content-Type", "text/html; charset=utf-8");

            await _next(context);
        }

        private static string GenerateNonce()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] nonceBytes = new byte[16];
                rng.GetBytes(nonceBytes);
                return Convert.ToBase64String(nonceBytes);
            }
        }
    }
}
