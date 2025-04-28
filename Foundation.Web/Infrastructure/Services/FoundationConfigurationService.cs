using Foundation.Components.Setttings;
using Foundation.Security.Settings;

namespace Foundation.Web.Infrastructure.Services
{
    /// <summary>
    /// Foundation configuration class
    /// All configuration related to foundation
    /// Should be call from program.cs
    /// </summary>
    public static class FoundationConfigurationService
    {
        private static List<string> GetWithDefaults(this IConfiguration configuration, string sectionName, List<string> defaults)
        {
            var list = configuration.GetSection(sectionName).Get<List<string>>() ?? new List<string>();
            list.AddRange(defaults);
            return list;
        }

        public static void ConfigureFoundationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Default settings
            var defaultSettings = new FoundationComponentsSettings();

            // Configure Foundation Components settings
            services.Configure<FoundationComponentsSettings>(options =>
            {
                options.FontAwesomeVersion = configuration.GetValue<string>("FoundationComponentsSettings:FontAwesomeVersion")
                                            ?? defaultSettings.FontAwesomeVersion;

                options.GCDSVersion = configuration.GetValue<string>("FoundationComponentsSettings:GCDSVersion")
                                      ?? defaultSettings.GCDSVersion;
                options.ApplicationNameEn = configuration.GetValue<string>("FoundationComponentsSettings:ApplicationNameFr") ?? "No name";
                options.ApplicationNameFr = configuration.GetValue<string>("FoundationComponentsSettings:ApplicationNameEn") ?? "Sans nom";
            });

            // Configure Content Policy settings
            services.Configure<ContentPolicySettings>(options =>
            {
                options.JavascriptCDN = configuration.GetWithDefaults("ContentPolicySettings:JavascriptCDN", new List<string> { defaultSettings.GCDSJavaScriptCDN.Host });
                options.CssCDN = configuration.GetWithDefaults("ContentPolicySettings:CssCDN", new List<string>
            {
                defaultSettings.GCDSCssCDN.Host,
                defaultSettings.FontAwesomeCDN.Host
            });
                options.CssCDNHash = configuration.GetWithDefaults("ContentPolicySettings:CssCDNHash", new List<string> { defaultSettings.GCDSCssCDNHash });
                options.FontCDN = configuration.GetWithDefaults("ContentPolicySettings:FontCDN", new List<string> { defaultSettings.FontAwesomeCDN.Host });
            });
        }
    }
}
