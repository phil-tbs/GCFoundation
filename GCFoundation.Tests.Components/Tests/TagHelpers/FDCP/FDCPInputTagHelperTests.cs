using GCFoundation.Components.Attributes;
using GCFoundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel.DataAnnotations;

namespace GCFoundation.Tests.Components.Tests.TagHelpers.FDCP
{
    public class FDCPInputTagHelperTests
    {
        private readonly TagHelperContext _context;
        private readonly TagHelperOutput _output;

        public FDCPInputTagHelperTests()
        {
            _context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-id");

            _output = new TagHelperOutput("fdcp-input",
                new TagHelperAttributeList(),
                (result, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
        }

        private class TestModel
        {
            public string TextProperty { get; set; } = string.Empty;

            [DataType(DataType.EmailAddress)]
            public string EmailProperty { get; set; } = string.Empty;

            [DataType(DataType.Password)]
            public string PasswordProperty { get; set; } = string.Empty;

            [DataType(DataType.Date)]
            public DateTime DateProperty { get; set; }

            public bool BoolProperty { get; set; }

            [DataType(DataType.MultilineText)]
            public string MultilineProperty { get; set; } = string.Empty;

            public int NumberProperty { get; set; }

            [DateFormat("short")]
            public DateTime DateWithFormatProperty { get; set; }
        }

        private FDCPInputTagHelper SetupTagHelper(string propertyName)
        {
            var propertyInfo = typeof(TestModel).GetProperty(propertyName)!;
            var modelMetadataIdentity = ModelMetadataIdentity.ForProperty(
                propertyInfo,
                propertyInfo.PropertyType,
                typeof(TestModel));

            var modelAttributes = ModelAttributes.GetAttributesForProperty(typeof(TestModel), propertyInfo);

            var metadataDetails = new DefaultMetadataDetails(modelMetadataIdentity, modelAttributes);

            // Fix: Create a ModelExplorer instance using the DefaultModelMetadataProvider
            var modelMetadataProvider = new EmptyModelMetadataProvider();
            var modelMetadata = modelMetadataProvider.GetMetadataForProperty(propertyInfo, typeof(TestModel));
            var modelExplorer = new ModelExplorer(modelMetadataProvider, modelMetadata, null);

            var modelExpression = new ModelExpression(propertyName, modelExplorer);

            var tagHelper = new FDCPInputTagHelper()
            {
                For = modelExpression,
                ViewContext = new ViewContext()
            };

            return tagHelper;
        }

        [Fact]
        public void Process_WithNullOutput_ThrowsArgumentNullException()
        {
            // Arrange
            var tagHelper = SetupTagHelper("TextProperty");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => tagHelper.Process(_context, null!));
        }

        [Fact]
        public void Process_WithTextInput_RendersCorrectly()
        {
            // Arrange
            var tagHelper = SetupTagHelper("TextProperty");

            // Act
            tagHelper.Process(_context, _output);

            // Assert
            Assert.Equal("gcds-input", _output.TagName);
            Assert.Equal("text", _output.Attributes["type"].Value);
            Assert.Equal("TextProperty", _output.Attributes["input-id"].Value);
        }

        [Fact]
        public void Process_WithEmailInput_RendersCorrectly()
        {
            // Arrange
            var tagHelper = SetupTagHelper("EmailProperty");

            // Act
            tagHelper.Process(_context, _output);

            // Assert
            Assert.Equal("gcds-input", _output.TagName);
            Assert.Equal("email", _output.Attributes["type"].Value);
        }

        [Fact]
        public void Process_WithPasswordInput_RendersCorrectly()
        {
            // Arrange
            var tagHelper = SetupTagHelper("PasswordProperty");

            // Act
            tagHelper.Process(_context, _output);

            // Assert
            Assert.Equal("gcds-input", _output.TagName);
            Assert.Equal("password", _output.Attributes["type"].Value);
        }

        [Fact]
        public void Process_WithDateInput_RendersCorrectly()
        {
            // Arrange
            var tagHelper = SetupTagHelper("DateProperty");

            // Act
            tagHelper.Process(_context, _output);

            // Assert
            Assert.Equal("gcds-date-input", _output.TagName);
            Assert.Equal("full", _output.Attributes["format"].Value);
        }

        [Fact]
        public void Process_WithCheckbox_RendersCorrectly()
        {
            // Arrange
            var tagHelper = SetupTagHelper("BoolProperty");

            // Act
            tagHelper.Process(_context, _output);

            // Assert
            Assert.Equal("gcds-checkbox", _output.TagName);
            Assert.Equal("BoolProperty", _output.Attributes["checkbox-id"].Value);
        }

        [Fact]
        public void Process_WithTextArea_RendersCorrectly()
        {
            // Arrange
            var tagHelper = SetupTagHelper("MultilineProperty");

            // Act
            tagHelper.Process(_context, _output);

            // Assert
            Assert.Equal("gcds-textarea", _output.TagName);
            Assert.Equal("MultilineProperty", _output.Attributes["textarea-id"].Value);
        }

        [Fact]
        public void Process_WithNumberInput_RendersCorrectly()
        {
            // Arrange
            var tagHelper = SetupTagHelper("NumberProperty");

            // Act
            tagHelper.Process(_context, _output);

            // Assert
            Assert.Equal("gcds-input", _output.TagName);
            Assert.Equal("number", _output.Attributes["type"].Value);
        }

        [Fact]
        public void Process_WithDateFormat_RendersCorrectly()
        {
            // Arrange
            var tagHelper = SetupTagHelper("DateProperty");

            // Act
            tagHelper.Process(_context, _output);

            // Assert
            Assert.Equal("gcds-date-input", _output.TagName);
            Assert.Equal("full", _output.Attributes["format"].Value);
        }
    }
}