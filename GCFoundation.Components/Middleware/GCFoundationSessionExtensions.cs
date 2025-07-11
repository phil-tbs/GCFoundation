using GCFoundation.Components.Services;
using GCFoundation.Components.Setttings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GCFoundation.Components.Middleware
{
    /// <summary>
    /// Provides extension methods for adding and configuring foundation session services in an ASP.NET Core application.
    /// </summary>
    public static class GCFoundationSessionExtensions
    {
        /// <summary>
        /// Adds and configures foundation session services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to which the foundation session services are added.</param>
        /// <param name="configuration">The application configuration containing foundation session settings.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with foundation session services added.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the configuration parameter is null.</exception>
        public static IServiceCollection AddGCFoundationSession(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

            var section = configuration.GetSection("FoundationSession");
            services.Configure<GCFoundationSessionSetting>(section);

            // Bind once for use here
            var settings = section.Get<GCFoundationSessionSetting>() ?? new GCFoundationSessionSetting();

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

        /// <summary>
        /// Adds the foundation session middleware to the application's request pipeline.
        /// </summary>
        /// <param name="app">The application builder to which the foundation session middleware is added.</param>
        /// <returns>The updated <see cref="IApplicationBuilder"/> with the foundation session middleware configured.</returns>
        public static IApplicationBuilder UseGCFoundationSession(this IApplicationBuilder app)
        {
            app.UseSession(); // Important: required by ASP.NET Core
            app.UseMiddleware<FoundationSessionMiddleware>(); // Your own logic
            return app;
        }
    }
}
