using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Services.Interfaces
{
    /// <summary>
    /// Interface for a service that provides localization for breadcrumbs.
    /// </summary>
    public interface IBreadcrumbsLocalizationService
    {
        /// <summary>
        /// Gets the localized value for a given key.
        /// </summary>
        /// <param name="key">The key for which the localized value is to be retrieved.</param>
        /// <returns>The localized value associated with the given key.</returns>
        public string GetLocalizeValue(string key);
    }
}
