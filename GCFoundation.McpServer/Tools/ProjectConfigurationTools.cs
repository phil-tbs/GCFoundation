using System.ComponentModel;
using System.Text;
using System.Text.Json;
using System.Net.Http;
using ModelContextProtocol.Server;

/// <summary>
/// MCP tools for configuring and scaffolding GC Foundation projects.
/// Provides setup templates, middleware configuration, and project structure generation.
/// </summary>
internal class ProjectConfigurationTools
{
    #region NuGet Package Management

    [McpServerTool]
    [Description("Queries a custom NuGet feed to get the latest version of GCFoundation packages.")]
    public async Task<string> GetLatestGCFoundationVersions(
        [Description("Custom NuGet feed URL")] string feedUrl = "https://pkgs.dev.azure.com/tbs-sct/_packaging/TBS_Custom_Feed/nuget/v3/index.json",
        [Description("Package names to query (comma-separated)")] string packageNames = "GCFoundation.Common,GCFoundation.Components,GCFoundation.Security")
    {
        var sb = new StringBuilder();
        var packages = packageNames.Split(',', StringSplitOptions.RemoveEmptyEntries);
        
        sb.AppendLine("# GC Foundation Package Versions");
        sb.AppendLine();
        sb.AppendLine($"**Feed URL**: `{feedUrl}`");
        sb.AppendLine();
        
        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(30);
        
        var packageVersions = new Dictionary<string, string>();
        
        foreach (var packageName in packages)
        {
            try
            {
                var trimmedPackageName = packageName.Trim();
                sb.AppendLine($"## {trimmedPackageName}");
                
                // Try NuGet V3 API format first
                var registrationUrl = $"{feedUrl.TrimEnd('/')}/registration/{trimmedPackageName.ToLowerInvariant()}/index.json";
                
                try
                {
                    var response = await httpClient.GetStringAsync(registrationUrl);
                    var registrationData = JsonDocument.Parse(response);
                    
                    if (registrationData.RootElement.TryGetProperty("items", out var items) && items.GetArrayLength() > 0)
                    {
                        var latestItem = items[items.GetArrayLength() - 1];
                        if (latestItem.TryGetProperty("upper", out var upperVersion))
                        {
                            var version = upperVersion.GetString();
                            if (!string.IsNullOrEmpty(version))
                                packageVersions[trimmedPackageName] = version;
                            sb.AppendLine($"- **Latest Version**: `{version}`");
                            sb.AppendLine($"- **Registration URL**: `{registrationUrl}`");
                        }
                    }
                }
                catch
                {
                    // Fallback to search API
                    var searchUrl = $"{feedUrl.TrimEnd('/')}/query?q={trimmedPackageName}&take=1";
                    
                    try
                    {
                        var searchResponse = await httpClient.GetStringAsync(searchUrl);
                        var searchData = JsonDocument.Parse(searchResponse);
                        
                        if (searchData.RootElement.TryGetProperty("data", out var searchResults) && searchResults.GetArrayLength() > 0)
                        {
                            var package = searchResults[0];
                            if (package.TryGetProperty("version", out var versionElement))
                            {
                                var version = versionElement.GetString();
                                if (!string.IsNullOrEmpty(version))
                                    packageVersions[trimmedPackageName] = version;
                                sb.AppendLine($"- **Latest Version**: `{version}`");
                                sb.AppendLine($"- **Search URL**: `{searchUrl}`");
                            }
                            else if (package.TryGetProperty("versions", out var versions) && versions.GetArrayLength() > 0)
                            {
                                var latestVersion = versions[versions.GetArrayLength() - 1];
                                if (latestVersion.TryGetProperty("version", out var latestVersionElement))
                                {
                                    var version = latestVersionElement.GetString();
                                    if (!string.IsNullOrEmpty(version))
                                        packageVersions[trimmedPackageName] = version;
                                    sb.AppendLine($"- **Latest Version**: `{version}`");
                                    sb.AppendLine($"- **Search URL**: `{searchUrl}`");
                                }
                            }
                        }
                    }
                    catch (Exception searchEx)
                    {
                        sb.AppendLine($"- **Error**: Could not retrieve version information");
                        sb.AppendLine($"- **Search Error**: {searchEx.Message}");
                        packageVersions[trimmedPackageName] = "1.0.0"; // Default fallback version
                    }
                }
                
                sb.AppendLine();
            }
            catch (Exception ex)
            {
                sb.AppendLine($"- **Error**: {ex.Message}");
                sb.AppendLine();
                packageVersions[packageName.Trim()] = "1.0.0"; // Default fallback version
            }
        }
        
        sb.AppendLine("## Package References for .csproj");
        sb.AppendLine();
        sb.AppendLine("```xml");
        sb.AppendLine("<ItemGroup>");
        sb.AppendLine("  <!-- GC Foundation Core Packages -->");
        
        foreach (var kvp in packageVersions)
        {
            sb.AppendLine($"  <PackageReference Include=\"{kvp.Key}\" Version=\"{kvp.Value}\" />");
        }
        
        sb.AppendLine("</ItemGroup>");
        sb.AppendLine("```");
        sb.AppendLine();
        
        sb.AppendLine("## NuGet.config for Custom Feed");
        sb.AppendLine();
        sb.AppendLine("```xml");
        sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        sb.AppendLine("<configuration>");
        sb.AppendLine("  <packageSources>");
        sb.AppendLine("    <add key=\"nuget.org\" value=\"https://api.nuget.org/v3/index.json\" protocolVersion=\"3\" />");
        sb.AppendLine($"    <add key=\"GCFoundation\" value=\"{feedUrl}\" protocolVersion=\"3\" />");
        sb.AppendLine("  </packageSources>");
        sb.AppendLine("</configuration>");
        sb.AppendLine("```");
        
        return sb.ToString();
    }

