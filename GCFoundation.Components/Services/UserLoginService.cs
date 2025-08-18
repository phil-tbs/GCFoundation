using GCFoundation.Components.Models;
using GCFoundation.Components.Settings;
using GCFoundation.Common.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Security.Claims;

namespace GCFoundation.Components.Services
{
    /// <summary>
    /// Service for creating and managing user login view models.
    /// Provides helper methods to build UserLoginViewModel instances from various sources.
    /// </summary>
    public class UserLoginService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GCFoundationUserLoginSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="userLoginSettings">The user login settings.</param>
        public UserLoginService(
            IHttpContextAccessor httpContextAccessor,
            IOptions<GCFoundationUserLoginSettings> userLoginSettings)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _settings = userLoginSettings?.Value ?? throw new ArgumentNullException(nameof(userLoginSettings));
        }

        /// <summary>
        /// Creates a UserLoginViewModel from the current HTTP context.
        /// Extracts user information from claims and session data.
        /// </summary>
        /// <returns>A configured UserLoginViewModel instance.</returns>
        public UserLoginViewModel CreateViewModelFromContext()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                return CreateAnonymousViewModel();
            }

            var user = context.User;
            var isAuthenticated = user?.Identity?.IsAuthenticated ?? false;

            if (!isAuthenticated)
            {
                return CreateAnonymousViewModel();
            }

            var viewModel = new UserLoginViewModel
            {
                IsAuthenticated = true,
                Settings = _settings,
                CurrentCulture = LanguageUtility.GetCurrentApplicationLanguage(),
                UserName = GetClaimValue(user, ClaimTypes.Name),
                UserEmail = GetClaimValue(user, ClaimTypes.Email),
                FirstName = GetClaimValue(user, ClaimTypes.GivenName),
                LastName = GetClaimValue(user, ClaimTypes.Surname),
                UserRole = GetClaimValue(user, ClaimTypes.Role),
                Department = GetClaimValue(user, "department") ?? GetClaimValue(user, "dept")
            };

            // Try to get login time from claims (stateless approach)
            var loginTimeClaim = GetClaimValue(user, "login_time") ?? GetClaimValue(user, "auth_time");
            if (!string.IsNullOrEmpty(loginTimeClaim))
            {
                if (long.TryParse(loginTimeClaim, out var unixTime))
                {
                    // Handle Unix timestamp (common in JWT tokens)
                    viewModel.LoginTime = DateTimeOffset.FromUnixTimeSeconds(unixTime).UtcDateTime;
                }
                else if (DateTime.TryParse(loginTimeClaim, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out var loginTime))
                {
                    // Handle ISO datetime string
                    viewModel.LoginTime = loginTime;
                }
            }

            // Calculate session expiry based on JWT expiration or configured timeout
            var expClaim = GetClaimValue(user, "exp");
            if (!string.IsNullOrEmpty(expClaim) && long.TryParse(expClaim, out var expUnixTime))
            {
                // Use JWT expiration time
                viewModel.SessionExpiry = DateTimeOffset.FromUnixTimeSeconds(expUnixTime).UtcDateTime;
            }
            else if (viewModel.LoginTime.HasValue)
            {
                // Fallback to configured timeout
                var sessionTimeoutMinutes = 20; // Get from configuration
                viewModel.SessionExpiry = viewModel.LoginTime.Value.AddMinutes(sessionTimeoutMinutes);
            }

            return viewModel;
        }

        /// <summary>
        /// Creates a UserLoginViewModel for a specific user with provided information.
        /// Useful for testing or when user information is available from other sources.
        /// </summary>
        /// <param name="userName">The user's full name.</param>
        /// <param name="userEmail">The user's email address.</param>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name.</param>
        /// <param name="userRole">The user's role.</param>
        /// <param name="department">The user's department.</param>
        /// <param name="loginTime">The time the user logged in.</param>
        /// <param name="sessionExpiry">The time the session expires.</param>
        /// <returns>A configured UserLoginViewModel instance.</returns>
        public UserLoginViewModel CreateViewModel(
            string? userName = null,
            string? userEmail = null,
            string? firstName = null,
            string? lastName = null,
            string? userRole = null,
            string? department = null,
            DateTime? loginTime = null,
            DateTime? sessionExpiry = null)
        {
            return new UserLoginViewModel
            {
                IsAuthenticated = true,
                Settings = _settings,
                CurrentCulture = LanguageUtility.GetCurrentApplicationLanguage(),
                UserName = userName,
                UserEmail = userEmail,
                FirstName = firstName,
                LastName = lastName,
                UserRole = userRole,
                Department = department,
                LoginTime = loginTime,
                SessionExpiry = sessionExpiry
            };
        }

        /// <summary>
        /// Creates a UserLoginViewModel for anonymous (not authenticated) users.
        /// </summary>
        /// <returns>A UserLoginViewModel configured for anonymous users.</returns>
        public UserLoginViewModel CreateAnonymousViewModel()
        {
            return new UserLoginViewModel
            {
                IsAuthenticated = false,
                Settings = _settings,
                CurrentCulture = LanguageUtility.GetCurrentApplicationLanguage()
            };
        }

        /// <summary>
        /// Updates the session expiry time for an existing view model.
        /// Useful for refreshing session information without recreating the entire model.
        /// </summary>
        /// <param name="viewModel">The view model to update.</param>
        /// <param name="sessionTimeoutMinutes">The session timeout in minutes.</param>
        public static void UpdateSessionExpiry(UserLoginViewModel viewModel, int sessionTimeoutMinutes = 20)
        {
            if (viewModel?.LoginTime.HasValue == true)
            {
                viewModel.SessionExpiry = viewModel.LoginTime.Value.AddMinutes(sessionTimeoutMinutes);
            }
        }

        /// <summary>
        /// Gets a claim value from the current user's claims.
        /// </summary>
        /// <param name="user">The claims principal.</param>
        /// <param name="claimType">The type of claim to retrieve.</param>
        /// <returns>The claim value or null if not found.</returns>
        private static string? GetClaimValue(ClaimsPrincipal? user, string claimType)
        {
            return user?.FindFirst(claimType)?.Value;
        }
    }
}

