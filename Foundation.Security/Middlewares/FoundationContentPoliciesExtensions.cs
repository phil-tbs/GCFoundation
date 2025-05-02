using Foundation.Common.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Security.Middlewares
{
    public static class FoundationContentPoliciesExtensions
    {

        public static IServiceCollection AddFoundationContentPolicies(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("ContentPolicySettings");
            services.Configure<FoundationContentPolicySettings>(section);

            return services;
        }

        public static IApplicationBuilder UseFoundationContentPolicies(this IApplicationBuilder app)
        {
            app.UseMiddleware<FoundationContentPoliciesMiddleware>();
            return app;
        }
    }
}
