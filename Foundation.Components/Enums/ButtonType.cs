namespace Foundation.Components.Enums
{
    /// <summary>
    /// Defines the different types of buttons that can be used in HTML forms.
    /// This enum specifies the behavior and purpose of a button element within a form or interface.
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// A generic button that can be used for custom actions. Does not trigger any form submission by default.
        /// </summary>
        button,

        /// <summary>
        /// A button that acts as a hyperlink, typically used for navigation or linking to other pages.
        /// </summary>
        link,

        /// <summary>
        /// A button that submits the form data to the server.
        /// </summary>
        submit,

        /// <summary>
        /// A button that resets the form, clearing all fields to their default values.
        /// </summary>
        reset
    }
}
