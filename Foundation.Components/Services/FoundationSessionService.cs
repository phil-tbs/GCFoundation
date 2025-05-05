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
    public class FoundationSessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly FoundationSessionSetting _settings;

        public FoundationSessionService(IHttpContextAccessor accessor, IOptions<FoundationSessionSetting> options)
        {
            ArgumentNullException.ThrowIfNull(options, nameof(options));

            _httpContextAccessor = accessor;
            _settings = options.Value;
        }

        public bool IsSessionActive()
        {
            var context = _httpContextAccessor.HttpContext;
            return context?.Session.GetString("SessionActive") == "true";
        }

        public void RefreshSession()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context != null)
            {
                context.Session.SetString("SessionActive", "true");
            }
        }

        public void EndSession()
        {
            _httpContextAccessor.HttpContext?.Session.Clear();
        }
    }
}
