using System.ComponentModel;
using System.Text;
using ModelContextProtocol.Server;

/// <summary>
/// Comprehensive MCP tools for all FDCP (Federal Design Component Package) components.
/// Provides generation capabilities for advanced components and specialized functionality.
/// </summary>
public class ComprehensiveFDCPTools
{
    #region Advanced Form Components

    [McpServerTool]
    [Description("Generates a comprehensive FDCP form builder with dynamic sections and validation.")]
    public string GenerateFDCPFormBuilder(
        [Description("Form action URL")] string action,
        [Description("Form method: get, post")] string method = "post",
        [Description("Form title")] string? title = null,
        [Description("Submit button text")] string submitText = "Submit",
        [Description("Include error summary")] bool errorSummary = true,
        [Description("Form CSS classes")] string? cssClass = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-form");
        sb.Append($" action=\"{action}\"");
        sb.Append($" method=\"{method}\"");
        
        if (!string.IsNullOrEmpty(title))
            sb.Append($" title=\"{title}\"");
        
        sb.Append($" submit-text=\"{submitText}\"");
        
        if (errorSummary)
            sb.Append(" error-summary=\"true\"");
        
        if (!string.IsNullOrEmpty(cssClass))
            sb.Append($" class=\"{cssClass}\"");
        
        sb.AppendLine(">");
        sb.AppendLine("  <!-- Form definition JSON or form sections go here -->");
        sb.Append("</fdcp-form>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates FDCP advanced input component with enhanced validation and styling.")]
    public string GenerateFDCPInput(
        [Description("Input name and ID")] string name,
        [Description("Input label")] string label,
        [Description("Input type: text, email, password, tel, url, number, search")] string type = "text",
        [Description("Input placeholder")] string? placeholder = null,
        [Description("Whether field is required")] bool required = false,
        [Description("Whether field is disabled")] bool disabled = false,
        [Description("Help text")] string? hint = null,
        [Description("Error message")] string? errorMessage = null,
        [Description("Input size: small, regular, large")] string size = "regular",
        [Description("Validation pattern (regex)")] string? pattern = null,
        [Description("Minimum length")] int? minLength = null,
        [Description("Maximum length")] int? maxLength = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-input");
        sb.Append($" name=\"{name}\"");
        sb.Append($" input-id=\"{name}\"");
        sb.Append($" label=\"{label}\"");
        sb.Append($" type=\"{type}\"");
        sb.Append($" size=\"{size}\"");
        
        if (!string.IsNullOrEmpty(placeholder))
            sb.Append($" placeholder=\"{placeholder}\"");
        
        if (required)
            sb.Append(" required=\"true\"");
        
        if (disabled)
            sb.Append(" disabled=\"true\"");
        
        if (!string.IsNullOrEmpty(hint))
            sb.Append($" hint=\"{hint}\"");
        
        if (!string.IsNullOrEmpty(errorMessage))
            sb.Append($" error-message=\"{errorMessage}\"");
        
        if (!string.IsNullOrEmpty(pattern))
            sb.Append($" pattern=\"{pattern}\"");
        
        if (minLength.HasValue)
            sb.Append($" min-length=\"{minLength}\"");
        
        if (maxLength.HasValue)
            sb.Append($" max-length=\"{maxLength}\"");
        
        sb.Append("></fdcp-input>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates FDCP select component with enhanced options and search capability.")]
    public string GenerateFDCPSelect(
        [Description("Select name and ID")] string name,
        [Description("Select label")] string label,
        [Description("Options in JSON format: [{\"value\":\"1\",\"label\":\"Option 1\"}]")] string optionsJson,
        [Description("Default selected value")] string? defaultValue = null,
        [Description("Whether field is required")] bool required = false,
        [Description("Whether field is disabled")] bool disabled = false,
        [Description("Help text")] string? hint = null,
        [Description("Error message")] string? errorMessage = null,
        [Description("Enable search/filter functionality")] bool searchable = false,
        [Description("Multiple selection allowed")] bool multiple = false)
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-select");
        sb.Append($" name=\"{name}\"");
        sb.Append($" select-id=\"{name}\"");
        sb.Append($" label=\"{label}\"");
        sb.Append($" options='{optionsJson}'");
        