    /// <summary>
    /// Helper method to get just the package versions without formatting
    /// </summary>
    private async Task<Dictionary<string, string>> GetGCFoundationPackageVersionsOnly(string feedUrl)
    {
        var packageVersions = new Dictionary<string, string>();
        var packages = new[] { "GCFoundation.Common", "GCFoundation.Components", "GCFoundation.Security" };
        
        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(30);
        
        foreach (var packageName in packages)
        {
            try
            {
                // Try NuGet V3 API format first
                var registrationUrl = $"{feedUrl.TrimEnd('/')}/registration/{packageName.ToLowerInvariant()}/index.json";
                
                try
                {
                    var response = await httpClient.GetStringAsync(registrationUrl);
                    var registrationData = JsonDocument.Parse(response);
                    
                    if (registrationData.RootElement.TryGetProperty("items", out var items) && items.GetArrayLength() > 0)
                    {
                        var latestItem = items[items.GetArrayLength() - 1];
                        if (latestItem.TryGetProperty("upper", out var upperVersion))
                        {
                            var version = upperVersion.GetString();
                            if (!string.IsNullOrEmpty(version))
                            {
                                packageVersions[packageName] = version;
                                continue;
                            }
                        }
                    }
                }
                catch
                {
                    // Fallback to search API
                    var searchUrl = $"{feedUrl.TrimEnd('/')}/query?q={packageName}&take=1";
                    
                    try
                    {
                        var searchResponse = await httpClient.GetStringAsync(searchUrl);
                        var searchData = JsonDocument.Parse(searchResponse);
                        
                        if (searchData.RootElement.TryGetProperty("data", out var searchResults) && searchResults.GetArrayLength() > 0)
                        {
                            var package = searchResults[0];
                            if (package.TryGetProperty("version", out var versionElement))
                            {
                                var version = versionElement.GetString();
                                if (!string.IsNullOrEmpty(version))
                                {
                                    packageVersions[packageName] = version;
                                    continue;
                                }
                            }
                            else if (package.TryGetProperty("versions", out var versions) && versions.GetArrayLength() > 0)
                            {
                                var latestVersion = versions[versions.GetArrayLength() - 1];
                                if (latestVersion.TryGetProperty("version", out var latestVersionElement))
                                {
                                    var version = latestVersionElement.GetString();
                                    if (!string.IsNullOrEmpty(version))
                                    {
                                        packageVersions[packageName] = version;
                                        continue;
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Silent fallback
                    }
                }
                
                // Default fallback version
                packageVersions[packageName] = "1.0.0";
            }
            catch
            {
                // Default fallback version
                packageVersions[packageName] = "1.0.0";
            }
        }
        
        return packageVersions;
    }

    /// <summary>
    /// Helper method to generate project file with specific versions
    /// </summary>
    private string GenerateProjectFileWithVersions(
        string projectName, 
        string targetFramework, 
        bool includeDataLayer, 
        bool includeAuthentication, 
        bool includeTests,
        Dictionary<string, string> gcFoundationVersions)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"<Project Sdk=\"Microsoft.NET.Sdk.Web\">");
        sb.AppendLine();
        sb.AppendLine("  <PropertyGroup>");
        sb.AppendLine($"    <TargetFramework>{targetFramework}</TargetFramework>");
        sb.AppendLine("    <Nullable>enable</Nullable>");
        sb.AppendLine("    <ImplicitUsings>enable</ImplicitUsings>");
        sb.AppendLine("  </PropertyGroup>");
        sb.AppendLine();
        
        sb.AppendLine("  <ItemGroup>");
        sb.AppendLine("    <!-- GC Foundation Core Packages (Latest Versions) -->");
        sb.AppendLine($"    <PackageReference Include=\"GCFoundation.Common\" Version=\"{gcFoundationVersions.GetValueOrDefault("GCFoundation.Common", "1.0.0")}\" />");
        sb.AppendLine($"    <PackageReference Include=\"GCFoundation.Components\" Version=\"{gcFoundationVersions.GetValueOrDefault("GCFoundation.Components", "1.0.0")}\" />");
        sb.AppendLine($"    <PackageReference Include=\"GCFoundation.Security\" Version=\"{gcFoundationVersions.GetValueOrDefault("GCFoundation.Security", "1.0.0")}\" />");
        sb.AppendLine();
        
        sb.AppendLine("    <!-- Localization and Navigation -->");
        sb.AppendLine("    <PackageReference Include=\"cloudscribe.Web.Navigation\" Version=\"8.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"cloudscribe.Web.Localization\" Version=\"8.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"cloudscribe.Web.SiteMap\" Version=\"8.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"cloudscribe.Web.SiteMap.FromNavigation\" Version=\"8.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"RouteLocalization.AspNetCore\" Version=\"1.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"Microsoft.Extensions.Localization\" Version=\"9.0.2\" />");
        sb.AppendLine();
        
        if (includeAuthentication)
        {
            sb.AppendLine("    <!-- Authentication (Cookie authentication built into .NET 8) -->");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.AspNetCore.Authentication.OpenIdConnect\" Version=\"8.0.0\" />");
            sb.AppendLine();
        }
        
        if (includeDataLayer)
        {
            sb.AppendLine("    <!-- Entity Framework Core -->");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.EntityFrameworkCore\" Version=\"8.0.0\" />");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.EntityFrameworkCore.SqlServer\" Version=\"8.0.0\" />");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.EntityFrameworkCore.Tools\" Version=\"8.0.0\" />");
            sb.AppendLine();
        }
        
        if (includeTests)
        {
            sb.AppendLine("    <!-- Testing -->");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.AspNetCore.Mvc.Testing\" Version=\"8.0.0\" />");
            sb.AppendLine();
        }
        
        sb.AppendLine("  </ItemGroup>");
        sb.AppendLine();
        sb.AppendLine("</Project>");
        
        return sb.ToString();
    }

    #endregion

    #region Project Setup Tools

    [McpServerTool]
    [Description("Generates a complete Program.cs file configured for GC Foundation with all middleware and services.")]
    public string GenerateGCFoundationProgramCs(
        [Description("Project name for namespace generation")] string projectName = "YourProject",
        [Description("Include authentication setup")] bool includeAuthentication = true,
        [Description("Include localization support")] bool includeLocalization = true,
        [Description("Include navigation services")] bool includeNavigation = true,
        [Description("Include session management")] bool includeSession = true,
        [Description("Include content security policies")] bool includeCSP = true,
        [Description("Authentication scheme name")] string authScheme = "DemoAuth",
        [Description("Cookie name for authentication")] string cookieName = "GCFoundationAuth",
        [Description("Login path")] string loginPath = "/account/login",
        [Description("Logout path")] string logoutPath = "/account/logout",
        [Description("Default culture")] string defaultCulture = "en-CA")
    {
        var sb = new StringBuilder();
        
        // Using statements
        sb.AppendLine("using GCFoundation.Common.Utilities;");
        sb.AppendLine("using GCFoundation.Components.Middleware;");
        sb.AppendLine("using GCFoundation.Components.Services.Interfaces;");
        sb.AppendLine("using GCFoundation.Security.Middlewares;");
        sb.AppendLine($"using {projectName}.Infrastructure.Extensions;");
        
        if (includeNavigation)
        {
            sb.AppendLine("using cloudscribe.Web.Localization;");
            sb.AppendLine("using cloudscribe.Web.SiteMap;");
            sb.AppendLine("using GCFoundation.Components.Services;");
        }
        
        if (includeAuthentication)
        {
            sb.AppendLine("using Microsoft.AspNetCore.CookiePolicy;");
        }
        
        if (includeLocalization)
        {
            sb.AppendLine("using Microsoft.AspNetCore.Localization;");
            sb.AppendLine("using Microsoft.AspNetCore.Mvc.Razor;");
            sb.AppendLine("using Microsoft.Extensions.Localization;");
            sb.AppendLine("using Microsoft.Extensions.Options;");
        }
        
        sb.AppendLine();
        sb.AppendLine("var builder = WebApplication.CreateBuilder(args);");
        sb.AppendLine();
        
        // Add services
        sb.AppendLine("// Add services to the container.");
        sb.AppendLine("builder.Services.AddControllersWithViews()");
        
        if (includeLocalization)
        {
            sb.AppendLine("    .AddViewLocalization()");
            sb.AppendLine("    .AddDataAnnotationsLocalization();");
        }
        else
        {
            sb.AppendLine(";");
        }
        sb.AppendLine();
        
        // Authentication setup
        if (includeAuthentication)
        {
            sb.AppendLine($"// Add authentication");
            sb.AppendLine($"builder.Services.AddAuthentication(\"{authScheme}\")");
            sb.AppendLine($"    .AddCookie(\"{authScheme}\", options =>");
            sb.AppendLine("    {");
            sb.AppendLine($"        options.LoginPath = \"{loginPath}\";");
            sb.AppendLine($"        options.LogoutPath = \"{logoutPath}\";");
            sb.AppendLine("        options.ExpireTimeSpan = TimeSpan.FromHours(1);");
            sb.AppendLine("        options.SlidingExpiration = false;");
            sb.AppendLine($"        options.Cookie.Name = \"{cookieName}\";");
            sb.AppendLine("        options.Cookie.HttpOnly = true;");
            sb.AppendLine("        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;");
            sb.AppendLine("    });");
            sb.AppendLine();
        }
        
        // Navigation services
        if (includeNavigation)
        {
            sb.AppendLine("// Add navigation services");
            sb.AppendLine("builder.Services.AddScoped<ISiteMapNodeService, NavigationTreeSiteMapNodeService>();");
            sb.AppendLine("builder.Services.AddCloudscribeNavigation(builder.Configuration.GetSection(\"NavigationOptions\"));");
            sb.AppendLine();
        }
        
        // Localization setup
        if (includeLocalization)
        {
            sb.AppendLine("// Localization configuration");
            sb.AppendLine("builder.Services.Configure<GlobalResourceOptions>(builder.Configuration.GetSection(\"GlobalResourceOptions\"));");
            sb.AppendLine("builder.Services.AddSingleton<IStringLocalizerFactory, GlobalResourceManagerStringLocalizerFactory>();");
            sb.AppendLine("builder.Services.AddLocalization();");
            sb.AppendLine();
            
            sb.AppendLine("// Configure breadcrumbs localization service");
            sb.AppendLine($"builder.Services.AddSingleton(typeof(IBreadcrumbsLocalizationService), typeof(BreadcrumbsLocalizationService<{projectName.Replace(".", "")}.Resources.Navigation>));");
            sb.AppendLine();
        }
        
        // GC Foundation services
        sb.AppendLine("// Configure GC Foundation services");
        sb.AppendLine("builder.Services.AddGCFoundationComponents(builder.Configuration);");
        
        if (includeCSP)
        {
            sb.AppendLine("builder.Services.AddGCFoundationContentPolicies(builder.Configuration);");
        }
        
        if (includeSession)
        {
            sb.AppendLine("builder.Services.AddGCFoundationSession(builder.Configuration);");
        }
        sb.AppendLine();
        
        // Localization configuration
        if (includeLocalization)
        {
            sb.AppendLine("// Language configuration");
            sb.AppendLine("var supportedCultures = LanguageUtility.GetSupportedCulture();");
            sb.AppendLine("var routeSegmentLocalizationProvider = new FirstUrlSegmentRequestCultureProvider(supportedCultures.ToList());");
            sb.AppendLine();
            sb.AppendLine("builder.Services.Configure<RequestLocalizationOptions>(options =>");
            sb.AppendLine("{");
            sb.AppendLine($"    options.DefaultRequestCulture = new RequestCulture(culture: \"{defaultCulture}\", uiCulture: \"{defaultCulture}\");");
            sb.AppendLine("    options.SupportedCultures = supportedCultures;");
            sb.AppendLine("    options.SupportedUICultures = supportedCultures;");
            sb.AppendLine("    options.RequestCultureProviders.Insert(0, routeSegmentLocalizationProvider);");
            sb.AppendLine("});");
            sb.AppendLine();
            
            sb.AppendLine("// Add route localization");
            sb.AppendLine("builder.Services.AddCustomRouteLocalization();");
            sb.AppendLine();
        }
        
        // Razor view engine configuration
        if (includeNavigation)
        {
            sb.AppendLine("// Configure Razor view engine for navigation components");
            sb.AppendLine("builder.Services.Configure<RazorViewEngineOptions>(options =>");
            sb.AppendLine("{");
            sb.AppendLine("    options.ViewLocationFormats.Add(\"/Views/Shared/Components/Navigation/{0}.cshtml\");");
            sb.AppendLine("    options.ViewLocationFormats.Add(\"/contentFiles/any/net8.0/Views/Shared/Components/Navigation/{0}.cshtml\");");
            sb.AppendLine("});");
            sb.AppendLine();
        }
        
        // Build app
        sb.AppendLine("var app = builder.Build();");
        sb.AppendLine();
        
        // Configure pipeline
        sb.AppendLine("// Configure the HTTP request pipeline.");
        sb.AppendLine("if (!app.Environment.IsDevelopment())");
        sb.AppendLine("{");
        sb.AppendLine("    app.UseExceptionHandler(\"/Home/Error\");");
        sb.AppendLine("    app.UseHsts();");
        sb.AppendLine("}");
        sb.AppendLine();
        
        // Middleware configuration
        sb.AppendLine("// Add GC Foundation middleware");
        sb.AppendLine("app.UseMiddleware<GCFoundationComponentsMiddleware>();");
        
        if (includeCSP)
        {
            sb.AppendLine("app.UseMiddleware<GCFoundationContentPoliciesMiddleware>();");
        }
        
        if (includeLocalization)
        {
            sb.AppendLine("app.UseMiddleware<GCFoundationLanguageMiddleware>();");
        }
        sb.AppendLine();
        
        // Cookie policy
        if (includeAuthentication)
        {
            sb.AppendLine("// Secure cookie policy");
            sb.AppendLine("app.UseCookiePolicy(new CookiePolicyOptions");
            sb.AppendLine("{");
            sb.AppendLine("    MinimumSameSitePolicy = SameSiteMode.Strict,");
            sb.AppendLine("    Secure = CookieSecurePolicy.Always,");
            sb.AppendLine("    HttpOnly = HttpOnlyPolicy.Always");
            sb.AppendLine("});");
            sb.AppendLine();
        }
        
        // Use Foundation services
        sb.AppendLine("// Use GC Foundation services");
        sb.AppendLine("app.UseGCFoundationComponents();");
        
        if (includeCSP)
        {
            sb.AppendLine("app.UseGCFoundationContentPolicies();");
        }
        
        if (includeSession)
        {
            sb.AppendLine("app.UseGCFoundationSession();");
        }
        sb.AppendLine();
        
        // Standard middleware
        sb.AppendLine("app.UseHttpsRedirection();");
        sb.AppendLine("app.UseStaticFiles();");
        sb.AppendLine("app.UseRouting();");
        sb.AppendLine();
        
        // Localization middleware
        if (includeLocalization)
        {
            sb.AppendLine("// Use localization middleware");
            sb.AppendLine("var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;");
            sb.AppendLine("app.UseRequestLocalization(localizationOptions);");
            sb.AppendLine();
        }
        
        // Authentication and authorization
        if (includeAuthentication)
        {
            sb.AppendLine("// Add authentication and authorization");
            sb.AppendLine("app.UseAuthentication();");
            sb.AppendLine("app.UseAuthorization();");
            sb.AppendLine();
        }
        
        // Routing
        sb.AppendLine("// Configure routing");
        if (includeLocalization)
        {
            sb.AppendLine("app.MapControllerRoute(");
            sb.AppendLine("    name: \"default\",");
            sb.AppendLine("    pattern: \"{culture=en}/{controller=Home}/{action=Index}/{id?}\");");
        }
        else
        {
            sb.AppendLine("app.MapControllerRoute(");
            sb.AppendLine("    name: \"default\",");
            sb.AppendLine("    pattern: \"{controller=Home}/{action=Index}/{id?}\");");
        }
        sb.AppendLine();
        
        sb.AppendLine("app.Run();");
        
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates appsettings.json configuration file with GC Foundation settings.")]
    public string GenerateAppSettingsJson(
        [Description("Application name")] string applicationName = "GC Foundation Application",
        [Description("Include GCDS CDN resources")] bool includeGCDSResources = true,
        [Description("Include Font Awesome")] bool includeFontAwesome = false,
        [Description("GCDS CSS CDN URL")] string gcdsCdnUrl = "https://cdn.design-system.alpha.canada.ca/@cdssnc/gcds-components@latest/dist/gcds.css",
        [Description("Font Awesome CDN URL")] string fontAwesomeCdnUrl = "https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css",
        [Description("Enable session management")] bool enableSession = true,
        [Description("Session timeout in minutes")] int sessionTimeoutMinutes = 30,
        [Description("Enable CSP")] bool enableCSP = true)
    {
        var sb = new StringBuilder();
        sb.AppendLine("{");
        sb.AppendLine("  \"Logging\": {");
        sb.AppendLine("    \"LogLevel\": {");
        sb.AppendLine("      \"Default\": \"Information\",");
        sb.AppendLine("      \"Microsoft.AspNetCore\": \"Warning\"");
        sb.AppendLine("    }");
        sb.AppendLine("  },");
        sb.AppendLine("  \"AllowedHosts\": \"*\",");
        sb.AppendLine();
        
        // Foundation Components Settings
        sb.AppendLine("  \"FoundationComponentsSettings\": {");
        sb.AppendLine($"    \"ApplicationName\": \"{applicationName}\",");
        sb.AppendLine($"    \"IncludeGCDSResources\": {includeGCDSResources.ToString().ToLower()},");
        sb.AppendLine($"    \"GCDSCssCDN\": \"{gcdsCdnUrl}\",");
        sb.AppendLine($"    \"IncludeFontAwesome\": {includeFontAwesome.ToString().ToLower()},");
        sb.AppendLine($"    \"FontAwesomeCDN\": \"{fontAwesomeCdnUrl}\",");
        sb.AppendLine("    \"GlobalMetaTags\": [");
        sb.AppendLine("      \"<meta name='viewport' content='width=device-width, initial-scale=1.0'>\",");
        sb.AppendLine("      \"<meta charset='utf-8'>\"");
        sb.AppendLine("    ],");
        sb.AppendLine("    \"GlobalLinkTags\": [");
        sb.AppendLine("      \"<link rel='preconnect' href='https://fonts.googleapis.com'>\",");
        sb.AppendLine("      \"<link rel='preconnect' href='https://fonts.gstatic.com' crossorigin>\"");
        sb.AppendLine("    ]");
        sb.AppendLine("  },");
        sb.AppendLine();
        
        // Session Settings
        if (enableSession)
        {
            sb.AppendLine("  \"SessionSettings\": {");
            sb.AppendLine($"    \"SessionTimeoutMinutes\": {sessionTimeoutMinutes},");
            sb.AppendLine("    \"WarningTimeMinutes\": 5,");
            sb.AppendLine("    \"EnableSessionTimeout\": true,");
            sb.AppendLine("    \"SessionCookieName\": \"GCFoundationSession\",");
            sb.AppendLine("    \"SessionTimeoutRedirectUrl\": \"/session/timeout\"");
            sb.AppendLine("  },");
            sb.AppendLine();
        }
        
        // Content Security Policy Settings
        if (enableCSP)
        {
            sb.AppendLine("  \"ContentSecurityPolicy\": {");
            sb.AppendLine("    \"EnableCSP\": true,");
            sb.AppendLine("    \"DefaultSrc\": \"'self'\",");
            sb.AppendLine("    \"ScriptSrc\": \"'self' 'unsafe-inline' 'unsafe-eval' https://cdn.design-system.alpha.canada.ca\",");
            sb.AppendLine("    \"StyleSrc\": \"'self' 'unsafe-inline' https://fonts.googleapis.com https://cdn.design-system.alpha.canada.ca\",");
            sb.AppendLine("    \"FontSrc\": \"'self' https://fonts.gstatic.com\",");
            sb.AppendLine("    \"ImgSrc\": \"'self' data: https:\",");
            sb.AppendLine("    \"ConnectSrc\": \"'self'\",");
            sb.AppendLine("    \"FrameSrc\": \"'none'\",");
            sb.AppendLine("    \"ObjectSrc\": \"'none'\"");
            sb.AppendLine("  },");
            sb.AppendLine();
        }
        
        // Localization Settings
        sb.AppendLine("  \"GlobalResourceOptions\": {");
        sb.AppendLine("    \"ResourcesPath\": \"Resources\",");
        sb.AppendLine("    \"DefaultCulture\": \"en-CA\",");
        sb.AppendLine("    \"SupportedCultures\": [\"en-CA\", \"fr-CA\"]");
        sb.AppendLine("  },");
        sb.AppendLine();
        
        // Navigation Options
        sb.AppendLine("  \"NavigationOptions\": {");
        sb.AppendLine("    \"NavigationCacheKey\": \"navigation-cache\",");
        sb.AppendLine("    \"CacheDurationInMinutes\": 30,");
        sb.AppendLine("    \"NavigationXmlFilePath\": \"navigation.xml\"");
        sb.AppendLine("  }");
        
        sb.AppendLine("}");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates a project file (.csproj) with all necessary GC Foundation dependencies using latest versions from custom feed.")]
    public async Task<string> GenerateProjectFileWithLatestVersions(
        [Description("Project name")] string projectName = "GCFoundation.Web",
        [Description(".NET target framework")] string targetFramework = "net8.0",
        [Description("Include Entity Framework")] bool includeEntityFramework = false,
        [Description("Include authentication packages")] bool includeAuthentication = true,
        [Description("Include testing packages")] bool includeTesting = false,
        [Description("Nullable reference types enabled")] bool nullable = true,
        [Description("Implicit usings enabled")] bool implicitUsings = true,
        [Description("Custom NuGet feed URL for GCFoundation packages")] string feedUrl = "https://pkgs.dev.azure.com/tbs-sct/_packaging/TBS_Custom_Feed/nuget/v3/index.json")
    {
        var sb = new StringBuilder();
        
        // Get latest versions from custom feed
        var latestVersions = await GetGCFoundationPackageVersionsOnly(feedUrl);
        
        sb.AppendLine($"<Project Sdk=\"Microsoft.NET.Sdk.Web\">");
        sb.AppendLine();
        sb.AppendLine("  <PropertyGroup>");
        sb.AppendLine($"    <TargetFramework>{targetFramework}</TargetFramework>");
        sb.AppendLine($"    <Nullable>{(nullable ? "enable" : "disable")}</Nullable>");
        sb.AppendLine($"    <ImplicitUsings>{(implicitUsings ? "enable" : "disable")}</ImplicitUsings>");
        sb.AppendLine("  </PropertyGroup>");
        sb.AppendLine();
        
        sb.AppendLine("  <ItemGroup>");
        sb.AppendLine("    <!-- GC Foundation Core Packages (Latest Versions) -->");
        sb.AppendLine($"    <PackageReference Include=\"GCFoundation.Common\" Version=\"{latestVersions.GetValueOrDefault("GCFoundation.Common", "1.0.0")}\" />");
        sb.AppendLine($"    <PackageReference Include=\"GCFoundation.Components\" Version=\"{latestVersions.GetValueOrDefault("GCFoundation.Components", "1.0.0")}\" />");
        sb.AppendLine($"    <PackageReference Include=\"GCFoundation.Security\" Version=\"{latestVersions.GetValueOrDefault("GCFoundation.Security", "1.0.0")}\" />");
        sb.AppendLine();
        
        sb.AppendLine("    <!-- Localization and Navigation -->");
        sb.AppendLine("    <PackageReference Include=\"cloudscribe.Web.Navigation\" Version=\"8.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"cloudscribe.Web.Localization\" Version=\"8.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"cloudscribe.Web.SiteMap\" Version=\"8.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"cloudscribe.Web.SiteMap.FromNavigation\" Version=\"8.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"RouteLocalization.AspNetCore\" Version=\"1.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"Microsoft.Extensions.Localization\" Version=\"9.0.2\" />");
        sb.AppendLine();
        
        if (includeAuthentication)
        {
            sb.AppendLine("    <!-- Authentication (Cookie authentication built into .NET 8) -->");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.AspNetCore.Authentication.OpenIdConnect\" Version=\"8.0.0\" />");
            sb.AppendLine();
        }
        
        if (includeEntityFramework)
        {
            sb.AppendLine("    <!-- Entity Framework Core -->");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.EntityFrameworkCore\" Version=\"8.0.0\" />");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.EntityFrameworkCore.SqlServer\" Version=\"8.0.0\" />");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.EntityFrameworkCore.Tools\" Version=\"8.0.0\" />");
            sb.AppendLine();
        }
        
        if (includeTesting)
        {
            sb.AppendLine("    <!-- Testing -->");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.AspNetCore.Mvc.Testing\" Version=\"8.0.0\" />");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.NET.Test.Sdk\" Version=\"17.8.0\" />");
            sb.AppendLine("    <PackageReference Include=\"xunit\" Version=\"2.6.1\" />");
            sb.AppendLine("    <PackageReference Include=\"xunit.runner.visualstudio\" Version=\"2.5.3\" />");
            sb.AppendLine();
        }
        
        sb.AppendLine("  </ItemGroup>");
        sb.AppendLine();
        sb.AppendLine("</Project>");
        
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates a project file (.csproj) with all necessary GC Foundation dependencies.")]
    public string GenerateProjectFile(
        [Description("Project name")] string projectName = "GCFoundation.Web",
        [Description(".NET target framework")] string targetFramework = "net8.0",
        [Description("Include Entity Framework")] bool includeEntityFramework = false,
        [Description("Include authentication packages")] bool includeAuthentication = true,
        [Description("Include testing packages")] bool includeTesting = false,
        [Description("Nullable reference types enabled")] bool nullable = true,
        [Description("Implicit usings enabled")] bool implicitUsings = true)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<Project Sdk=\"Microsoft.NET.Sdk.Web\">");
        sb.AppendLine();
        sb.AppendLine("  <PropertyGroup>");
        sb.AppendLine($"    <TargetFramework>{targetFramework}</TargetFramework>");
        sb.AppendLine($"    <Nullable>{(nullable ? "enable" : "disable")}</Nullable>");
        sb.AppendLine($"    <ImplicitUsings>{(implicitUsings ? "enable" : "disable")}</ImplicitUsings>");
        sb.AppendLine("  </PropertyGroup>");
        sb.AppendLine();
        
        sb.AppendLine("  <ItemGroup>");
        sb.AppendLine("    <!-- GC Foundation Core Packages -->");
        sb.AppendLine("    <PackageReference Include=\"GCFoundation.Common\" Version=\"1.0.20250819.1\" />");
        sb.AppendLine("    <PackageReference Include=\"GCFoundation.Components\" Version=\"1.0.20250819.1\" />");
        sb.AppendLine("    <PackageReference Include=\"GCFoundation.Security\" Version=\"1.0.20250819.1\" />");
        sb.AppendLine();
        
        sb.AppendLine("    <!-- Localization and Navigation -->");
        sb.AppendLine("    <PackageReference Include=\"cloudscribe.Web.Navigation\" Version=\"8.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"cloudscribe.Web.Localization\" Version=\"8.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"cloudscribe.Web.SiteMap\" Version=\"8.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"cloudscribe.Web.SiteMap.FromNavigation\" Version=\"8.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"RouteLocalization.AspNetCore\" Version=\"1.0.0\" />");
        sb.AppendLine("    <PackageReference Include=\"Microsoft.Extensions.Localization\" Version=\"9.0.2\" />");
        sb.AppendLine();
        
        if (includeAuthentication)
        {
            sb.AppendLine("    <!-- Authentication (Cookie authentication built into .NET 8) -->");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.AspNetCore.Identity.EntityFrameworkCore\" Version=\"8.0.0\" />");
            sb.AppendLine();
        }
        
        if (includeEntityFramework)
        {
            sb.AppendLine("    <!-- Entity Framework -->");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.EntityFrameworkCore.SqlServer\" Version=\"8.0.0\" />");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.EntityFrameworkCore.Tools\" Version=\"8.0.0\" />");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.EntityFrameworkCore.Design\" Version=\"8.0.0\" />");
            sb.AppendLine();
        }
        
        if (includeTesting)
        {
            sb.AppendLine("    <!-- Testing Packages -->");
            sb.AppendLine("    <PackageReference Include=\"Microsoft.AspNetCore.Mvc.Testing\" Version=\"8.0.0\" />");
            sb.AppendLine("    <PackageReference Include=\"xunit\" Version=\"2.4.2\" />");
            sb.AppendLine("    <PackageReference Include=\"xunit.runner.visualstudio\" Version=\"2.4.5\" />");
            sb.AppendLine();
        }
        
        sb.AppendLine("    <!-- Additional Utilities -->");
        sb.AppendLine("    <PackageReference Include=\"Newtonsoft.Json\" Version=\"13.0.3\" />");
        sb.AppendLine("    <PackageReference Include=\"HtmlAgilityPack\" Version=\"1.12.1\" />");
        sb.AppendLine("  </ItemGroup>");
        sb.AppendLine();
        
        sb.AppendLine("  <ItemGroup>");
        sb.AppendLine("    <Folder Include=\"wwwroot\\\" />");
        sb.AppendLine("    <Folder Include=\"Views\\Shared\\\" />");
        sb.AppendLine("    <Folder Include=\"Resources\\\" />");
        sb.AppendLine("  </ItemGroup>");
        sb.AppendLine();
        
        sb.AppendLine("</Project>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates the RouteLocalizationExtensions.cs file with AddCustomRouteLocalization method.")]
    public string GenerateRouteLocalizationExtensions(
        [Description("Project name for namespace")] string projectName = "YourProject")
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"using {projectName}.Controllers;");
        sb.AppendLine("using RouteLocalization.AspNetCore;");
        sb.AppendLine();
        sb.AppendLine($"namespace {projectName}.Infrastructure.Extensions");
        sb.AppendLine("{");
        sb.AppendLine("    /// <summary>");
        sb.AppendLine("    /// Provides extension methods for setting up route localization in the application.");
        sb.AppendLine("    /// </summary>");
        sb.AppendLine("    public static class RouteLocalizationExtensions");
        sb.AppendLine("    {");
        sb.AppendLine("        /// <summary>");
        sb.AppendLine("        /// Supported cultures for the application routing.");
        sb.AppendLine("        /// </summary>");
        sb.AppendLine("        private static readonly string[] SupportedCultures = [\"en\", \"fr\"];");
        sb.AppendLine();
        sb.AppendLine("        /// <summary>");
        sb.AppendLine("        /// Adds custom route localization configuration to the application.");
        sb.AppendLine("        /// </summary>");
        sb.AppendLine("        /// <param name=\"services\">The service collection to which the route localization is added.</param>");
        sb.AppendLine("        /// <returns>The modified <see cref=\"IServiceCollection\"/>.</returns>");
        sb.AppendLine("        public static IServiceCollection AddCustomRouteLocalization(this IServiceCollection services)");
        sb.AppendLine("        {");
        sb.AppendLine("            services.AddRouteLocalization(setup =>");
        sb.AppendLine("            {");
        sb.AppendLine("                // Home Controller");
        sb.AppendLine("                setup.UseCulture(\"fr\")");
        sb.AppendLine("                .WhereController(nameof(HomeController))");
        sb.AppendLine("                .TranslateController(\"accueil\")");
        sb.AppendLine("                .WhereAction(nameof(HomeController.Index))");
        sb.AppendLine("                .TranslateAction(\"\");");
        sb.AppendLine();
        sb.AppendLine("                // Keep original English routes for all controllers");
        sb.AppendLine("                setup.UseCulture(\"en\")");
        sb.AppendLine("                    .WhereController(nameof(HomeController))");
        sb.AppendLine("                    .WhereAction(nameof(HomeController.Index))");
        sb.AppendLine("                    .TranslateAction(\"\");");
        sb.AppendLine();
        sb.AppendLine("                // Ensure untranslated routes exist for controllers with attribute routes only");
        sb.AppendLine("                setup.UseCultures(SupportedCultures)");
        sb.AppendLine("                    .WhereUntranslated()");
        sb.AppendLine("                    .AddDefaultTranslation();");
        sb.AppendLine("            });");
        sb.AppendLine();
        sb.AppendLine("            return services;");
        sb.AppendLine("        }");
        sb.AppendLine("    }");
        sb.AppendLine("}");
        
        return sb.ToString();
    }

    #endregion

    #region Controller and View Templates

    [McpServerTool]
    [Description("Generates a base controller inheriting from GCFoundationBaseController with common functionality.")]
    public string GenerateBaseController(
        [Description("Controller name")] string controllerName = "HomeController",
        [Description("Include authentication")] bool includeAuth = false,
        [Description("Include localization")] bool includeLocalization = true,
        [Description("Include logging")] bool includeLogging = true,
        [Description("Include error handling")] bool includeErrorHandling = true)
    {
        var sb = new StringBuilder();
        
        // Using statements
        sb.AppendLine("using GCFoundation.Components.Controllers;");
        sb.AppendLine("using Microsoft.AspNetCore.Mvc;");
        
        if (includeAuth)
        {
            sb.AppendLine("using Microsoft.AspNetCore.Authorization;");
        }
        
        if (includeLogging)
        {
            sb.AppendLine("using Microsoft.Extensions.Logging;");
        }
        
        if (includeLocalization)
        {
            sb.AppendLine("using Microsoft.Extensions.Localization;");
        }
        
        sb.AppendLine();
        sb.AppendLine("namespace YourProject.Controllers");
        sb.AppendLine("{");
        
        if (includeAuth)
        {
            sb.AppendLine("    [Authorize]");
        }
        
        sb.AppendLine($"    public class {controllerName} : GCFoundationBaseController");
        sb.AppendLine("    {");
        
        if (includeLogging)
        {
            sb.AppendLine($"        private readonly ILogger<{controllerName}> _logger;");
        }
        
        if (includeLocalization)
        {
            sb.AppendLine("        private readonly IStringLocalizer _localizer;");
        }
        
        sb.AppendLine();
        
        // Constructor
        sb.Append($"        public {controllerName}(");
        var constructorParams = new List<string>();
        
        if (includeLogging)
        {
            constructorParams.Add($"ILogger<{controllerName}> logger");
        }
        
        if (includeLocalization)
        {
            constructorParams.Add("IStringLocalizer localizer");
        }
        
        sb.Append(string.Join(", ", constructorParams));
        sb.AppendLine(")");
        sb.AppendLine("        {");
        
        if (includeLogging)
        {
            sb.AppendLine("            _logger = logger;");
        }
        
        if (includeLocalization)
        {
            sb.AppendLine("            _localizer = localizer;");
        }
        
        sb.AppendLine("        }");
        sb.AppendLine();
        
        // Index action
        sb.AppendLine("        public IActionResult Index()");
        sb.AppendLine("        {");
        
        if (includeLogging)
        {
            sb.AppendLine("            _logger.LogInformation(\"Index action called\");");
        }
        
        sb.AppendLine("            return View();");
        sb.AppendLine("        }");
        sb.AppendLine();
        
        // Error handling
        if (includeErrorHandling)
        {
            sb.AppendLine("        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]");
            sb.AppendLine("        public IActionResult Error()");
            sb.AppendLine("        {");
            sb.AppendLine("            return View(\"Error\");");
            sb.AppendLine("        }");
        }
        
        sb.AppendLine("    }");
        sb.AppendLine("}");
        
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates a foundation layout view with GCDS components and proper structure.")]
    public string GenerateFoundationLayoutView(
        [Description("Include GCDS header")] bool includeHeader = true,
        [Description("Include GCDS footer")] bool includeFooter = true,
        [Description("Include breadcrumbs")] bool includeBreadcrumbs = true,
        [Description("Include language toggle")] bool includeLanguageToggle = true,
        [Description("Include skip links")] bool includeSkipLinks = true,
        [Description("Application name")] string appName = "Government of Canada Application")
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("@{");
        sb.AppendLine("    Layout = \"_FoundationLayout\";");
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine("<!DOCTYPE html>");
        sb.AppendLine("<html lang=\"@LanguageUtility.GetCurrentApplicationLanguage()\">");
        sb.AppendLine("<head>");
        sb.AppendLine("    <meta charset=\"utf-8\" />");
        sb.AppendLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />");
        sb.AppendLine("    <title>@ViewData[\"Title\"] - " + appName + "</title>");
        sb.AppendLine("    @await RenderSectionAsync(\"Styles\", required: false)");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        
        if (includeSkipLinks)
        {
            sb.AppendLine("    <!-- Skip Links -->");
            sb.AppendLine("    <gcds-skip-to-nav href=\"#main-nav\">Skip to main navigation</gcds-skip-to-nav>");
            sb.AppendLine("    <gcds-skip-to-nav href=\"#main-content\">Skip to main content</gcds-skip-to-nav>");
            sb.AppendLine();
        }
        
        if (includeHeader)
        {
            sb.AppendLine("    <!-- Header -->");
            sb.AppendLine("    <gcds-header variant=\"signature\">");
            sb.AppendLine("        <gcds-signature type=\"colour\" variant=\"signature\" href=\"https://canada.ca\"></gcds-signature>");
            
            if (includeLanguageToggle)
            {
                sb.AppendLine("        <gcds-lang-toggle lang=\"@LanguageUtility.GetCurrentApplicationLanguage()\" href=\"@Url.Action(\"ToggleLanguage\", \"Language\")\"></gcds-lang-toggle>");
            }
            
            sb.AppendLine("    </gcds-header>");
            sb.AppendLine();
        }
        
        sb.AppendLine("    <!-- Main Navigation -->");
        sb.AppendLine("    <nav id=\"main-nav\" aria-label=\"Main navigation\">");
        sb.AppendLine("        @await Component.InvokeAsync(\"GCDSBreadcrumbs\")");
        sb.AppendLine("    </nav>");
        sb.AppendLine();
        
        if (includeBreadcrumbs)
        {
            sb.AppendLine("    <!-- Breadcrumbs -->");
            sb.AppendLine("    @await Component.InvokeAsync(\"GCDSBreadcrumbs\")");
            sb.AppendLine();
        }
        
        sb.AppendLine("    <!-- Main Content -->");
        sb.AppendLine("    <main id=\"main-content\">");
        sb.AppendLine("        <gcds-container size=\"xl\" centered=\"true\">");
        sb.AppendLine("            @RenderBody()");
        sb.AppendLine("        </gcds-container>");
        sb.AppendLine("    </main>");
        sb.AppendLine();
        
        if (includeFooter)
        {
            sb.AppendLine("    <!-- Footer -->");
            sb.AppendLine("    <gcds-footer display=\"full\">");
            sb.AppendLine("        <gcds-footer-nav>");
            sb.AppendLine("            <gcds-footer-link href=\"https://canada.ca/en/contact.html\">Contact us</gcds-footer-link>");
            sb.AppendLine("            <gcds-footer-link href=\"https://canada.ca/en/transparency/terms.html\">Terms and conditions</gcds-footer-link>");
            sb.AppendLine("            <gcds-footer-link href=\"https://canada.ca/en/transparency/privacy.html\">Privacy</gcds-footer-link>");
            sb.AppendLine("        </gcds-footer-nav>");
            sb.AppendLine("    </gcds-footer>");
        }
        
        sb.AppendLine();
        sb.AppendLine("    @await RenderSectionAsync(\"Scripts\", required: false)");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");
        
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates a sample form view using GCDS and FDCP components with validation.")]
    public string GenerateSampleFormView(
        [Description("Form title")] string formTitle = "Application Form",
        [Description("Form action")] string formAction = "/Home/SubmitForm",
        [Description("Include personal information section")] bool includePersonalInfo = true,
        [Description("Include contact information section")] bool includeContactInfo = true,
        [Description("Include file upload")] bool includeFileUpload = false,
        [Description("Include terms and conditions")] bool includeTerms = true)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine("@{");
        sb.AppendLine($"    ViewData[\"Title\"] = \"{formTitle}\";");
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine($"<gcds-heading tag=\"h1\">{formTitle}</gcds-heading>");
        sb.AppendLine();
        sb.AppendLine("@if (!ViewData.ModelState.IsValid)");
        sb.AppendLine("{");
        sb.AppendLine("    <gcds-error-summary>");
        sb.AppendLine("        <gcds-heading tag=\"h2\" size=\"h3\">There are errors on this page</gcds-heading>");
        sb.AppendLine("        <ul>");
        sb.AppendLine("            @foreach (var error in ViewData.ModelState)");
        sb.AppendLine("            {");
        sb.AppendLine("                @foreach (var errorMessage in error.Value.Errors)");
        sb.AppendLine("                {");
        sb.AppendLine("                    <li><a href=\"#@error.Key\">@errorMessage.ErrorMessage</a></li>");
        sb.AppendLine("                }");
        sb.AppendLine("            }");
        sb.AppendLine("        </ul>");
        sb.AppendLine("    </gcds-error-summary>");
        sb.AppendLine("}");
        sb.AppendLine();
        
        sb.AppendLine($"<form action=\"{formAction}\" method=\"post\" novalidate>");
        sb.AppendLine("    @Html.AntiForgeryToken()");
        sb.AppendLine();
        
        if (includePersonalInfo)
        {
            sb.AppendLine("    <gcds-fieldset fieldset-id=\"personal-info\" legend=\"Personal Information\" legend-size=\"h2\">");
            sb.AppendLine("        <gcds-input");
            sb.AppendLine("            asp-for=\"FirstName\"");
            sb.AppendLine("            input-id=\"FirstName\"");
            sb.AppendLine("            label=\"First Name\"");
            sb.AppendLine("            type=\"text\"");
            sb.AppendLine("            required=\"true\"");
            sb.AppendLine("            hint=\"Enter your legal first name\">");
            sb.AppendLine("        </gcds-input>");
            sb.AppendLine();
            sb.AppendLine("        <gcds-input");
            sb.AppendLine("            asp-for=\"LastName\"");
            sb.AppendLine("            input-id=\"LastName\"");
            sb.AppendLine("            label=\"Last Name\"");
            sb.AppendLine("            type=\"text\"");
            sb.AppendLine("            required=\"true\"");
            sb.AppendLine("            hint=\"Enter your legal last name\">");
            sb.AppendLine("        </gcds-input>");
            sb.AppendLine();
            sb.AppendLine("        <gcds-date-input");
            sb.AppendLine("            asp-for=\"DateOfBirth\"");
            sb.AppendLine("            name=\"DateOfBirth\"");
            sb.AppendLine("            legend=\"Date of Birth\"");
            sb.AppendLine("            format=\"full\"");
            sb.AppendLine("            hint=\"For example: 01 01 1990\">");
            sb.AppendLine("        </gcds-date-input>");
            sb.AppendLine("    </gcds-fieldset>");
            sb.AppendLine();
        }
        
        if (includeContactInfo)
        {
            sb.AppendLine("    <gcds-fieldset fieldset-id=\"contact-info\" legend=\"Contact Information\" legend-size=\"h2\">");
            sb.AppendLine("        <gcds-input");
            sb.AppendLine("            asp-for=\"Email\"");
            sb.AppendLine("            input-id=\"Email\"");
            sb.AppendLine("            label=\"Email Address\"");
            sb.AppendLine("            type=\"email\"");
            sb.AppendLine("            required=\"true\"");
            sb.AppendLine("            hint=\"We'll use this to contact you about your application\">");
            sb.AppendLine("        </gcds-input>");
            sb.AppendLine();
            sb.AppendLine("        <gcds-input");
            sb.AppendLine("            asp-for=\"Phone\"");
            sb.AppendLine("            input-id=\"Phone\"");
            sb.AppendLine("            label=\"Phone Number\"");
            sb.AppendLine("            type=\"tel\"");
            sb.AppendLine("            hint=\"Include area code (e.g., 613-555-1234)\">");
            sb.AppendLine("        </gcds-input>");
            sb.AppendLine();
            sb.AppendLine("        <gcds-textarea");
            sb.AppendLine("            asp-for=\"Address\"");
            sb.AppendLine("            textarea-id=\"Address\"");
            sb.AppendLine("            label=\"Mailing Address\"");
            sb.AppendLine("            rows=\"3\"");
            sb.AppendLine("            hint=\"Enter your complete mailing address\">");
            sb.AppendLine("        </gcds-textarea>");
            sb.AppendLine("    </gcds-fieldset>");
            sb.AppendLine();
        }
        
        if (includeFileUpload)
        {
            sb.AppendLine("    <gcds-fieldset fieldset-id=\"documents\" legend=\"Supporting Documents\" legend-size=\"h2\">");
            sb.AppendLine("        <gcds-file-upload");
            sb.AppendLine("            asp-for=\"Documents\"");
            sb.AppendLine("            uploader-id=\"Documents\"");
            sb.AppendLine("            label=\"Upload Required Documents\"");
            sb.AppendLine("            accept=\".pdf,.doc,.docx\"");
            sb.AppendLine("            multiple=\"true\"");
            sb.AppendLine("            hint=\"Accepted formats: PDF, DOC, DOCX. Maximum 5MB per file.\">");
            sb.AppendLine("        </gcds-file-upload>");
            sb.AppendLine("    </gcds-fieldset>");
            sb.AppendLine();
        }
        
        if (includeTerms)
        {
            sb.AppendLine("    <gcds-fieldset fieldset-id=\"terms\" legend=\"Terms and Conditions\" legend-size=\"h2\">");
            sb.AppendLine("        <gcds-checkbox");
            sb.AppendLine("            asp-for=\"AcceptTerms\"");
            sb.AppendLine("            checkbox-id=\"AcceptTerms\"");
            sb.AppendLine("            label=\"I agree to the terms and conditions\"");
            sb.AppendLine("            required=\"true\">");
            sb.AppendLine("        </gcds-checkbox>");
            sb.AppendLine("    </gcds-fieldset>");
            sb.AppendLine();
        }
        
        sb.AppendLine("    <gcds-button type=\"submit\" variant=\"primary\">Submit Application</gcds-button>");
        sb.AppendLine("    <gcds-button type=\"button\" variant=\"secondary\" href=\"@Url.Action(\"Index\")\">Cancel</gcds-button>");
        sb.AppendLine("</form>");
        
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Creates a complete GC Foundation project with all essential files in one command: project structure, .csproj, Program.cs, appsettings.json, route localization, and base controller.")]
    public string GenerateCompleteGCFoundationProject(
        [Description("Project name")] string projectName = "MyGCApp",
        [Description("Application display name")] string applicationName = "My GC Application",
        [Description("Include authentication setup")] bool includeAuthentication = true,
        [Description("Include localization support")] bool includeLocalization = true,
        [Description("Include Entity Framework")] bool includeEntityFramework = false,
        [Description("Include testing packages")] bool includeTesting = false)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"# Complete GC Foundation Project Setup: {projectName}");
        sb.AppendLine("========================================");
        sb.AppendLine();
        sb.AppendLine("This command generates all the essential files needed for a complete GC Foundation project.");
        sb.AppendLine();

        // 1. Project Structure
        sb.AppendLine("## 1. Project Structure");
        sb.AppendLine("Create the following directory structure:");
        sb.AppendLine("```");
        sb.AppendLine($"{projectName}/");
        sb.AppendLine("├── src/");
        sb.AppendLine($"│   └── {projectName}/");
        sb.AppendLine("│       ├── Controllers/");
        sb.AppendLine("│       ├── Models/");
        sb.AppendLine("│       ├── Views/");
        sb.AppendLine("│       ├── Infrastructure/");
        sb.AppendLine("│       │   └── Extensions/");
        sb.AppendLine("│       ├── Resources/");
        sb.AppendLine("│       └── wwwroot/");
        sb.AppendLine("├── README.md");
        sb.AppendLine("└── .gitignore");
        sb.AppendLine("```");
        sb.AppendLine();

        // 2. Project File
        sb.AppendLine("## 2. Project File (.csproj)");
        sb.AppendLine($"**File**: `src/{projectName}/{projectName}.csproj`");
        sb.AppendLine("```xml");
        sb.AppendLine(GenerateProjectFile(projectName, "net8.0", includeEntityFramework, includeAuthentication, includeTesting));
        sb.AppendLine("```");
        sb.AppendLine();

        // 3. Program.cs
        sb.AppendLine("## 3. Program.cs");
        sb.AppendLine($"**File**: `src/{projectName}/Program.cs`");
        sb.AppendLine("```csharp");
        sb.AppendLine(GenerateGCFoundationProgramCs(projectName, includeAuthentication, includeLocalization, true, true, true));
        sb.AppendLine("```");
        sb.AppendLine();

        // 4. App Settings
        sb.AppendLine("## 4. Application Settings");
        sb.AppendLine($"**File**: `src/{projectName}/appsettings.json`");
        sb.AppendLine("```json");
        sb.AppendLine(GenerateAppSettingsJson(applicationName));
        sb.AppendLine("```");
        sb.AppendLine();

        // 5. Route Localization Extensions
        sb.AppendLine("## 5. Route Localization Extensions");
        sb.AppendLine($"**File**: `src/{projectName}/Infrastructure/Extensions/RouteLocalizationExtensions.cs`");
        sb.AppendLine("```csharp");
        sb.AppendLine(GenerateRouteLocalizationExtensions(projectName));
        sb.AppendLine("```");
        sb.AppendLine();

        // 6. Base Controller
        sb.AppendLine("## 6. Base Controller");
        sb.AppendLine($"**File**: `src/{projectName}/Controllers/HomeController.cs`");
        sb.AppendLine("```csharp");
        sb.AppendLine(GenerateBaseController("HomeController", includeAuthentication, includeLocalization, true, true));
        sb.AppendLine("```");
        sb.AppendLine();

        // 7. Layout View
        sb.AppendLine("## 7. Layout View");
        sb.AppendLine($"**File**: `src/{projectName}/Views/Shared/_Layout.cshtml`");
        sb.AppendLine("```html");
        sb.AppendLine(GenerateFoundationLayoutView(true, true, true, true, true, applicationName));
        sb.AppendLine("```");
        sb.AppendLine();

        // 8. Home Index View
        sb.AppendLine("## 8. Home Index View");
        sb.AppendLine($"**File**: `src/{projectName}/Views/Home/Index.cshtml`");
        sb.AppendLine("```html");
        sb.AppendLine("@{");
        sb.AppendLine("    ViewData[\"Title\"] = \"Home Page\";");
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine("<gcds-heading tag=\"h1\">Welcome to @ViewData[\"Title\"]</gcds-heading>");
        sb.AppendLine();
        sb.AppendLine("<gcds-text>This is your new GC Foundation application!</gcds-text>");
        sb.AppendLine();
        sb.AppendLine("<gcds-card>");
        sb.AppendLine("    <gcds-text>Your application is ready for development with:</gcds-text>");
        sb.AppendLine("    <ul>");
        sb.AppendLine("        <li>GCDS Design System components</li>");
        sb.AppendLine("        <li>FDCP enhanced components</li>");
        sb.AppendLine("        <li>Bilingual support (EN/FR)</li>");
        sb.AppendLine("        <li>Government of Canada branding</li>");
        sb.AppendLine("        <li>Accessibility features</li>");
        sb.AppendLine("    </ul>");
        sb.AppendLine("</gcds-card>");
        sb.AppendLine("```");
        sb.AppendLine();

        // 9. ViewImports and ViewStart
        sb.AppendLine("## 9. View Configuration Files");
        sb.AppendLine();
        sb.AppendLine("### _ViewImports.cshtml");
        sb.AppendLine($"**File**: `src/{projectName}/Views/_ViewImports.cshtml`");
        sb.AppendLine("```csharp");
        sb.AppendLine("@using GCFoundation.Components.TagHelpers.GCDS");
        sb.AppendLine("@using GCFoundation.Components.TagHelpers.FDCP");
        sb.AppendLine($"@using {projectName}");
        sb.AppendLine($"@using {projectName}.Models");
        sb.AppendLine("@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers");
        sb.AppendLine("@addTagHelper *, GCFoundation.Components");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("### _ViewStart.cshtml");
        sb.AppendLine($"**File**: `src/{projectName}/Views/_ViewStart.cshtml`");
        sb.AppendLine("```csharp");
        sb.AppendLine("@{");
        sb.AppendLine("    Layout = \"_Layout\";");
        sb.AppendLine("}");
        sb.AppendLine("```");
        sb.AppendLine();

        // 10. Build Instructions
        sb.AppendLine("## 10. Build and Run Instructions");
        sb.AppendLine();
        sb.AppendLine("1. **Create the directory structure** as shown above");
        sb.AppendLine("2. **Create all files** with the provided content");
        sb.AppendLine("3. **Navigate to the project directory**:");
        sb.AppendLine($"   ```bash");
        sb.AppendLine($"   cd src/{projectName}");
        sb.AppendLine("   ```");
        sb.AppendLine("4. **Restore packages**:");
        sb.AppendLine("   ```bash");
        sb.AppendLine("   dotnet restore");
        sb.AppendLine("   ```");
        sb.AppendLine("5. **Build the project**:");
        sb.AppendLine("   ```bash");
        sb.AppendLine("   dotnet build");
        sb.AppendLine("   ```");
        sb.AppendLine("6. **Run the application**:");
        sb.AppendLine("   ```bash");
        sb.AppendLine("   dotnet run");
        sb.AppendLine("   ```");
        sb.AppendLine();
        sb.AppendLine("## 🎉 Your Complete GC Foundation Project is Ready! 🇨🇦");
        sb.AppendLine();
        sb.AppendLine("Features included:");
        sb.AppendLine("- ✅ Latest GCFoundation packages (1.0.20250819.1)");
        sb.AppendLine("- ✅ GCDS Design System components");
        sb.AppendLine("- ✅ FDCP enhanced components");
        sb.AppendLine("- ✅ Bilingual support (English/French)");
        sb.AppendLine("- ✅ Route localization");
        sb.AppendLine("- ✅ Authentication setup (if enabled)");
        sb.AppendLine("- ✅ Content Security Policies");
        sb.AppendLine("- ✅ Session management");
        sb.AppendLine("- ✅ Government of Canada branding");
        sb.AppendLine("- ✅ Accessibility compliance");

        return sb.ToString();
    }

    #endregion

    #region Project Scaffolding Tools

    [McpServerTool]
    [Description("Generates a complete directory structure for a new GC Foundation project.")]
    public string GenerateProjectStructure(
        [Description("Project name")] string projectName = "MyGCFoundationApp",
        [Description("Include areas for modularity")] bool includeAreas = false,
        [Description("Include API controllers")] bool includeApi = false,
        [Description("Include data layer")] bool includeDataLayer = false,
        [Description("Include test projects")] bool includeTests = false)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"# {projectName} - Project Structure");
        sb.AppendLine();
        sb.AppendLine($"{projectName}/");
        sb.AppendLine("├── src/");
        sb.AppendLine($"│   ├── {projectName}.Web/");
        sb.AppendLine("│   │   ├── Controllers/");
        sb.AppendLine("│   │   │   ├── HomeController.cs");
        sb.AppendLine("│   │   │   ├── ErrorsController.cs");
        sb.AppendLine("│   │   │   └── LanguageController.cs");
        sb.AppendLine("│   │   ├── Models/");
        sb.AppendLine("│   │   │   ├── ViewModels/");
        sb.AppendLine("│   │   │   └── ErrorViewModel.cs");
        sb.AppendLine("│   │   ├── Views/");
        sb.AppendLine("│   │   │   ├── Home/");
        sb.AppendLine("│   │   │   │   └── Index.cshtml");
        sb.AppendLine("│   │   │   ├── Shared/");
        sb.AppendLine("│   │   │   │   ├── _Layout.cshtml");
        sb.AppendLine("│   │   │   │   ├── _ViewStart.cshtml");
        sb.AppendLine("│   │   │   │   ├── _ViewImports.cshtml");
        sb.AppendLine("│   │   │   │   └── Error.cshtml");
        sb.AppendLine("│   │   │   └── _ViewImports.cshtml");
        sb.AppendLine("│   │   ├── Pages/ (for Razor Pages)");
        sb.AppendLine("│   │   │   ├── Shared/");
        sb.AppendLine("│   │   │   │   ├── _Layout.cshtml");
        sb.AppendLine("│   │   │   │   └── _ViewImports.cshtml");
        sb.AppendLine("│   │   │   ├── _ViewStart.cshtml");
        sb.AppendLine("│   │   │   └── Index.cshtml");
        
        if (includeAreas)
        {
            sb.AppendLine("│   │   ├── Areas/");
            sb.AppendLine("│   │   │   ├── Admin/");
            sb.AppendLine("│   │   │   │   ├── Controllers/");
            sb.AppendLine("│   │   │   │   ├── Models/");
            sb.AppendLine("│   │   │   │   └── Views/");
            sb.AppendLine("│   │   │   └── Identity/");
            sb.AppendLine("│   │   │       ├── Controllers/");
            sb.AppendLine("│   │   │       ├── Models/");
            sb.AppendLine("│   │   │       └── Views/");
        }
        
        sb.AppendLine("│   │   ├── Resources/");
        sb.AppendLine("│   │   │   ├── Views/");
        sb.AppendLine("│   │   │   │   ├── Home.en.resx");
        sb.AppendLine("│   │   │   │   └── Home.fr.resx");
        sb.AppendLine("│   │   │   ├── Controllers/");
        sb.AppendLine("│   │   │   └── Shared/");
        sb.AppendLine("│   │   ├── wwwroot/");
        sb.AppendLine("│   │   │   ├── css/");
        sb.AppendLine("│   │   │   │   └── site.css");
        sb.AppendLine("│   │   │   ├── js/");
        sb.AppendLine("│   │   │   │   └── site.js");
        sb.AppendLine("│   │   │   ├── images/");
        sb.AppendLine("│   │   │   └── lib/");
        sb.AppendLine("│   │   ├── Program.cs");
        sb.AppendLine("│   │   ├── Startup.cs");
        sb.AppendLine("│   │   ├── appsettings.json");
        sb.AppendLine("│   │   ├── appsettings.Development.json");
        sb.AppendLine("│   │   ├── navigation.xml");
        sb.AppendLine($"│   │   └── {projectName}.Web.csproj");
        
        if (includeDataLayer)
        {
            sb.AppendLine($"│   ├── {projectName}.Data/");
            sb.AppendLine("│   │   ├── Models/");
            sb.AppendLine("│   │   │   └── ApplicationUser.cs");
            sb.AppendLine("│   │   ├── Contexts/");
            sb.AppendLine("│   │   │   └── ApplicationDbContext.cs");
            sb.AppendLine("│   │   ├── Repositories/");
            sb.AppendLine("│   │   │   ├── Interfaces/");
            sb.AppendLine("│   │   │   │   └── IGenericRepository.cs");
            sb.AppendLine("│   │   │   └── Implementations/");
            sb.AppendLine("│   │   │       └── GenericRepository.cs");
            sb.AppendLine("│   │   ├── Migrations/");
            sb.AppendLine($"│   │   └── {projectName}.Data.csproj");
            sb.AppendLine($"│   ├── {projectName}.Services/");
            sb.AppendLine("│   │   ├── Interfaces/");
            sb.AppendLine("│   │   │   └── IEmailService.cs");
            sb.AppendLine("│   │   ├── Implementations/");
            sb.AppendLine("│   │   │   └── EmailService.cs");
            sb.AppendLine($"│   │   └── {projectName}.Services.csproj");
            sb.AppendLine($"│   └── {projectName}.Models/");
            sb.AppendLine("│       ├── DTOs/");
            sb.AppendLine("│       │   └── UserDTO.cs");
            sb.AppendLine("│       ├── ViewModels/");
            sb.AppendLine("│       │   └── HomeViewModel.cs");
            sb.AppendLine($"│       └── {projectName}.Models.csproj");
        }
        
        if (includeApi)
        {
            sb.AppendLine($"│   └── {projectName}.Api/");
            sb.AppendLine("│       ├── Controllers/");
            sb.AppendLine("│       ├── Models/");
            sb.AppendLine("│       │   ├── DTOs/");
            sb.AppendLine("│       │   └── Requests/");
            sb.AppendLine("│       ├── Program.cs");
            sb.AppendLine("│       ├── appsettings.json");
            sb.AppendLine($"│       └── {projectName}.Api.csproj");
        }
        
        if (includeTests)
        {
            sb.AppendLine("├── tests/");
            sb.AppendLine($"│   ├── {projectName}.UnitTests/");
            sb.AppendLine("│   │   ├── Controllers/");
            sb.AppendLine("│   │   ├── Services/");
            sb.AppendLine("│   │   ├── Helpers/");
            sb.AppendLine($"│   │   └── {projectName}.UnitTests.csproj");
            sb.AppendLine($"│   └── {projectName}.IntegrationTests/");
            sb.AppendLine("│       ├── Controllers/");
            sb.AppendLine("│       ├── Pages/");
            sb.AppendLine("│       ├── TestUtilities/");
            sb.AppendLine($"│       └── {projectName}.IntegrationTests.csproj");
        }
        
        sb.AppendLine("├── tests/");
        sb.AppendLine($"│   ├── {projectName}.UnitTests/");
        sb.AppendLine("│   │   ├── Controllers/");
        sb.AppendLine("│   │   │   └── HomeControllerTests.cs");
        sb.AppendLine("│   │   ├── Services/");
        sb.AppendLine("│   │   │   └── EmailServiceTests.cs");
        sb.AppendLine("│   │   ├── Helpers/");
        sb.AppendLine("│   │   │   └── TestUtilities.cs");
        sb.AppendLine($"│   │   └── {projectName}.UnitTests.csproj");
        sb.AppendLine($"│   └── {projectName}.IntegrationTests/");
        sb.AppendLine("│       ├── Controllers/");
        sb.AppendLine("│       │   └── HomeControllerIntegrationTests.cs");
        sb.AppendLine("│       ├── Pages/");
        sb.AppendLine("│       │   └── IndexPageTests.cs");
        sb.AppendLine("│       ├── TestUtilities/");
        sb.AppendLine("│       │   └── WebApplicationFactory.cs");
        sb.AppendLine($"│       └── {projectName}.IntegrationTests.csproj");
        sb.AppendLine("├── artifacts/");
        sb.AppendLine("│   └── logs/");
        sb.AppendLine("├── docs/");
        sb.AppendLine("│   ├── README.md");
        sb.AppendLine("│   ├── DEPLOYMENT.md");
        sb.AppendLine("│   └── CONFIGURATION.md");
        sb.AppendLine("├── .editorconfig");
        sb.AppendLine("├── .gitignore");
        sb.AppendLine("├── global.json");
        sb.AppendLine($"└── {projectName}.sln");
        
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Provides comprehensive information about GC Foundation project configuration and setup.")]
    public string GetProjectConfigurationInfo(
        [Description("Information type: setup, middleware, services, deployment, all")] string infoType = "all")
    {
        var sb = new StringBuilder();
        
        switch (infoType.ToLower())
        {
            case "setup":
                sb.AppendLine("## GC Foundation Project Setup:");
                sb.AppendLine("1. **Create new ASP.NET Core project**: Use the web application template");
                sb.AppendLine("2. **Add GC Foundation packages**: Install GCFoundation.Common, Components, and Security");
                sb.AppendLine("3. **Configure Program.cs**: Add Foundation services and middleware");
                sb.AppendLine("4. **Setup appsettings.json**: Configure Foundation settings");
                sb.AppendLine("5. **Add resource files**: For localization support");
                sb.AppendLine("6. **Configure views**: Use Foundation layout and components");
                break;
                
            case "middleware":
                sb.AppendLine("## GC Foundation Middleware Order:");
                sb.AppendLine("1. **GCFoundationComponentsMiddleware**: Loads JavaScript dependencies");
                sb.AppendLine("2. **GCFoundationContentPoliciesMiddleware**: Adds Content Security Policy headers");
                sb.AppendLine("3. **GCFoundationLanguageMiddleware**: Handles language routing");
                sb.AppendLine("4. **Standard ASP.NET Core middleware**: Authentication, authorization, etc.");
                sb.AppendLine("5. **UseGCFoundationComponents()**: Foundation component services");
                sb.AppendLine("6. **UseGCFoundationContentPolicies()**: CSP policy application");
                sb.AppendLine("7. **UseGCFoundationSession()**: Session management");
                break;
                
            case "services":
                sb.AppendLine("## GC Foundation Services:");
                sb.AppendLine("- **AddGCFoundationComponents()**: Core component services");
                sb.AppendLine("- **AddGCFoundationContentPolicies()**: Security policy services");
                sb.AppendLine("- **AddGCFoundationSession()**: Session management services");
                sb.AppendLine("- **Navigation services**: Breadcrumbs and site map");
                sb.AppendLine("- **Localization services**: Multi-language support");
                sb.AppendLine("- **Resource management**: Global resources and helpers");
                break;
                
            case "deployment":
                sb.AppendLine("## GC Foundation Deployment Considerations:");
                sb.AppendLine("- **HTTPS**: Always use HTTPS in production");
                sb.AppendLine("- **Content Security Policy**: Configure appropriate CSP headers");
                sb.AppendLine("- **Session Security**: Use secure session cookies");
                sb.AppendLine("- **Localization**: Ensure proper culture configuration");
                sb.AppendLine("- **Static Files**: Optimize and compress static assets");
                sb.AppendLine("- **Health Checks**: Implement application health monitoring");
                break;
                
            default:
                sb.AppendLine("## GC Foundation Project Configuration Guide");
                sb.AppendLine("### Complete setup for Government of Canada web applications");
                sb.AppendLine("### Includes GCDS compliance and accessibility features");
                sb.AppendLine("### Built-in localization and navigation support");
                sb.AppendLine("### Security-first design with CSP and session management");
                sb.AppendLine("### Scalable architecture for government applications");
                sb.AppendLine("");
                sb.AppendLine("Use specific types (setup, middleware, services, deployment) for detailed information.");
                break;
        }
        
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Creates a complete GC Foundation project with all files, folders, and proper structure.")]
    public async Task<string> CreateCompleteGCFoundationProject(
        [Description("Project name")] string projectName = "MyGCFoundationApp",
        [Description("Include data layer projects")] bool includeDataLayer = true,
        [Description("Include API project")] bool includeApi = false,
        [Description("Include test projects")] bool includeTests = true,
        [Description("Include authentication setup")] bool includeAuthentication = true,
        [Description("Include localization")] bool includeLocalization = true)
    {
        var sb = new StringBuilder();
        
        sb.AppendLine($"# Complete GC Foundation Project Creation: {projectName}");
        sb.AppendLine();
        sb.AppendLine("## 1. Solution File Creation");
        sb.AppendLine($"**File**: `{projectName}.sln`");
        sb.AppendLine("```");
        sb.AppendLine("Microsoft Visual Studio Solution File, Format Version 12.00");
        sb.AppendLine("# Visual Studio Version 17");
        sb.AppendLine("VisualStudioVersion = 17.0.31903.59");
        sb.AppendLine("MinimumVisualStudioVersion = 10.0.40219.1");
        sb.AppendLine($"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{projectName}.Web\", \"src\\{projectName}.Web\\{projectName}.Web.csproj\", \"{{11111111-1111-1111-1111-111111111111}}\"");
        sb.AppendLine("EndProject");
        
        if (includeDataLayer)
        {
            sb.AppendLine($"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{projectName}.Data\", \"src\\{projectName}.Data\\{projectName}.Data.csproj\", \"{{22222222-2222-2222-2222-222222222222}}\"");
            sb.AppendLine("EndProject");
            sb.AppendLine($"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{projectName}.Services\", \"src\\{projectName}.Services\\{projectName}.Services.csproj\", \"{{33333333-3333-3333-3333-333333333333}}\"");
            sb.AppendLine("EndProject");
            sb.AppendLine($"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{projectName}.Models\", \"src\\{projectName}.Models\\{projectName}.Models.csproj\", \"{{44444444-4444-4444-4444-444444444444}}\"");
            sb.AppendLine("EndProject");
        }
        
        if (includeTests)
        {
            sb.AppendLine($"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{projectName}.UnitTests\", \"tests\\{projectName}.UnitTests\\{projectName}.UnitTests.csproj\", \"{{55555555-5555-5555-5555-555555555555}}\"");
            sb.AppendLine("EndProject");
            sb.AppendLine($"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{projectName}.IntegrationTests\", \"tests\\{projectName}.IntegrationTests\\{projectName}.IntegrationTests.csproj\", \"{{66666666-6666-6666-6666-666666666666}}\"");
            sb.AppendLine("EndProject");
        }
        
        sb.AppendLine("Global");
        sb.AppendLine("\tGlobalSection(SolutionConfigurationPlatforms) = preSolution");
        sb.AppendLine("\t\tDebug|Any CPU = Debug|Any CPU");
        sb.AppendLine("\t\tRelease|Any CPU = Release|Any CPU");
        sb.AppendLine("\tEndGlobalSection");
        sb.AppendLine("EndGlobal");
        sb.AppendLine("```");
        sb.AppendLine();
        
        // Main Web Project with latest versions
        sb.AppendLine("## 2. Main Web Project");
        sb.AppendLine($"**File**: `src/{projectName}.Web/{projectName}.Web.csproj`");
        sb.AppendLine("```xml");
        
        // Get latest versions for project generation
        var latestVersions = await GetGCFoundationPackageVersionsOnly("https://pkgs.dev.azure.com/tbs-sct/_packaging/TBS_Custom_Feed/nuget/v3/index.json");
        var projectFileContent = GenerateProjectFileWithVersions($"{projectName}.Web", "net8.0", includeDataLayer, includeAuthentication, includeTests, latestVersions);
        sb.AppendLine(projectFileContent);
        sb.AppendLine("```");
        sb.AppendLine();
        
        // Program.cs
        sb.AppendLine("## 3. Program.cs with GC Foundation Setup");
        sb.AppendLine($"**File**: `src/{projectName}.Web/Program.cs`");
        sb.AppendLine("```csharp");
        sb.AppendLine(GenerateGCFoundationProgramCs($"{projectName}.Web", includeAuthentication, includeLocalization, true, true, true));
        sb.AppendLine("```");
        sb.AppendLine();
        
        // appsettings.json
        sb.AppendLine("## 4. Application Settings");
        sb.AppendLine($"**File**: `src/{projectName}.Web/appsettings.json`");
        sb.AppendLine("```json");
        sb.AppendLine(GenerateAppSettingsJson($"{projectName} - Government of Canada Application"));
        sb.AppendLine("```");
        sb.AppendLine();
        
        // Data Layer Projects
        if (includeDataLayer)
        {
            sb.AppendLine("## 5. Data Layer Projects");
            sb.AppendLine();
            
            // Data Project
            sb.AppendLine($"**File**: `src/{projectName}.Data/{projectName}.Data.csproj`");
            sb.AppendLine("```xml");
            sb.AppendLine($"<Project Sdk=\"Microsoft.NET.Sdk\">");
            sb.AppendLine($"  <PropertyGroup>");
            sb.AppendLine($"    <TargetFramework>net8.0</TargetFramework>");
            sb.AppendLine($"    <Nullable>enable</Nullable>");
            sb.AppendLine($"  </PropertyGroup>");
            sb.AppendLine($"  <ItemGroup>");
            sb.AppendLine($"    <PackageReference Include=\"Microsoft.EntityFrameworkCore\" Version=\"8.0.0\" />");
            sb.AppendLine($"    <PackageReference Include=\"Microsoft.EntityFrameworkCore.SqlServer\" Version=\"8.0.0\" />");
            sb.AppendLine($"  </ItemGroup>");
            sb.AppendLine($"</Project>");
            sb.AppendLine("```");
            sb.AppendLine();
            
            // Services Project
            sb.AppendLine($"**File**: `src/{projectName}.Services/{projectName}.Services.csproj`");
            sb.AppendLine("```xml");
            sb.AppendLine($"<Project Sdk=\"Microsoft.NET.Sdk\">");
            sb.AppendLine($"  <PropertyGroup>");
            sb.AppendLine($"    <TargetFramework>net8.0</TargetFramework>");
            sb.AppendLine($"    <Nullable>enable</Nullable>");
            sb.AppendLine($"  </PropertyGroup>");
            sb.AppendLine($"  <ItemGroup>");
            sb.AppendLine($"    <ProjectReference Include=\"..\\{projectName}.Data\\{projectName}.Data.csproj\" />");
            sb.AppendLine($"    <ProjectReference Include=\"..\\{projectName}.Models\\{projectName}.Models.csproj\" />");
            sb.AppendLine($"  </ItemGroup>");
            sb.AppendLine($"</Project>");
            sb.AppendLine("```");
            sb.AppendLine();
            
            // Models Project
            sb.AppendLine($"**File**: `src/{projectName}.Models/{projectName}.Models.csproj`");
            sb.AppendLine("```xml");
            sb.AppendLine($"<Project Sdk=\"Microsoft.NET.Sdk\">");
            sb.AppendLine($"  <PropertyGroup>");
            sb.AppendLine($"    <TargetFramework>net8.0</TargetFramework>");
            sb.AppendLine($"    <Nullable>enable</Nullable>");
            sb.AppendLine($"  </PropertyGroup>");
            sb.AppendLine($"</Project>");
            sb.AppendLine("```");
            sb.AppendLine();
        }
        
        // Test Projects
        if (includeTests)
        {
            sb.AppendLine("## 6. Test Projects");
            sb.AppendLine();
            
            sb.AppendLine($"**File**: `tests/{projectName}.UnitTests/{projectName}.UnitTests.csproj`");
            sb.AppendLine("```xml");
            sb.AppendLine($"<Project Sdk=\"Microsoft.NET.Sdk\">");
            sb.AppendLine($"  <PropertyGroup>");
            sb.AppendLine($"    <TargetFramework>net8.0</TargetFramework>");
            sb.AppendLine($"    <IsPackable>false</IsPackable>");
            sb.AppendLine($"    <IsTestProject>true</IsTestProject>");
            sb.AppendLine($"  </PropertyGroup>");
            sb.AppendLine($"  <ItemGroup>");
            sb.AppendLine($"    <PackageReference Include=\"Microsoft.NET.Test.Sdk\" Version=\"17.8.0\" />");
            sb.AppendLine($"    <PackageReference Include=\"xunit\" Version=\"2.6.1\" />");
            sb.AppendLine($"    <PackageReference Include=\"xunit.runner.visualstudio\" Version=\"2.5.3\" />");
            sb.AppendLine($"    <PackageReference Include=\"Moq\" Version=\"4.20.69\" />");
            sb.AppendLine($"  </ItemGroup>");
            sb.AppendLine($"  <ItemGroup>");
            sb.AppendLine($"    <ProjectReference Include=\"..\\..\\src\\{projectName}.Web\\{projectName}.Web.csproj\" />");
            if (includeDataLayer)
            {
                sb.AppendLine($"    <ProjectReference Include=\"..\\..\\src\\{projectName}.Services\\{projectName}.Services.csproj\" />");
            }
            sb.AppendLine($"  </ItemGroup>");
            sb.AppendLine($"</Project>");
            sb.AppendLine("```");
            sb.AppendLine();
        }
        
        // Basic Controller
        sb.AppendLine("## 7. Sample Home Controller");
        sb.AppendLine($"**File**: `src/{projectName}.Web/Controllers/HomeController.cs`");
        sb.AppendLine("```csharp");
        sb.AppendLine(GenerateBaseController("HomeController", includeAuthentication, includeLocalization));
        sb.AppendLine("```");
        sb.AppendLine();
        
        // Basic View
        sb.AppendLine("## 8. Sample Home View with GCDS Components");
        sb.AppendLine($"**File**: `src/{projectName}.Web/Views/Home/Index.cshtml`");
        sb.AppendLine("```html");
        sb.AppendLine("@{");
        sb.AppendLine("    ViewData[\"Title\"] = \"Home Page\";");
        sb.AppendLine("}");
        sb.AppendLine();
        sb.AppendLine("<gcds-container size=\"xl\" centered=\"true\">");
        sb.AppendLine("    <gcds-heading tag=\"h1\">Welcome to " + projectName + "</gcds-heading>");
        sb.AppendLine("    ");
        sb.AppendLine("    <gcds-text size=\"body\">");
        sb.AppendLine("        This is a Government of Canada application built with GC Foundation.");
        sb.AppendLine("    </gcds-text>");
        sb.AppendLine("    ");
        sb.AppendLine("    <gcds-card>");
        sb.AppendLine("        <gcds-card-header>");
        sb.AppendLine("            <gcds-heading tag=\"h2\" size=\"h3\">Getting Started</gcds-heading>");
        sb.AppendLine("        </gcds-card-header>");
        sb.AppendLine("        <gcds-card-body>");
        sb.AppendLine("            <gcds-text>");
        sb.AppendLine("                Your GC Foundation application is ready! This application includes:");
        sb.AppendLine("            </gcds-text>");
        sb.AppendLine("            <ul>");
        sb.AppendLine("                <li>Government of Canada Design System (GCDS) components</li>");
        sb.AppendLine("                <li>Bilingual support (English/French)</li>");
        sb.AppendLine("                <li>Accessibility compliance (WCAG 2.1 AA)</li>");
        sb.AppendLine("                <li>Content Security Policy configuration</li>");
        sb.AppendLine("                <li>Session management</li>");
        if (includeAuthentication)
        {
            sb.AppendLine("                <li>Authentication and authorization</li>");
        }
        sb.AppendLine("            </ul>");
        sb.AppendLine("        </gcds-card-body>");
        sb.AppendLine("    </gcds-card>");
        sb.AppendLine("</gcds-container>");
        sb.AppendLine("```");
        sb.AppendLine();
        
        // Configuration files
        sb.AppendLine("## 9. Essential Configuration Files");
        sb.AppendLine();
        
        sb.AppendLine("**File**: `.gitignore`");
        sb.AppendLine("```");
        sb.AppendLine("bin/");
        sb.AppendLine("obj/");
        sb.AppendLine("*.user");
        sb.AppendLine("*.suo");
        sb.AppendLine("*.cache");
        sb.AppendLine("Thumbs.db");
        sb.AppendLine("*.DS_Store");
        sb.AppendLine("artifacts/");
        sb.AppendLine("*.log");
        sb.AppendLine("appsettings.Development.json");
        sb.AppendLine("```");
        sb.AppendLine();
        
        sb.AppendLine("**File**: `.editorconfig`");
        sb.AppendLine("```ini");
        sb.AppendLine("root = true");
        sb.AppendLine();
        sb.AppendLine("[*]");
        sb.AppendLine("indent_style = space");
        sb.AppendLine("indent_size = 4");
        sb.AppendLine("end_of_line = crlf");
        sb.AppendLine("charset = utf-8");
        sb.AppendLine("trim_trailing_whitespace = true");
        sb.AppendLine("insert_final_newline = true");
        sb.AppendLine();
        sb.AppendLine("[*.{js,ts,json,yml,yaml}]");
        sb.AppendLine("indent_size = 2");
        sb.AppendLine("```");
        sb.AppendLine();
        
        sb.AppendLine("## 10. Next Steps");
        sb.AppendLine();
        sb.AppendLine("1. **Create the directory structure** as shown above");
        sb.AppendLine("2. **Create all project files** with the provided content");
        sb.AppendLine("3. **Run `dotnet restore`** to restore packages");
        sb.AppendLine("4. **Run `dotnet build`** to build the solution");
        sb.AppendLine("5. **Run `dotnet run --project src/" + projectName + ".Web`** to start the application");
        sb.AppendLine();
        sb.AppendLine("Your complete GC Foundation project is now ready for development! 🇨🇦");
        
        return sb.ToString();
    }

    #endregion
}
