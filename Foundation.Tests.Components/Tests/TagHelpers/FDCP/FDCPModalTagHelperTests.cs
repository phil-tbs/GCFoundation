using System.Globalization;
using Foundation.Components.Enums;
using Foundation.Components.Resources;
using Foundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace Foundation.Tests.Components.Tests.TagHelpers.FDCP;

public class FDCPModalTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_RendersModalWithDefaultSettings()
    {
        // Arrange
        var tagHelper = new FDCPModalTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-modal",
            new TagHelperAttributeList(),
            (cache, encoder) => Task.FromResult<TagHelperContent>(
                new DefaultTagHelperContent().SetHtmlContent("<div class='modal-body'>Test content</div>")));

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("div", output.TagName);
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
        Assert.Equal("modal fade", output.Attributes["class"].Value);
        Assert.Equal("modal", output.Attributes["id"].Value);
        Assert.Equal("-1", output.Attributes["tabindex"].Value);
        Assert.Equal("modalLabel", output.Attributes["aria-labelledby"].Value);
        Assert.Equal("true", output.Attributes["aria-hidden"].Value);

        var content = output.Content.GetContent();
        Assert.Contains("modal-dialog modal-dialog-centered", content);
        Assert.Contains("<h5 class='modal-title' id='modalLabel'>Modal Title</h5>", content);
        Assert.Contains("btn-close", content);
        Assert.Contains("Test content", content);
    }

    [Fact]
    public async Task ProcessAsync_WithNullOutput_ThrowsArgumentNullException()
    {
        // Arrange
        var tagHelper = new FDCPModalTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await tagHelper.ProcessAsync(context, null!));
    }

    [Theory]
    [InlineData(ModalSize.Small, "modal-sm")]
    [InlineData(ModalSize.Large, "modal-lg")]
    [InlineData(ModalSize.Default, "")]
    public async Task ProcessAsync_AppliesCorrectSizeClass(ModalSize size, string expectedClass)
    {
        // Arrange
        var tagHelper = new FDCPModalTagHelper { Size = size };
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-modal",
            new TagHelperAttributeList(),
            (cache, encoder) => Task.FromResult<TagHelperContent>(
                new DefaultTagHelperContent()));

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var content = output.Content.GetContent();
        if (!string.IsNullOrEmpty(expectedClass))
        {
            Assert.Contains(expectedClass, content);
        }
    }

    [Fact]
    public async Task ProcessAsync_WithCustomSettings_RendersCorrectly()
    {
        // Arrange
        var tagHelper = new FDCPModalTagHelper
        {
            Id = "customModal",
            Title = "Custom Title",
            Centered = false,
            Scrollable = true,
            ShowCloseButton = false,
            IsStaticBackdrop = true
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-modal",
            new TagHelperAttributeList(),
            (cache, encoder) => Task.FromResult<TagHelperContent>(
                new DefaultTagHelperContent()));

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("customModal", output.Attributes["id"].Value);
        Assert.Contains("static", output.Attributes["data-bs-backdrop"].Value.ToString());
        
        var content = output.Content.GetContent();
        Assert.Contains("modal-dialog-scrollable", content);
        Assert.DoesNotContain("modal-dialog-centered", content);
        Assert.Contains("Custom Title", content);
        Assert.DoesNotContain("btn-close", content);
    }
}