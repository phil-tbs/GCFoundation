using System.ComponentModel;
using System.Text;
using ModelContextProtocol.Server;

/// <summary>
/// Comprehensive MCP tools for all GCDS (Government of Canada Design System) components.
/// Provides generation capabilities for every GCDS component available in the framework.
/// </summary>
internal class ComprehensiveGCDSTools
{
    #region Form Components

    [McpServerTool]
    [Description("Generates a complete GCDS input component with label, validation, and accessibility features.")]
    public string GenerateGCDSInput(
        [Description("Input field name and ID")] string name,
        [Description("Input label text")] string label,
        [Description("Input type: text, email, password, tel, url, number, search, file")] string type = "text",
        [Description("Input placeholder text")] string? placeholder = null,
        [Description("Whether field is required")] bool required = false,
        [Description("Whether field is disabled")] bool disabled = false,
        [Description("Help text for the field")] string? hint = null,
        [Description("Error message text")] string? errorMessage = null,
        [Description("Input size: regular, small")] string size = "regular",
        [Description("Autocomplete behavior: on, off")] string autocomplete = "off",
        [Description("Hide the label visually but keep it for screen readers")] bool hideLabel = false)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-input");
        sb.Append($" input-id=\"{name}\"");
        sb.Append($" name=\"{name}\"");
        sb.Append($" label=\"{label}\"");
        sb.Append($" type=\"{type}\"");
        sb.Append($" size=\"{size}\"");
        sb.Append($" autocomplete=\"{autocomplete}\"");
        
        if (!string.IsNullOrEmpty(placeholder))
            sb.Append($" placeholder=\"{placeholder}\"");
        
        if (required)
            sb.Append(" required=\"true\"");
        
        if (disabled)
            sb.Append(" disabled=\"true\"");
        
        if (hideLabel)
            sb.Append(" hide-label=\"true\"");
        
        if (!string.IsNullOrEmpty(hint))
            sb.Append($" hint=\"{hint}\"");
        
        if (!string.IsNullOrEmpty(errorMessage))
            sb.Append($" error-message=\"{errorMessage}\"");
        
