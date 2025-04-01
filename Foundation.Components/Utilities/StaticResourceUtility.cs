
namespace Foundation.Components.Utilities
{
    public static class StaticResourceUtility
    {
        public static string GetResourcePath(string ressourceRelativePath)
        {
            string entryAssemblyName = "Foundation.Components";
            return $"/_content/{entryAssemblyName}/{ressourceRelativePath}";
        }

        public static string GetCssResourcePath(string cssFile)
        {
            return GetResourcePath($"css/{cssFile}");
        }

        public static string GetJsResourcePath(string jsFile)
        {
            return GetResourcePath($"js/{jsFile}");
        }

        public static string GetImageResourcePath(string imageFile)
        {
            return GetResourcePath($"images/{imageFile}");
        }

        public static string GetLibResourcePath(string libFile)
        {
            return GetResourcePath($"lib/{libFile}");
        }
    }
}
