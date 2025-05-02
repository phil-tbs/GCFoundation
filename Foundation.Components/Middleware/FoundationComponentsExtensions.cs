using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Common.Settings;
using Foundation.Components.Configuration;
using Foundation.Components.Setttings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Foundation.Components.Middleware
{
    public static class FoundationComponentsExtensions
    {

        public static IServiceCollection AddFoundationComponents(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("FoundationComponentsSettings");
            services.Configure<FoundationComponentsSettings>(section);

            // Register the CdnPolicyConfigurator
            services.AddSingleton<IConfigureOptions<FoundationContentPolicySettings>, FoundationComponentsCdnPolicyConfigurator>();

            return services;
        }

        public static IApplicationBuilder UseFoundationComponents(this IApplicationBuilder app)
        {
            app.UseMiddleware<FoundationComponentsMiddleware>();
            return app;
        }

    }
}
