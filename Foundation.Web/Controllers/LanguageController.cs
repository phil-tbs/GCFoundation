using Foundation.Components.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Foundation.Web.Controllers
{
    [Route("language")]
    public class LanguageController : BaseController
    {
        private readonly ICompositeViewEngine _viewEngine;
        public LanguageController(ICompositeViewEngine viewEngine) 
        {
            _viewEngine = viewEngine;
        }

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
