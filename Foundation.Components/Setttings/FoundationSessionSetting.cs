using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foundation.Components.Setttings
{
    /// <summary>
    /// Foundation configuration for the session
    /// </summary>
    public class FoundationSessionSetting
    {

        /// <summary>
        /// Time before the session expired in minutes
        /// </summary>
        public int SessionTimeout { get; set; } = 20;

        /// <summary>
        /// Time when the modal will show to let know the user it need to extend is session
        /// </summary>
        public int ReminderTime { get; set; } = 5;


        /// <summary>
        /// if you want to disable the reminder modal to extend the session
        /// </summary>
        public bool UseReminder { get; set; } = true;


        /// <summary>
        /// If you want to activate the session right away
        /// You can activate it later in you code logic
        /// </summary>
        public bool UseSession { get; set; }

        /// <summary>
        /// Indicates whether the session should reset the timeout on activity.
        /// </summary>
        public bool UseSlidingExpiration { get; set; } = true;


        /// <summary>
        /// The url where the session can be extend this will be call in AJAX Javascript
        /// </summary>
        public Uri RefreshURL { get; set; } = default!;

        /// <summary>
        /// This will be used to logout the user if he click on ending is session
        /// </summary>
        public Uri LogoutURL { get; set; } = default!;


    }
}
