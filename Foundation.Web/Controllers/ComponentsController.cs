using Foundation.Web.Models;
using Foundation.Web.Resources;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    [Route("components")]
    public class ComponentsController : BaseController
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            SetPageTitle(Menu.Menu_Components);
            return View();
        }


        [HttpGet("component")]
        public IActionResult GetComponent(string componentName)
        {
            SetPageTitle($"{Menu.Menu_Components}: {componentName}");
            return View(componentName);
        }

        [HttpGet("gcds")]
        public IActionResult Gcds()
        {
            SetPageTitle(Menu.Menu_Components_GCDesign);
            return View();
        }

        [HttpGet("testingForm")]
        public IActionResult FormValidationTest()
        {
            SetPageTitle("Form");
            FormTestViewModel model = new FormTestViewModel();

            return View("Forms", model);
        }


        [HttpPost]
        public IActionResult FormValidationTest(FormTestViewModel model)
        {

            if (ModelState.IsValid)
            {

            }

            return View("Forms", model);
        }
    }
}
