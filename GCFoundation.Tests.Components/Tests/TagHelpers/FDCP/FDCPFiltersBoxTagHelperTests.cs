using GCFoundation.Components.Models;
using GCFoundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GCFoundation.Tests.Components.Tests.TagHelpers.FDCP
{
    public class FDCPFiltersBoxTagHelperTests
    {
        private readonly FDCPFiltersBoxTagHelper _tagHelper;
        private readonly TagHelperContext _context;
        private readonly TagHelperOutput _output;

        public FDCPFiltersBoxTagHelperTests()
        {
            _tagHelper = new FDCPFiltersBoxTagHelper
            {
                Title = string.Empty, // Initialize required property 'Title'  
                Filters = new List<SearchFilterCategory>() // Initialize required property 'Filters'  
            };

            _context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-id");

            _output = new TagHelperOutput("fdcp-filters-box",
                new TagHelperAttributeList(),
                (result, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
        }

        [Fact]
        public void Process_WithValidInput_RendersCorrectStructure()
        {
            // Arrange  
            _tagHelper.Title = "Test Filters";
            _tagHelper.Filters = new List<SearchFilterCategory>
                {
                    new()
                    {
                        Title = "Category 1",
                        SearchFilterCategoryId = "cat1",
                        IsOpen = true,
                        Filters = new List<SearchFilterOption>
                        {
                            new() { Name = "option1", Title = "Option 1", Count = 5 }
                        }
                    }
                };

            // Act  
            _tagHelper.Process(_context, _output);

            // Assert  
            Assert.Equal("div", _output.TagName);
            Assert.Equal("filter-panel", _output.Attributes["class"].Value);
            Assert.Contains("<h3>Test Filters</h3>", _output.Content.GetContent());
            Assert.Contains("class='filter-section'", _output.Content.GetContent());
            Assert.Contains("class='fdcp-collapse-button'", _output.Content.GetContent());
            Assert.Contains("data-fdcp-collapse-toggle='collapse-cat1'", _output.Content.GetContent());
            Assert.Contains("aria-expanded='true'", _output.Content.GetContent());
            Assert.Contains("class='fdcp-collapse fdcp-show'", _output.Content.GetContent());
        }

        [Fact]
        public void Process_WithClosedCategory_RendersCollapsedState()
        {
            // Arrange  
            _tagHelper.Title = "Test Filters";
            _tagHelper.Filters = new List<SearchFilterCategory>
                {
                    new()
                    {
                        Title = "Category 1",
                        SearchFilterCategoryId = "cat1",
                        IsOpen = false,
                        Filters = new List<SearchFilterOption>
                        {
                            new() { Name = "option1", Title = "Option 1", Count = 5 }
                        }
                    }
                };

            // Act  
            _tagHelper.Process(_context, _output);

            // Assert  
            Assert.Contains("aria-expanded='false'", _output.Content.GetContent());
            Assert.DoesNotContain("fdcp-show", _output.Content.GetContent());
        }

        [Fact]
        public void Process_WithNullOutput_ThrowsArgumentNullException()
        {
            // Arrange  
            _tagHelper.Title = "Test Filters";
            _tagHelper.Filters = new List<SearchFilterCategory>();

            // Act & Assert  
            Assert.Throws<ArgumentNullException>(() => _tagHelper.Process(_context, null!));
        }

        [Fact]
        public void Process_RendersFilterOptionsCorrectly()
        {
            // Arrange  
            _tagHelper.Title = "Test Filters";
            _tagHelper.Filters = new List<SearchFilterCategory>
                {
                    new()
                    {
                        Title = "Category 1",
                        SearchFilterCategoryId = "cat1",
                        IsOpen = true,
                        Filters = new List<SearchFilterOption>
                        {
                            new() { Name = "option1", Title = "Option 1", Count = 5 },
                            new() { Name = "option2", Title = "Option 2", Count = 10 }
                        }
                    }
                };

            // Act  
            _tagHelper.Process(_context, _output);
            var content = _output.Content.GetContent();

            // Assert  
            Assert.Contains("<input type='checkbox' name='option1' id='option1'", content);
            Assert.Contains("<label for='option1' class=''>Option 1</label>", content);
            Assert.Contains("<span class='filter-count'>5</span>", content);
            Assert.Contains("<input type='checkbox' name='option2' id='option2'", content);
            Assert.Contains("<label for='option2' class=''>Option 2</label>", content);
            Assert.Contains("<span class='filter-count'>10</span>", content);
        }
    }
}