        if (!string.IsNullOrEmpty(defaultValue))
            sb.Append($" default-value=\"{defaultValue}\"");
        
        if (required)
            sb.Append(" required=\"true\"");
        
        if (disabled)
            sb.Append(" disabled=\"true\"");
        
        if (!string.IsNullOrEmpty(hint))
            sb.Append($" hint=\"{hint}\"");
        
        if (!string.IsNullOrEmpty(errorMessage))
            sb.Append($" error-message=\"{errorMessage}\"");
        
        if (searchable)
            sb.Append(" searchable=\"true\"");
        
        if (multiple)
            sb.Append(" multiple=\"true\"");
        
        sb.Append("></fdcp-select>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates FDCP checkbox component with enhanced styling and validation.")]
    public string GenerateFDCPCheckbox(
        [Description("Checkbox name and ID")] string name,
        [Description("Checkbox label")] string label,
        [Description("Checkbox value")] string value,
        [Description("Whether checkbox is checked by default")] bool checked_ = false,
        [Description("Whether checkbox is required")] bool required = false,
        [Description("Whether checkbox is disabled")] bool disabled = false,
        [Description("Help text")] string? hint = null,
        [Description("Error message")] string? errorMessage = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-checkbox");
        sb.Append($" name=\"{name}\"");
        sb.Append($" checkbox-id=\"{name}\"");
        sb.Append($" label=\"{label}\"");
        sb.Append($" value=\"{value}\"");
        
        if (checked_)
            sb.Append(" checked=\"true\"");
        
        if (required)
            sb.Append(" required=\"true\"");
        
        if (disabled)
            sb.Append(" disabled=\"true\"");
        
        if (!string.IsNullOrEmpty(hint))
            sb.Append($" hint=\"{hint}\"");
        
        if (!string.IsNullOrEmpty(errorMessage))
            sb.Append($" error-message=\"{errorMessage}\"");
        
        sb.Append("></fdcp-checkbox>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates FDCP checkboxes group component with multiple options.")]
    public string GenerateFDCPCheckboxes(
        [Description("Checkbox group name")] string name,
        [Description("Checkbox group legend")] string legend,
        [Description("Options in JSON format: [{\"id\":\"1\",\"label\":\"Option 1\",\"value\":\"1\"}]")] string optionsJson,
        [Description("Legend size: h1, h2, h3, h4, h5, h6")] string legendSize = "h3",
        [Description("Whether field is required")] bool required = false,
        [Description("Whether field is disabled")] bool disabled = false,
        [Description("Help text")] string? hint = null,
        [Description("Error message")] string? errorMessage = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-checkboxes");
        sb.Append($" name=\"{name}\"");
        sb.Append($" legend=\"{legend}\"");
        sb.Append($" legend-size=\"{legendSize}\"");
        sb.Append($" options='{optionsJson}'");
        
        if (required)
            sb.Append(" required=\"true\"");
        
        if (disabled)
            sb.Append(" disabled=\"true\"");
        
        if (!string.IsNullOrEmpty(hint))
            sb.Append($" hint=\"{hint}\"");
        
        if (!string.IsNullOrEmpty(errorMessage))
            sb.Append($" error-message=\"{errorMessage}\"");
        
        sb.Append("></fdcp-checkboxes>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates FDCP radio buttons group with enhanced styling.")]
    public string GenerateFDCPRadios(
        [Description("Radio group name")] string name,
        [Description("Radio group legend")] string legend,
        [Description("Options in JSON format: [{\"id\":\"1\",\"label\":\"Option 1\",\"value\":\"1\"}]")] string optionsJson,
        [Description("Legend size: h1, h2, h3, h4, h5, h6")] string legendSize = "h3",
        [Description("Whether field is required")] bool required = false,
        [Description("Whether field is disabled")] bool disabled = false,
        [Description("Help text")] string? hint = null,
        [Description("Error message")] string? errorMessage = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-radios");
        sb.Append($" name=\"{name}\"");
        sb.Append($" legend=\"{legend}\"");
        sb.Append($" legend-size=\"{legendSize}\"");
        sb.Append($" options='{optionsJson}'");
        
        if (required)
            sb.Append(" required=\"true\"");
        
        if (disabled)
            sb.Append(" disabled=\"true\"");
        
        if (!string.IsNullOrEmpty(hint))
            sb.Append($" hint=\"{hint}\"");
        
        if (!string.IsNullOrEmpty(errorMessage))
            sb.Append($" error-message=\"{errorMessage}\"");
        
        sb.Append("></fdcp-radios>");
        return sb.ToString();
    }

    #endregion

    #region Advanced UI Components

    [McpServerTool]
    [Description("Generates an advanced FDCP card component with flexible layout options.")]
    public string GenerateFDCPCard(
        [Description("Card content/body")] string content,
        [Description("Card width (CSS value)")] string? width = null,
        [Description("Card height (CSS value)")] string? height = null,
        [Description("Card has border")] bool border = true,
        [Description("Card has shadow")] bool shadow = false,
        [Description("Card is horizontal layout")] bool horizontal = false,
        [Description("Top image URL")] string? imageTop = null,
        [Description("Bottom image URL")] string? imageBottom = null,
        [Description("Image alt text")] string? imageAlt = null,
        [Description("Card ID")] string? cardId = null,
        [Description("Header content")] string? header = null,
        [Description("Footer content")] string? footer = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-card");
        
        if (!string.IsNullOrEmpty(width))
            sb.Append($" width=\"{width}\"");
        
        if (!string.IsNullOrEmpty(height))
            sb.Append($" height=\"{height}\"");
        
        sb.Append($" border=\"{border.ToString().ToLower()}\"");
        sb.Append($" shadow=\"{shadow.ToString().ToLower()}\"");
        
        if (horizontal)
            sb.Append(" horizontal=\"true\"");
        
        if (!string.IsNullOrEmpty(imageTop))
            sb.Append($" image-top=\"{imageTop}\"");
        
        if (!string.IsNullOrEmpty(imageBottom))
            sb.Append($" image-bottom=\"{imageBottom}\"");
        
        if (!string.IsNullOrEmpty(imageAlt))
            sb.Append($" image-alt=\"{imageAlt}\"");
        
        if (!string.IsNullOrEmpty(cardId))
            sb.Append($" tag-id=\"{cardId}\"");
        
        sb.AppendLine(">");
        
        if (!string.IsNullOrEmpty(header))
        {
            sb.AppendLine($"  <div slot=\"header\">{header}</div>");
        }
        
        if (!string.IsNullOrEmpty(content))
        {
            sb.AppendLine($"  <div slot=\"body\">{content}</div>");
        }
        
        if (!string.IsNullOrEmpty(footer))
        {
            sb.AppendLine($"  <div slot=\"footer\">{footer}</div>");
        }
        
        sb.Append("</fdcp-card>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates FDCP badge component for status indicators and labels.")]
    public string GenerateFDCPBadge(
        [Description("Badge text content")] string text,
        [Description("Badge type: primary, secondary, success, warning, danger, info, light, dark")] string type = "primary",
        [Description("Badge size: small, regular, large")] string size = "regular",
        [Description("Badge is pill-shaped")] bool pill = false,
        [Description("Badge icon name (optional)")] string? icon = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-badge");
        sb.Append($" type=\"{type}\"");
        sb.Append($" size=\"{size}\"");
        
        if (pill)
            sb.Append(" pill=\"true\"");
        
        if (!string.IsNullOrEmpty(icon))
            sb.Append($" icon=\"{icon}\"");
        
        sb.Append(">");
        sb.Append(text);
        sb.Append("</fdcp-badge>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates FDCP modal component for overlay dialogs and popups.")]
    public string GenerateFDCPModal(
        [Description("Modal ID")] string modalId,
        [Description("Modal title")] string title,
        [Description("Modal body content")] string content,
        [Description("Modal size: small, regular, large, xl")] string size = "regular",
        [Description("Modal is closable")] bool closable = true,
        [Description("Modal backdrop is static (can't close by clicking backdrop)")] bool staticBackdrop = false,
        [Description("Footer buttons in format 'text|type|action,text|type|action'")] string? footerButtons = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-modal");
        sb.Append($" modal-id=\"{modalId}\"");
        sb.Append($" title=\"{title}\"");
        sb.Append($" size=\"{size}\"");
        
        if (!closable)
            sb.Append(" closable=\"false\"");
        
        if (staticBackdrop)
            sb.Append(" static-backdrop=\"true\"");
        
        sb.AppendLine(">");
        
        // Modal body
        sb.AppendLine($"  <fdcp-modal-body>");
        sb.AppendLine($"    {content}");
        sb.AppendLine($"  </fdcp-modal-body>");
        
        // Modal footer
        if (!string.IsNullOrEmpty(footerButtons))
        {
            sb.AppendLine($"  <fdcp-modal-footer>");
            var buttons = footerButtons.Split(',');
            foreach (var button in buttons)
            {
                var parts = button.Split('|');
                if (parts.Length >= 2)
                {
                    var text = parts[0].Trim();
                    var type = parts[1].Trim();
                    var action = parts.Length > 2 ? parts[2].Trim() : "";
                    sb.AppendLine($"    <gcds-button type=\"button\" variant=\"{type}\" onclick=\"{action}\">{text}</gcds-button>");
                }
            }
            sb.AppendLine($"  </fdcp-modal-footer>");
        }
        
        sb.Append("</fdcp-modal>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates FDCP session modal for session timeout warnings.")]
    public string GenerateFDCPSessionModal(
        [Description("Session timeout in seconds")] int timeoutSeconds = 1800,
        [Description("Warning time in seconds before timeout")] int warningSeconds = 300,
        [Description("Modal title")] string title = "Session Timeout Warning",
        [Description("Warning message")] string message = "Your session will expire soon. Do you want to extend it?",
        [Description("Extend button text")] string extendText = "Extend Session",
        [Description("Logout button text")] string logoutText = "Logout")
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-session-modal");
        sb.Append($" timeout-seconds=\"{timeoutSeconds}\"");
        sb.Append($" warning-seconds=\"{warningSeconds}\"");
        sb.Append($" title=\"{title}\"");
        sb.Append($" message=\"{message}\"");
        sb.Append($" extend-text=\"{extendText}\"");
        sb.Append($" logout-text=\"{logoutText}\"");
        sb.Append("></fdcp-session-modal>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates FDCP stepper component for multi-step processes.")]
    public string GenerateFDCPStepper(
        [Description("Current step number (1-based)")] int currentStep,
        [Description("Steps in format 'title|description,title|description'")] string steps,
        [Description("Stepper orientation: horizontal, vertical")] string orientation = "horizontal",
        [Description("Show step numbers")] bool showNumbers = true,
        [Description("Allow navigation to completed steps")] bool allowNavigation = false)
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-stepper");
        sb.Append($" current-step=\"{currentStep}\"");
        sb.Append($" orientation=\"{orientation}\"");
        
        if (!showNumbers)
            sb.Append(" show-numbers=\"false\"");
        
        if (allowNavigation)
            sb.Append(" allow-navigation=\"true\"");
        
        sb.AppendLine(">");
        
        var stepList = steps.Split(',');
        for (int i = 0; i < stepList.Length; i++)
        {
            var stepParts = stepList[i].Split('|');
            var title = stepParts.Length > 0 ? stepParts[0].Trim() : $"Step {i + 1}";
            var description = stepParts.Length > 1 ? stepParts[1].Trim() : "";
            
            sb.AppendLine($"  <fdcp-step title=\"{title}\" description=\"{description}\"></fdcp-step>");
        }
        
        sb.Append("</fdcp-stepper>");
        return sb.ToString();
    }

    #endregion

    #region Data Components

    [McpServerTool]
    [Description("Generates FDCP Tabulator table component for advanced data display with pagination, sorting, and filtering.")]
    public string GenerateFDCPTabulatorTable(
        [Description("Table container ID")] string tableId,
        [Description("Table data source URL (for AJAX)")] string? ajaxUrl = null,
        [Description("Static data in JSON format (alternative to AJAX)")] string? staticData = null,
        [Description("Table columns in JSON format: [{\"title\":\"Name\",\"field\":\"name\",\"sorter\":\"string\"}]")] string columnsJson = "[]",
        [Description("Records per page")] int paginationSize = 10,
        [Description("Enable search functionality")] bool enableSearch = true,
        [Description("Table layout: fitData, fitColumns, fitDataFill, fitDataStretch, fitDataTable")] string layout = "fitColumns",
        [Description("Enable column resizing")] bool resizableColumns = true,
        [Description("Enable row selection")] bool selectableRows = false)
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-tabulator-table");
        sb.Append($" id=\"{tableId}\"");
        sb.Append($" pagination-size=\"{paginationSize}\"");
        
        if (!string.IsNullOrEmpty(ajaxUrl))
        {
            sb.Append($" ajax-url=\"{ajaxUrl}\"");
            sb.Append(" use-static-data=\"false\"");
        }
        else if (!string.IsNullOrEmpty(staticData))
        {
            sb.Append(" use-static-data=\"true\"");
            sb.Append($" data='{staticData}'");
        }
        
        sb.Append($" columns='{columnsJson}'");
        
        if (!enableSearch)
            sb.Append(" enable-search=\"false\"");
        
        sb.Append($" layout=\"{layout}\"");
        
        if (!resizableColumns)
            sb.Append(" resizable-columns=\"false\"");
        
        if (selectableRows)
            sb.Append(" selectable-rows=\"true\"");
        
        sb.Append("></fdcp-tabulator-table>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates FDCP filter box component for advanced search and filtering.")]
    public string GenerateFDCPFilterBox(
        [Description("Filter box ID")] string filterId,
        [Description("Target element ID to filter")] string targetId,
        [Description("Filter placeholder text")] string placeholder = "Search and filter...",
        [Description("Filter fields in JSON format: [{\"field\":\"name\",\"label\":\"Name\",\"type\":\"text\"}]")] string? filterFields = null,
        [Description("Enable advanced filters")] bool advancedFilters = false,
        [Description("Enable saved filters")] bool savedFilters = false,
        [Description("Filter on input (live filtering)")] bool liveFilter = true)
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-filter-box");
        sb.Append($" filter-id=\"{filterId}\"");
        sb.Append($" target-id=\"{targetId}\"");
        sb.Append($" placeholder=\"{placeholder}\"");
        
        if (!string.IsNullOrEmpty(filterFields))
            sb.Append($" filter-fields='{filterFields}'");
        
        if (advancedFilters)
            sb.Append(" advanced-filters=\"true\"");
        
        if (savedFilters)
            sb.Append(" saved-filters=\"true\"");
        
        if (!liveFilter)
            sb.Append(" live-filter=\"false\"");
        
        sb.Append("></fdcp-filter-box>");
        return sb.ToString();
    }

    #endregion

    #region Layout Components

    [McpServerTool]
    [Description("Generates FDCP page header component with title, breadcrumbs, and actions.")]
    public string GenerateFDCPPageHeader(
        [Description("Page title")] string title,
        [Description("Page subtitle or description")] string? subtitle = null,
        [Description("Breadcrumb items in format 'text|url,text|url'")] string? breadcrumbs = null,
        [Description("Action buttons in format 'text|variant|action,text|variant|action'")] string? actions = null,
        [Description("Header background color")] string? backgroundColor = null,
        [Description("Header text color")] string? textColor = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-page-header");
        sb.Append($" title=\"{title}\"");
        
        if (!string.IsNullOrEmpty(subtitle))
            sb.Append($" subtitle=\"{subtitle}\"");
        
        if (!string.IsNullOrEmpty(backgroundColor))
            sb.Append($" background-color=\"{backgroundColor}\"");
        
        if (!string.IsNullOrEmpty(textColor))
            sb.Append($" text-color=\"{textColor}\"");
        
        sb.AppendLine(">");
        
        // Add breadcrumbs if provided
        if (!string.IsNullOrEmpty(breadcrumbs))
        {
            sb.AppendLine("  <gcds-breadcrumbs>");
            var breadcrumbItems = breadcrumbs.Split(',');
            foreach (var item in breadcrumbItems)
            {
                var parts = item.Split('|');
                if (parts.Length == 2)
                {
                    var text = parts[0].Trim();
                    var url = parts[1].Trim();
                    sb.AppendLine($"    <gcds-breadcrumbs-item href=\"{url}\">{text}</gcds-breadcrumbs-item>");
                }
                else
                {
                    sb.AppendLine($"    <gcds-breadcrumbs-item>{item.Trim()}</gcds-breadcrumbs-item>");
                }
            }
            sb.AppendLine("  </gcds-breadcrumbs>");
        }
        
        // Add action buttons if provided
        if (!string.IsNullOrEmpty(actions))
        {
            sb.AppendLine("  <div class=\"page-actions\">");
            var actionList = actions.Split(',');
            foreach (var action in actionList)
            {
                var parts = action.Split('|');
                if (parts.Length >= 2)
                {
                    var text = parts[0].Trim();
                    var variant = parts[1].Trim();
                    var onclick = parts.Length > 2 ? parts[2].Trim() : "";
                    sb.AppendLine($"    <gcds-button variant=\"{variant}\" onclick=\"{onclick}\">{text}</gcds-button>");
                }
            }
            sb.AppendLine("  </div>");
        }
        
        sb.Append("</fdcp-page-header>");
        return sb.ToString();
    }

    #endregion

    #region Validation and Error Handling

    [McpServerTool]
    [Description("Generates FDCP error summary component with enhanced error handling.")]
    public string GenerateFDCPErrorSummary(
        [Description("Error summary title")] string title = "There are errors on this page",
        [Description("Errors in format 'field|message|type,field|message|type'")] string? errors = null,
        [Description("Show error count")] bool showCount = true,
        [Description("Auto-focus first error")] bool autoFocus = true,
        [Description("Error summary is dismissible")] bool dismissible = false)
    {
        var sb = new StringBuilder();
        sb.Append($"<fdcp-error-summary");
        sb.Append($" title=\"{title}\"");
        
        if (!showCount)
            sb.Append(" show-count=\"false\"");
        
        if (!autoFocus)
            sb.Append(" auto-focus=\"false\"");
        
        if (dismissible)
            sb.Append(" dismissible=\"true\"");
        
        sb.AppendLine(">");
        
        if (!string.IsNullOrEmpty(errors))
        {
            sb.AppendLine("  <ul>");
            var errorList = errors.Split(',');
            foreach (var error in errorList)
            {
                var parts = error.Split('|');
                if (parts.Length >= 2)
                {
                    var field = parts[0].Trim();
                    var message = parts[1].Trim();
                    var type = parts.Length > 2 ? parts[2].Trim() : "error";
                    sb.AppendLine($"    <li class=\"{type}\"><a href=\"#{field}\">{message}</a></li>");
                }
            }
            sb.AppendLine("  </ul>");
        }
        
        sb.Append("</fdcp-error-summary>");
        return sb.ToString();
    }

    #endregion

    #region Utility Tools

    [McpServerTool]
    [Description("Generates a complete FDCP form example with common form elements and validation.")]
    public string GenerateCompleteFDCPForm(
        [Description("Form title")] string title,
        [Description("Form action URL")] string action,
        [Description("Include personal information section")] bool includePersonalInfo = true,
        [Description("Include contact information section")] bool includeContactInfo = true,
        [Description("Include preferences section")] bool includePreferences = false,
        [Description("Include file upload")] bool includeFileUpload = false,
        [Description("Submit button text")] string submitText = "Submit Application")
    {
        var sb = new StringBuilder();
        
        // Form wrapper
        sb.AppendLine($"<fdcp-form action=\"{action}\" method=\"post\" title=\"{title}\">");
        sb.AppendLine("  <fdcp-error-summary></fdcp-error-summary>");
        
        // Personal Information Section
        if (includePersonalInfo)
        {
            sb.AppendLine("  <gcds-fieldset fieldset-id=\"personal-info\" legend=\"Personal Information\" legend-size=\"h2\">");
            sb.AppendLine("    <fdcp-input name=\"firstName\" label=\"First Name\" type=\"text\" required=\"true\"></fdcp-input>");
            sb.AppendLine("    <fdcp-input name=\"lastName\" label=\"Last Name\" type=\"text\" required=\"true\"></fdcp-input>");
            sb.AppendLine("    <gcds-date-input name=\"dateOfBirth\" legend=\"Date of Birth\" format=\"full\"></gcds-date-input>");
            sb.AppendLine("  </gcds-fieldset>");
        }
        
        // Contact Information Section
        if (includeContactInfo)
        {
            sb.AppendLine("  <gcds-fieldset fieldset-id=\"contact-info\" legend=\"Contact Information\" legend-size=\"h2\">");
            sb.AppendLine("    <fdcp-input name=\"email\" label=\"Email Address\" type=\"email\" required=\"true\"></fdcp-input>");
            sb.AppendLine("    <fdcp-input name=\"phone\" label=\"Phone Number\" type=\"tel\"></fdcp-input>");
            sb.AppendLine("    <gcds-textarea textarea-id=\"address\" name=\"address\" label=\"Address\" rows=\"3\"></gcds-textarea>");
            sb.AppendLine("  </gcds-fieldset>");
        }
        
        // Preferences Section
        if (includePreferences)
        {
            sb.AppendLine("  <gcds-fieldset fieldset-id=\"preferences\" legend=\"Preferences\" legend-size=\"h2\">");
            sb.AppendLine("    <fdcp-radios name=\"contactMethod\" legend=\"Preferred Contact Method\" options='[{\"id\":\"email\",\"label\":\"Email\",\"value\":\"email\"},{\"id\":\"phone\",\"label\":\"Phone\",\"value\":\"phone\"},{\"id\":\"mail\",\"label\":\"Mail\",\"value\":\"mail\"}]'></fdcp-radios>");
            sb.AppendLine("    <fdcp-checkboxes name=\"notifications\" legend=\"Notification Types\" options='[{\"id\":\"updates\",\"label\":\"System Updates\",\"value\":\"updates\"},{\"id\":\"news\",\"label\":\"News and Announcements\",\"value\":\"news\"}]'></fdcp-checkboxes>");
            sb.AppendLine("  </gcds-fieldset>");
        }
        
        // File Upload Section
        if (includeFileUpload)
        {
            sb.AppendLine("  <gcds-fieldset fieldset-id=\"documents\" legend=\"Required Documents\" legend-size=\"h2\">");
            sb.AppendLine("    <gcds-file-upload uploader-id=\"documents\" name=\"documents\" label=\"Upload Supporting Documents\" accept=\".pdf,.doc,.docx\" multiple=\"true\"></gcds-file-upload>");
            sb.AppendLine("  </gcds-fieldset>");
        }
        
        // Terms and Conditions
        sb.AppendLine("  <gcds-fieldset fieldset-id=\"terms\" legend=\"Terms and Conditions\" legend-size=\"h2\">");
        sb.AppendLine("    <fdcp-checkbox name=\"terms\" checkbox-id=\"terms\" label=\"I agree to the terms and conditions\" value=\"agreed\" required=\"true\"></fdcp-checkbox>");
        sb.AppendLine("  </gcds-fieldset>");
        
        // Submit Button
        sb.AppendLine($"  <gcds-button type=\"submit\" variant=\"primary\">{submitText}</gcds-button>");
        sb.AppendLine("</fdcp-form>");
        
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Provides comprehensive information about FDCP components and their usage patterns.")]
    public string GetFDCPComponentInformation(
        [Description("Component category: forms, data, layout, ui, all")] string category = "all")
    {
        var sb = new StringBuilder();
        
        switch (category.ToLower())
        {
            case "forms":
                sb.AppendLine("## FDCP Form Components:");
                sb.AppendLine("- fdcp-form: Complete form wrapper with validation");
                sb.AppendLine("- fdcp-input: Enhanced input fields with advanced validation");
                sb.AppendLine("- fdcp-select: Dropdown with search and multiple selection");
                sb.AppendLine("- fdcp-checkbox: Individual checkbox with styling");
                sb.AppendLine("- fdcp-checkboxes: Checkbox groups with validation");
                sb.AppendLine("- fdcp-radios: Radio button groups with enhanced styling");
                sb.AppendLine("- fdcp-error-summary: Advanced error handling and display");
                break;
                
            case "data":
                sb.AppendLine("## FDCP Data Components:");
                sb.AppendLine("- fdcp-tabulator-table: Advanced data tables with sorting, filtering, pagination");
                sb.AppendLine("- fdcp-filter-box: Powerful search and filter interfaces");
                sb.AppendLine("- Support for AJAX data loading and static data");
                sb.AppendLine("- Built-in export functionality (CSV, PDF, Excel)");
                sb.AppendLine("- Column customization and responsive design");
                break;
                
            case "layout":
                sb.AppendLine("## FDCP Layout Components:");
                sb.AppendLine("- fdcp-page-header: Comprehensive page headers with breadcrumbs");
                sb.AppendLine("- fdcp-card: Flexible card layouts with slots");
                sb.AppendLine("- fdcp-modal: Modal dialogs with various sizes and configurations");
                sb.AppendLine("- fdcp-session-modal: Automatic session timeout handling");
                sb.AppendLine("- fdcp-stepper: Multi-step process navigation");
                break;
                
            case "ui":
                sb.AppendLine("## FDCP UI Components:");
                sb.AppendLine("- fdcp-badge: Status indicators and labels");
                sb.AppendLine("- fdcp-stepper: Process flow visualization");
                sb.AppendLine("- Enhanced styling and theming options");
                sb.AppendLine("- Accessibility-first design approach");
                sb.AppendLine("- Integration with GCDS components");
                break;
                
            default:
                sb.AppendLine("## FDCP (Federal Design Component Package)");
                sb.AppendLine("### Advanced form components with enhanced validation");
                sb.AppendLine("### Powerful data tables with Tabulator.js integration");
                sb.AppendLine("### Flexible layout components for complex applications");
                sb.AppendLine("### Enhanced UI elements with advanced functionality");
                sb.AppendLine("### Built-in accessibility and responsive design");
                sb.AppendLine("");
                sb.AppendLine("Use specific categories (forms, data, layout, ui) for detailed information.");
                break;
        }
        
        return sb.ToString();
    }

    #endregion
}
