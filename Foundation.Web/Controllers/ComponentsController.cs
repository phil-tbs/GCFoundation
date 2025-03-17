using Foundation.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    [Route("components")]
    public class ComponentsController : BaseController
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("component")]
        public IActionResult GetComponent(string componentName)
        {
            return View(componentName);
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
