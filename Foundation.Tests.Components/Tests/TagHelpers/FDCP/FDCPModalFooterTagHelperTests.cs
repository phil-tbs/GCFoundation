using Foundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace Foundation.Tests.Components.Tests.TagHelpers.FDCP;

public class FDCPModalFooterTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_RendersModalFooterDivWithCorrectAttributes()
    {
        // Arrange
        var tagHelper = new FDCPModalFooterTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-modal-footer",
            new TagHelperAttributeList(),
            (cache, encoder) => Task.FromResult<TagHelperContent>(
                new DefaultTagHelperContent().SetHtmlContent("Test content")));

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("div", output.TagName);
        Assert.Equal(TagMode.StartTagAndEndTag, output.TagMode);
        Assert.Equal("modal-footer", output.Attributes["class"].Value);
        Assert.Equal("Test content", output.Content.GetContent());
    }

    [Fact]
    public async Task ProcessAsync_WithNullOutput_ThrowsArgumentNullException()
    {
        // Arrange
        var tagHelper = new FDCPModalFooterTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(
            async () => await tagHelper.ProcessAsync(context, null!));
    }

    [Fact]
    public async Task ProcessAsync_PreservesComplexChildContent()
    {
        // Arrange
        var tagHelper = new FDCPModalFooterTagHelper();
        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var complexContent = "<button class=\"btn btn-primary\">Save</button><button class=\"btn btn-secondary\">Cancel</button>";
        var output = new TagHelperOutput("fdcp-modal-footer",
            new TagHelperAttributeList(),
            (cache, encoder) => Task.FromResult<TagHelperContent>(
                new DefaultTagHelperContent().SetHtmlContent(complexContent)));

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal(complexContent, output.Content.GetContent());
    }
}