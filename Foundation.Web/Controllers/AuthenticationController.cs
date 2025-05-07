using System.Globalization;
using Foundation.Components.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Web.Controllers
{
    /// <summary>
    /// Provides actions for user authentication, including login, logout, and session refresh.
    /// </summary>
    [Route("authentication")]
    public class AuthenticationController : BaseController
    {
        private readonly ILogger<AuthenticationController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthenticationController"/> class.
        /// </summary>
        /// <param name="logger">The logger used for logging actions and events in this controller.</param>
        public AuthenticationController(ILogger<AuthenticationController> logger)
            : base(logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Simulates a login action by setting a session variable indicating the session start time.
        /// </summary>
        /// <returns>Redirects to the Home/Index action.</returns>
        [HttpGet("login")]
        public IActionResult Login()
        {
            HttpContext.Session.SetString("UserSessionStarted", DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));

            // Optional: redirect or return success
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Simulates a logout action and sets a danger-type notification indicating session timeout.
        /// </summary>
        /// <returns>Redirects to the Home/Index action with a session timeout notification.</returns>
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            SetPageNotification(new Components.Models.PageNotification
            {
                Title = "Your session timeout!",
                Message = "For inactivity your session timeout.",
                AlertType = AlertType.Danger
            });
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Refreshes the session by updating a "KeepAlive" timestamp in the session.
        /// Typically used for keeping the session alive via AJAX ping.
        /// </summary>
        /// <returns>An HTTP 200 OK result.</returns>
        [HttpPost("refresh")]
        public IActionResult RefreshSession()
        {
            HttpContext.Session.SetString("KeepAlive", DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture));

            return Ok();
        }
    }
}
