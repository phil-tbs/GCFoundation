using Foundation.Web.Controllers;
using RouteLocalization.AspNetCore;

namespace Foundation.Web.Infrastructure.Extensions
{
    /// <summary>
    /// Provides extension methods for setting up route localization in the application.
    /// </summary>
    public static class RouteLocalizationExtensions
    {
        /// <summary>
        /// Supported cultures for the application routing.
        /// </summary>
        private static readonly string[] SupportedCultures = ["en", "fr"];

        /// <summary>
        /// Adds custom route localization configuration to the application.
        /// </summary>
        /// <param name="services">The service collection to which the route localization is added.</param>
        /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddCustomRouteLocalization(this IServiceCollection services)
        {
            services.AddRouteLocalization(setup =>
            {
                setup.UseCulture("fr")
                .WhereController(nameof(HomeController))
                .TranslateController("Accueil")
                .WhereAction(nameof(HomeController.Index)) // Specify action for translation
                .TranslateAction("");

                setup.UseCulture("fr")
                .WhereController(nameof(ComponentsController))
                .TranslateController("composants")
                .WhereAction(nameof(ComponentsController.Index))
                .TranslateAction("");

                setup.UseCulture("fr")
                .WhereController(nameof(ComponentsController))
                .WhereAction(nameof(ComponentsController.GetComponent))
                .TranslateAction("composant");

                // Keep original English routes
                setup.UseCulture("en")
                    .WhereController(nameof(ComponentsController))
                    .WhereAction(nameof(ComponentsController.Index))
                    .TranslateAction(""); // Ensures "/en/components" works

                setup.UseCulture("en")
                    .WhereController(nameof(ComponentsController))
                    .WhereAction(nameof(ComponentsController.GetComponent))
                    .TranslateAction("component"); // Ensures "/en/components/component" works

                // Ensure untranslated routes exist
                setup.UseCultures(SupportedCultures)
                    .WhereUntranslated()
                    .AddDefaultTranslation();
            });

            return services;
        }
    }
}
