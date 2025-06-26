using System.Text.Json;
using Foundation.Components.Enums;
using Foundation.Components.Models;
using Foundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace Foundation.Tests.Components.Tests.TagHelpers.FDCP;

public class FDCPTabulatorTableTagHelperTests
{
    [Fact]
    public void Process_WithAjaxConfiguration_RendersExpectedOutput()
    {
        // Arrange
        var tagHelper = new FDCPTabulatorTableTagHelper
        {
            Id = "test-table",
            AjaxUrl = "/api/data",
            UseStaticData = false,
            PaginationSize = 15,
            Columns = new[]
            {
                new TabulatorColumn { Title = "Name", Field = "name" },
                new TabulatorColumn { Title = "Age", Field = "age" }
            }
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-tabulator-table",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("div", output.TagName);
        Assert.Equal("test-table", output.Attributes["id"].Value);
        Assert.Equal("tabulator-container", output.Attributes["class"].Value);
        
        var content = output.Content.GetContent();
        Assert.Contains("test-table-search-form", content);
        Assert.Contains("test-table-tabulator", content);
        Assert.Contains("data-ajaxURL='/api/data'", content);
        Assert.Contains("data-pagination-size='15'", content);
        Assert.DoesNotContain("data-set", content);
    }

    [Fact]
    public void Process_WithStaticData_RendersExpectedOutput()
    {
        // Arrange
        var staticData = new[]
        {
            new { name = "John", age = 30 },
            new { name = "Jane", age = 25 }
        };

        var tagHelper = new FDCPTabulatorTableTagHelper
        {
            Id = "static-table",
            UseStaticData = true,
            Data = staticData,
            PaginationSize = 10,
            Columns = new[]
            {
                new TabulatorColumn { Title = "Name", Field = "name" },
                new TabulatorColumn { Title = "Age", Field = "age" }
            }
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-tabulator-table",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        Assert.Equal("div", output.TagName);
        Assert.Equal("static-table", output.Attributes["id"].Value);
        
        var content = output.Content.GetContent();
        Assert.Contains("static-table-search-form", content);
        Assert.Contains("static-table-tabulator", content);
        Assert.Contains("data-set='[{", content);
        Assert.DoesNotContain("data-ajaxURL", content);
    }

    [Fact]
    public void Process_WithCustomColumns_RendersExpectedOutput()
    {
        // Arrange
        var columns = new[]
        {
            new TabulatorColumn 
            { 
                Title = "Name", 
                Field = "name",
                HeaderSort = true,
                HozAlign = "left",
                Formatter = "html",
                CssClass = "name-column",
                Resizable = TabulatorResizableOption.Header
            }
        };

        var tagHelper = new FDCPTabulatorTableTagHelper
        {
            Id = "custom-table",
            AjaxUrl = "/api/data",
            Columns = columns
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        var output = new TagHelperOutput("fdcp-tabulator-table",
            new TagHelperAttributeList(),
            (_, _) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        // Act
        tagHelper.Process(context, output);

        // Assert
        var content = output.Content.GetContent();
        Assert.Contains("\"title\":\"Name\"", content);
        Assert.Contains("\"field\":\"name\"", content);
        Assert.Contains("\"headerSort\":true", content);
        Assert.Contains("\"hozAlign\":\"left\"", content);
        Assert.Contains("\"formatter\":\"html\"", content);
        Assert.Contains("\"cssClass\":\"name-column\"", content);
    }

    [Fact]
    public void Process_NullOutput_ThrowsArgumentNullException()
    {
        // Arrange
        var tagHelper = new FDCPTabulatorTableTagHelper
        {
            Id = "test-table"
        };

        var context = new TagHelperContext(
            new TagHelperAttributeList(),
            new Dictionary<object, object>(),
            "test"
        );

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => tagHelper.Process(context, null!));
    }
}