using Foundation.Components.Models;
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