        sb.Append("></gcds-input>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates a GCDS select (dropdown) component with options.")]
    public string GenerateGCDSSelect(
        [Description("Select field name and ID")] string name,
        [Description("Select label text")] string label,
        [Description("Comma-separated options in format 'value:label,value:label'")] string options,
        [Description("Default selected value")] string? defaultValue = null,
        [Description("Whether field is required")] bool required = false,
        [Description("Whether field is disabled")] bool disabled = false,
        [Description("Help text for the field")] string? hint = null,
        [Description("Error message text")] string? errorMessage = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-select");
        sb.Append($" select-id=\"{name}\"");
        sb.Append($" name=\"{name}\"");
        sb.Append($" label=\"{label}\"");
        
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
        
        sb.AppendLine(">");
        
        // Add options
        var optionPairs = options.Split(',');
        foreach (var option in optionPairs)
        {
            var parts = option.Split(':');
            if (parts.Length == 2)
            {
                sb.AppendLine($"  <option value=\"{parts[0].Trim()}\">{parts[1].Trim()}</option>");
            }
            else
            {
                sb.AppendLine($"  <option value=\"{option.Trim()}\">{option.Trim()}</option>");
            }
        }
        
        sb.Append("</gcds-select>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates a GCDS textarea component for multi-line text input.")]
    public string GenerateGCDSTextarea(
        [Description("Textarea field name and ID")] string name,
        [Description("Textarea label text")] string label,
        [Description("Number of visible rows")] int rows = 3,
        [Description("Placeholder text")] string? placeholder = null,
        [Description("Whether field is required")] bool required = false,
        [Description("Whether field is disabled")] bool disabled = false,
        [Description("Help text for the field")] string? hint = null,
        [Description("Error message text")] string? errorMessage = null,
        [Description("Character limit")] int? characterLimit = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-textarea");
        sb.Append($" textarea-id=\"{name}\"");
        sb.Append($" name=\"{name}\"");
        sb.Append($" label=\"{label}\"");
        sb.Append($" rows=\"{rows}\"");
        
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
        
        if (characterLimit.HasValue)
            sb.Append($" character-limit=\"{characterLimit}\"");
        
        sb.Append("></gcds-textarea>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS radio buttons group with multiple options.")]
    public string GenerateGCDSRadios(
        [Description("Radio group name")] string name,
        [Description("Radio group legend/label")] string legend,
        [Description("Radio options in JSON format: [{\"id\":\"1\",\"label\":\"Option 1\",\"value\":\"1\"}]")] string optionsJson,
        [Description("Legend size: h1, h2, h3, h4, h5, h6")] string legendSize = "h3",
        [Description("Whether field is required")] bool required = false,
        [Description("Whether field is disabled")] bool disabled = false,
        [Description("Help text for the group")] string? hint = null,
        [Description("Error message text")] string? errorMessage = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-radios");
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
        
        sb.Append("></gcds-radios>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS checkboxes group with multiple options.")]
    public string GenerateGCDSCheckboxes(
        [Description("Checkbox group name")] string name,
        [Description("Checkbox group legend/label")] string legend,
        [Description("Checkbox options in JSON format: [{\"id\":\"1\",\"label\":\"Option 1\",\"value\":\"1\"}]")] string optionsJson,
        [Description("Legend size: h1, h2, h3, h4, h5, h6")] string legendSize = "h3",
        [Description("Whether field is required")] bool required = false,
        [Description("Whether field is disabled")] bool disabled = false,
        [Description("Help text for the group")] string? hint = null,
        [Description("Error message text")] string? errorMessage = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-checkboxes");
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
        
        sb.Append("></gcds-checkboxes>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates a GCDS date input component for date selection.")]
    public string GenerateGCDSDateInput(
        [Description("Date input name")] string name,
        [Description("Date input legend/label")] string legend,
        [Description("Date format: full, compact, month-year")] string format = "full",
        [Description("Default date value (YYYY-MM-DD)")] string? value = null,
        [Description("Whether field is required")] bool required = false,
        [Description("Whether field is disabled")] bool disabled = false,
        [Description("Help text for the field")] string? hint = null,
        [Description("Error message text")] string? errorMessage = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-date-input");
        sb.Append($" name=\"{name}\"");
        sb.Append($" legend=\"{legend}\"");
        sb.Append($" format=\"{format}\"");
        
        if (!string.IsNullOrEmpty(value))
            sb.Append($" value=\"{value}\"");
        
        if (required)
            sb.Append(" required=\"true\"");
        
        if (disabled)
            sb.Append(" disabled=\"true\"");
        
        if (!string.IsNullOrEmpty(hint))
            sb.Append($" hint=\"{hint}\"");
        
        if (!string.IsNullOrEmpty(errorMessage))
            sb.Append($" error-message=\"{errorMessage}\"");
        
        sb.Append("></gcds-date-input>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates a GCDS file upload component.")]
    public string GenerateGCDSFileUpload(
        [Description("File upload name and ID")] string name,
        [Description("File upload label")] string label,
        [Description("Accepted file types (e.g., '.pdf,.doc,.docx')")] string? accept = null,
        [Description("Whether multiple files are allowed")] bool multiple = false,
        [Description("Whether field is required")] bool required = false,
        [Description("Whether field is disabled")] bool disabled = false,
        [Description("Help text for the field")] string? hint = null,
        [Description("Error message text")] string? errorMessage = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-file-upload");
        sb.Append($" uploader-id=\"{name}\"");
        sb.Append($" name=\"{name}\"");
        sb.Append($" label=\"{label}\"");
        
        if (!string.IsNullOrEmpty(accept))
            sb.Append($" accept=\"{accept}\"");
        
        if (multiple)
            sb.Append(" multiple=\"true\"");
        
        if (required)
            sb.Append(" required=\"true\"");
        
        if (disabled)
            sb.Append(" disabled=\"true\"");
        
        if (!string.IsNullOrEmpty(hint))
            sb.Append($" hint=\"{hint}\"");
        
        if (!string.IsNullOrEmpty(errorMessage))
            sb.Append($" error-message=\"{errorMessage}\"");
        
        sb.Append("></gcds-file-upload>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates a GCDS fieldset component for grouping form elements.")]
    public string GenerateGCDSFieldset(
        [Description("Fieldset ID")] string fieldsetId,
        [Description("Fieldset legend text")] string legend,
        [Description("Legend size: h1, h2, h3, h4, h5, h6")] string legendSize = "h2",
        [Description("Help text for the fieldset")] string? hint = null,
        [Description("Whether fieldset is required")] bool required = false)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-fieldset");
        sb.Append($" fieldset-id=\"{fieldsetId}\"");
        sb.Append($" legend=\"{legend}\"");
        sb.Append($" legend-size=\"{legendSize}\"");
        
        if (!string.IsNullOrEmpty(hint))
            sb.Append($" hint=\"{hint}\"");
        
        if (required)
            sb.Append(" required=\"true\"");
        
        sb.AppendLine(">");
        sb.AppendLine("  <!-- Add form elements here -->");
        sb.Append("</gcds-fieldset>");
        return sb.ToString();
    }

    #endregion

    #region Navigation Components

    [McpServerTool]
    [Description("Generates GCDS breadcrumbs navigation component.")]
    public string GenerateGCDSBreadcrumbs(
        [Description("Breadcrumb items in format 'text|url,text|url' (last item without URL for current page)")] string items,
        [Description("Hide breadcrumbs on mobile devices")] bool hideMobile = false)
    {
        var sb = new StringBuilder();
        sb.Append("<gcds-breadcrumbs");
        
        if (hideMobile)
            sb.Append(" hide-mobile=\"true\"");
        
        sb.AppendLine(">");
        
        var breadcrumbItems = items.Split(',');
        foreach (var item in breadcrumbItems)
        {
            var parts = item.Split('|');
            if (parts.Length == 2)
            {
                var text = parts[0].Trim();
                var url = parts[1].Trim();
                sb.AppendLine($"  <gcds-breadcrumbs-item href=\"{url}\">{text}</gcds-breadcrumbs-item>");
            }
            else
            {
                // Current page (no link)
                sb.AppendLine($"  <gcds-breadcrumbs-item>{item.Trim()}</gcds-breadcrumbs-item>");
            }
        }
        
        sb.Append("</gcds-breadcrumbs>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS top navigation component.")]
    public string GenerateGCDSTopNavigation(
        [Description("Application name/title")] string appName,
        [Description("Logo URL (optional)")] string? logoUrl = null,
        [Description("Navigation alignment: left, right, center")] string alignment = "left",
        [Description("Whether navigation is sticky")] bool sticky = false)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-top-nav");
        sb.Append($" app-name=\"{appName}\"");
        sb.Append($" alignment=\"{alignment}\"");
        
        if (!string.IsNullOrEmpty(logoUrl))
            sb.Append($" logo=\"{logoUrl}\"");
        
        if (sticky)
            sb.Append(" sticky=\"true\"");
        
        sb.AppendLine(">");
        sb.AppendLine("  <!-- Add nav links here -->");
        sb.Append("</gcds-top-nav>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS side navigation component.")]
    public string GenerateGCDSSideNavigation(
        [Description("Navigation label")] string label,
        [Description("Whether navigation is open by default")] bool open = false)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-side-nav");
        sb.Append($" label=\"{label}\"");
        
        if (open)
            sb.Append(" open=\"true\"");
        
        sb.AppendLine(">");
        sb.AppendLine("  <!-- Add navigation groups and links here -->");
        sb.Append("</gcds-side-nav>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS navigation group for organizing nav links.")]
    public string GenerateGCDSNavGroup(
        [Description("Group name")] string groupName,
        [Description("Navigation links in format 'text|url,text|url'")] string links)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"<gcds-nav-group name=\"{groupName}\">");
        
        var navLinks = links.Split(',');
        foreach (var link in navLinks)
        {
            var parts = link.Split('|');
            if (parts.Length == 2)
            {
                var text = parts[0].Trim();
                var url = parts[1].Trim();
                sb.AppendLine($"  <gcds-nav-link href=\"{url}\">{text}</gcds-nav-link>");
            }
        }
        
        sb.Append("</gcds-nav-group>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS pagination component for page navigation.")]
    public string GenerateGCDSPagination(
        [Description("Current page number")] int currentPage,
        [Description("Total number of pages")] int totalPages,
        [Description("Pagination display type: list, simple")] string display = "list",
        [Description("Base URL for pagination links")] string? url = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-pagination");
        sb.Append($" current-page=\"{currentPage}\"");
        sb.Append($" total-pages=\"{totalPages}\"");
        sb.Append($" display=\"{display}\"");
        
        if (!string.IsNullOrEmpty(url))
            sb.Append($" url=\"{url}\"");
        
        sb.Append("></gcds-pagination>");
        return sb.ToString();
    }

    #endregion

    #region Layout Components

    [McpServerTool]
    [Description("Generates GCDS container component for layout structure.")]
    public string GenerateGCDSContainer(
        [Description("Container content")] string content,
        [Description("Container size: xl, lg, md, sm, xs, full")] string size = "xl",
        [Description("Container padding: none, xs, sm, md, lg, xl")] string padding = "md",
        [Description("Container margin: none, xs, sm, md, lg, xl, auto")] string margin = "auto",
        [Description("Center the container")] bool centered = true)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-container");
        sb.Append($" size=\"{size}\"");
        sb.Append($" padding=\"{padding}\"");
        sb.Append($" margin=\"{margin}\"");
        
        if (centered)
            sb.Append(" centered=\"true\"");
        
        sb.AppendLine(">");
        sb.AppendLine(content);
        sb.Append("</gcds-container>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS grid component for responsive layouts.")]
    public string GenerateGCDSGrid(
        [Description("Grid content (columns)")] string content,
        [Description("Grid display: grid, inline-grid")] string display = "grid",
        [Description("Number of columns: 1-12 or auto")] string columns = "auto",
        [Description("Grid gap: none, xs, sm, md, lg, xl")] string gap = "md",
        [Description("Grid tag: div, section, main, aside")] string tag = "div")
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-grid");
        sb.Append($" display=\"{display}\"");
        sb.Append($" columns=\"{columns}\"");
        sb.Append($" gap=\"{gap}\"");
        sb.Append($" tag=\"{tag}\"");
        sb.AppendLine(">");
        sb.AppendLine(content);
        sb.Append("</gcds-grid>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS header component for page headers.")]
    public string GenerateGCDSHeader(
        [Description("Header content")] string content,
        [Description("Header variant: default, signature")] string variant = "default",
        [Description("Language toggle enabled")] bool languageToggle = false,
        [Description("Skip to content link enabled")] bool skipToContent = true)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-header");
        sb.Append($" variant=\"{variant}\"");
        
        if (languageToggle)
            sb.Append(" language-toggle=\"true\"");
        
        if (skipToContent)
            sb.Append(" skip-to-content=\"true\"");
        
        sb.AppendLine(">");
        sb.AppendLine(content);
        sb.Append("</gcds-header>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS footer component for page footers.")]
    public string GenerateGCDSFooter(
        [Description("Footer content")] string content,
        [Description("Footer display: full, compact")] string display = "full",
        [Description("Show contextual links")] bool contextualLinks = true,
        [Description("Show sub-footer")] bool subFooter = true)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-footer");
        sb.Append($" display=\"{display}\"");
        
        if (contextualLinks)
            sb.Append(" contextual-links=\"true\"");
        
        if (subFooter)
            sb.Append(" sub-footer=\"true\"");
        
        sb.AppendLine(">");
        sb.AppendLine(content);
        sb.Append("</gcds-footer>");
        return sb.ToString();
    }

    #endregion

    #region Content Components

    [McpServerTool]
    [Description("Generates GCDS text component with semantic styling.")]
    public string GenerateGCDSText(
        [Description("Text content")] string content,
        [Description("Text size: caption, small, body, h6, h5, h4, h3, h2, h1, display")] string size = "body",
        [Description("Text role: primary, secondary, light, inherit")] string role = "primary",
        [Description("Display style: block, inline, inline-block")] string display = "block",
        [Description("Margin bottom value")] string? marginBottom = null,
        [Description("Margin top value")] string? marginTop = null,
        [Description("Enable character limit")] bool characterLimit = false)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-text");
        sb.Append($" size=\"{size}\"");
        sb.Append($" text-role=\"{role}\"");
        sb.Append($" display=\"{display}\"");
        
        if (!string.IsNullOrEmpty(marginBottom))
            sb.Append($" margin-bottom=\"{marginBottom}\"");
        
        if (!string.IsNullOrEmpty(marginTop))
            sb.Append($" margin-top=\"{marginTop}\"");
        
        if (characterLimit)
            sb.Append(" character-limit=\"true\"");
        
        sb.Append(">");
        sb.Append(content);
        sb.Append("</gcds-text>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS heading component with proper semantic structure.")]
    public string GenerateGCDSHeading(
        [Description("Heading text")] string text,
        [Description("Heading tag: h1, h2, h3, h4, h5, h6")] string tag = "h2",
        [Description("Heading size: h1, h2, h3, h4, h5, h6, display")] string? size = null,
        [Description("Margin bottom value")] string? marginBottom = null,
        [Description("Margin top value")] string? marginTop = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-heading");
        sb.Append($" tag=\"{tag}\"");
        
        if (!string.IsNullOrEmpty(size))
            sb.Append($" size=\"{size}\"");
        
        if (!string.IsNullOrEmpty(marginBottom))
            sb.Append($" margin-bottom=\"{marginBottom}\"");
        
        if (!string.IsNullOrEmpty(marginTop))
            sb.Append($" margin-top=\"{marginTop}\"");
        
        sb.Append(">");
        sb.Append(text);
        sb.Append("</gcds-heading>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS button component with various styles and behaviors.")]
    public string GenerateGCDSButton(
        [Description("Button text")] string text,
        [Description("Button type: button, submit, reset")] string type = "button",
        [Description("Button variant: primary, secondary, destructive, skip-to-content")] string variant = "primary",
        [Description("Button size: regular, small")] string size = "regular",
        [Description("Button ID")] string? id = null,
        [Description("Whether button is disabled")] bool disabled = false,
        [Description("Button URL (for links)")] string? href = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-button");
        sb.Append($" type=\"{type}\"");
        sb.Append($" variant=\"{variant}\"");
        sb.Append($" size=\"{size}\"");
        
        if (!string.IsNullOrEmpty(id))
            sb.Append($" id=\"{id}\"");
        
        if (disabled)
            sb.Append(" disabled=\"true\"");
        
        if (!string.IsNullOrEmpty(href))
            sb.Append($" href=\"{href}\"");
        
        sb.Append(">");
        sb.Append(text);
        sb.Append("</gcds-button>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS link component with proper styling and accessibility.")]
    public string GenerateGCDSLink(
        [Description("Link text")] string text,
        [Description("Link URL")] string href,
        [Description("Link size: inherit, small, regular")] string size = "regular",
        [Description("Link variant: default, light")] string variant = "default",
        [Description("External link (opens in new tab)")] bool external = false,
        [Description("Download link")] bool download = false)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-link");
        sb.Append($" href=\"{href}\"");
        sb.Append($" size=\"{size}\"");
        sb.Append($" variant=\"{variant}\"");
        
        if (external)
            sb.Append(" external=\"true\"");
        
        if (download)
            sb.Append(" download=\"true\"");
        
        sb.Append(">");
        sb.Append(text);
        sb.Append("</gcds-link>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS card component with header, body, and footer.")]
    public string GenerateGCDSCard(
        [Description("Card content/body")] string content,
        [Description("Card title/header")] string? title = null,
        [Description("Card footer content")] string? footer = null,
        [Description("Card title tag: h1, h2, h3, h4, h5, h6")] string titleTag = "h3")
    {
        var sb = new StringBuilder();
        sb.AppendLine("<gcds-card>");
        
        if (!string.IsNullOrEmpty(title))
        {
            sb.AppendLine($"  <gcds-card-header>");
            sb.AppendLine($"    <gcds-heading tag=\"{titleTag}\">{title}</gcds-heading>");
            sb.AppendLine($"  </gcds-card-header>");
        }
        
        sb.AppendLine($"  <gcds-card-body>");
        sb.AppendLine($"    {content}");
        sb.AppendLine($"  </gcds-card-body>");
        
        if (!string.IsNullOrEmpty(footer))
        {
            sb.AppendLine($"  <gcds-card-footer>");
            sb.AppendLine($"    {footer}");
            sb.AppendLine($"  </gcds-card-footer>");
        }
        
        sb.Append("</gcds-card>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS details component for expandable content.")]
    public string GenerateGCDSDetails(
        [Description("Summary text (clickable header)")] string summary,
        [Description("Details content (expandable body)")] string content,
        [Description("Whether details are open by default")] bool open = false)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-details");
        
        if (open)
            sb.Append(" open=\"true\"");
        
        sb.AppendLine(">");
        sb.AppendLine($"  <gcds-summary>{summary}</gcds-summary>");
        sb.AppendLine($"  {content}");
        sb.Append("</gcds-details>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS notice component for important messages.")]
    public string GenerateGCDSNotice(
        [Description("Notice content/message")] string content,
        [Description("Notice title")] string? title = null,
        [Description("Notice type: info, warning, danger, success")] string type = "info",
        [Description("Notice is dismissible")] bool dismissible = false)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-notice");
        sb.Append($" type=\"{type}\"");
        
        if (dismissible)
            sb.Append(" dismissible=\"true\"");
        
        if (!string.IsNullOrEmpty(title))
            sb.Append($" title=\"{title}\"");
        
        sb.Append(">");
        sb.Append(content);
        sb.Append("</gcds-notice>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS icon component for displaying icons.")]
    public string GenerateGCDSIcon(
        [Description("Icon name")] string name,
        [Description("Icon size: caption, text, h6, h5, h4, h3, h2, h1")] string size = "text",
        [Description("Icon label for accessibility")] string? label = null,
        [Description("Icon margin right")] string? marginRight = null,
        [Description("Icon margin left")] string? marginLeft = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-icon");
        sb.Append($" name=\"{name}\"");
        sb.Append($" size=\"{size}\"");
        
        if (!string.IsNullOrEmpty(label))
            sb.Append($" label=\"{label}\"");
        
        if (!string.IsNullOrEmpty(marginRight))
            sb.Append($" margin-right=\"{marginRight}\"");
        
        if (!string.IsNullOrEmpty(marginLeft))
            sb.Append($" margin-left=\"{marginLeft}\"");
        
        sb.Append("></gcds-icon>");
        return sb.ToString();
    }

    #endregion

    #region Utility Components

    [McpServerTool]
    [Description("Generates GCDS error message component for form validation.")]
    public string GenerateGCDSErrorMessage(
        [Description("Error message text")] string message,
        [Description("Error message ID")] string? messageId = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-error-message");
        
        if (!string.IsNullOrEmpty(messageId))
            sb.Append($" message-id=\"{messageId}\"");
        
        sb.Append(">");
        sb.Append(message);
        sb.Append("</gcds-error-message>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS error summary component for form errors.")]
    public string GenerateGCDSErrorSummary(
        [Description("Error summary heading")] string heading = "There are errors on this page",
        [Description("List of errors in format 'field|message,field|message'")] string? errors = null)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"<gcds-error-summary>");
        sb.AppendLine($"  <gcds-heading tag=\"h2\" size=\"h3\">{heading}</gcds-heading>");
        
        if (!string.IsNullOrEmpty(errors))
        {
            sb.AppendLine($"  <ul>");
            var errorList = errors.Split(',');
            foreach (var error in errorList)
            {
                var parts = error.Split('|');
                if (parts.Length == 2)
                {
                    var field = parts[0].Trim();
                    var message = parts[1].Trim();
                    sb.AppendLine($"    <li><a href=\"#{field}\">{message}</a></li>");
                }
            }
            sb.AppendLine($"  </ul>");
        }
        
        sb.Append("</gcds-error-summary>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS search component for site search.")]
    public string GenerateGCDSSearch(
        [Description("Search input name")] string name,
        [Description("Search placeholder text")] string placeholder = "Search",
        [Description("Search button text")] string buttonText = "Search",
        [Description("Search method: get, post")] string method = "get",
        [Description("Search action URL")] string? action = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-search");
        sb.Append($" name=\"{name}\"");
        sb.Append($" placeholder=\"{placeholder}\"");
        sb.Append($" button-text=\"{buttonText}\"");
        sb.Append($" method=\"{method}\"");
        
        if (!string.IsNullOrEmpty(action))
            sb.Append($" action=\"{action}\"");
        
        sb.Append("></gcds-search>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS language toggle component for bilingual sites.")]
    public string GenerateGCDSLanguageToggle(
        [Description("Current language code: en, fr")] string currentLang = "en",
        [Description("Toggle URL for language switch")] string? href = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-lang-toggle");
        sb.Append($" lang=\"{currentLang}\"");
        
        if (!string.IsNullOrEmpty(href))
            sb.Append($" href=\"{href}\"");
        
        sb.Append("></gcds-lang-toggle>");
        return sb.ToString();
    }

    [McpServerTool]
    [Description("Generates GCDS signature component for Government of Canada branding.")]
    public string GenerateGCDSSignature(
        [Description("Signature type: colour, white")] string type = "colour",
        [Description("Signature variant: signature, wordmark")] string variant = "signature",
        [Description("Whether signature has link")] bool hasLink = true,
        [Description("Signature URL")] string? href = null)
    {
        var sb = new StringBuilder();
        sb.Append($"<gcds-signature");
        sb.Append($" type=\"{type}\"");
        sb.Append($" variant=\"{variant}\"");
        
        if (hasLink && !string.IsNullOrEmpty(href))
            sb.Append($" href=\"{href}\"");
        else if (hasLink)
            sb.Append(" href=\"https://canada.ca\"");
        
        sb.Append("></gcds-signature>");
        return sb.ToString();
    }

    #endregion
}
