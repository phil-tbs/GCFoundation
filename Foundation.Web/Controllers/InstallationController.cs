using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    [Route("installtion")]
    public class InstallationController : BaseController
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
