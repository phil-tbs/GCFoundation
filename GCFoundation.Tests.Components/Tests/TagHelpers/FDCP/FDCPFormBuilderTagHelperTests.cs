using GCFoundation.Components.Models.FormBuilder;
using GCFoundation.Components.TagHelpers.FDCP;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.RegularExpressions;
using Xunit;

namespace GCFoundation.Tests.Components.Tests.TagHelpers.FDCP
{
    public class FDCPFormBuilderTagHelperTests
    {
        [Fact]
        public void Process_WithBasicForm_RendersExpectedStructure()
        {
            // Arrange
            var tagHelper = new FDCPFormBuilderTagHelper
            {
                Form = new FormDefinition
                {
                    Id = "testForm",
                    Title = "Test Form",
                    Action = "/submit",
                    Methode = "post",
                    SubmithButtonText = "Submit",
                    Sections = new[]
                    {
                        new FormSection
                        {
                            Title = "Personal Information",
                            Hint = "Please provide your details",
                            Questions = new[]
                            {
                                new FormQuestion
                                {
                                    Id = "name",
                                    Label = "Full Name",
                                    Type = QuestionType.Text,
                                    IsRequired = true
                                }
                            }
                        }
                    }
                }
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test");

            var output = new TagHelperOutput("fdcp-form-builder",
                new TagHelperAttributeList(),
                (result, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            // Act
            tagHelper.Process(context, output);

            // Assert
            Assert.Equal("div", output.TagName);
            Assert.Equal("gc-form", output.Attributes["class"].Value);

            var content = output.Content.GetContent();
            Assert.Contains("<form action='/submit' method='post' class='gc-form'>", content);
            Assert.Contains("<gcds-error-summary>", content);
            Assert.Contains("<gcds-fieldset", content);
            Assert.Contains("legend='Personal Information'", content);
            Assert.Contains("hint='Please provide your details'", content);
            Assert.Contains("required", content);
            Assert.Contains("Submit", content);
        }

        [Fact]
        public void Process_WithValidationRules_RendersValidationAttributes()
        {
            // Arrange
            var tagHelper = new FDCPFormBuilderTagHelper
            {
                Form = new FormDefinition
                {
                    Id = "testForm",
                    Title = "Test Form",
                    Action = "/submit",
                    Methode = "post",
                    SubmithButtonText = "Submit",
                    Sections = new[]
                    {
                        new FormSection
                        {
                            Title = "Test Section",
                            Questions = new[]
                            {
                                new FormQuestion
                                {
                                    Id = "email",
                                    Label = "Email",
                                    Type = QuestionType.Email,
                                    IsRequired = true,
                                    ValidateOnBlur = true,
                                    ValidationRules = new[]
                                    {
                                        new ValidationRule
                                        {
                                            Type = ValidationRuleType.Email
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test");

            var output = new TagHelperOutput("fdcp-form-builder",
                new TagHelperAttributeList(),
                (result, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            // Act
            tagHelper.Process(context, output);

            // Assert
            var content = output.Content.GetContent();
            Assert.Contains("validate-on-blur", content);
            Assert.Contains("data-validation-rules", content);
            Assert.Contains("type='email'", content);
        }

        [Fact]
        public void Process_WithDependencies_RendersDependencyAttributes()
        {
            // Arrange
            var tagHelper = new FDCPFormBuilderTagHelper
            {
                Form = new FormDefinition
                {
                    Id = "testForm",
                    Title = "Test Form",
                    Action = "/submit",
                    Methode = "post",
                    SubmithButtonText = "Submit",
                    Sections = new[]
                    {
                        new FormSection
                        {
                            Title = "Test Section",
                            Questions = new[]
                            {
                                new FormQuestion
                                {
                                    Id = "question2",
                                    Label = "Dependent Question",
                                    Type = QuestionType.Text,
                                    Dependencies = new[]
                                    {
                                        new QuestionDependency
                                        {
                                            SourceQuestionId = "question1",
                                            TargetQuestionId = "question2",
                                            TriggerValue = "yes",
                                            Action = DependencyAction.Show
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test");

            var output = new TagHelperOutput("fdcp-form-builder",
                new TagHelperAttributeList(),
                (result, encoder) => Task.FromResult<TagHelperContent>(new DefaultTagHelperContent()));

            // Act
            tagHelper.Process(context, output);

            // Assert
            var content = output.Content.GetContent();
            Assert.Contains("data-dependencies", content);
        }

        [Fact]
        public void Process_NullOutput_ThrowsArgumentNullException()
        {
            // Arrange
            var tagHelper = new FDCPFormBuilderTagHelper
            {
                Form = new FormDefinition
                {
                    Id = "testForm",
                    Title = "Test Form",
                    Action = "/submit",
                    Methode = "post",
                    SubmithButtonText = "Submit",
                    Sections = Array.Empty<FormSection>()
                }
            };

            var context = new TagHelperContext(
                new TagHelperAttributeList(),
                new Dictionary<object, object>(),
                "test");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => tagHelper.Process(context, null!));
        }
    }
}