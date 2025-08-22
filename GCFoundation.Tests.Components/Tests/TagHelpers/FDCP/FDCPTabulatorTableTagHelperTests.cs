using GCFoundation.Components.Enums;
using GCFoundation.Components.Models;
using GCFoundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Html;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.IO;

namespace GCFoundation.Tests.Components.Tests.TagHelpers.FDCP;

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
            },
            ViewContext = CreateMockViewContext()
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
            },
            ViewContext = CreateMockViewContext()
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
            Columns = columns,
            ViewContext = CreateMockViewContext()
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

    [Fact]
    public void Process_WithFilterableColumns_RendersFilterableFields()
    {
        // Arrange
        var columns = new[]
        {
            new TabulatorColumn { Title = "Name", Field = "name", Filter = true },
            new TabulatorColumn { Title = "Age", Field = "age", Filter = false },
            new TabulatorColumn { Title = "Email", Field = "email", Filter = true }
        };

        var tagHelper = new FDCPTabulatorTableTagHelper
        {
            Id = "filter-table",
            AjaxUrl = "/api/data",
            Columns = columns,
            ViewContext = CreateMockViewContext()
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
        Assert.Contains("data-filterable-fields='[\"name\",\"email\"]'", content);
    }

    [Fact]
    public void Process_WithEmptyColumns_RendersEmptyColumnsArray()
    {
        // Arrange
        var tagHelper = new FDCPTabulatorTableTagHelper
        {
            Id = "empty-table",
            AjaxUrl = "/api/data",
            Columns = Array.Empty<TabulatorColumn>(),
            ViewContext = CreateMockViewContext()
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
        Assert.Contains("data-columns='[]'", content);
        Assert.Contains("data-filterable-fields='[]'", content);
    }

    [Fact]
    public void Process_WithNullData_RendersAjaxUrl()
    {
        // Arrange
        var tagHelper = new FDCPTabulatorTableTagHelper
        {
            Id = "null-data-table",
            UseStaticData = true,
            Data = null,
            AjaxUrl = "/api/fallback",
            ViewContext = CreateMockViewContext()
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
        Assert.Contains("data-ajaxURL='/api/fallback'", content);
        Assert.DoesNotContain("data-set", content);
    }

    [Fact]
    public void Process_WithCustomPaginationSize_RendersCorrectPaginationSize()
    {
        // Arrange
        var tagHelper = new FDCPTabulatorTableTagHelper
        {
            Id = "pagination-table",
            AjaxUrl = "/api/data",
            PaginationSize = 25,
            ViewContext = CreateMockViewContext()
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
        Assert.Contains("data-pagination-size='25'", content);
    }

    [Fact]
    public void Process_WithAllColumnProperties_RendersAllColumnData()
    {
        // Arrange
        var columns = new[]
        {
            new TabulatorColumn
            {
                Title = "Full Column",
                Field = "fullField",
                HeaderSort = false,
                Width = "150px",
                HozAlign = "center",
                Formatter = "tickCross",
                CssClass = "custom-cell",
                Resizable = TabulatorResizableOption.True,
                Frozen = true,
                Tooltip = "This is a tooltip",
                Filter = true
            }
        };

        var tagHelper = new FDCPTabulatorTableTagHelper
        {
            Id = "full-column-table",
            AjaxUrl = "/api/data",
            Columns = columns,
            ViewContext = CreateMockViewContext()
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
        Assert.Contains("\"title\":\"Full Column\"", content);
        Assert.Contains("\"field\":\"fullField\"", content);
        Assert.Contains("\"headerSort\":false", content);
        Assert.Contains("\"width\":\"150px\"", content);
        Assert.Contains("\"hozAlign\":\"center\"", content);
        Assert.Contains("\"formatter\":\"tickCross\"", content);
        Assert.Contains("\"cssClass\":\"custom-cell\"", content);
        Assert.Contains("\"resizable\":true", content);
        Assert.Contains("\"frozen\":true", content);
        Assert.Contains("\"tooltip\":\"This is a tooltip\"", content);
    }

    [Fact]
    public void Process_SearchInputGeneration_ContainsCorrectElements()
    {
        // Arrange
        var tagHelper = new FDCPTabulatorTableTagHelper
        {
            Id = "search-test-table",
            AjaxUrl = "/api/data",
            ViewContext = CreateMockViewContext()
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
        Assert.Contains("search-test-table-search-form", content);
        Assert.Contains("search-test-table-search=input", content);
        Assert.Contains("class='tabulator-search-input'", content);
        Assert.Contains("data-tabulator-id='search-test-table-tabulator'", content);
        Assert.Contains("type='search'", content);
        Assert.Contains("label='Search'", content);
        Assert.Contains("hint='You can search across all columns'", content);
    }

    [Fact]
    public void Process_ComplexStaticData_RendersSerializedJson()
    {
        // Arrange
        var complexData = new[]
        {
            new { id = 1, name = "John Doe", details = new { age = 30, active = true }, tags = new[] { "admin", "user" } },
            new { id = 2, name = "Jane Smith", details = new { age = 25, active = false }, tags = new[] { "user" } }
        };

        var tagHelper = new FDCPTabulatorTableTagHelper
        {
            Id = "complex-data-table",
            UseStaticData = true,
            Data = complexData,
            ViewContext = CreateMockViewContext()
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
        Assert.Contains("data-set='[{", content);
        Assert.Contains("\"id\":1", content);
        Assert.Contains("\"name\":\"John Doe\"", content);
        Assert.Contains("\"details\":{\"age\":30,\"active\":true}", content);
        Assert.Contains("\"tags\":[\"admin\",\"user\"]", content);
    }

    [Fact]
    public void Process_WithViewContext_IncludesAntiForgeryToken()
    {
        // Arrange
        var tagHelper = new FDCPTabulatorTableTagHelper
        {
            Id = "antiforgery-table",
            AjaxUrl = "/api/data",
            ViewContext = CreateMockViewContext()
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
        Assert.Contains("data-antiforgery-token=", content);
        Assert.Contains("&quot;", content); // HTML-encoded quotes
    }

    [Fact]
    public void Process_DefaultValues_AreAppliedCorrectly()
    {
        // Arrange
        var tagHelper = new FDCPTabulatorTableTagHelper
        {
            Id = "default-table",
            ViewContext = CreateMockViewContext()
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
        Assert.Contains("data-pagination-size='10'", content); // Default pagination size
        Assert.Contains("data-layout='fitColumns'", content);
        Assert.Contains("data-pagination='local'", content);
        Assert.Contains("data-columns='[]'", content); // Empty columns by default
    }

    private static ViewContext CreateMockViewContext()
    {
        var mockHttpContext = new Mock<HttpContext>();
        var mockServiceProvider = new Mock<IServiceProvider>();
        
        // Create a mock that implements both IHtmlHelper and IViewContextAware
        var mockHtmlHelper = new Mock<IHtmlHelper>();
        var mockViewContextAware = mockHtmlHelper.As<IViewContextAware>();

        // Setup anti-forgery token HTML content
        var tokenHtml = new HtmlString("<input name=\"__RequestVerificationToken\" type=\"hidden\" value=\"test-token-value\" />");
        mockHtmlHelper.Setup(h => h.AntiForgeryToken()).Returns(tokenHtml);

        // Setup the IViewContextAware.Contextualize method
        mockViewContextAware.Setup(vca => vca.Contextualize(It.IsAny<ViewContext>()));

        mockServiceProvider.Setup(sp => sp.GetService(typeof(IHtmlHelper)))
                          .Returns(mockHtmlHelper.Object);

        mockHttpContext.Setup(c => c.RequestServices)
                       .Returns(mockServiceProvider.Object);

        return new ViewContext
        {
            HttpContext = mockHttpContext.Object
        };
    }
}