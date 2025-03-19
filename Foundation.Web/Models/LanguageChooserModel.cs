namespace Foundation.Web.Models
{
    /// <summary>
    /// Model for the language chooser page
    /// </summary>
    public class LanguageChooserModel
    {
        /// <summary>
        /// Title of the application in english
        /// </summary>
        public string ApplicationTitleEn {  get; set; } = string.Empty;

        /// <summary>
        /// Title of the application in French
        /// </summary>
        public string ApplicationTitleFr { get; set; } = string.Empty;

        /// <summary>
        /// Link to the english version of the application
        /// </summary>
        public string EnglishAction {  get; set; } = string.Empty;

        /// <summary>
        /// Link to the frech version of the application
        /// </summary>
        public string FrenchAction {  get; set; } = string.Empty;

        /// <summary>
        /// Link to the term of service of the application in english
        /// </summary>
        public string TermLinkEn { get; set; } = string.Empty;

        /// <summary>
        /// Link to the term of service of the application in french
        /// </summary>
        public string TermLinkFr { get; set; } = string.Empty;

        /// <summary>
        /// Array of image path for the background image
        /// </summary>
        public string[]? BackgroundImagePaths { get; set; } = null;
    }
}
