using GCFoundation.Common.Utilities;
using GCFoundation.Components.Controllers;
using GCFoundation.Components.Models;
using Microsoft.AspNetCore.Mvc;

namespace GCFoundation.Web.Controllers
{
    /// <summary>
    /// Controller responsible for handling language selection and redirection based on user culture.
    /// </summary>
    [Route("/")]
    public class LanguageController : FoundationBaseController
    {
        private readonly ILogger<LanguageController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageController"/> class.
        /// </summary>
        /// <param name="logger">The logger used for logging actions and events in this controller.</param>
        public LanguageController(ILogger<LanguageController> logger)
            : base(logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Displays the language chooser page or redirects to the home page based on the user's culture cookie.
        /// </summary>
        /// <returns>
        /// A redirection to the home page in the appropriate language if the culture cookie is valid;
        /// otherwise, the language selection view.
        /// </returns>
        [HttpGet("")]
        public IActionResult Index()
        {
            string? culture = Request.Cookies["Culture"];
            if (!string.IsNullOrEmpty(culture) && LanguageUtility.IsCultureSupported(culture))
            {
                var url = Url.Action("Index", "Home", new { culture });
                if (!string.IsNullOrEmpty(url))
                {
                    return Redirect(url);
                }
            }

            LanguageChooserModel model = new()
            {
                ApplicationTitleEn = "Foundation",
                ApplicationTitleFr = "Fondation",
                EnglishAction = Url.Action("Index", "Home", new { culture = "en" }) ?? "",
                FrenchAction = Url.Action("Index", "Home", new { culture = "fr" }) ?? "",
                TermLinkEn = "",
                TermLinkFr = "",
            };

            ViewData["Title"] = $"{model.ApplicationTitleEn} / {model.ApplicationTitleFr}";

            return View(model);
        }
    }
}
