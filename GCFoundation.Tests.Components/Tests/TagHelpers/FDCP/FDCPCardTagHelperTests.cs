using GCFoundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GCFoundation.Tests.Components.Tests.TagHelpers.FDCP
{
    public class FDCPCardTagHelperTests
    {
        [Fact]
        public async Task ProcessAsync_BasicCard_RendersCorrectly()
        {
            // Arrange
            var helper = new FDCPCardTagHelper();

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(new DefaultTagHelperContent().SetContent("Basic card content"));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("div", output.TagName);
            Assert.Equal("fdcp-card", output.Attributes["class"].Value.ToString());
            
            var content = output.Content.GetContent();
            Assert.Contains("<div class=\"fdcp-card-body\">Basic card content</div>", content);
        }

        [Fact]
        public async Task ProcessAsync_WithCustomDimensions_RendersStyleAttributes()
        {
            // Arrange
            var helper = new FDCPCardTagHelper
            {
                Width = "300px",
                Height = "200px"
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(new DefaultTagHelperContent().SetContent("Test content"));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            var styleValue = output.Attributes["style"].Value.ToString();
            Assert.Contains("width: 300px", styleValue);
            Assert.Contains("height: 200px", styleValue);
        }

        [Theory]
        [InlineData(false, "fdcp-card fdcp-card-no-border")]
        [InlineData(true, "fdcp-card")]
        public async Task ProcessAsync_WithBorderOption_RendersCorrectClass(bool border, string expectedClass)
        {
            // Arrange
            var helper = new FDCPCardTagHelper
            {
                Border = border
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(new DefaultTagHelperContent().SetContent("Test"));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            Assert.Equal(expectedClass, output.Attributes["class"].Value.ToString());
        }

        [Theory]
        [InlineData(false, "fdcp-card")]
        [InlineData(true, "fdcp-card fdcp-card-shadow")]
        public async Task ProcessAsync_WithShadowOption_RendersCorrectClass(bool shadow, string expectedClass)
        {
            // Arrange
            var helper = new FDCPCardTagHelper
            {
                Shadow = shadow
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(new DefaultTagHelperContent().SetContent("Test"));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            Assert.Equal(expectedClass, output.Attributes["class"].Value.ToString());
        }

        [Fact]
        public async Task ProcessAsync_WithHorizontalLayout_RendersCorrectClass()
        {
            // Arrange
            var helper = new FDCPCardTagHelper
            {
                Horizontal = true
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(new DefaultTagHelperContent().SetContent("Test"));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            Assert.Contains("fdcp-card-horizontal", output.Attributes["class"].Value.ToString());
        }

        [Fact]
        public async Task ProcessAsync_WithAllStyleOptions_RendersAllClasses()
        {
            // Arrange
            var helper = new FDCPCardTagHelper
            {
                Border = false,
                Shadow = true,
                Horizontal = true
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(new DefaultTagHelperContent().SetContent("Test"));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            var classValue = output.Attributes["class"].Value.ToString();
            Assert.Contains("fdcp-card", classValue);
            Assert.Contains("fdcp-card-no-border", classValue);
            Assert.Contains("fdcp-card-shadow", classValue);
            Assert.Contains("fdcp-card-horizontal", classValue);
        }

        [Fact]
        public async Task ProcessAsync_WithTopImage_RendersImageElement()
        {
            // Arrange
            var helper = new FDCPCardTagHelper
            {
                ImageTop = "/images/test.jpg",
                ImageAlt = "Test image"
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(new DefaultTagHelperContent().SetContent("Test content"));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            var content = output.Content.GetContent();
            Assert.Contains("<img src=\"/images/test.jpg\" class=\"fdcp-card-img-top\" alt=\"Test image\">", content);
        }

        [Fact]
        public async Task ProcessAsync_WithBottomImage_RendersImageElement()
        {
            // Arrange
            var helper = new FDCPCardTagHelper
            {
                ImageBottom = "/images/bottom.jpg",
                ImageAlt = "Bottom image"
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(new DefaultTagHelperContent().SetContent("Test content"));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            var content = output.Content.GetContent();
            Assert.Contains("<img src=\"/images/bottom.jpg\" class=\"fdcp-card-img-bottom\" alt=\"Bottom image\">", content);
        }

        [Fact]
        public async Task ProcessAsync_WithImageNoAlt_RendersEmptyAlt()
        {
            // Arrange
            var helper = new FDCPCardTagHelper
            {
                ImageTop = "/images/test.jpg"
                // ImageAlt intentionally not set
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(new DefaultTagHelperContent().SetContent("Test content"));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            var content = output.Content.GetContent();
            Assert.Contains("alt=\"\"", content);
        }

        [Fact]
        public async Task ProcessAsync_WithSlotContent_RendersHeaderBodyFooter()
        {
            // Arrange
            var helper = new FDCPCardTagHelper();

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(
                        new DefaultTagHelperContent().SetHtmlContent(
                            "<div slot='header'>Card Header</div>" +
                            "<div slot='body'>Card Body Content</div>" +
                            "<div slot='footer'>Card Footer</div>" +
                            "Additional content"
                        ));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            var content = output.Content.GetContent();
            Assert.Contains("<div class=\"fdcp-card-header\">Card Header</div>", content);
            Assert.Contains("<div class=\"fdcp-card-body\">Card Body Content</div>", content);
            Assert.Contains("<div class=\"fdcp-card-footer\">Card Footer</div>", content);
        }

        [Fact]
        public async Task ProcessAsync_WithOnlyBodySlot_RendersOnlyBody()
        {
            // Arrange
            var helper = new FDCPCardTagHelper();

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(
                        new DefaultTagHelperContent().SetHtmlContent(
                            "<div slot='body'>Only body content</div>"
                        ));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            var content = output.Content.GetContent();
            Assert.Contains("<div class=\"fdcp-card-body\">Only body content</div>", content);
            Assert.DoesNotContain("fdcp-card-header", content);
            Assert.DoesNotContain("fdcp-card-footer", content);
        }

        [Fact]
        public async Task ProcessAsync_WithNoSlots_RendersMainContentInBody()
        {
            // Arrange
            var helper = new FDCPCardTagHelper();

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(new DefaultTagHelperContent().SetContent("Main content without slots"));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            var content = output.Content.GetContent();
            Assert.Contains("<div class=\"fdcp-card-body\">Main content without slots</div>", content);
        }

        [Fact]
        public async Task ProcessAsync_WithEmptyContent_DoesNotRenderBody()
        {
            // Arrange
            var helper = new FDCPCardTagHelper();

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(new DefaultTagHelperContent().SetContent("   "));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            var content = output.Content.GetContent();
            Assert.DoesNotContain("fdcp-card-body", content);
        }

        [Fact]
        public async Task ProcessAsync_WithTagId_RendersIdAttribute()
        {
            // Arrange
            var helper = new FDCPCardTagHelper
            {
                TagId = "my-card-id"
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(new DefaultTagHelperContent().SetContent("Test"));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("my-card-id", output.Attributes["id"].Value);
        }

        [Fact]
        public async Task ProcessAsync_CompleteCard_RendersAllElements()
        {
            // Arrange
            var helper = new FDCPCardTagHelper
            {
                TagId = "complete-card",
                Width = "400px",
                Height = "300px",
                Border = false,
                Shadow = true,
                Horizontal = true,
                ImageTop = "/images/top.jpg",
                ImageBottom = "/images/bottom.jpg",
                ImageAlt = "Card images"
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(
                        new DefaultTagHelperContent().SetHtmlContent(
                            "<h3 slot='header'>Complete Header</h3>" +
                            "<p slot='body'>Complete Body</p>" +
                            "<div slot='footer'>Complete Footer</div>"
                        ));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            Assert.Equal("div", output.TagName);
            Assert.Equal("complete-card", output.Attributes["id"].Value);
            
            var classValue = output.Attributes["class"].Value.ToString();
            Assert.Contains("fdcp-card", classValue);
            Assert.Contains("fdcp-card-no-border", classValue);
            Assert.Contains("fdcp-card-shadow", classValue);
            Assert.Contains("fdcp-card-horizontal", classValue);

            var styleValue = output.Attributes["style"].Value.ToString();
            Assert.Contains("width: 400px", styleValue);
            Assert.Contains("height: 300px", styleValue);

            var content = output.Content.GetContent();
            Assert.Contains("<img src=\"/images/top.jpg\" class=\"fdcp-card-img-top\" alt=\"Card images\">", content);
            Assert.Contains("<div class=\"fdcp-card-header\">Complete Header</div>", content);
            Assert.Contains("<div class=\"fdcp-card-body\">Complete Body</div>", content);
            Assert.Contains("<div class=\"fdcp-card-footer\">Complete Footer</div>", content);
            Assert.Contains("<img src=\"/images/bottom.jpg\" class=\"fdcp-card-img-bottom\" alt=\"Card images\">", content);
        }

        [Fact]
        public async Task ProcessAsync_WithNullOutput_ThrowsArgumentNullException()
        {
            // Arrange
            var helper = new FDCPCardTagHelper();
            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => helper.ProcessAsync(context, null!));
        }

        [Fact]
        public async Task ProcessAsync_ContentOrder_RendersInCorrectSequence()
        {
            // Arrange
            var helper = new FDCPCardTagHelper
            {
                ImageTop = "/images/top.jpg",
                ImageBottom = "/images/bottom.jpg",
                ImageAlt = "Test"
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-unique-id");

            var output = new TagHelperOutput("fdcp-card",
                new TagHelperAttributeList(),
                (_, __) =>
                {
                    return Task.FromResult(
                        new DefaultTagHelperContent().SetHtmlContent(
                            "<div slot='header'>Header</div>" +
                            "<div slot='body'>Body</div>" +
                            "<div slot='footer'>Footer</div>"
                        ));
                });

            // Act
            await helper.ProcessAsync(context, output);

            // Assert
            var content = output.Content.GetContent();
            
            // Verify the order: top image, header, body, footer, bottom image
            var topImageIndex = content.IndexOf("fdcp-card-img-top", StringComparison.Ordinal);
            var headerIndex = content.IndexOf("fdcp-card-header", StringComparison.Ordinal);
            var bodyIndex = content.IndexOf("fdcp-card-body", StringComparison.Ordinal);
            var footerIndex = content.IndexOf("fdcp-card-footer", StringComparison.Ordinal);
            var bottomImageIndex = content.IndexOf("fdcp-card-img-bottom", StringComparison.Ordinal);

            Assert.True(topImageIndex < headerIndex);
            Assert.True(headerIndex < bodyIndex);
            Assert.True(bodyIndex < footerIndex);
            Assert.True(footerIndex < bottomImageIndex);
        }
    }
}
