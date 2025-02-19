using Foundation.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    public class ComponentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public IActionResult GetComponent(string componentName)
        {
            return View(componentName);
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
