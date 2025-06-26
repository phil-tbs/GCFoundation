using System;
using Foundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Foundation.Tests.Components.Tests.TagHelpers.FDCP
{
    public class FDCPSearchBoxTagHelperTests
    {
        private readonly FDCPSearchBoxTagHelper _tagHelper;
        private readonly TagHelperContext _context;
        private readonly TagHelperOutput _output;

        public FDCPSearchBoxTagHelperTests()
        {
            _tagHelper = new FDCPSearchBoxTagHelper
            {
                Label = "Search",
                Placeholder = "Enter search terms",
                Name = "searchInput",
                SearchBoxId = "search-1"
            };

            _context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-id");

            _output = new TagHelperOutput("fdcp-search-box",
                new TagHelperAttributeList(),
                (result, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
        }

        [Fact]
        public void Process_RendersSearchBox_WithRequiredAttributes()
        {
            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            Assert.Equal("div", _output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, _output.TagMode);
            Assert.Equal("fdcp-filtered-search", _output.Attributes["class"].Value);

            var content = _output.Content.GetContent();
            Assert.Contains("<div class='fdcp-search-box-wrapper'>", content);
            Assert.Contains("<label class='sr-only'>Search</label>", content);
            Assert.Contains("type='search'", content);
            Assert.Contains("placeholder='Enter search terms'", content);
        }

        [Fact]
        public void Process_WithValue_RendersSearchBoxWithValue()
        {
            // Arrange
            _tagHelper.Value = "test query";

            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            var content = _output.Content.GetContent();
            Assert.Contains("value='test query'", content);
        }

        [Fact]
        public void Process_NullOutput_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _tagHelper.Process(_context, null!));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Process_WithoutValue_RendersSearchBoxWithoutValueAttribute(string? value)
        {
            // Arrange
            _tagHelper.Value = value;

            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            var content = _output.Content.GetContent();
            Assert.DoesNotContain("value=", content);
        }
    }
}