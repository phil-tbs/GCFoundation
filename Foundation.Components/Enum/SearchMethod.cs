using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Enum
{
    /// <summary>
    /// Represents the HTTP method to use for a search form.
    /// </summary>
    public enum SearchMethod
    {
        /// <summary>
        /// Use the HTTP GET method to submit the form data.
        /// </summary>
        Get,

        /// <summary>
        /// Use the HTTP POST method to submit the form data.
        /// </summary>
        Post
    }
}
