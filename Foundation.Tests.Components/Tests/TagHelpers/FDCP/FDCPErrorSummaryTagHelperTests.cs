using System;
using System.Collections.Generic;
using System.Text.Json;
using Foundation.Common.Utilities;
using Foundation.Components.Models;
using Foundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace Foundation.Tests.Components.Tests.TagHelpers.FDCP
{
    public class FDCPErrorSummaryTagHelperTests
    {
        private readonly FDCPErrorSummaryTagHelper _tagHelper;
        private readonly TagHelperContext _context;
        private readonly TagHelperOutput _output;

        public FDCPErrorSummaryTagHelperTests()
        {
            _tagHelper = new FDCPErrorSummaryTagHelper();

            _context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-id");

            _output = new TagHelperOutput("fdcp-error-summary",
                new TagHelperAttributeList(),
                (result, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
        }

        [Fact]
        public void Process_WithNullOutput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _tagHelper.Process(_context, null!));
        }

        [Fact]
        public void Process_WithNullModel_SuppressesOutput()
        {
            // Arrange
            _tagHelper.Model = null!;

            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            Assert.True(_output.TagName == null && _output.Content.IsEmptyOrWhiteSpace);
        }

        [Fact]
        public void Process_RendersCorrectTagName()
        {
            // Arrange
            _tagHelper.Model = new TestViewModel();

            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            Assert.Equal("gcds-error-summary", _output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, _output.TagMode);
        }

        [Fact]
        public void Process_SetsLanguageAttribute()
        {
            // Arrange
            _tagHelper.Model = new TestViewModel();

            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            var langAttribute = Assert.Single(_output.Attributes, a => a.Name == "lang");
            Assert.Equal(LanguageUtility.GetCurrentApplicationLanguage(), langAttribute.Value);
        }

        [Fact]
        public void Process_WithValidModel_DoesNotSetErrorLinks()
        {
            // Arrange
            var model = new TestViewModel();
            _tagHelper.Model = model;

            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            Assert.DoesNotContain(_output.Attributes, a => a.Name == "error-links");
        }

        [Fact]
        public void Process_WithInvalidModel_SetsErrorLinks()
        {
            // Arrange
            var model = new TestViewModel();
            model.AddError("field1", "Error message 1");
            model.AddError("field2", "Error message 2");
            _tagHelper.Model = model;

            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            var errorLinksAttribute = Assert.Single(_output.Attributes, a => a.Name == "error-links");
            var errorLinks = JsonSerializer.Deserialize<Dictionary<string, string>>(
                errorLinksAttribute.Value!.ToString()!);

            Assert.NotNull(errorLinks);
            Assert.Equal(2, errorLinks.Count);
            Assert.Equal("Error message 1", errorLinks["#field1"]);
            Assert.Equal("Error message 2", errorLinks["#field2"]);
        }

        [Fact]
        public void Process_AlwaysSetsListenAttributeToTrue()
        {
            // Arrange
            _tagHelper.Model = new TestViewModel();

            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            var listenAttribute = Assert.Single(_output.Attributes, a => a.Name == "listen");
            Assert.True((bool)listenAttribute.Value!);
        }

        private class TestViewModel : BaseViewModel { }
    }
}