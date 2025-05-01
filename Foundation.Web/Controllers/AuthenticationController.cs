using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    [Route("authentication")]
    public class AuthenticationController : Controller
    {
        [HttpGet("login")]
        public IActionResult Login()
        {
            HttpContext.Session.SetString("UserSessionStarted", DateTime.UtcNow.ToString());

            // Optional: redirect or return success
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("extend")]
        public IActionResult RefreshSession()
        {
            HttpContext.Session.SetString("KeepAlive", DateTime.UtcNow.ToString());

            return Ok();
        }
    }
}
