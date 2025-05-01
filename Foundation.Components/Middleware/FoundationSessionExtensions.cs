using Foundation.Components.Services;
using Foundation.Components.Setttings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Components.Middleware
{
    public static class FoundationSessionExtensions
    {
        public static IServiceCollection AddFoundationSession(this IServiceCollection services, IConfiguration configuration)
        {
            var section = configuration.GetSection("FoundationSession");
            services.Configure<FoundationSessionSetting>(section);

            // Bind once for use here
            var settings = section.Get<FoundationSessionSetting>() ?? new FoundationSessionSetting();

            // Add built-in session services
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(settings.SessionTimeout);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddSingleton<FoundationSessionService>();
            return services;
        }

        public static IApplicationBuilder UseFoundationSession(this IApplicationBuilder app)
        {
            app.UseSession(); // Important: required by ASP.NET Core
            app.UseMiddleware<FoundationSessionMiddleware>(); // Your own logic
            return app;
        }
    }
}
