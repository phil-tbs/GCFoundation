using System.ComponentModel.DataAnnotations;
using Foundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Tests.Components.Tests.FDCP
{
    public class FDCPInputTagHelperTests
    {
        private static TagHelperContext CreateContext() =>
            new TagHelperContext(
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

        private static TagHelperOutput CreateOutput(string tagName) =>
            new TagHelperOutput(tagName,
                new TagHelperAttributeList(),
                (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        [Fact]
        public async Task Process_SetsCorrectTag_ForTextInput()
        {
            // Arrange
            var modelExplorer = new ModelExplorer(new EmptyModelMetadataProvider(), null, "Test Value");
            var helper = new FDCPInputTagHelper
            {
                For = new ModelExpression("TestProperty", modelExplorer),
                ViewContext = new ViewContext()
            };

            var context = CreateContext();
            var output = CreateOutput("fdcp-input");

            // Act
            helper.Process(context, output);

            // Assert
            Assert.Equal("gcds-input", output.TagName);
            Assert.Contains("type=\"text\"", output.Attributes.ToString());

            await Task.CompletedTask;
        }

        [Fact]
        public async Task Process_SetsCorrectTag_ForDateInput()
        {
            // Arrange
            var propertyInfo = typeof(TestModel).GetProperty(nameof(TestModel.Date));
            var modelExplorer = new ModelExplorer(new EmptyModelMetadataProvider(), null, DateTime.Now);

            var helper = new FDCPInputTagHelper
            {
                For = new ModelExpression("Date", modelExplorer),
                ViewContext = new ViewContext()
            };

            var context = CreateContext();
            var output = CreateOutput("fdcp-input");

            // Act
            helper.Process(context, output);

            // Assert
            Assert.Equal("gcds-date-input", output.TagName);
            Assert.Contains("format=\"full\"", output.Attributes.ToString());

            await Task.CompletedTask;
        }

        [Fact]
        public async Task Process_SetsCorrectTag_ForCheckbox()
        {
            // Arrange
            var modelExplorer = new ModelExplorer(new EmptyModelMetadataProvider(), null, true);
            var helper = new FDCPInputTagHelper
            {
                For = new ModelExpression("IsChecked", modelExplorer),
                ViewContext = new ViewContext()
            };

            var context = CreateContext();
            var output = CreateOutput("fdcp-input");

            // Act
            helper.Process(context, output);

            // Assert
            Assert.Equal("gcds-checkbox", output.TagName);
            Assert.Contains("checkbox-id=\"IsChecked\"", output.Attributes.ToString());

            await Task.CompletedTask;
        }

        [Fact]
        public async Task Process_SetsCorrectTag_ForTextArea()
        {
            // Arrange
            var propertyInfo = typeof(TestModel).GetProperty(nameof(TestModel.Comments));
            var modelExplorer = new ModelExplorer(new EmptyModelMetadataProvider(), null, "Some text");

            var helper = new FDCPInputTagHelper
            {
                For = new ModelExpression("Comments", modelExplorer),
                ViewContext = new ViewContext()
            };

            var context = CreateContext();
            var output = CreateOutput("fdcp-input");

            // Act
            helper.Process(context, output);

            // Assert
            Assert.Equal("gcds-textarea", output.TagName);
            Assert.Contains("textarea-id=\"Comments\"", output.Attributes.ToString());

            await Task.CompletedTask;
        }
    }

    public class TestModel
    {
        public string? Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public bool IsChecked { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Comments { get; set; }
    }

}
