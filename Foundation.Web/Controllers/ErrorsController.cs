using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    public class ErrorsController : Controller
    {
        [Route("Error/NotFound")]
        public IActionResult NotFoundError()
        {
            return View("NotFound");
        }

        [Route("Error/Global")]
        public IActionResult GlobalError()
        {
            return View("GlobalError");
        }
    }
}
