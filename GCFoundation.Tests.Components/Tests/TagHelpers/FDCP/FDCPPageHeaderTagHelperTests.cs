using GCFoundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GCFoundation.Tests.Components.Tests.TagHelpers.FDCP;

public class FDCPPageHeaderTagHelperTests
{
    [Fact]
    public void Process_WithTitleOnly_RendersExpectedOutput()
    {
        // Arrange
        var tagHelper = new FDCPPageHeaderTagHelper
        {
            Title = "Test Title"
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-page-heading",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("fdcp-page-header-container", output.Attributes["class"].Value);
        Assert.Contains("<gcds-heading tag='h1'>Test Title</gcds-heading>", output.Content.GetContent());
        Assert.DoesNotContain("<gcds-text>", output.Content.GetContent());
    }

    [Fact]
    public void Process_WithTitleAndDescription_RendersExpectedOutput()
    {
        // Arrange
        var tagHelper = new FDCPPageHeaderTagHelper
        {
            Title = "Test Title",
            Description = "Test Description"
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-page-heading",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("fdcp-page-header-container", output.Attributes["class"].Value);
        Assert.Contains("<gcds-heading tag='h1'>Test Title</gcds-heading>", output.Content.GetContent());
        Assert.Contains("<gcds-text>Test Description</gcds-text>", output.Content.GetContent());
    }

    [Fact]
    public void Process_WithBackgroundImage_RendersExpectedOutput()
    {
        // Arrange
        var tagHelper = new FDCPPageHeaderTagHelper
        {
            Title = "Test Title",
            Src = "background-image.jpg"
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-page-heading",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("fdcp-page-header-container fdcp-page-header--has-bg", output.Attributes["class"].Value);
        Assert.Equal("background-image.jpg", output.Attributes["data-bg-src"].Value);
    }

    [Fact]
    public void Process_NullOutput_ThrowsArgumentNullException()
    {
        // Arrange
        var tagHelper = new FDCPPageHeaderTagHelper
        {
            Title = "Test Title"
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => tagHelper.Process(context, null!));
    }

    [Fact]
    public void Process_SizeLargeWithTitleAndDescription_RendersExpectedOutput()
    {
        // Arrange
        var tagHelper = new FDCPPageHeaderTagHelper
        {
            Title = "Test Title",
            Description = "Test Description",
            Size = GCFoundation.Components.Enums.PageHeaderSize.Large
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-page-heading",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("fdcp-page-header-container fdcp-page-header-large", output.Attributes["class"].Value);
        Assert.Contains("<gcds-heading tag='h1'>Test Title</gcds-heading>", output.Content.GetContent());
        Assert.Contains("<gcds-text>Test Description</gcds-text>", output.Content.GetContent());
    }

    [Fact]
    public void Process_SizeLargeWithTitleDescriptionAndTextEmphasis_RendersExpectedOutput()
    {
        // Arrange
        var tagHelper = new FDCPPageHeaderTagHelper
        {
            Title = "Test Title",
            Description = "Test Description",
            Size = GCFoundation.Components.Enums.PageHeaderSize.Large,
            TextEmphasis = true
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-page-heading",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("fdcp-page-header-container fdcp-page-header-large", output.Attributes["class"].Value);
        Assert.Contains("<gcds-heading tag='h1'>Test Title</gcds-heading>", output.Content.GetContent());
        Assert.Contains("<div class='text-container text-container-well'>", output.Content.GetContent());
        Assert.Contains("<gcds-text>Test Description</gcds-text>", output.Content.GetContent());
    }
}