using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    [Route("authentication")]
    public class AuthenticationController : BaseController
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
            SetPageNotification(new Components.Models.PageNotification
            {
                Title = "Your session timeout!",
                Message = "For inactivity your session timeout.",
                AlertType = Components.Enum.AlertType.Danger
            });
            return RedirectToAction("Index", "Home");
        }

        [HttpPost("refresh")]
        public IActionResult RefreshSession()
        {
            HttpContext.Session.SetString("KeepAlive", DateTime.UtcNow.ToString());

            return Ok();
        }
    }
}
