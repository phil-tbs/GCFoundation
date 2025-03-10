using Foundation.Web.Controllers;
using RouteLocalization.AspNetCore;

namespace Foundation.Web.Infrastructure.Extensions
{
    public static class RouteLocalizationExtensions
    {
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
                setup.UseCultures(new[] { "en", "fr" })
                    .WhereUntranslated()
                    .AddDefaultTranslation();
            });

            return services;
        }
    }
}
