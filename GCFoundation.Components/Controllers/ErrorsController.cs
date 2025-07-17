using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GCFoundation.Components.Controllers
{
    /// <summary>
    /// Controller responsible for handling error pages.
    /// </summary>
    public class ErrorsController : GCFoundationBaseController
    {
        /// <summary>
        /// Delegate for logging 404 Not Found errors.
        /// </summary>
        private static readonly Action<ILogger, string, string, Exception?> _logNotFound =
            LoggerMessage.Define<string, string>(
            LogLevel.Information,
            new EventId(1404, nameof(NotFoundError)),
            "404 Not Found error for path: {Path}{QueryString}");

        /// <summary>
        /// Delegate for logging unhandled exceptions leading to a global error page.
        /// </summary>
        private static readonly Action<ILogger, string, Exception?> _logGlobalError =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                new EventId(1500, nameof(GlobalError)),
                "Unhandled exception at path: {Path}");

        private readonly ILogger<ErrorsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorsController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance for the controller.</param>
        public ErrorsController(ILogger<ErrorsController> logger) : base(logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Displays the custom 404 Not Found error page.
        /// </summary>
        /// <returns>The NotFound view.</returns>
        [AllowAnonymous]
        [Route("Error/NotFound")]
        public IActionResult NotFoundError()
        {
            Response.StatusCode = 404;
            var originalPath = HttpContext.Request.Path;
            var queryString = HttpContext.Request.QueryString.ToString();

            _logNotFound(_logger, originalPath, queryString, null);
            return View("NotFound");
        }

        /// <summary>
        /// Displays a global error page for unhandled exceptions.
        /// </summary>
        /// <returns>The GlobalError view.</returns>
        [AllowAnonymous]
        [Route("Error/Global")]
        public IActionResult GlobalError()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionFeature != null)
            {
                _logGlobalError(_logger, exceptionFeature.Path, exceptionFeature.Error);
            }
            return View("GlobalError");
        }
    }
}
