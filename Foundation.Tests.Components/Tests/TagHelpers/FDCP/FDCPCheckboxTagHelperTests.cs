using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using Foundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace Foundation.Tests.Components.Tests.TagHelpers.FDCP
{
    public class FDCPCheckboxTagHelperTests
    {
        private readonly FDCPCheckboxTagHelper _tagHelper;
        private readonly TagHelperContext _context;
        private readonly TagHelperOutput _output;

        public FDCPCheckboxTagHelperTests()
        {
            _tagHelper = new FDCPCheckboxTagHelper();

            _context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-id");

            _output = new TagHelperOutput("fdcp-checkbox",
                new TagHelperAttributeList(),
                (result, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });

            var viewContext = new ViewContext();
            _tagHelper.ViewContext = viewContext;
        }

        [Fact]
        public void Process_RendersCorrectTagName()
        {
            // Arrange
            SetupModelExpression();

            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            Assert.Equal("gcds-checkboxes", _output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, _output.TagMode);
        }

        [Fact]
        public void Process_WithNullFor_ThrowsInvalidOperationException()
        {
            // Arrange
            _tagHelper.For = null;

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _tagHelper.Process(_context, _output));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Process_WithRequired_AddsRequiredAttribute(bool isRequired)
        {
            // Arrange
            SetupModelExpression();
            _tagHelper.IsRequired = isRequired;

            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            Assert.Equal(isRequired, _output.Attributes.ContainsName("required"));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Process_GeneratesCorrectOptionsJson(bool isChecked)
        {
            // Arrange
            SetupModelExpression(new TestModel { IsChecked = isChecked });

            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            var optionsAttribute = Assert.Single(_output.Attributes, a => a.Name == "options");
            Assert.NotNull(optionsAttribute.Value);

            var options = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(
                optionsAttribute.Value.ToString()!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(options);
            Assert.Single(options);

            var option = options[0];
            Assert.Equal("IsChecked", option["id"].ToString());
            Assert.Equal("Is Checked", option["label"].ToString());
            Assert.Equal("true", option["value"].ToString());
            Assert.Equal(isChecked, GetChecked(option));
        }

        private void SetupModelExpression(TestModel? model = null)
        {
            var modelType = typeof(TestModel);
            var modelExplorer = new EmptyModelMetadataProvider()
                .GetModelExplorerForType(modelType, model ?? new TestModel());

            var propertyExplorer = modelExplorer.GetExplorerForProperty(nameof(TestModel.IsChecked));
            _tagHelper.For = new ModelExpression(nameof(TestModel.IsChecked), propertyExplorer);
        }

        private static bool GetChecked(Dictionary<string, object> option)
        {
            if (option["checked"] is JsonElement je)
            {
                if (je.ValueKind == JsonValueKind.True) return true;
                if (je.ValueKind == JsonValueKind.False) return false;
                if (je.ValueKind == JsonValueKind.String)
                    return bool.Parse(je.GetString()!);
            }
            if (option["checked"] is bool b)
                return b;
            if (option["checked"] is string s)
                return bool.Parse(s);
            throw new InvalidCastException("Cannot convert checked value to bool.");
        }

        private class TestModel
        {
            [Display(Name = "Is Checked")]
            public bool IsChecked { get; set; }
        }
    }
}