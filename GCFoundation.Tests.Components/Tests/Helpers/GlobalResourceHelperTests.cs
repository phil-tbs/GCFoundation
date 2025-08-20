using GCFoundation.Common.Settings;
using GCFoundation.Components.Helpers;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;
using Xunit;

namespace GCFoundation.Tests.Components.Tests.Helpers
{
    /// <summary>
    /// Unit tests for the GlobalResourceHelper class.
    /// </summary>
    public class GlobalResourceHelperTests
    {
        private readonly GCFoundationComponentsSettings _settings;
        private readonly GlobalResourceHelper _helper;

        public GlobalResourceHelperTests()
        {
            _settings = new GCFoundationComponentsSettings
            {
                IncludeDefaultCss = true,
                IncludeDefaultJavaScript = true,
                IncludeGCDSResources = true,
                IncludeFontAwesome = true
            };

            // Add items to the collections instead of replacing them
            _settings.GlobalCssFiles.Add("https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css");
            _settings.GlobalCssFiles.Add("/css/custom-styles.css");

            _settings.GlobalJavaScriptFiles.Add("https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js");
            _settings.GlobalJavaScriptFiles.Add("/js/custom-scripts.js");

            _settings.GlobalMetaTags.Add("<meta name=\"description\" content=\"Test description\">");
            _settings.GlobalMetaTags.Add("<meta property=\"og:title\" content=\"Test Title\">");

            _settings.GlobalLinkTags.Add("<link rel=\"preconnect\" href=\"https://fonts.googleapis.com\">");
            _settings.GlobalLinkTags.Add("<link rel=\"canonical\" href=\"https://example.com\">");

            var options = Options.Create(_settings);
            _helper = new GlobalResourceHelper(options);
        }

        [Fact]
        public void RenderGlobalCssFiles_ShouldReturnCorrectHtml()
        {
            // Act
            var result = _helper.RenderGlobalCssFiles();

            // Assert
            Assert.Contains("href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css\"", result);
            Assert.Contains("href=\"/css/custom-styles.css\"", result);
            Assert.Contains("asp-append-version=\"true\"", result);
        }

        [Fact]
        public void RenderGlobalJavaScriptFiles_ShouldReturnCorrectHtml()
        {
            // Act
            var result = _helper.RenderGlobalJavaScriptFiles();

            // Assert
            Assert.Contains("src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js\"", result);
            Assert.Contains("src=\"/js/custom-scripts.js\"", result);
            Assert.Contains("asp-append-version=\"true\"", result);
        }

        [Fact]
        public void RenderGlobalMetaTags_ShouldReturnCorrectHtml()
        {
            // Act
            var result = _helper.RenderGlobalMetaTags();

            // Assert
            Assert.Contains("<meta name=\"description\" content=\"Test description\">", result);
            Assert.Contains("<meta property=\"og:title\" content=\"Test Title\">", result);
        }

        [Fact]
        public void RenderGlobalLinkTags_ShouldReturnCorrectHtml()
        {
            // Act
            var result = _helper.RenderGlobalLinkTags();

            // Assert
            Assert.Contains("<link rel=\"preconnect\" href=\"https://fonts.googleapis.com\">", result);
            Assert.Contains("<link rel=\"canonical\" href=\"https://example.com\">", result);
        }

        [Fact]
        public void GetGlobalCssFiles_ShouldReturnCorrectList()
        {
            // Act
            var result = _helper.GetGlobalCssFiles();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css", result);
            Assert.Contains("/css/custom-styles.css", result);
        }

        [Fact]
        public void GetGlobalJavaScriptFiles_ShouldReturnCorrectList()
        {
            // Act
            var result = _helper.GetGlobalJavaScriptFiles();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js", result);
            Assert.Contains("/js/custom-scripts.js", result);
        }

        [Fact]
        public void GetGlobalMetaTags_ShouldReturnCorrectList()
        {
            // Act
            var result = _helper.GetGlobalMetaTags();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("<meta name=\"description\" content=\"Test description\">", result);
            Assert.Contains("<meta property=\"og:title\" content=\"Test Title\">", result);
        }

        [Fact]
        public void GetGlobalLinkTags_ShouldReturnCorrectList()
        {
            // Act
            var result = _helper.GetGlobalLinkTags();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("<link rel=\"preconnect\" href=\"https://fonts.googleapis.com\">", result);
            Assert.Contains("<link rel=\"canonical\" href=\"https://example.com\">", result);
        }

        [Fact]
        public void ShouldIncludeDefaultCss_ShouldReturnTrue()
        {
            // Act
            #pragma warning disable CS0618 // Type or member is obsolete
            var result = _helper.ShouldIncludeDefaultCss();
            #pragma warning restore CS0618 // Type or member is obsolete

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldIncludeDefaultJavaScript_ShouldReturnTrue()
        {
            // Act
            #pragma warning disable CS0618 // Type or member is obsolete
            var result = _helper.ShouldIncludeDefaultJavaScript();
            #pragma warning restore CS0618 // Type or member is obsolete

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldIncludeGCDSResources_ShouldReturnTrue()
        {
            // Act
            var result = _helper.ShouldIncludeGCDSResources();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldIncludeFontAwesome_ShouldReturnTrue()
        {
            // Act
            var result = _helper.ShouldIncludeFontAwesome();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EmptySettings_ShouldHandleGracefully()
        {
            // Arrange
            var emptySettings = new GCFoundationComponentsSettings();
            var options = Options.Create(emptySettings);
            var emptyHelper = new GlobalResourceHelper(options);

            // Act & Assert
            Assert.Empty(emptyHelper.GetGlobalCssFiles());
            Assert.Empty(emptyHelper.GetGlobalJavaScriptFiles());
            Assert.Empty(emptyHelper.GetGlobalMetaTags());
            Assert.Empty(emptyHelper.GetGlobalLinkTags());
            #pragma warning disable CS0618 // Type or member is obsolete
            Assert.True(emptyHelper.ShouldIncludeDefaultCss());
            Assert.True(emptyHelper.ShouldIncludeDefaultJavaScript());
            #pragma warning restore CS0618 // Type or member is obsolete
            Assert.True(emptyHelper.ShouldIncludeGCDSResources());
            Assert.True(emptyHelper.ShouldIncludeFontAwesome());
        }
    }
} 