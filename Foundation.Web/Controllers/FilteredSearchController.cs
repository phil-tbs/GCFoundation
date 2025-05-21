using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    /// <summary>
    /// Controller for handling filtered search operations.
    /// </summary>
    [Route("filteredSearch")]
    public class FilteredSearchController : Controller
    {
        /// <summary>
        /// Displays the filtered search index page.
        /// </summary>
        /// <returns>The view for the filtered search index page.</returns>
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
