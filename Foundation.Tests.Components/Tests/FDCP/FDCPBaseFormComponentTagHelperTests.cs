using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Foundation.Tests.Components.Tests.FDCP
{
    public class FDCPBaseFormComponentTagHelperTests
    {
        private class TestTagHelper : FDCPBaseFormComponentTagHelper
        {
            public override void Process(TagHelperContext context, TagHelperOutput output)
            {
                base.Process(context, output);
            }
        }

        private static TagHelperContext CreateContext() =>
            new TagHelperContext(
                allAttributes: new TagHelperAttributeList(),
                items: new Dictionary<object, object>(),
                uniqueId: "test");

        private static TagHelperOutput CreateOutput(string tagName) =>
            new TagHelperOutput(tagName,
                new TagHelperAttributeList(),
                (useCachedResult, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

        private static ModelExpression CreateModelExpression(string name, object model)
        {
            var metadataProvider = new EmptyModelMetadataProvider();
            var metadata = metadataProvider.GetMetadataForType(model.GetType());
            var modelExplorer = new ModelExplorer(metadataProvider, metadata, model);
            return new ModelExpression(name, modelExplorer);
        }

        [Fact]
        public async Task Process_SetsCorrectAttributes()
        {
            // Arrange
            var helper = new TestTagHelper
            {
                For = CreateModelExpression("TestProperty", "Test Value"),
                ViewContext = new ViewContext()
            };

            var context = CreateContext();
            var output = CreateOutput("fdcp-input");

            // Act
            helper.Process(context, output);

            // Assert
            Assert.Equal("gcds-input", output.TagName);
            Assert.Contains("name=\"TestProperty\"", output.Attributes.ToString());
            Assert.Contains("value=\"Test Value\"", output.Attributes.ToString());
        }

        [Fact]
        public void Process_SuppressesOutput_WhenForIsNull()
        {
            // Arrange
            var helper = new TestTagHelper
            {
                ViewContext = new ViewContext()
            };

            var context = CreateContext();
            var output = CreateOutput("fdcp-input");

            // Act
            helper.Process(context, output);

            // Assert
            Assert.True(output.IsContentModified);
        }
    }
}
