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
    }
}
