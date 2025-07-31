# Global Resources Configuration

This document explains how to configure global CSS, JavaScript, meta tags, and link tags in GCFoundation using the `appsettings.json` configuration file.

## Overview

The GCFoundation framework now supports configurable global resources that can be set through the `FoundationComponentsSettings` section in your `appsettings.json` file. This allows you to:

- Add global CSS files (local or CDN)
- Add global JavaScript files (local or CDN)
- Add global meta tags for SEO and social media
- Add global link tags for performance optimization
- Control which optional resources are included

## Configuration Options

### Basic Settings

```json
{
  "FoundationComponentsSettings": {
    "ApplicationNameEn": "My Application",
    "ApplicationNameFr": "Mon Application",
    "SupportLinkEn": "mailto:support@example.com",
    "SupportLinkFr": "mailto:support@example.com"
  }
}
```

### Resource Control Settings

```json
{
  "FoundationComponentsSettings": {
    "IncludeGCDSResources": true,        // Include GCDS CSS and JS from CDN
    "IncludeFontAwesome": true           // Include Font Awesome from CDN
  }
}
```

**Note:** Default foundation CSS and JavaScript files (foundation.min.css, foundation.min.js, tabulator, prism) are **always included** and cannot be disabled. The `IncludeDefaultCss` and `IncludeDefaultJavaScript` settings are deprecated and no longer used.

### Global CSS Files

Add CSS files that will be included on every page:

```json
{
  "FoundationComponentsSettings": {
    "GlobalCssFiles": [
      "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css",
      "/css/custom-styles.css",
      "/css/print-styles.css"
    ]
  }
}
```

**Notes:**
- URLs starting with `http` or `https` are treated as CDN resources
- Local paths (starting with `/`) will have version appending enabled
- Files are loaded in the order specified
- **Default foundation CSS files are always loaded first**

### Global JavaScript Files

Add JavaScript files that will be included on every page:

```json
{
  "FoundationComponentsSettings": {
    "GlobalJavaScriptFiles": [
      "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js",
      "/js/custom-scripts.js",
      "/js/analytics.js"
    ]
  }
}
```

**Notes:**
- URLs starting with `http` or `https` are treated as CDN resources
- Local paths (starting with `/`) will have version appending enabled
- Files are loaded in the order specified
- Scripts are loaded at the end of the body tag
- **Default foundation JavaScript files are always loaded first**

### Global Meta Tags

Add meta tags for SEO, social media, and other purposes:

```json
{
  "FoundationComponentsSettings": {
    "GlobalMetaTags": [
      "<meta name=\"description\" content=\"My application description\">",
      "<meta name=\"keywords\" content=\"keyword1, keyword2, keyword3\">",
      "<meta name=\"author\" content=\"Your Name\">",
      "<meta name=\"robots\" content=\"index, follow\">",
      "<meta property=\"og:title\" content=\"My Application\">",
      "<meta property=\"og:description\" content=\"Application description for social media\">",
      "<meta property=\"og:type\" content=\"website\">",
      "<meta property=\"og:url\" content=\"https://example.com\">",
      "<meta property=\"og:image\" content=\"https://example.com/og-image.jpg\">",
      "<meta name=\"twitter:card\" content=\"summary_large_image\">",
      "<meta name=\"twitter:title\" content=\"My Application\">",
      "<meta name=\"twitter:description\" content=\"Application description for Twitter\">"
    ]
  }
}
```

### Global Link Tags

Add link tags for performance optimization and resource hints:

```json
{
  "FoundationComponentsSettings": {
    "GlobalLinkTags": [
      "<link rel=\"preconnect\" href=\"https://fonts.googleapis.com\">",
      "<link rel=\"preconnect\" href=\"https://fonts.gstatic.com\" crossorigin>",
      "<link rel=\"dns-prefetch\" href=\"https://cdn.design-system.alpha.canada.ca\">",
      "<link rel=\"preload\" href=\"/css/critical.css\" as=\"style\">",
      "<link rel=\"canonical\" href=\"https://example.com\">"
    ]
  }
}
```

## Complete Example

Here's a complete example of the `FoundationComponentsSettings` section:

