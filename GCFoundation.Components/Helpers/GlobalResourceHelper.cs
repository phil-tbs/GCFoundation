using GCFoundation.Common.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace GCFoundation.Components.Helpers
{
    /// <summary>
    /// Helper class for managing global CSS, JavaScript, meta tags, and link tags from configuration.
    /// Provides methods to render these resources in Razor views.
    /// </summary>
    public class GlobalResourceHelper
    {
        private readonly GCFoundationComponentsSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalResourceHelper"/> class.
        /// </summary>
        /// <param name="settings">The foundation components settings.</param>
        public GlobalResourceHelper(IOptions<GCFoundationComponentsSettings> settings)
        {
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <summary>
        /// Renders all global CSS files as link tags.
        /// </summary>
        /// <returns>HTML string containing all global CSS link tags.</returns>
        public string RenderGlobalCssFiles()
        {
            var cssTags = new List<string>();
            
            foreach (var cssFile in _settings.GlobalCssFiles)
            {
                if (cssFile.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    cssTags.Add($"<link rel=\"stylesheet\" href=\"{cssFile}\" />");
                }
                else
                {
                    cssTags.Add($"<link rel=\"stylesheet\" href=\"{cssFile}\" asp-append-version=\"true\" />");
                }
            }
            
            return string.Join("\n    ", cssTags);
        }

        /// <summary>
        /// Renders all global JavaScript files as script tags.
        /// </summary>
        /// <returns>HTML string containing all global JavaScript script tags.</returns>
        public string RenderGlobalJavaScriptFiles()
        {
            var jsTags = new List<string>();
            
            foreach (var jsFile in _settings.GlobalJavaScriptFiles)
            {
                if (jsFile.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    jsTags.Add($"<script src=\"{jsFile}\"></script>");
                }
                else
                {
                    jsTags.Add($"<script src=\"{jsFile}\" asp-append-version=\"true\"></script>");
                }
            }
            
            return string.Join("\n    ", jsTags);
        }

        /// <summary>
        /// Renders all global meta tags.
        /// </summary>
        /// <returns>HTML string containing all global meta tags.</returns>
        public string RenderGlobalMetaTags()
        {
            return string.Join("\n    ", _settings.GlobalMetaTags);
        }

        /// <summary>
        /// Renders all global link tags.
        /// </summary>
        /// <returns>HTML string containing all global link tags.</returns>
        public string RenderGlobalLinkTags()
        {
            return string.Join("\n    ", _settings.GlobalLinkTags);
        }

        /// <summary>
        /// Gets all global CSS files as a list.
        /// </summary>
        /// <returns>Read-only collection of CSS file paths/URLs.</returns>
        public IReadOnlyCollection<string> GetGlobalCssFiles()
        {
            return _settings.GlobalCssFiles.AsReadOnly();
        }

        /// <summary>
        /// Gets all global JavaScript files as a list.
        /// </summary>
        /// <returns>Read-only collection of JavaScript file paths/URLs.</returns>
        public IReadOnlyCollection<string> GetGlobalJavaScriptFiles()
        {
            return _settings.GlobalJavaScriptFiles.AsReadOnly();
        }

        /// <summary>
        /// Gets all global meta tags as a list.
        /// </summary>
        /// <returns>Read-only collection of meta tag HTML strings.</returns>
        public IReadOnlyCollection<string> GetGlobalMetaTags()
        {
            return _settings.GlobalMetaTags.AsReadOnly();
        }

        /// <summary>
        /// Gets all global link tags as a list.
        /// </summary>
        /// <returns>Read-only collection of link tag HTML strings.</returns>
        public IReadOnlyCollection<string> GetGlobalLinkTags()
        {
            return _settings.GlobalLinkTags.AsReadOnly();
        }

        /// <summary>
        /// Checks if default CSS files should be included.
        /// </summary>
        /// <returns>Always returns true since default CSS is always included.</returns>
        [Obsolete("Default CSS files are now always included. This method always returns true and will be removed in a future version.")]
        public bool ShouldIncludeDefaultCss()
        {
            return true;
        }

        /// <summary>
        /// Checks if default JavaScript files should be included.
        /// </summary>
        /// <returns>Always returns true since default JavaScript is always included.</returns>
        [Obsolete("Default JavaScript files are now always included. This method always returns true and will be removed in a future version.")]
        public bool ShouldIncludeDefaultJavaScript()
        {
            return true;
        }

        /// <summary>
        /// Checks if GCDS resources should be included.
        /// </summary>
        /// <returns>True if GCDS resources should be included; otherwise, false.</returns>
        public bool ShouldIncludeGCDSResources()
        {
            return _settings.IncludeGCDSResources;
        }

        /// <summary>
        /// Checks if Font Awesome resources should be included.
        /// </summary>
        /// <returns>True if Font Awesome resources should be included; otherwise, false.</returns>
        public bool ShouldIncludeFontAwesome()
        {
            return _settings.IncludeFontAwesome;
        }
    }
} 