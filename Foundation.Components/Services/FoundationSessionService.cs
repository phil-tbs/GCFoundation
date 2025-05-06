using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.Setttings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Foundation.Components.Services
{
    /// <summary>
    /// Provides session management services for the application, including checking the session status,
    /// refreshing the session, and ending the session.
    /// </summary>
    public class FoundationSessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FoundationSessionSetting _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="FoundationSessionService"/> class.
        /// </summary>
        /// <param name="accessor">The HTTP context accessor to access the HTTP context.</param>
        /// <param name="options">The options containing session settings.</param>
        /// <exception cref="ArgumentNullException">Thrown when the options parameter is null.</exception>
        public FoundationSessionService(IHttpContextAccessor accessor, IOptions<FoundationSessionSetting> options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            _httpContextAccessor = accessor;
            _settings = options.Value;
        }

        /// <summary>
        /// Checks whether the session is active.
        /// </summary>
        /// <returns><c>true</c> if the session is active; otherwise, <c>false</c>.</returns>
        public bool IsSessionActive()
        {
            var context = _httpContextAccessor.HttpContext;
            return context?.Session.GetString("SessionActive") == "true";
        }

        /// <summary>
        /// Refreshes the session by setting the "SessionActive" flag to true.
        /// </summary>
        public void RefreshSession()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                context.Session.SetString("SessionActive", "true");
            }
        }

        /// <summary>
        /// Ends the current session by clearing the session data.
        /// </summary>
        public void EndSession()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
        }
    }
}