```json
{
  "FoundationComponentsSettings": {
    "ApplicationNameEn": "GCFoundation Demo",
    "ApplicationNameFr": "GCFoundation Démo",
    "SupportLinkEn": "mailto:support@example.com",
    "SupportLinkFr": "mailto:support@example.com",
    "IncludeGCDSResources": true,
    "IncludeFontAwesome": true,
    "GlobalCssFiles": [
      "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css",
      "/css/custom-styles.css",
      "/css/print-styles.css"
    ],
    "GlobalJavaScriptFiles": [
      "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js",
      "/js/custom-scripts.js",
      "/js/analytics.js"
    ],
    "GlobalMetaTags": [
      "<meta name=\"description\" content=\"GCFoundation - A comprehensive foundation for building Government of Canada applications\">",
      "<meta name=\"keywords\" content=\"GC Design System, FDCP, Government of Canada, ASP.NET Core\">",
      "<meta name=\"author\" content=\"TBS-SCT\">",
      "<meta name=\"robots\" content=\"index, follow\">",
      "<meta property=\"og:title\" content=\"GCFoundation\">",
      "<meta property=\"og:description\" content=\"A comprehensive foundation for building Government of Canada applications\">",
      "<meta property=\"og:type\" content=\"website\">",
      "<meta property=\"og:url\" content=\"https://example.com\">",
      "<meta property=\"og:image\" content=\"https://example.com/og-image.jpg\">"
    ],
    "GlobalLinkTags": [
      "<link rel=\"preconnect\" href=\"https://fonts.googleapis.com\">",
      "<link rel=\"preconnect\" href=\"https://fonts.gstatic.com\" crossorigin>",
      "<link rel=\"dns-prefetch\" href=\"https://cdn.design-system.alpha.canada.ca\">",
      "<link rel=\"canonical\" href=\"https://example.com\">"
    ]
  }
}
```

## Using the GlobalResourceHelper in Views

You can also use the `GlobalResourceHelper` in your Razor views for more control:

```csharp
@inject GlobalResourceHelper ResourceHelper

@* Render all global CSS files *@
@Html.Raw(ResourceHelper.RenderGlobalCssFiles())

@* Render all global JavaScript files *@
@Html.Raw(ResourceHelper.RenderGlobalJavaScriptFiles())

@* Render all global meta tags *@
@Html.Raw(ResourceHelper.RenderGlobalMetaTags())

@* Render all global link tags *@
@Html.Raw(ResourceHelper.RenderGlobalLinkTags())

@* Check if specific resources should be included *@
@if (ResourceHelper.ShouldIncludeGCDSResources())
{
    <link rel="stylesheet" href="/css/gcds-override.css" />
}
```

## Resource Loading Order

The resources are loaded in the following order:

### CSS (in `<head>`)
1. **Default foundation CSS** (always included):
   - `tabulator/dist/css/GCTabulatorTheme.css`
   - `foundation.min.css`
   - `prism/css/prism.css`
2. GCDS CSS from CDN (if enabled)
3. Font Awesome CSS from CDN (if enabled)
4. Global CSS files from configuration
5. Page-specific styles (`@RenderSectionAsync("Styles")`)

### JavaScript (at end of `<body>`)
1. GCDS JavaScript from CDN (if enabled)
2. **Default foundation JavaScript** (always included):
   - `prism/js/prism.js`
   - `tabulator/dist/js/tabulator.min.js`
   - `foundation.min.js`
3. Global JavaScript files from configuration
4. Page-specific scripts (`@RenderSectionAsync("Scripts")`)

## Best Practices

1. **Performance**: Use CDN resources for popular libraries (Bootstrap, jQuery, etc.)
2. **Local Files**: Keep custom styles and scripts as local files for better control
3. **Meta Tags**: Include essential SEO meta tags and Open Graph tags for social media
4. **Link Tags**: Use preconnect and dns-prefetch for external resources
5. **Order**: CSS files are loaded in the head, JavaScript files at the end of body
6. **Versioning**: Local files automatically get version appending for cache busting
7. **Dependencies**: Default foundation files are always loaded first, so your custom files can override them if needed

## Security Considerations

- Be careful with CDN resources - ensure they come from trusted sources
- Consider using Subresource Integrity (SRI) for CDN resources
- Validate any user-provided content in meta tags and link tags
- Use HTTPS for all external resources

## Troubleshooting

1. **Resources not loading**: Check that the paths are correct and files exist
2. **CDN resources failing**: Verify the URLs are accessible and not blocked by firewalls
3. **Version appending not working**: Ensure local paths start with `/`
4. **Meta tags not appearing**: Check that the HTML is properly formatted
5. **CSS conflicts**: Default foundation CSS is always loaded first, so your custom CSS can override it 