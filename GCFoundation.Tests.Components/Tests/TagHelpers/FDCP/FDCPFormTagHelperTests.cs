using GCFoundation.Components.Models;
using GCFoundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GCFoundation.Tests.Components.Tests.TagHelpers.FDCP;

public class FDCPFormTagHelperTests
{
    [Fact]
    public async Task ProcessAsync_WithValidModel_RendersFormCorrectly()
    {
        // Arrange
        var tagHelper = new FDCPFormTagHelper
        {
            Model = new TestViewModel(),
            Method = "post",
            Action = "/test-action"
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-form",
            new TagHelperAttributeList(),
            (cache, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.Equal("form", output.TagName);
        Assert.Equal("post", output.Attributes["method"].Value);
        Assert.Equal("/test-action", output.Attributes["action"].Value);

        var content = output.Content.GetContent();
        Assert.Contains("<gcds-error-summary", content);
        Assert.Contains($"lang=\"{CultureInfo.CurrentCulture.Name}\"", content);
    }

    [Fact]
    public async Task ProcessAsync_WithModelErrors_RendersErrorSummary()
    {
        // Arrange
        var model = new TestViewModel();
        model.AddError("field1", "Error message 1");
        model.AddError("field2", "Error message 2");

        var tagHelper = new FDCPFormTagHelper
        {
            Model = model,
            Method = "post",
            Action = "/test-action"
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-form",
            new TagHelperAttributeList(),
            (cache, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("error-links", content);

        // Verify error links JSON structure
        var expectedErrors = new Dictionary<string, string>
        {
            { "#field1", "Error message 1" },
            { "#field2", "Error message 2" }
        };
        var expectedJson = JsonSerializer.Serialize(expectedErrors);

        // Extract the error-links attribute value using regex
        var match = Regex.Match(content, "error-links=\"([^\"]*)\"");
        Assert.True(match.Success, "error-links attribute not found in content");

        // HTML-decode the attribute value
        var actualJson = WebUtility.HtmlDecode(match.Groups[1].Value);

        // Compare with expected JSON
        Assert.Equal(expectedJson, actualJson);
    }

    [Fact]
    public async Task ProcessAsync_WithNullAction_OmitsActionAttribute()
    {
        // Arrange
        var tagHelper = new FDCPFormTagHelper
        {
            Model = new TestViewModel(),
            Method = "post",
            Action = string.Empty
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-form",
            new TagHelperAttributeList(),
            (cache, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        await tagHelper.ProcessAsync(context, output);

        // Assert
        Assert.False(output.Attributes.ContainsName("action"));
    }

    [Fact]
    public async Task ProcessAsync_WithNullModel_ThrowsInvalidOperationException()
    {
        // Arrange
        var tagHelper = new FDCPFormTagHelper
        {
            Model = null!,
            Method = "post",
            Action = "/test-action"
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-form",
            new TagHelperAttributeList(),
            (cache, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(
            () => tagHelper.ProcessAsync(context, output));
    }

    private class TestViewModel : BaseViewModel
    {
        // Test implementation of BaseViewModel
    }
}