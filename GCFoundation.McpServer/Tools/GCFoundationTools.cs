using System.ComponentModel;
using System.Text;
using System.Text.Json;
using ModelContextProtocol.Server;

/// <summary>
/// Core MCP tools for GC Foundation utilities and helpers.
/// Provides quick component generation, validation helpers, and project guidance.
/// </summary>
public class GCFoundationTools
{
    [McpServerTool]
    [Description("Quick generator for common GCDS text patterns and components.")]
    public string QuickGCDSComponent(
        [Description("Component type: heading, text, button, input, card, breadcrumbs")] string componentType,
        [Description("Primary content or text")] string content,
        [Description("Component variant or style")] string? variant = null,
        [Description("Additional attributes in 'key=value,key=value' format")] string? attributes = null)
    {
        var sb = new StringBuilder();
        
        switch (componentType.ToLower())
        {
            case "heading":
                var headingTag = variant ?? "h2";
                sb.Append($"<gcds-heading tag=\"{headingTag}\">{content}</gcds-heading>");
                break;
                
            case "text":
                var textSize = variant ?? "body";
                sb.Append($"<gcds-text size=\"{textSize}\">{content}</gcds-text>");
                break;
                
            case "button":
                var buttonVariant = variant ?? "primary";
                sb.Append($"<gcds-button variant=\"{buttonVariant}\">{content}</gcds-button>");
                break;
                
            case "input":
                var inputType = variant ?? "text";
                sb.Append($"<gcds-input input-id=\"{content.Replace(" ", "")}\" label=\"{content}\" type=\"{inputType}\"></gcds-input>");
                break;
                
            case "card":
                sb.Append($"<gcds-card><gcds-card-body>{content}</gcds-card-body></gcds-card>");
                break;
                
            case "breadcrumbs":
                sb.Append("<gcds-breadcrumbs>");
                var items = content.Split(',');
                foreach (var item in items)
                {
                    sb.Append($"<gcds-breadcrumbs-item>{item.Trim()}</gcds-breadcrumbs-item>");
                }
                sb.Append("</gcds-breadcrumbs>");
                break;
                
            default:
                return $"<!-- Unknown component type: {componentType} -->";
        }
        
        // Add custom attributes if provided
        if (!string.IsNullOrEmpty(attributes))
        {
            var result = sb.ToString();
            var attrPairs = attributes.Split(',');
            foreach (var pair in attrPairs)
            {
                var keyValue = pair.Split('=');
                if (keyValue.Length == 2)
                {
                    var key = keyValue[0].Trim();
                    var value = keyValue[1].Trim();
                    // Replace the first occurrence of ">" with the attribute
                    int index = result.IndexOf('>');
                    if (index >= 0)
                    {
                        result = result.Substring(0, index) + $" {key}=\"{value}\"" + result.Substring(index);
                    }
                }
            }
            return result;
        }
        
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Validates GCDS component markup for compliance and accessibility.")]
    public string ValidateGCDSMarkup(
        [Description("HTML markup to validate")] string markup,
        [Description("Validation type: accessibility, compliance, structure, all")] string validationType = "all")
    {
        var sb = new StringBuilder();
        var issues = new List<string>();
        
        // Basic validation checks
        if (string.IsNullOrWhiteSpace(markup))
        {
            return "Error: No markup provided for validation.";
        }
        
        sb.AppendLine("## GCDS Markup Validation Results");
        sb.AppendLine();
        
        // Check for GCDS component usage
        if (!markup.Contains("gcds-") && !markup.Contains("fdcp-"))
        {
            issues.Add("No GCDS or FDCP components detected in markup");
        }
        
        // Accessibility checks
        if (validationType == "accessibility" || validationType == "all")
        {
            sb.AppendLine("### Accessibility Checks:");
            
            if (markup.Contains("<gcds-button") && !markup.Contains("aria-label") && !markup.Contains("aria-describedby"))
            {
                issues.Add("Button elements should have proper ARIA labels for screen readers");
            }
            
            if (markup.Contains("<gcds-input") && !markup.Contains("label"))
            {
                issues.Add("Input elements must have associated labels");
            }
            
            if (markup.Contains("<gcds-heading") && markup.Contains("h1") && markup.Split("h1").Length > 3)
            {
                issues.Add("Multiple H1 headings detected - only one H1 per page is recommended");
            }
        }
        
        // Compliance checks
        if (validationType == "compliance" || validationType == "all")
        {
            sb.AppendLine("### GC Design System Compliance:");
            
            if (markup.Contains("class=") && markup.Contains("gcds-"))
            {
                issues.Add("Avoid adding custom CSS classes to GCDS components");
            }
            
            if (markup.Contains("style=") && markup.Contains("gcds-"))
            {
                issues.Add("Avoid inline styles on GCDS components - use component properties instead");
            }
        }
        
        // Structure checks
        if (validationType == "structure" || validationType == "all")
        {
            sb.AppendLine("### Structure Validation:");
            
            if (markup.Contains("<gcds-fieldset") && !markup.Contains("legend"))
            {
                issues.Add("Fieldset elements should include legend attributes");
            }
            
            if (markup.Contains("<form") && !markup.Contains("gcds-error-summary"))
            {
                issues.Add("Forms should include error summary components for better UX");
            }
        }
        
        if (issues.Count == 0)
        {
            sb.AppendLine("✅ **No issues found!** Your markup follows GCDS best practices.");
        }
        else
        {
            sb.AppendLine($"⚠️  **{issues.Count} issue(s) found:**");
            foreach (var issue in issues)
            {
                sb.AppendLine($"- {issue}");
            }
        }
        
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Converts common HTML patterns to GCDS component equivalents.")]
    public string ConvertToGCDS(
        [Description("HTML markup to convert")] string htmlMarkup,
        [Description("Conversion mode: conservative, aggressive, semantic")] string mode = "conservative")
    {
        var sb = new StringBuilder();
        var converted = htmlMarkup;
        
        switch (mode.ToLower())
        {
            case "conservative":
                // Only convert obvious mappings
                converted = converted.Replace("<h1>", "<gcds-heading tag=\"h1\">");
                converted = converted.Replace("</h1>", "</gcds-heading>");
                converted = converted.Replace("<h2>", "<gcds-heading tag=\"h2\">");
                converted = converted.Replace("</h2>", "</gcds-heading>");
                converted = converted.Replace("<button", "<gcds-button");
                converted = converted.Replace("</button>", "</gcds-button>");
                break;
                
            case "aggressive":
                // Convert more HTML elements
                converted = converted.Replace("<p>", "<gcds-text>");
                converted = converted.Replace("</p>", "</gcds-text>");
                converted = converted.Replace("<div class=\"container\">", "<gcds-container>");
                converted = converted.Replace("</div>", "</gcds-container>");
                converted = converted.Replace("<input", "<gcds-input");
                converted = converted.Replace("<select", "<gcds-select");
                converted = converted.Replace("<textarea", "<gcds-textarea");
                break;
                
            case "semantic":
                // Focus on semantic improvements
                converted = converted.Replace("<div class=\"card\">", "<gcds-card>");
                converted = converted.Replace("<nav", "<gcds-breadcrumbs");
                converted = converted.Replace("<fieldset", "<gcds-fieldset");
                break;
        }
        
        sb.AppendLine("## HTML to GCDS Conversion Results");
        sb.AppendLine();
        sb.AppendLine("**Original HTML:**");
        sb.AppendLine("```html");
        sb.AppendLine(htmlMarkup);
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("**Converted GCDS:**");
        sb.AppendLine("```html");
        sb.AppendLine(converted);
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("**Note:** Manual review and attribute adjustment may be required for optimal GCDS compliance.");
        
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates form validation patterns and examples for GCDS forms.")]
    public string GenerateFormValidation(
        [Description("Validation type: email, phone, postal-code, sin, date, custom")] string validationType,
        [Description("Field name")] string fieldName,
        [Description("Custom pattern for validation (regex)")] string? customPattern = null,
        [Description("Error message")] string? errorMessage = null)
    {
        var sb = new StringBuilder();
        string pattern;
        string message;
        string example;
        
        switch (validationType.ToLower())
        {
            case "email":
                pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                message = errorMessage ?? "Please enter a valid email address";
                example = "user@example.com";
                break;
                
            case "phone":
                pattern = @"^(\+1-?)?(\()?[0-9]{3}(\))?(-|\s)?[0-9]{3}(-|\s)?[0-9]{4}$";
                message = errorMessage ?? "Please enter a valid phone number (e.g., 613-555-1234)";
                example = "613-555-1234";
                break;
                
            case "postal-code":
                pattern = @"^[A-Za-z]\d[A-Za-z][ -]?\d[A-Za-z]\d$";
                message = errorMessage ?? "Please enter a valid postal code (e.g., K1A 0A6)";
                example = "K1A 0A6";
                break;
                
            case "sin":
                pattern = @"^\d{3}-?\d{3}-?\d{3}$";
                message = errorMessage ?? "Please enter a valid Social Insurance Number (e.g., 123-456-789)";
                example = "123-456-789";
                break;
                
            case "date":
                pattern = @"^\d{4}-\d{2}-\d{2}$";
                message = errorMessage ?? "Please enter a valid date (YYYY-MM-DD)";
                example = "2024-01-15";
                break;
                
            case "custom":
                pattern = customPattern ?? @".*";
                message = errorMessage ?? "Please enter a valid value";
                example = "Custom validation example";
                break;
                
            default:
                return "Error: Unsupported validation type. Use: email, phone, postal-code, sin, date, or custom.";
        }
        
        sb.AppendLine($"## Form Validation for {validationType.ToUpper()}");
        sb.AppendLine();
        sb.AppendLine("**GCDS Input Component:**");
        sb.AppendLine("```html");
        sb.AppendLine($"<gcds-input");
        sb.AppendLine($"    input-id=\"{fieldName}\"");
        sb.AppendLine($"    name=\"{fieldName}\"");
        sb.AppendLine($"    label=\"{char.ToUpper(fieldName[0]) + fieldName[1..].Replace("_", " ")}\"");
        sb.AppendLine($"    type=\"text\"");
        sb.AppendLine($"    pattern=\"{pattern}\"");
        sb.AppendLine($"    error-message=\"{message}\"");
        sb.AppendLine($"    hint=\"Example: {example}\"");
        sb.AppendLine($"    required=\"true\">");
        sb.AppendLine("</gcds-input>");
        sb.AppendLine("```");
        sb.AppendLine();
        sb.AppendLine("**Validation Details:**");
        sb.AppendLine($"- **Pattern:** `{pattern}`");
        sb.AppendLine($"- **Error Message:** {message}");
        sb.AppendLine($"- **Example Value:** {example}");
        
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates a complete form solution with view, model, controller, and validation for GCFoundation applications.")]
    public string GenerateCompleteFormSolution(
        [Description("Form name (e.g., 'UserRegistration', 'ContactForm')")] string formName,
        [Description("Form fields in JSON format: [{\"name\":\"firstName\",\"type\":\"text\",\"label\":\"First Name\",\"required\":true,\"validation\":\"string\"}]")] string fieldsJson,
        [Description("Controller namespace")] string controllerNamespace = "YourApp.Controllers",
        [Description("Model namespace")] string modelNamespace = "YourApp.Models",
        [Description("Include file upload capabilities")] bool includeFileUpload = false,
        [Description("Multi-step form")] bool isMultiStep = false,
        [Description("Success redirect URL after form submission")] string successRedirectUrl = "/Home/Success")
    {
        var sb = new StringBuilder();
        var modelClassName = $"{formName}Model";
        var controllerClassName = $"{formName}Controller";
        
        try
        {
            var fields = JsonSerializer.Deserialize<FormField[]>(fieldsJson);
            
            sb.AppendLine("# Complete Form Solution Generated");
            sb.AppendLine($"**Form Name**: {formName}");
            sb.AppendLine($"**Generated Files**: View, Model, Controller");
            sb.AppendLine();
            
            // Generate Model Class
            sb.AppendLine("## 1. Model Class");
            sb.AppendLine($"**File**: `Models/{modelClassName}.cs`");
            sb.AppendLine("```csharp");
            sb.AppendLine($"using System.ComponentModel.DataAnnotations;");
            sb.AppendLine($"using GCFoundation.Components.Validation;");
            sb.AppendLine();
            sb.AppendLine($"namespace {modelNamespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {modelClassName}");
            sb.AppendLine("    {");
            
            foreach (var field in fields ?? [])
            {
                // Add validation attributes
                if (field.Required)
                {
                    sb.AppendLine($"        [Required(ErrorMessage = \"The {field.Label} field is required.\")]");
                }
                
                switch (field.Validation?.ToLower())
                {
                    case "email":
                        sb.AppendLine($"        [EmailAddress(ErrorMessage = \"Please enter a valid email address.\")]");
                        break;
                    case "phone":
                        sb.AppendLine($"        [RegularExpression(@\"^(\\+1-?)?(\\()?[0-9]{{3}}(\\))?(-|\\s)?[0-9]{{3}}(-|\\s)?[0-9]{{4}}$\", ErrorMessage = \"Please enter a valid phone number.\")]");
                        break;
                    case "postal-code":
                        sb.AppendLine($"        [RegularExpression(@\"^[A-Za-z]\\d[A-Za-z][ -]?\\d[A-Za-z]\\d$\", ErrorMessage = \"Please enter a valid postal code.\")]");
                        break;
                    case "url":
                        sb.AppendLine($"        [Url(ErrorMessage = \"Please enter a valid URL.\")]");
                        break;
                }
                
                if (field.MaxLength > 0)
                {
                    sb.AppendLine($"        [MaxLength({field.MaxLength}, ErrorMessage = \"The {field.Label} cannot exceed {field.MaxLength} characters.\")]");
                }
                
                sb.AppendLine($"        [Display(Name = \"{field.Label}\")]");
                
                // Determine property type
                var propertyType = field.Type?.ToLower() switch
                {
                    "email" => "string",
                    "password" => "string",
                    "tel" => "string",
                    "url" => "string",
                    "number" => "int",
                    "date" => "DateTime?",
                    "checkbox" => "bool",
                    "file" => "IFormFile",
                    _ => "string"
                };
                
                sb.AppendLine($"        public {propertyType}? {ToPascalCase(field.Name)} {{ get; set; }}");
                sb.AppendLine();
            }
            
            if (includeFileUpload)
            {
                sb.AppendLine($"        [Display(Name = \"Supporting Documents\")]");
                sb.AppendLine($"        public List<IFormFile>? Documents {{ get; set; }}");
                sb.AppendLine();
            }
            
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine("```");
            sb.AppendLine();
            
            // Generate Controller
            sb.AppendLine("## 2. Controller");
            sb.AppendLine($"**File**: `Controllers/{controllerClassName}.cs`");
            sb.AppendLine("```csharp");
            sb.AppendLine($"using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine($"using GCFoundation.Components.Controllers;");
            sb.AppendLine($"using {modelNamespace};");
            sb.AppendLine();
            sb.AppendLine($"namespace {controllerNamespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {controllerClassName} : GCFoundationBaseController");
            sb.AppendLine("    {");
            sb.AppendLine($"        private readonly ILogger<{controllerClassName}> _logger;");
            sb.AppendLine();
            sb.AppendLine($"        public {controllerClassName}(ILogger<{controllerClassName}> logger)");
            sb.AppendLine("        {");
            sb.AppendLine("            _logger = logger;");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [HttpGet]");
            sb.AppendLine($"        public IActionResult Index()");
            sb.AppendLine("        {");
            sb.AppendLine($"            var model = new {modelClassName}();");
            sb.AppendLine("            return View(model);");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        [ValidateAntiForgeryToken]");
            sb.AppendLine($"        public async Task<IActionResult> Submit({modelClassName} model)");
            sb.AppendLine("        {");
            sb.AppendLine("            if (!ModelState.IsValid)");
            sb.AppendLine("            {");
            sb.AppendLine("                return View(\"Index\", model);");
            sb.AppendLine("            }");
            sb.AppendLine();
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine($"                // Process the {formName.ToLower()} form submission");
            sb.AppendLine("                _logger.LogInformation($\"Processing {formName} submission for user {User.Identity?.Name}\");");
            sb.AppendLine();
            
            if (includeFileUpload)
            {
                sb.AppendLine("                // Handle file uploads");
                sb.AppendLine("                if (model.Documents?.Any() == true)");
                sb.AppendLine("                {");
                sb.AppendLine("                    foreach (var file in model.Documents)");
                sb.AppendLine("                    {");
                sb.AppendLine("                        // Save file logic here");
                sb.AppendLine("                        _logger.LogInformation($\"Uploaded file: {file.FileName}\");");
                sb.AppendLine("                    }");
                sb.AppendLine("                }");
                sb.AppendLine();
            }
            
            sb.AppendLine("                // Add your business logic here");
            sb.AppendLine("                // await _yourService.ProcessFormAsync(model);");
            sb.AppendLine();
            sb.AppendLine($"                return Redirect(\"{successRedirectUrl}\");");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine($"                _logger.LogError(ex, \"Error processing {formName} form submission\");");
            sb.AppendLine("                ModelState.AddModelError(\"\", \"An error occurred while processing your request. Please try again.\");");
            sb.AppendLine("                return View(\"Index\", model);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine("```");
            sb.AppendLine();
            
            // Generate View
            sb.AppendLine("## 3. Razor View");
            sb.AppendLine($"**File**: `Views/{formName}/Index.cshtml`");
            sb.AppendLine("```html");
            sb.AppendLine($"@model {modelNamespace}.{modelClassName}");
            sb.AppendLine("@{");
            sb.AppendLine($"    ViewData[\"Title\"] = \"{formName}\";");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine($"<gcds-heading tag=\"h1\">{SplitPascalCase(formName)}</gcds-heading>");
            sb.AppendLine();
            sb.AppendLine("@if (!ViewData.ModelState.IsValid)");
            sb.AppendLine("{");
            sb.AppendLine("    <gcds-error-summary>");
            sb.AppendLine("        <gcds-heading tag=\"h2\" size=\"h3\">There are errors on this page</gcds-heading>");
            sb.AppendLine("        <ul>");
            sb.AppendLine("            @foreach (var error in ViewData.ModelState)");
            sb.AppendLine("            {");
            sb.AppendLine("                @foreach (var errorMessage in error.Value.Errors)");
            sb.AppendLine("                {");
            sb.AppendLine("                    <li><a href=\"#@error.Key\">@errorMessage.ErrorMessage</a></li>");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("        </ul>");
            sb.AppendLine("    </gcds-error-summary>");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine($"<form asp-action=\"Submit\" method=\"post\" novalidate{(includeFileUpload ? " enctype=\"multipart/form-data\"" : "")}>");
            sb.AppendLine("    @Html.AntiForgeryToken()");
            sb.AppendLine();
            
            // Group fields by type for better organization
            var regularFields = fields.Where(f => f.Type != "file").ToArray();
            var fileFields = fields.Where(f => f.Type == "file").ToArray();
            
            if (regularFields.Any())
            {
                sb.AppendLine("    <gcds-fieldset fieldset-id=\"form-fields\" legend=\"Information\" legend-size=\"h2\">");
                
                foreach (var field in regularFields)
                {
                    GenerateFieldMarkup(sb, field);
                }
                
                sb.AppendLine("    </gcds-fieldset>");
                sb.AppendLine();
            }
            
            if (includeFileUpload || fileFields.Any())
            {
                sb.AppendLine("    <gcds-fieldset fieldset-id=\"documents\" legend=\"Documents\" legend-size=\"h2\">");
                
                if (includeFileUpload)
                {
                    sb.AppendLine("        <gcds-file-upload");
                    sb.AppendLine("            uploader-id=\"Documents\"");
                    sb.AppendLine("            name=\"Documents\"");
                    sb.AppendLine("            label=\"Supporting Documents\"");
                    sb.AppendLine("            accept=\".pdf,.doc,.docx,.jpg,.png\"");
                    sb.AppendLine("            multiple=\"true\"");
                    sb.AppendLine("            hint=\"Upload any supporting documents (PDF, Word, or Image files)\">");
                    sb.AppendLine("        </gcds-file-upload>");
                }
                
                foreach (var field in fileFields)
                {
                    GenerateFieldMarkup(sb, field);
                }
                
                sb.AppendLine("    </gcds-fieldset>");
                sb.AppendLine();
            }
            
            sb.AppendLine("    <gcds-button type=\"submit\" variant=\"primary\">Submit</gcds-button>");
            sb.AppendLine("</form>");
            sb.AppendLine("```");
            sb.AppendLine();
            
            // Generate Usage Instructions
            sb.AppendLine("## 4. Implementation Steps");
            sb.AppendLine();
            sb.AppendLine("1. **Create the Model**: Add the model class to your `Models` folder");
            sb.AppendLine("2. **Create the Controller**: Add the controller to your `Controllers` folder");
            sb.AppendLine("3. **Create the View**: Create a folder named `" + formName + "` in your `Views` folder and add the Index.cshtml file");
            sb.AppendLine("4. **Add Route**: Ensure your route configuration includes the controller");
            sb.AppendLine("5. **Add Dependencies**: Make sure you have the required GCFoundation packages installed");
            sb.AppendLine();
            sb.AppendLine("## 5. Additional Configuration");
            sb.AppendLine();
            sb.AppendLine("### Required Package References");
            sb.AppendLine("```xml");
            sb.AppendLine("<PackageReference Include=\"GCFoundation.Common\" Version=\"latest\" />");
            sb.AppendLine("<PackageReference Include=\"GCFoundation.Components\" Version=\"latest\" />");
            sb.AppendLine("```");
            sb.AppendLine();
            sb.AppendLine("### Recommended Enhancements");
            sb.AppendLine("- Add custom validation logic in the controller");
            sb.AppendLine("- Implement business logic services for form processing");
            sb.AppendLine("- Add localization for multilingual support");
            sb.AppendLine("- Configure file upload validation and storage");
            sb.AppendLine("- Add unit tests for the controller and model validation");
            
        }
        catch (JsonException)
        {
            sb.AppendLine("Error: Invalid JSON format for fields. Please provide fields in the correct format:");
            sb.AppendLine("```json");
            sb.AppendLine("[");
            sb.AppendLine("  {");
            sb.AppendLine("    \"name\": \"firstName\",");
            sb.AppendLine("    \"type\": \"text\",");
            sb.AppendLine("    \"label\": \"First Name\",");
            sb.AppendLine("    \"required\": true,");
            sb.AppendLine("    \"validation\": \"string\",");
            sb.AppendLine("    \"maxLength\": 100");
            sb.AppendLine("  }");
            sb.AppendLine("]");
            sb.AppendLine("```");
        }
        
        return sb.ToString();
    }

    private void GenerateFieldMarkup(StringBuilder sb, FormField field)
    {
        switch (field.Type?.ToLower())
        {
            case "select":
                sb.AppendLine($"        <gcds-select");
                sb.AppendLine($"            select-id=\"{field.Name}\"");
                sb.AppendLine($"            name=\"{field.Name}\"");
                sb.AppendLine($"            label=\"{field.Label}\"");
                if (field.Required) sb.AppendLine($"            required=\"true\"");
                if (!string.IsNullOrEmpty(field.Hint)) sb.AppendLine($"            hint=\"{field.Hint}\"");
                sb.AppendLine($"            asp-for=\"{ToPascalCase(field.Name)}\"");
                sb.AppendLine($"            asp-items=\"@(new SelectList(ViewBag.{ToPascalCase(field.Name)}Options, \"Value\", \"Text\"))\">");
                sb.AppendLine($"        </gcds-select>");
                break;
                
            case "textarea":
                sb.AppendLine($"        <gcds-textarea");
                sb.AppendLine($"            textarea-id=\"{field.Name}\"");
                sb.AppendLine($"            name=\"{field.Name}\"");
                sb.AppendLine($"            label=\"{field.Label}\"");
                sb.AppendLine($"            rows=\"{(field.Rows > 0 ? field.Rows : 4)}\"");
                if (field.Required) sb.AppendLine($"            required=\"true\"");
                if (!string.IsNullOrEmpty(field.Hint)) sb.AppendLine($"            hint=\"{field.Hint}\"");
                if (field.MaxLength > 0) sb.AppendLine($"            character-limit=\"{field.MaxLength}\"");
                sb.AppendLine($"            asp-for=\"{ToPascalCase(field.Name)}\">");
                sb.AppendLine($"        </gcds-textarea>");
                break;
                
            case "checkbox":
                sb.AppendLine($"        <fdcp-checkbox");
                sb.AppendLine($"            name=\"{field.Name}\"");
                sb.AppendLine($"            checkbox-id=\"{field.Name}\"");
                sb.AppendLine($"            label=\"{field.Label}\"");
                sb.AppendLine($"            value=\"true\"");
                if (field.Required) sb.AppendLine($"            required=\"true\"");
                if (!string.IsNullOrEmpty(field.Hint)) sb.AppendLine($"            hint=\"{field.Hint}\"");
                sb.AppendLine($"            asp-for=\"{ToPascalCase(field.Name)}\">");
                sb.AppendLine($"        </fdcp-checkbox>");
                break;
                
            case "date":
                sb.AppendLine($"        <gcds-date-input");
                sb.AppendLine($"            name=\"{field.Name}\"");
                sb.AppendLine($"            legend=\"{field.Label}\"");
                sb.AppendLine($"            format=\"full\"");
                if (field.Required) sb.AppendLine($"            required=\"true\"");
                if (!string.IsNullOrEmpty(field.Hint)) sb.AppendLine($"            hint=\"{field.Hint}\"");
                sb.AppendLine($"            asp-for=\"{ToPascalCase(field.Name)}\">");
                sb.AppendLine($"        </gcds-date-input>");
                break;
                
            case "file":
                sb.AppendLine($"        <gcds-file-upload");
                sb.AppendLine($"            uploader-id=\"{field.Name}\"");
                sb.AppendLine($"            name=\"{field.Name}\"");
                sb.AppendLine($"            label=\"{field.Label}\"");
                if (!string.IsNullOrEmpty(field.Accept)) sb.AppendLine($"            accept=\"{field.Accept}\"");
                if (field.Multiple) sb.AppendLine($"            multiple=\"true\"");
                if (field.Required) sb.AppendLine($"            required=\"true\"");
                if (!string.IsNullOrEmpty(field.Hint)) sb.AppendLine($"            hint=\"{field.Hint}\"");
                sb.AppendLine($"            asp-for=\"{ToPascalCase(field.Name)}\">");
                sb.AppendLine($"        </gcds-file-upload>");
                break;
                
            default: // text, email, password, tel, url, number
                sb.AppendLine($"        <gcds-input");
                sb.AppendLine($"            input-id=\"{field.Name}\"");
                sb.AppendLine($"            name=\"{field.Name}\"");
                sb.AppendLine($"            label=\"{field.Label}\"");
                sb.AppendLine($"            type=\"{field.Type ?? "text"}\"");
                if (field.Required) sb.AppendLine($"            required=\"true\"");
                if (!string.IsNullOrEmpty(field.Placeholder)) sb.AppendLine($"            placeholder=\"{field.Placeholder}\"");
                if (!string.IsNullOrEmpty(field.Hint)) sb.AppendLine($"            hint=\"{field.Hint}\"");
                sb.AppendLine($"            asp-for=\"{ToPascalCase(field.Name)}\"");
                sb.AppendLine($"            asp-validation-for=\"{ToPascalCase(field.Name)}\">");
                sb.AppendLine($"        </gcds-input>");
                break;
        }
        sb.AppendLine();
    }

    private string ToPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return char.ToUpper(input[0]) + input.Substring(1);
    }

    private string SplitPascalCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return input;
        return System.Text.RegularExpressions.Regex.Replace(input, "(\\B[A-Z])", " $1");
    }



    [McpServerTool]
    [Description("Generates an advanced multi-step form with conditional fields and complex validation.")]
    public string GenerateAdvancedFormSolution(
        [Description("Form name (e.g., 'JobApplication', 'GrantRequest')")] string formName,
        [Description("Form steps in JSON format: [{\"stepName\":\"Personal\",\"title\":\"Personal Information\",\"fields\":[...]}]")] string stepsJson,
        [Description("Controller namespace")] string controllerNamespace = "YourApp.Controllers",
        [Description("Model namespace")] string modelNamespace = "YourApp.Models",
        [Description("Include conditional field logic")] bool includeConditionalFields = true,
        [Description("Include progress tracking")] bool includeProgressTracking = true,
        [Description("Session storage for multi-step data")] bool useSessionStorage = true)
    {
        var sb = new StringBuilder();
        var modelClassName = $"{formName}Model";
        var controllerClassName = $"{formName}Controller";
        
        try
        {
            var steps = JsonSerializer.Deserialize<FormStep[]>(stepsJson);
            
            sb.AppendLine("# Advanced Multi-Step Form Solution");
            sb.AppendLine($"**Form Name**: {formName}");
            sb.AppendLine($"**Steps**: {steps?.Length ?? 0}");
            sb.AppendLine($"**Features**: Multi-step navigation, Conditional fields, Progress tracking");
            sb.AppendLine();
            
            // Generate comprehensive model with validation
            sb.AppendLine("## 1. Model Classes");
            sb.AppendLine($"**File**: `Models/{modelClassName}.cs`");
            sb.AppendLine("```csharp");
            sb.AppendLine($"using System.ComponentModel.DataAnnotations;");
            sb.AppendLine($"using GCFoundation.Components.Validation;");
            sb.AppendLine($"using GCFoundation.Components.Models;");
            sb.AppendLine();
            sb.AppendLine($"namespace {modelNamespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {modelClassName} : BaseViewModel");
            sb.AppendLine("    {");
            sb.AppendLine("        public int CurrentStep { get; set; } = 1;");
            sb.AppendLine($"        public int TotalSteps {{ get; set; }} = {steps.Length};");
            sb.AppendLine("        public List<string> CompletedSteps { get; set; } = new();");
            sb.AppendLine();
            
            // Generate properties for all fields across all steps
            foreach (var step in steps)
            {
                sb.AppendLine($"        // {step.Title} Fields");
                if (step.Fields != null)
                {
                    foreach (var field in step.Fields)
                    {
                        GenerateModelProperty(sb, field);
                    }
                }
                sb.AppendLine();
            }
            
            sb.AppendLine("        public bool CanNavigateToStep(int stepNumber)");
            sb.AppendLine("        {");
            sb.AppendLine("            return stepNumber <= CurrentStep || CompletedSteps.Contains($\"Step{stepNumber}\");");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        public void MarkStepCompleted(int stepNumber)");
            sb.AppendLine("        {");
            sb.AppendLine("            var stepKey = $\"Step{stepNumber}\";");
            sb.AppendLine("            if (!CompletedSteps.Contains(stepKey))");
            sb.AppendLine("            {");
            sb.AppendLine("                CompletedSteps.Add(stepKey);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine();
            
            // Generate step-specific validation models
            foreach (var step in steps)
            {
                sb.AppendLine($"    public class {step.StepName}StepModel");
                sb.AppendLine("    {");
                if (step.Fields != null)
                {
                    foreach (var field in step.Fields)
                    {
                        GenerateModelProperty(sb, field);
                    }
                }
                sb.AppendLine("    }");
                sb.AppendLine();
            }
            
            sb.AppendLine("}");
            sb.AppendLine("```");
            sb.AppendLine();
            
            // Generate advanced controller with step management
            sb.AppendLine("## 2. Advanced Controller");
            sb.AppendLine($"**File**: `Controllers/{controllerClassName}.cs`");
            sb.AppendLine("```csharp");
            sb.AppendLine($"using Microsoft.AspNetCore.Mvc;");
            sb.AppendLine($"using GCFoundation.Components.Controllers;");
            sb.AppendLine($"using {modelNamespace};");
            sb.AppendLine($"using System.Text.Json;");
            sb.AppendLine();
            sb.AppendLine($"namespace {controllerNamespace}");
            sb.AppendLine("{");
            sb.AppendLine($"    public class {controllerClassName} : GCFoundationBaseController");
            sb.AppendLine("    {");
            sb.AppendLine($"        private readonly ILogger<{controllerClassName}> _logger;");
            if (useSessionStorage)
            {
                sb.AppendLine("        private const string SessionKey = \"" + formName + "FormData\";");
            }
            sb.AppendLine();
            sb.AppendLine($"        public {controllerClassName}(ILogger<{controllerClassName}> logger)");
            sb.AppendLine("        {");
            sb.AppendLine("            _logger = logger;");
            sb.AppendLine("        }");
            sb.AppendLine();
            
            // Generate action methods for each step
            foreach (var (step, index) in steps.Select((step, i) => (step, i + 1)))
            {
                sb.AppendLine($"        [HttpGet]");
                sb.AppendLine($"        public IActionResult {step.StepName}(int stepNumber = {index})");
                sb.AppendLine("        {");
                if (useSessionStorage)
                {
                    sb.AppendLine($"            var model = GetModelFromSession() ?? new {modelClassName}();");
                }
                else
                {
                    sb.AppendLine($"            var model = new {modelClassName}();");
                }
                sb.AppendLine($"            model.CurrentStep = stepNumber;");
                sb.AppendLine();
                sb.AppendLine("            if (!model.CanNavigateToStep(stepNumber))");
                sb.AppendLine("            {");
                sb.AppendLine("                return RedirectToAction(nameof(Step1));");
                sb.AppendLine("            }");
                sb.AppendLine();
                sb.AppendLine($"            ViewBag.StepName = \"{step.StepName}\";");
                sb.AppendLine($"            ViewBag.StepTitle = \"{step.Title}\";");
                sb.AppendLine("            return View(\"Index\", model);");
                sb.AppendLine("        }");
                sb.AppendLine();
                
                sb.AppendLine($"        [HttpPost]");
                sb.AppendLine($"        public IActionResult {step.StepName}({modelClassName} model)");
                sb.AppendLine("        {");
                sb.AppendLine($"            model.CurrentStep = {index};");
                sb.AppendLine($"            ViewBag.StepName = \"{step.StepName}\";");
                sb.AppendLine($"            ViewBag.StepTitle = \"{step.Title}\";");
                sb.AppendLine();
                sb.AppendLine("            // Validate only current step fields");
                sb.AppendLine($"            if (!ValidateStep{step.StepName}(model))");
                sb.AppendLine("            {");
                sb.AppendLine("                return View(\"Index\", model);");
                sb.AppendLine("            }");
                sb.AppendLine();
                sb.AppendLine($"            model.MarkStepCompleted({index});");
                if (useSessionStorage)
                {
                    sb.AppendLine("            SaveModelToSession(model);");
                }
                sb.AppendLine();
                if (index < steps.Length)
                {
                    sb.AppendLine($"            return RedirectToAction(\"{steps[index].StepName}\", new {{ stepNumber = {index + 1} }});");
                }
                else
                {
                    sb.AppendLine("            return RedirectToAction(nameof(Review));");
                }
                sb.AppendLine("        }");
                sb.AppendLine();
            }
            
            // Add review and submit methods
            sb.AppendLine("        [HttpGet]");
            sb.AppendLine("        public IActionResult Review()");
            sb.AppendLine("        {");
            if (useSessionStorage)
            {
                sb.AppendLine("            var model = GetModelFromSession();");
                sb.AppendLine("            if (model == null)");
                sb.AppendLine("            {");
                sb.AppendLine("                return RedirectToAction(nameof(Step1));");
                sb.AppendLine("            }");
            }
            else
            {
                sb.AppendLine($"            var model = new {modelClassName}();");
            }
            sb.AppendLine("            ViewBag.IsReview = true;");
            sb.AppendLine("            return View(\"Review\", model);");
            sb.AppendLine("        }");
            sb.AppendLine();
            
            sb.AppendLine("        [HttpPost]");
            sb.AppendLine("        public async Task<IActionResult> Submit()");
            sb.AppendLine("        {");
            if (useSessionStorage)
            {
                sb.AppendLine("            var model = GetModelFromSession();");
                sb.AppendLine("            if (model == null)");
                sb.AppendLine("            {");
                sb.AppendLine("                return RedirectToAction(nameof(Step1));");
                sb.AppendLine("            }");
            }
            else
            {
                sb.AppendLine($"            // Model would need to be reconstructed from form data");
                sb.AppendLine($"            var model = new {modelClassName}();");
            }
            sb.AppendLine();
            sb.AppendLine("            try");
            sb.AppendLine("            {");
            sb.AppendLine($"                // Process the complete {formName.ToLower()} submission");
            sb.AppendLine("                _logger.LogInformation($\"Processing {formName} submission for user {User.Identity?.Name}\");");
            sb.AppendLine();
            sb.AppendLine("                // Add your business logic here");
            sb.AppendLine("                // await _yourService.ProcessCompleteFormAsync(model);");
            sb.AppendLine();
            if (useSessionStorage)
            {
                sb.AppendLine("                // Clear session data after successful submission");
                sb.AppendLine("                HttpContext.Session.Remove(SessionKey);");
            }
            sb.AppendLine();
            sb.AppendLine("                return RedirectToAction(\"Success\");");
            sb.AppendLine("            }");
            sb.AppendLine("            catch (Exception ex)");
            sb.AppendLine("            {");
            sb.AppendLine($"                _logger.LogError(ex, \"Error processing {formName} form submission\");");
            sb.AppendLine("                ModelState.AddModelError(\"\", \"An error occurred while processing your request.\");");
            sb.AppendLine("                return View(\"Review\", model);");
            sb.AppendLine("            }");
            sb.AppendLine("        }");
            sb.AppendLine();
            
            // Add validation methods for each step
            foreach (var step in steps)
            {
                sb.AppendLine($"        private bool ValidateStep{step.StepName}({modelClassName} model)");
                sb.AppendLine("        {");
                sb.AppendLine("            var isValid = true;");
                sb.AppendLine("            ModelState.Clear();");
                sb.AppendLine();
                sb.AppendLine($"            // Add validation logic for {step.StepName} step");
                if (step.Fields != null)
                {
                    foreach (var field in step.Fields.Where(f => f.Required))
                    {
                        sb.AppendLine($"            if (string.IsNullOrEmpty(model.{ToPascalCase(field.Name)}?.ToString()))");
                        sb.AppendLine("            {");
                        sb.AppendLine($"                ModelState.AddModelError(\"{ToPascalCase(field.Name)}\", \"The {field.Label} field is required.\");");
                        sb.AppendLine("                isValid = false;");
                        sb.AppendLine("            }");
                    }
                }
                sb.AppendLine();
                sb.AppendLine("            return isValid;");
                sb.AppendLine("        }");
                sb.AppendLine();
            }
            
            if (useSessionStorage)
            {
                // Add session management methods
                sb.AppendLine($"        private {modelClassName}? GetModelFromSession()");
                sb.AppendLine("        {");
                sb.AppendLine("            var sessionData = HttpContext.Session.GetString(SessionKey);");
                sb.AppendLine("            if (string.IsNullOrEmpty(sessionData))");
                sb.AppendLine("                return null;");
                sb.AppendLine();
                sb.AppendLine($"            return JsonSerializer.Deserialize<{modelClassName}>(sessionData);");
                sb.AppendLine("        }");
                sb.AppendLine();
                sb.AppendLine($"        private void SaveModelToSession({modelClassName} model)");
                sb.AppendLine("        {");
                sb.AppendLine("            var sessionData = JsonSerializer.Serialize(model);");
                sb.AppendLine("            HttpContext.Session.SetString(SessionKey, sessionData);");
                sb.AppendLine("        }");
                sb.AppendLine();
            }
            
            sb.AppendLine("        public IActionResult Success()");
            sb.AppendLine("        {");
            sb.AppendLine("            return View();");
            sb.AppendLine("        }");
            sb.AppendLine("    }");
            sb.AppendLine("}");
            sb.AppendLine("```");
            sb.AppendLine();
            
            // Generate main view with step navigation
            sb.AppendLine("## 3. Multi-Step View");
            sb.AppendLine($"**File**: `Views/{formName}/Index.cshtml`");
            sb.AppendLine("```html");
            sb.AppendLine($"@model {modelNamespace}.{modelClassName}");
            sb.AppendLine("@{");
            sb.AppendLine($"    ViewData[\"Title\"] = ViewBag.StepTitle ?? \"{formName}\";");
            sb.AppendLine("    var stepName = ViewBag.StepName as string;");
            sb.AppendLine("}");
            sb.AppendLine();
            
            if (includeProgressTracking)
            {
                sb.AppendLine("<!-- Progress Stepper -->");
                sb.AppendLine("<fdcp-stepper current-step=\"@Model.CurrentStep\" orientation=\"horizontal\">");
                foreach (var (step, index) in steps.Select((step, i) => (step, i + 1)))
                {
                    sb.AppendLine($"    <fdcp-step title=\"{step.Title}\" description=\"Step {index}\"></fdcp-step>");
                }
                sb.AppendLine("</fdcp-stepper>");
                sb.AppendLine();
            }
            
            sb.AppendLine($"<gcds-heading tag=\"h1\">@ViewBag.StepTitle</gcds-heading>");
            sb.AppendLine();
            sb.AppendLine("@if (!ViewData.ModelState.IsValid)");
            sb.AppendLine("{");
            sb.AppendLine("    <gcds-error-summary>");
            sb.AppendLine("        <gcds-heading tag=\"h2\" size=\"h3\">There are errors on this page</gcds-heading>");
            sb.AppendLine("        <ul>");
            sb.AppendLine("            @foreach (var error in ViewData.ModelState)");
            sb.AppendLine("            {");
            sb.AppendLine("                @foreach (var errorMessage in error.Value.Errors)");
            sb.AppendLine("                {");
            sb.AppendLine("                    <li><a href=\"#@error.Key\">@errorMessage.ErrorMessage</a></li>");
            sb.AppendLine("                }");
            sb.AppendLine("            }");
            sb.AppendLine("        </ul>");
            sb.AppendLine("    </gcds-error-summary>");
            sb.AppendLine("}");
            sb.AppendLine();
            
            sb.AppendLine($"<form asp-action=\"@stepName\" method=\"post\" novalidate>");
            sb.AppendLine("    @Html.AntiForgeryToken()");
            sb.AppendLine("    @Html.HiddenFor(m => m.CurrentStep)");
            sb.AppendLine("    @Html.HiddenFor(m => m.TotalSteps)");
            sb.AppendLine();
            
            // Generate conditional step content
            foreach (var (step, index) in steps.Select((step, i) => (step, i + 1)))
            {
                sb.AppendLine($"    @if (Model.CurrentStep == {index})");
                sb.AppendLine("    {");
                sb.AppendLine($"        <gcds-fieldset fieldset-id=\"step{index}\" legend=\"{step.Title}\" legend-size=\"h2\">");
                
                if (step.Fields != null)
                {
                    foreach (var field in step.Fields)
                    {
                        GenerateAdvancedFieldMarkup(sb, field, includeConditionalFields);
                    }
                }
                
                sb.AppendLine("        </gcds-fieldset>");
                sb.AppendLine("    }");
                sb.AppendLine();
            }
            
            sb.AppendLine("    <!-- Navigation Buttons -->");
            sb.AppendLine("    <div class=\"form-navigation\">");
            sb.AppendLine("        @if (Model.CurrentStep > 1)");
            sb.AppendLine("        {");
            sb.AppendLine($"            <gcds-button type=\"button\" variant=\"secondary\" onclick=\"window.location.href='@Url.Action(\"Step\" + (Model.CurrentStep - 1))'\">Previous</gcds-button>");
            sb.AppendLine("        }");
            sb.AppendLine();
            sb.AppendLine("        @if (Model.CurrentStep < Model.TotalSteps)");
            sb.AppendLine("        {");
            sb.AppendLine("            <gcds-button type=\"submit\" variant=\"primary\">Next</gcds-button>");
            sb.AppendLine("        }");
            sb.AppendLine("        else");
            sb.AppendLine("        {");
            sb.AppendLine("            <gcds-button type=\"submit\" variant=\"primary\">Review</gcds-button>");
            sb.AppendLine("        }");
            sb.AppendLine("    </div>");
            sb.AppendLine("</form>");
            sb.AppendLine("```");
            sb.AppendLine();
            
            // Generate review view
            sb.AppendLine("## 4. Review View");
            sb.AppendLine($"**File**: `Views/{formName}/Review.cshtml`");
            sb.AppendLine("```html");
            sb.AppendLine($"@model {modelNamespace}.{modelClassName}");
            sb.AppendLine("@{");
            sb.AppendLine($"    ViewData[\"Title\"] = \"Review Your {SplitPascalCase(formName)}\";");
            sb.AppendLine("}");
            sb.AppendLine();
            sb.AppendLine($"<gcds-heading tag=\"h1\">Review Your {SplitPascalCase(formName)}</gcds-heading>");
            sb.AppendLine();
            sb.AppendLine("<gcds-text>Please review your information before submitting.</gcds-text>");
            sb.AppendLine();
            
            foreach (var (step, index) in steps.Select((step, i) => (step, i + 1)))
            {
                sb.AppendLine($"<gcds-details summary=\"{step.Title}\" open=\"true\">");
                if (step.Fields != null)
                {
                    foreach (var field in step.Fields)
                    {
                        sb.AppendLine("    <div class=\"review-field\">");
                        sb.AppendLine($"        <strong>{field.Label}:</strong>");
                        sb.AppendLine($"        @(Model.{ToPascalCase(field.Name)} ?? \"Not provided\")");
                        sb.AppendLine("    </div>");
                    }
                }
                sb.AppendLine("</gcds-details>");
                sb.AppendLine();
            }
            
            sb.AppendLine("<form asp-action=\"Submit\" method=\"post\">");
            sb.AppendLine("    @Html.AntiForgeryToken()");
            sb.AppendLine("    <div class=\"form-navigation\">");
            sb.AppendLine($"        <gcds-button type=\"button\" variant=\"secondary\" onclick=\"window.location.href='@Url.Action(\"Step\" + Model.TotalSteps)'\">Back to Edit</gcds-button>");
            sb.AppendLine("        <gcds-button type=\"submit\" variant=\"primary\">Submit Application</gcds-button>");
            sb.AppendLine("    </div>");
            sb.AppendLine("</form>");
            sb.AppendLine("```");
            sb.AppendLine();
            
            // Implementation notes
            sb.AppendLine("## 5. Implementation Notes");
            sb.AppendLine();
            sb.AppendLine("### Features Included:");
            sb.AppendLine("- **Multi-step navigation** with progress tracking");
            sb.AppendLine("- **Session persistence** to maintain data between steps");
            sb.AppendLine("- **Step-by-step validation** for better user experience");
            sb.AppendLine("- **Review page** before final submission");
            sb.AppendLine("- **Conditional field support** based on user responses");
            sb.AppendLine("- **Progress stepper** component for visual feedback");
            sb.AppendLine();
            sb.AppendLine("### Required Configuration:");
            sb.AppendLine("```csharp");
            sb.AppendLine("// In Program.cs, enable session support:");
            sb.AppendLine("builder.Services.AddSession(options =>");
            sb.AppendLine("{");
            sb.AppendLine("    options.IdleTimeout = TimeSpan.FromMinutes(30);");
            sb.AppendLine("    options.Cookie.HttpOnly = true;");
            sb.AppendLine("    options.Cookie.IsEssential = true;");
            sb.AppendLine("});");
            sb.AppendLine();
            sb.AppendLine("// Use session middleware:");
            sb.AppendLine("app.UseSession();");
            sb.AppendLine("```");
            
        }
        catch (JsonException)
        {
            sb.AppendLine("Error: Invalid JSON format for steps. Please provide steps in the correct format:");
            sb.AppendLine("```json");
            sb.AppendLine("[");
            sb.AppendLine("  {");
            sb.AppendLine("    \"stepName\": \"Personal\",");
            sb.AppendLine("    \"title\": \"Personal Information\",");
            sb.AppendLine("    \"fields\": [");
            sb.AppendLine("      {");
            sb.AppendLine("        \"name\": \"firstName\",");
            sb.AppendLine("        \"type\": \"text\",");
            sb.AppendLine("        \"label\": \"First Name\",");
            sb.AppendLine("        \"required\": true");
            sb.AppendLine("      }");
            sb.AppendLine("    ]");
            sb.AppendLine("  }");
            sb.AppendLine("]");
            sb.AppendLine("```");
        }
        
        return sb.ToString();
    }

    private void GenerateModelProperty(StringBuilder sb, FormField field)
    {
        // Add validation attributes
        if (field.Required)
        {
            sb.AppendLine($"        [Required(ErrorMessage = \"The {field.Label} field is required.\")]");
        }
        
        switch (field.Validation?.ToLower())
        {
            case "email":
                sb.AppendLine($"        [EmailAddress(ErrorMessage = \"Please enter a valid email address.\")]");
                break;
            case "phone":
                sb.AppendLine($"        [RegularExpression(@\"^(\\+1-?)?(\\()?[0-9]{{3}}(\\))?(-|\\s)?[0-9]{{3}}(-|\\s)?[0-9]{{4}}$\", ErrorMessage = \"Please enter a valid phone number.\")]");
                break;
            case "postal-code":
                sb.AppendLine($"        [RegularExpression(@\"^[A-Za-z]\\d[A-Za-z][ -]?\\d[A-Za-z]\\d$\", ErrorMessage = \"Please enter a valid postal code.\")]");
                break;
            case "url":
                sb.AppendLine($"        [Url(ErrorMessage = \"Please enter a valid URL.\")]");
                break;
        }
        
        if (field.MaxLength > 0)
        {
            sb.AppendLine($"        [MaxLength({field.MaxLength}, ErrorMessage = \"The {field.Label} cannot exceed {field.MaxLength} characters.\")]");
        }
        
        sb.AppendLine($"        [Display(Name = \"{field.Label}\")]");
        
        // Determine property type
        var propertyType = field.Type?.ToLower() switch
        {
            "email" => "string",
            "password" => "string",
            "tel" => "string",
            "url" => "string",
            "number" => "int",
            "date" => "DateTime?",
            "checkbox" => "bool",
            "file" => "IFormFile",
            _ => "string"
        };
        
        sb.AppendLine($"        public {propertyType}? {ToPascalCase(field.Name)} {{ get; set; }}");
    }

    private void GenerateAdvancedFieldMarkup(StringBuilder sb, FormField field, bool includeConditionalFields)
    {
        // Add conditional logic wrapper if needed
        if (includeConditionalFields && !string.IsNullOrEmpty(field.ConditionalOn))
        {
            sb.AppendLine($"        <div data-conditional-field=\"{field.ConditionalOn}\" data-conditional-value=\"{field.ConditionalValue}\" style=\"display: none;\">");
        }
        
        // Generate the actual field markup (reuse existing method)
        GenerateFieldMarkup(sb, field);
        
        if (includeConditionalFields && !string.IsNullOrEmpty(field.ConditionalOn))
        {
            sb.AppendLine("        </div>");
        }
        
        // Add JavaScript for conditional logic
        if (includeConditionalFields && !string.IsNullOrEmpty(field.ConditionalOn))
        {
            sb.AppendLine("        <script>");
            sb.AppendLine($"        document.addEventListener('DOMContentLoaded', function() {{");
            sb.AppendLine($"            const triggerField = document.getElementById('{field.ConditionalOn}');");
            sb.AppendLine($"            const conditionalDiv = document.querySelector('[data-conditional-field=\"{field.ConditionalOn}\"]');");
            sb.AppendLine($"            ");
            sb.AppendLine($"            function toggleConditionalField() {{");
            sb.AppendLine($"                if (triggerField.value === '{field.ConditionalValue}') {{");
            sb.AppendLine($"                    conditionalDiv.style.display = 'block';");
            sb.AppendLine($"                }} else {{");
            sb.AppendLine($"                    conditionalDiv.style.display = 'none';");
            sb.AppendLine($"                }}");
            sb.AppendLine($"            }}");
            sb.AppendLine($"            ");
            sb.AppendLine($"            triggerField.addEventListener('change', toggleConditionalField);");
            sb.AppendLine($"            toggleConditionalField(); // Initial check");
            sb.AppendLine($"        }});");
            sb.AppendLine("        </script>");
        }
    }

    // Helper class for multi-step forms
    private class FormStep
    {
        public string StepName { get; set; } = "";
        public string Title { get; set; } = "";
        public FormField[]? Fields { get; set; }
    }

    // Enhanced FormField class for advanced features
    private class FormField
    {
        public string Name { get; set; } = "";
        public string Type { get; set; } = "text";
        public string Label { get; set; } = "";
        public bool Required { get; set; } = false;
        public string? Validation { get; set; }
        public int MaxLength { get; set; } = 0;
        public string? Placeholder { get; set; }
        public string? Hint { get; set; }
        public int Rows { get; set; } = 0;
        public bool Multiple { get; set; } = false;
        public string? Accept { get; set; }
        // Conditional field properties
        public string? ConditionalOn { get; set; }
        public string? ConditionalValue { get; set; }
    }

    [McpServerTool]
    [Description("Provides comprehensive guidance on GC Foundation component usage and patterns.")]
    public string GetGCFoundationGuidance(
        [Description("Guidance topic: getting-started, components, forms, accessibility, localization, deployment, best-practices")] string topic = "getting-started")
    {
        var sb = new StringBuilder();
        
        switch (topic.ToLower())
        {
            case "getting-started":
                sb.AppendLine("## Getting Started with GC Foundation");
                sb.AppendLine();
                sb.AppendLine("### 1. Project Setup");
                sb.AppendLine("- Install GCFoundation.Common, GCFoundation.Components, and GCFoundation.Security packages");
                sb.AppendLine("- Configure Program.cs with Foundation middleware and services");
                sb.AppendLine("- Set up appsettings.json with Foundation configuration");
                sb.AppendLine();
                sb.AppendLine("### 2. Basic Usage");
                sb.AppendLine("- Use `<gcds-*>` components for Government of Canada Design System compliance");
                sb.AppendLine("- Use `<fdcp-*>` components for advanced functionality");
                sb.AppendLine("- Follow accessibility guidelines and semantic HTML structure");
                sb.AppendLine();
                sb.AppendLine("### 3. Next Steps");
                sb.AppendLine("- Implement forms using GCDS form components");
                sb.AppendLine("- Set up localization for bilingual applications");
                sb.AppendLine("- Configure content security policies for production");
                break;
                
            case "best-practices":
                sb.AppendLine("## GC Foundation Best Practices");
                sb.AppendLine();
                sb.AppendLine("### Component Usage");
                sb.AppendLine("- Always use GCDS components instead of custom HTML elements");
                sb.AppendLine("- Maintain consistent spacing using GCDS margin and padding utilities");
                sb.AppendLine("- Use semantic component properties rather than CSS classes");
                sb.AppendLine();
                sb.AppendLine("### Accessibility");
                sb.AppendLine("- Ensure all form inputs have proper labels and hints");
                sb.AppendLine("- Use heading hierarchy correctly (only one H1 per page)");
                sb.AppendLine("- Include error summaries in forms for screen reader users");
                sb.AppendLine();
                sb.AppendLine("### Performance");
                sb.AppendLine("- Enable GCDS CDN resources in production");
                sb.AppendLine("- Use async operations for form submissions");
                sb.AppendLine("- Implement proper caching strategies");
                break;
                
            default:
                sb.AppendLine("## Available Guidance Topics");
                sb.AppendLine("- **getting-started**: Initial setup and basic usage");
                sb.AppendLine("- **components**: Component selection and usage patterns");
                sb.AppendLine("- **forms**: Form building and validation best practices");
                sb.AppendLine("- **accessibility**: WCAG compliance and inclusive design");
                sb.AppendLine("- **localization**: Bilingual application setup");
                sb.AppendLine("- **deployment**: Production deployment considerations");
                sb.AppendLine("- **best-practices**: Comprehensive development guidelines");
                break;
        }
        
        return sb.ToString();
    }
}

