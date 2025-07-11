using GCFoundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Globalization;
using Xunit;
using GCFoundation.Components.Models;

namespace GCFoundation.Tests.Components.Tests.TagHelpers.FDCP
{
    public class FDCPStepperTagHelperTests
    {
        private readonly TagHelperContext _context;
        private readonly TagHelperOutput _output;

        public FDCPStepperTagHelperTests()
        {
            _context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test-id");

            _output = new TagHelperOutput("fdcp-stepper",
                new TagHelperAttributeList(),
                (result, encoder) =>
                {
                    var tagHelperContent = new DefaultTagHelperContent();
                    return Task.FromResult<TagHelperContent>(tagHelperContent);
                });
        }

        [Fact]
        public void Process_WithDefaultValues_RendersCorrectly()
        {
            // Arrange
            var tagHelper = new FDCPStepperTagHelper();

            // Act
            tagHelper.Process(_context, _output);

            // Assert
            Assert.Equal("div", _output.TagName);
            var content = _output.Content.GetContent();
            Assert.Contains("<gcds-heading tag='h2'>Current step</gcds-heading>", content);
            Assert.Contains("<div class='fdcp-stepper'>", content);
            Assert.Contains("</div>", content);
        }

        [Fact]
        public void Process_WithSteps_RendersAllSteps()
        {
            // Arrange
            var steps = new[]
            {
                new StepperStep { StepNumber = 1, Label = "Step 1" },
                new StepperStep { StepNumber = 2, Label = "Step 2" },
                new StepperStep { StepNumber = 3, Label = "Step 3" }
            };

            var tagHelper = new FDCPStepperTagHelper
            {
                CurrentStep = 2,
                Steps = steps
            };

            // Act
            tagHelper.Process(_context, _output);

            // Assert
            var content = _output.Content.GetContent();
            Assert.Contains("class='fdcp-step completed'", content);
            Assert.Contains("class='fdcp-step active'", content);
            Assert.Contains("class='fdcp-step incomplete'", content);
            Assert.Contains("<div class='fdcp-step-label'>Step 1</div>", content);
            Assert.Contains("<div class='fdcp-step-label'>Step 2</div>", content);
            Assert.Contains("<div class='fdcp-step-label'>Step 3</div>", content);
        }

        [Fact]
        public void Process_WithNullOutput_ThrowsArgumentNullException()
        {
            // Arrange
            var tagHelper = new FDCPStepperTagHelper();

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => tagHelper.Process(_context, null!));
        }

        [Theory]
        [InlineData(1, "active", "incomplete", "incomplete")]
        [InlineData(2, "completed", "active", "incomplete")]
        [InlineData(3, "completed", "completed", "active")]
        public void Process_CorrectlyAssignsStepClasses(int currentStep, string step1Class, string step2Class, string step3Class)
        {
            // Arrange
            var steps = new[]
            {
                new StepperStep { StepNumber = 1, Label = "Step 1" },
                new StepperStep { StepNumber = 2, Label = "Step 2" },
                new StepperStep { StepNumber = 3, Label = "Step 3" }
            };

            var tagHelper = new FDCPStepperTagHelper
            {
                CurrentStep = currentStep,
                Steps = steps
            };

            // Act
            tagHelper.Process(_context, _output);

            // Assert
            var content = _output.Content.GetContent();
            Assert.Contains($"class='fdcp-step {step1Class}'", content);
            Assert.Contains($"class='fdcp-step {step2Class}'", content);
            Assert.Contains($"class='fdcp-step {step3Class}'", content);
        }

        [Fact]
        public void Process_WithHiddenStep_DoesNotRenderHiddenStep()
        {
            // Arrange
            var steps = new[]
            {
                new StepperStep { StepNumber = 1, Label = "Step 1" },
                new StepperStep { StepNumber = 2, Label = "Step 2", IsHidden = true },
                new StepperStep { StepNumber = 3, Label = "Step 3" }
            };

            var tagHelper = new FDCPStepperTagHelper
            {
                CurrentStep = 1,
                Steps = steps
            };

            // Act
            tagHelper.Process(_context, _output);

            // Assert
            var content = _output.Content.GetContent();
            Assert.Contains("Step 1", content);
            Assert.DoesNotContain("Step 2", content);
            Assert.Contains("Step 3", content);
        }

        [Fact]
        public void Process_WithStepLink_RendersLinkCorrectly()
        {
            // Arrange
            var steps = new[]
            {
                new StepperStep { 
                    StepNumber = 1, 
                    Label = "Step 1",
                    IsLink = true,
                    LinkUrl = "/step1"
                }
            };

            var tagHelper = new FDCPStepperTagHelper
            {
                CurrentStep = 1,
                Steps = steps
            };

            // Act
            tagHelper.Process(_context, _output);

            // Assert
            var content = _output.Content.GetContent();
            Assert.Contains("<gcds-link href='/step1'>Step 1</gcds-link>", content);
        }
    }
}