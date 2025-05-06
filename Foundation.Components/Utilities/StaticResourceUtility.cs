
namespace Foundation.Components.Utilities
{
    /// <summary>
    /// Provides utility methods to retrieve the path of static resources (CSS, JS, images, libraries) in the project.
    /// </summary>
    public static class StaticResourceUtility
    {
        /// <summary>
        /// Gets the full resource path for a given resource relative path.
        /// </summary>
        /// <param name="ressourceRelativePath">The relative path to the resource.</param>
        /// <returns>A string representing the full path to the resource.</returns>
        public static string GetResourcePath(string ressourceRelativePath)
        {
            string entryAssemblyName = "Foundation.Components";
            return $"/_content/{entryAssemblyName}/{ressourceRelativePath}";
        }

        /// <summary>
        /// Gets the full path for a given CSS file.
        /// </summary>
        /// <param name="cssFile">The name of the CSS file.</param>
        /// <returns>A string representing the full path to the CSS file.</returns>
        public static string GetCssResourcePath(string cssFile)
        {
            return GetResourcePath($"css/{cssFile}");
        }

        /// <summary>
        /// Gets the full path for a given JavaScript file.
        /// </summary>
        /// <param name="jsFile">The name of the JavaScript file.</param>
        /// <returns>A string representing the full path to the JavaScript file.</returns>
        public static string GetJsResourcePath(string jsFile)
        {
            return GetResourcePath($"js/{jsFile}");
        }

        /// <summary>
        /// Gets the full path for a given image file.
        /// </summary>
        /// <param name="imageFile">The name of the image file.</param>
        /// <returns>A string representing the full path to the image file.</returns>
        public static string GetImageResourcePath(string imageFile)
        {
            return GetResourcePath($"images/{imageFile}");
        }

        /// <summary>
        /// Gets the full path for a given library file.
        /// </summary>
        /// <param name="libFile">The name of the library file.</param>
        /// <returns>A string representing the full path to the library file.</returns>
        public static string GetLibResourcePath(string libFile)
        {
            return GetResourcePath($"lib/{libFile}");
        }
    }
}
