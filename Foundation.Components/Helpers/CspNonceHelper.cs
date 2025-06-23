using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace Foundation.Components.Helpers
{
    /// <summary>
    /// Provides helper methods for generating and managing Content Security Policy (CSP) nonces for inline resources.
    /// This is useful for securely allowing inline scripts or styles in Razor Pages by attaching a unique nonce to the CSP header.
    /// </summary>
    public static class CspNonceHelper
    {
        private const string NonceKey = "CspNonce";

        /// <summary>
        /// Generates a cryptographically secure nonce, adds it to the specified CSP directive
        /// (<c>style-src</c> or <c>script-src</c>), and stores it in the current <see cref="HttpContext"/> for reuse.
        /// If a nonce has already been generated for the current request, it is reused and added to the specified directive.
        /// The rest of the existing Content Security Policy is preserved.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="directiveType">
        /// The CSP directive to add the nonce to. Use <see cref="DirectiveType.Style"/> for <c>style-src</c> or
        /// <see cref="DirectiveType.Script"/> for <c>script-src</c>.
        /// </param>
        /// <returns>
        /// The generated nonce as a base64-encoded string, suitable for use in inline <c>&lt;style&gt;</c> or <c>&lt;script&gt;</c> tags.
        /// </returns>
        /// <example>
        /// <code>
        /// var nonce = CspNonceHelper.AddNonceToDirective(context, DirectiveType.Style);
        /// // Use the nonce in a Razor Page:
        /// // &lt;style nonce="@nonce"&gt; ... &lt;/style&gt;
        /// </code>
        /// </example>
        public static string AddNonceToDirective(HttpContext context, DirectiveType directiveType)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));
            string directive = (directiveType == DirectiveType.Style) ? "style-src" : "script-src";

            if (context.Items.TryGetValue(NonceKey, out var existingNonce) && existingNonce is string cachedNonce)
            {
                AddNonceToHeader(context, directive, cachedNonce);
                return cachedNonce;
            }

            // Use cryptographically secure random number generator
            var nonceBytes = RandomNumberGenerator.GetBytes(16);
            var nonce = Convert.ToBase64String(nonceBytes);

            context.Items[NonceKey] = nonce;

            AddNonceToHeader(context, directive, nonce);

            return nonce;
        }

        /// <summary>
        /// Adds the specified nonce to the given CSP directive in the response header.
        /// If the directive does not exist, it is added. Existing policy directives are preserved.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="directive">The CSP directive name (e.g., "style-src" or "script-src").</param>
        /// <param name="nonce">The nonce value to add.</param>
        private static void AddNonceToHeader(HttpContext context, string directive, string nonce)
        {
            const string headerName = "Content-Security-Policy";
            var existingHeader = context.Response.Headers[headerName].ToString();

            var nonceValue = $"'nonce-{nonce}'";

            if (string.IsNullOrWhiteSpace(existingHeader))
            {
                context.Response.Headers[headerName] = $"{directive} 'self' {nonceValue}";
                return;
            }

            var directives = existingHeader.Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(d => d.Trim())
                .ToList();

            var dirIndex = directives.FindIndex(d => d.StartsWith(directive, StringComparison.Ordinal));

            if (dirIndex >= 0)
            {
                var dirParts = directives[dirIndex].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

                if (!dirParts.Contains(nonceValue))
                {
                    dirParts.Add(nonceValue);
                    directives[dirIndex] = string.Join(' ', dirParts);
                }
            }
            else
            {
                directives.Add($"{directive} 'self' {nonceValue}");
            }

            context.Response.Headers[headerName] = string.Join("; ", directives);
        }
    }

    /// <summary>
    /// Specifies the CSP directive type to which a nonce should be added.
    /// </summary>
    public enum DirectiveType
    {
        /// <summary>
        /// The <c>style-src</c> directive, used for inline styles.
        /// </summary>
        Style,

        /// <summary>
        /// The <c>script-src</c> directive, used for inline scripts.
        /// </summary>
        Script
    }
}
