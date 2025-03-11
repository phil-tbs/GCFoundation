namespace Foundation.Security.Settings
{
    /// <summary>
    /// Content policy setting for foundation
    /// </summary>
    public class ContentPolicySettings
    {
        /// <summary>
        /// Contain all javascript CDN host (allow)
        /// </summary>
        public List<string> JavascriptCDN { get; set; } = new List<string>();

        /// <summary>
        /// Contain all css CDN host (allow)
        /// </summary>
        public List<string> CssCDN { get; set; } = new List<string>();

        /// <summary>
        /// Contain all hash that need to be added for inline execution (allow)
        /// </summary>
        public List<string> CssCDNHash { get; set; } = new List<string>();

        /// <summary>
        /// Contain all font CDN host (allow)
        /// </summary>
        public List<string> FontCDN { get; set; } = new List<string>();
    }
}
