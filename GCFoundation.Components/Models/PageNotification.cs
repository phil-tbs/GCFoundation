using GCFoundation.Components.Enums;

namespace GCFoundation.Components.Models
{
    /// <summary>
    /// Represents a notification to be displayed on a page.
    /// </summary>
    public class PageNotification
    {
        /// <summary>
        /// Gets or sets the title of the notification.
        /// </summary>
        /// <value>The title of the notification.</value>
        public required string Title { get; set; }

        /// <summary>
        /// Gets or sets the message of the notification.
        /// </summary>
        /// <value>The message content of the notification.</value>
        public required string Message { get; set; }

        /// <summary>
        /// Gets or sets the alert type of the notification, defining its visual appearance and behavior.
        /// </summary>
        /// <value>The type of the alert. Default is <see cref="AlertType.Info"/>.</value>
        public AlertType AlertType { get; set; } = AlertType.Info;
    }
}
