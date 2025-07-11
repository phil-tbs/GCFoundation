using GCFoundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace GCFoundation.Tests.Components.Tests.TagHelpers.FDCP
{
    public class FDCPCheckboxesTagHelperTests
    {
        private readonly FDCPCheckboxesTagHelper _tagHelper;
        private readonly TagHelperContext _context;
        private readonly TagHelperOutput _output;

        public FDCPCheckboxesTagHelperTests()
        {
            _tagHelper = new FDCPCheckboxesTagHelper();

            _context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-id");

            _output = new TagHelperOutput("fdcp-checkboxes",
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
            _tagHelper.Items = new List<SelectListItem>
            {
                new SelectListItem { Text = "Option 1", Value = "1" }
            };

            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            Assert.Equal("gcds-checkboxes", _output.TagName);
            Assert.Equal(TagMode.StartTagAndEndTag, _output.TagMode);
        }

        [Fact]
        public void Process_WithRequiredAttribute_AddsRequiredAttribute()
        {
            // Arrange
            SetupModelExpression();
            _tagHelper.IsRequired = true;
            _tagHelper.Items = new List<SelectListItem>();

            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            Assert.True(_output.Attributes.ContainsName("required"));
        }

        [Fact]
        public void Process_WithItems_GeneratesCorrectOptionsJson()
        {
            // Arrange
            var selected = new List<string> { "music" };
            SetupModelExpression(new TestModel { SelectedInterests = selected });

            _tagHelper.Items = new List<SelectListItem>
            {
                new SelectListItem { Text = "Sports", Value = "sports" },
                new SelectListItem { Text = "Music", Value = "music" }
            };

            // Act
            _tagHelper.Process(_context, _output);

            // Assert
            var optionsAttribute = _output.Attributes.FirstOrDefault(a => a.Name == "options");
            Assert.NotNull(optionsAttribute);

            var options = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(
                optionsAttribute.Value.ToString()!,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(options);
            Assert.Equal(2, options.Count);

            // Verify first option
            Assert.Equal("SelectedInterests_sports", options[0]["id"].ToString());
            Assert.Equal("Sports", options[0]["label"].ToString());
            Assert.Equal("sports", options[0]["value"].ToString());
            Assert.False(GetChecked(options[0]));

            // Verify second option
            Assert.Equal("SelectedInterests_music", options[1]["id"].ToString());
            Assert.Equal("Music", options[1]["label"].ToString());
            Assert.Equal("music", options[1]["value"].ToString());
            Assert.True(GetChecked(options[1]));
        }

        [Fact]
        public void Process_WithNullFor_ThrowsInvalidOperationException()
        {
            // Arrange
            _tagHelper.Items = new List<SelectListItem>();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            _tagHelper.For = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => _tagHelper.Process(_context, _output));
        }

        /// <summary>
        /// Sets up the ModelExpression for the SelectedInterests property of TestModel.
        /// </summary>
        private void SetupModelExpression(TestModel? model = null)
        {
            var modelType = typeof(TestModel);
            var modelExplorer = new EmptyModelMetadataProvider()
                .GetModelExplorerForType(modelType, model ?? new TestModel());

            var propertyExplorer = modelExplorer.GetExplorerForProperty(nameof(TestModel.SelectedInterests));
            _tagHelper.For = new ModelExpression(nameof(TestModel.SelectedInterests), propertyExplorer);
        }

        private static bool GetChecked(Dictionary<string, object> option)
        {
            if (option["checked"] is JsonElement je)
            {
                if (je.ValueKind == JsonValueKind.True) return true;
                if (je.ValueKind == JsonValueKind.False) return false;
                // If it's a string "True"/"False"
                if (je.ValueKind == JsonValueKind.String)
                    return bool.Parse(je.GetString()!);
            }
            if (option["checked"] is bool b)
                return b;
            if (option["checked"] is string s)
                return bool.Parse(s);
            throw new InvalidCastException("Cannot convert checked value to bool.");
        }
    }

    public class TestModel
    {
        [Required]
        [Display(Name = "Interest")]
        public IEnumerable<string> SelectedInterests { get; set; } = new List<string>();

        public IEnumerable<SelectListItem> InterestOptions { get; set; } =
            new List<SelectListItem>
            {
                new() { Value = "sports", Text = "Sports" },
                new() { Value = "music", Text = "Music" },
                new() { Value = "travel", Text = "Travel" },
                new() { Value = "reading", Text = "Reading" }
            };
    }
}