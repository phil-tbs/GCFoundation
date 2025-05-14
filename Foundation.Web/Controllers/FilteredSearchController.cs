using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    [Route("filteredSearch")]
    public class FilteredSearchController : Controller
    {

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
