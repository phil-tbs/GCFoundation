namespace Foundation.Web.Models
{
    /// <summary>
    /// Represents a test user with basic identity information.
    /// </summary>
    public class TestUser
    {
        /// <summary>
        /// Gets or sets the unique identifier for the user.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the full name of the user.
        /// </summary>
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets the email address of the user.
        /// </summary>
        public string Email { get; set; } = "";
    }
}
