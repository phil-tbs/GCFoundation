using Foundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Tests.Components.Tests.FDCP
{
    public class FDCPSelectTagHelperTests
    {
        [Fact]
        public void Process_ShouldGenerateSelectElement()
        {
            // Arrange
            var tagHelper = new FDCPSelectTagHelper
            {
                For = MockModelExpression("SelectedCountry", "US"),
                Items = new List<SelectListItem>
                {
                    new SelectListItem { Value = "CA", Text = "Canada" },
                    new SelectListItem { Value = "US", Text = "United States" }
                }
            };

            var tagHelperContext = new TagHelperContext(
               new TagHelperAttributeList(),
               new Dictionary<object, object>(),
               "test"
            );
            var tagHelperOutput = new TagHelperOutput(
                "fdcp-select",
                new TagHelperAttributeList(),
                (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent())
            );

            // Act
            tagHelper.Process(tagHelperContext, tagHelperOutput);

            // Assert
            Assert.Equal("gcds-select", tagHelperOutput.TagName);
            Assert.Contains("United States", tagHelperOutput.Content.GetContent());
            Assert.Contains("Canada", tagHelperOutput.Content.GetContent());
            Assert.Contains("selected", tagHelperOutput.Content.GetContent()); // Ensures correct value is selected
        }

        private ModelExpression MockModelExpression(string name, string value)
        {
            var metadataProvider = new EmptyModelMetadataProvider();
            var metadata = metadataProvider.GetMetadataForType(typeof(string));
            var modelExpression = new ModelExpression(name, new ModelExplorer(metadataProvider, metadata, value));
            return modelExpression;
        }
    }
}
