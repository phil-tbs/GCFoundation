using GCFoundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GCFoundation.Tests.Components.Tests.TagHelpers.FDCP;

public class FDCPModalBodyTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_RendersModalBodyDivWithCorrectAttributes()
    {
        // Arrange
        var tagHelper = new FDCPModalBodyTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-modal-body",
            new TagHelperAttributeList(),
            (cache, encoder) => Task.FromResult<TagHelperContent>(
                new DefaultTagHelperContent().SetHtmlContent("Test content")));

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("div", output.TagName);
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
        Assert.Equal("fdcp-modal__body", output.Attributes["class"].Value);
        Assert.Equal("Test content", output.Content.GetContent());
    }

    [Fact]
    public async Task ProcessAsync_WithNullOutput_ThrowsArgumentNullException()
    {
        // Arrange
        var tagHelper = new FDCPModalBodyTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await tagHelper.ProcessAsync(context, null!));
        Assert.Equal("output", exception.ParamName);
    }

    [Fact]
    public async Task ProcessAsync_PreservesChildContent()
    {
        // Arrange
        var tagHelper = new FDCPModalBodyTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var complexContent = "<p>Hello</p><span>World</span>";
        var output = new TagHelperOutput("fdcp-modal-body",
            new TagHelperAttributeList(),
            (cache, encoder) => Task.FromResult<TagHelperContent>(
                new DefaultTagHelperContent().SetHtmlContent(complexContent)));

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(complexContent, output.Content.GetContent());
    }
}