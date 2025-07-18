using GCFoundation.Components.Enums;
using GCFoundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GCFoundation.Tests.Components.Tests.TagHelpers.FDCP;

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
        Assert.Equal("fdcp-modal", output.Attributes["class"].Value);
        Assert.Equal("modal", output.Attributes["id"].Value);
        Assert.Equal("-1", output.Attributes["tabindex"].Value);
        Assert.Equal("modalLabel", output.Attributes["aria-labelledby"].Value);
        Assert.Equal("true", output.Attributes["aria-hidden"].Value);
        Assert.Equal("dialog", output.Attributes["role"].Value);

        var content = output.Content.GetContent();
        Assert.Contains("fdcp-modal__backdrop", content);
        Assert.Contains("fdcp-modal__dialog fdcp-modal__dialog--centered", content);
        Assert.Contains("<h5 class='fdcp-modal__title' id='modalLabel'>Modal Title</h5>", content);
        Assert.Contains("fdcp-modal__close", content);
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
    [InlineData(ModalSize.Small, "fdcp-modal__dialog--sm")]
    [InlineData(ModalSize.Large, "fdcp-modal__dialog--lg")]
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
        Assert.Equal("true", output.Attributes["data-static"].Value);

        var content = output.Content.GetContent();
        Assert.Contains("fdcp-modal__dialog--scrollable", content);
        Assert.DoesNotContain("fdcp-modal__dialog--centered", content);
        Assert.Contains("Custom Title", content);
        Assert.DoesNotContain("fdcp-modal__close", content);
        Assert.Contains("fdcp-modal__backdrop", content);
    }

    [Fact]
    public async Task ProcessAsync_WithEmptyTitle_UsesEmptyTitle()
    {
        // Arrange
        var tagHelper = new FDCPModalTagHelper { Title = string.Empty };
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
        Assert.Contains($"<h5 class='fdcp-modal__title' id='modalLabel'></h5>", content);
    }
}