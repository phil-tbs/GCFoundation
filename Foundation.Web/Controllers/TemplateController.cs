using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    [Route("template")]
    public class TemplateController : BaseController
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
