using GCFoundation.Web.Controllers;
using RouteLocalization.AspNetCore;

namespace GCFoundation.Web.Infrastructure.Extensions
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
                // Home Controller
                setup.UseCulture("fr")
                .WhereController(nameof(HomeController))
                .TranslateController("accueil")
                .WhereAction(nameof(HomeController.Index))
                .TranslateAction("");

                // Components Controller
                setup.UseCulture("fr")
                .WhereController(nameof(ComponentsController))
                .TranslateController("composants")
                .WhereAction(nameof(ComponentsController.Index))
                .TranslateAction("");

                setup.UseCulture("fr")
                .WhereController(nameof(ComponentsController))
                .WhereAction(nameof(ComponentsController.Gcds))
                .TranslateAction("gcds");

                setup.UseCulture("fr")
                .WhereController(nameof(ComponentsController))
                .WhereAction(nameof(ComponentsController.FormValidationTest))
                .TranslateAction("test-validation-formulaire");

                setup.UseCulture("fr")
                .WhereController(nameof(ComponentsController))
                .WhereAction(nameof(ComponentsController.GlobalResources))
                .TranslateAction("ressources-globales");

                setup.UseCulture("fr")
                .WhereController(nameof(ComponentsController))
                .WhereAction(nameof(ComponentsController.ExampleFormBuilder))
                .TranslateAction("constructeur-formulaire-exemple");

                setup.UseCulture("fr")
                .WhereController(nameof(ComponentsController))
                .WhereAction(nameof(ComponentsController.SubmitFormBuilder))
                .TranslateAction("soumettre-constructeur-formulaire");

                setup.UseCulture("fr")
                .WhereController(nameof(ComponentsController))
                .WhereAction(nameof(ComponentsController.Badge))
                .TranslateAction("badge");

                setup.UseCulture("fr")
                .WhereController(nameof(ComponentsController))
                .WhereAction(nameof(ComponentsController.Modal))
                .TranslateAction("modal");

                setup.UseCulture("fr")
                .WhereController(nameof(ComponentsController))
                .WhereAction(nameof(ComponentsController.Table))
                .TranslateAction("tableau");

                setup.UseCulture("fr")
                .WhereController(nameof(ComponentsController))
                .WhereAction(nameof(ComponentsController.PageHeading))
                .TranslateAction("titre-page");

                setup.UseCulture("fr")
                .WhereController(nameof(ComponentsController))
                .WhereAction(nameof(ComponentsController.Stepper))
                .TranslateAction("etapes");

                // Template Controller
                setup.UseCulture("fr")
                .WhereController(nameof(TemplateController))
                .TranslateController("modele")
                .WhereAction(nameof(TemplateController.Index))
                .TranslateAction("");

                // Authentication Controller
                setup.UseCulture("fr")
                .WhereController(nameof(AuthenticationController))
                .TranslateController("authentification")
                .WhereAction(nameof(AuthenticationController.Login))
                .TranslateAction("connexion");

                setup.UseCulture("fr")
                .WhereController(nameof(AuthenticationController))
                .WhereAction(nameof(AuthenticationController.Logout))
                .TranslateAction("deconnexion");

                setup.UseCulture("fr")
                .WhereController(nameof(AuthenticationController))
                .WhereAction(nameof(AuthenticationController.RefreshSession))
                .TranslateAction("rafraichir-session");

                // Installation Controller
                setup.UseCulture("fr")
                .WhereController(nameof(InstallationController))
                .TranslateController("installation")
                .WhereAction(nameof(InstallationController.Index))
                .TranslateAction("");

                // Language Controller
                setup.UseCulture("fr")
                .WhereController(nameof(LanguageController))
                .TranslateController("langue")
                .WhereAction(nameof(LanguageController.Index))
                .TranslateAction("");

                // Styles Controller
                setup.UseCulture("fr")
                .WhereController(nameof(StylesController))
                .TranslateController("styles")
                .WhereAction(nameof(StylesController.Index))
                .TranslateAction("");

                // FilteredSearch Controller
                setup.UseCulture("fr")
                .WhereController(nameof(FilteredSearchController))
                .TranslateController("recherche-filtree")
                .WhereAction(nameof(FilteredSearchController.Index))
                .TranslateAction("");

                // Keep original English routes for all controllers
                setup.UseCulture("en")
                    .WhereController(nameof(HomeController))
                    .WhereAction(nameof(HomeController.Index))
                    .TranslateAction("");

                setup.UseCulture("en")
                    .WhereController(nameof(ComponentsController))
                    .WhereAction(nameof(ComponentsController.Index))
                    .TranslateAction("");

                setup.UseCulture("en")
                    .WhereController(nameof(ComponentsController))
                    .WhereAction(nameof(ComponentsController.Gcds))
                    .TranslateAction("gcds");

                setup.UseCulture("en")
                    .WhereController(nameof(ComponentsController))
                    .WhereAction(nameof(ComponentsController.FormValidationTest))
                    .TranslateAction("testingForm");

                setup.UseCulture("en")
                    .WhereController(nameof(ComponentsController))
                    .WhereAction(nameof(ComponentsController.GlobalResources))
                    .TranslateAction("global-resources");

                setup.UseCulture("en")
                    .WhereController(nameof(ComponentsController))
                    .WhereAction(nameof(ComponentsController.ExampleFormBuilder))
                    .TranslateAction("TestFormBuilder");

                setup.UseCulture("en")
                    .WhereController(nameof(ComponentsController))
                    .WhereAction(nameof(ComponentsController.SubmitFormBuilder))
                    .TranslateAction("SubmitFormBuilder");

                setup.UseCulture("en")
                    .WhereController(nameof(ComponentsController))
                    .WhereAction(nameof(ComponentsController.Badge))
                    .TranslateAction("badge");

                setup.UseCulture("en")
                    .WhereController(nameof(ComponentsController))
                    .WhereAction(nameof(ComponentsController.Modal))
                    .TranslateAction("modal");

                setup.UseCulture("en")
                    .WhereController(nameof(ComponentsController))
                    .WhereAction(nameof(ComponentsController.Table))
                    .TranslateAction("table");

                setup.UseCulture("en")
                    .WhereController(nameof(ComponentsController))
                    .WhereAction(nameof(ComponentsController.PageHeading))
                    .TranslateAction("page-heading");

                setup.UseCulture("en")
                    .WhereController(nameof(ComponentsController))
                    .WhereAction(nameof(ComponentsController.Stepper))
                    .TranslateAction("stepper");

                setup.UseCulture("en")
                    .WhereController(nameof(TemplateController))
                    .WhereAction(nameof(TemplateController.Index))
                    .TranslateAction("");

                setup.UseCulture("en")
                    .WhereController(nameof(AuthenticationController))
                    .WhereAction(nameof(AuthenticationController.Login))
                    .TranslateAction("login");

                setup.UseCulture("en")
                    .WhereController(nameof(AuthenticationController))
                    .WhereAction(nameof(AuthenticationController.Logout))
                    .TranslateAction("logout");

                setup.UseCulture("en")
                    .WhereController(nameof(AuthenticationController))
                    .WhereAction(nameof(AuthenticationController.RefreshSession))
                    .TranslateAction("refresh");

                setup.UseCulture("en")
                    .WhereController(nameof(InstallationController))
                    .WhereAction(nameof(InstallationController.Index))
                    .TranslateAction("");

                setup.UseCulture("en")
                    .WhereController(nameof(LanguageController))
                    .WhereAction(nameof(LanguageController.Index))
                    .TranslateAction("");

                setup.UseCulture("en")
                    .WhereController(nameof(StylesController))
                    .WhereAction(nameof(StylesController.Index))
                    .TranslateAction("");

                setup.UseCulture("en")
                    .WhereController(nameof(FilteredSearchController))
                    .WhereAction(nameof(FilteredSearchController.Index))
                    .TranslateAction("");

                // Ensure untranslated routes exist for controllers with attribute routes only
                setup.UseCultures(SupportedCultures)
                    .WhereUntranslated()
                    .AddDefaultTranslation();
            });

            return services;
        }
    }
}
