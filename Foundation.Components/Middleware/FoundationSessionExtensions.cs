using Foundation.Components.Services;
using Foundation.Components.Setttings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Components.Middleware
{
    /// <summary>
    /// Provides extension methods for adding and configuring foundation session services in an ASP.NET Core application.
    /// </summary>
    public static class FoundationSessionExtensions
    {
        /// <summary>
        /// Adds and configures foundation session services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to which the foundation session services are added.</param>
        /// <param name="configuration">The application configuration containing foundation session settings.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> with foundation session services added.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the configuration parameter is null.</exception>
        public static IServiceCollection AddFoundationSession(this IServiceCollection services, IConfiguration configuration)
        {
            ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));

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

        /// <summary>
        /// Adds the foundation session middleware to the application's request pipeline.
        /// </summary>
        /// <param name="app">The application builder to which the foundation session middleware is added.</param>
        /// <returns>The updated <see cref="IApplicationBuilder"/> with the foundation session middleware configured.</returns>
        public static IApplicationBuilder UseFoundationSession(this IApplicationBuilder app)
        {
            app.UseSession(); // Important: required by ASP.NET Core
            app.UseMiddleware<FoundationSessionMiddleware>(); // Your own logic
            return app;
        }
    }
}
