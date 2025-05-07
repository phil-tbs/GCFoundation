namespace Foundation.Components.Enums
{
    /// <summary>
    /// Defines the available types for representing the "date modified" information.
    /// This enum specifies whether the "date modified" should be based on the date itself or the version.
    /// </summary>
    public enum DateModifiedType
    {
        /// <summary>
        /// Represents the modification date as a specific date (e.g., YYYY-MM-DD).
        /// </summary>
        date,

        /// <summary>
        /// Represents the modification as a version (e.g., v1.0, v2.0).
        /// </summary>
        version
    }
}
