using Foundation.Components.Models;
using Foundation.Components.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Foundation.Web.Controllers
{
    [Route("/")]
    public class LanguageController : BaseController
    {

        [HttpGet("")]
        public IActionResult Index()
        {
            string? culture = Request.Cookies["Culture"];
            if (!string.IsNullOrEmpty(culture) && LanguageUtilitiy.IsCultureSupported(culture))
            {
                Response.Redirect(Url.Action("Index", "Home", new { culture = culture }));
            }

            LanguageChooserModel model = new LanguageChooserModel
            {
                ApplicationTitleEn = "Foundation",
                ApplicationTitleFr = "Fondation",
                EnglishAction = Url.Action("Index", "Home", new {culture = "en"}) ?? "",
                FrenchAction = Url.Action("Index", "Home", new { culture = "fr" }) ?? "",
                TermLinkEn = "",
                TermLinkFr = "",
            };

            return View(model);
        }
    }
}
