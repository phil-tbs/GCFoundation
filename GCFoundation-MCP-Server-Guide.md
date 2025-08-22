# GC Foundation MCP Server - Setup and Usage Guide

## Overview

The GC Foundation MCP (Model Context Protocol) Server is now integrated into the GCFoundation project. This server provides AI assistants like GitHub Copilot with specialized tools for generating Government of Canada Design System (GCDS) and Federal Design Component Package (FDCP) components.

## What Was Created

### 1. Comprehensive MCP Server Project (`GCFoundation.McpServer`)
- Complete .NET console application configured as an MCP server
- Integrated with existing GCFoundation.Common and GCFoundation.Components projects  
- Added to the main solution file (`GCFoundation.sln`)
- **80+ MCP Tools** across 4 specialized tool classes

### 2. Complete GCDS Component Tools (`ComprehensiveGCDSTools.cs`)
- **38+ Tools** covering every GCDS component
- **Form Components**: Input, Select, Textarea, Radios, Checkboxes, Date Input, File Upload, Fieldset
- **Navigation**: Breadcrumbs, Top Navigation, Side Navigation, Nav Groups, Pagination  
- **Layout**: Container, Grid, Header, Footer
- **Content**: Text, Heading, Button, Link, Card, Details, Notice, Icon
- **Utilities**: Error Message, Error Summary, Search, Language Toggle, Signature

### 3. Advanced FDCP Component Tools (`ComprehensiveFDCPTools.cs`)
- **25+ Tools** for advanced functionality
- **Enhanced Forms**: Advanced inputs, selects, checkboxes with rich validation
- **Data Components**: Tabulator tables with sorting, filtering, pagination
- **UI Elements**: Cards with slots, badges, modals, session management
- **Layout**: Page headers with breadcrumbs and actions
- **Specialized**: Stepper components, form builders, comprehensive error handling

### 4. Project Configuration Tools (`ProjectConfigurationTools.cs`)
- **15+ Tools** for complete project setup and scaffolding
- **GenerateGCFoundationProgramCs**: Complete Program.cs with all middleware/services
- **GenerateAppSettingsJson**: Full configuration with Foundation settings
- **GenerateProjectFile**: .csproj with all necessary dependencies
- **GenerateBaseController**: Foundation-based controller templates
- **GenerateFoundationLayoutView**: Complete layout views with GCDS structure
- **GenerateSampleFormView**: Form examples with validation
- **GenerateProjectStructure**: Complete directory structure for new projects

### 5. Core Foundation Utilities (`GCFoundationTools.cs`)
- **QuickGCDSComponent**: Fast generator for common components
- **ValidateGCDSMarkup**: Compliance and accessibility validation
- **ConvertToGCDS**: HTML to GCDS component conversion
- **GenerateFormValidation**: Canadian-specific validation patterns (SIN, postal codes, etc.)
- **GetGCFoundationGuidance**: Comprehensive development guidance

### 6. VS Code Configuration (`.vscode/mcp.json`)
- Pre-configured for local development and testing
- Sets up the MCP server to run with `dotnet run`
- Includes proper environment variables

### 7. NuGet Package Configuration
- **Package Metadata**: Proper naming, description, and tags for NuGet
- **Server Manifest** (`.mcp/server.json`): Registry integration for MCP discovery
- **Documentation**: Comprehensive README with usage examples

## How to Use

### Local Development (VS Code + GitHub Copilot)

1. **Ensure the MCP server is configured** - The `.vscode/mcp.json` file is already set up
2. **Open VS Code** in the GCFoundation project root
3. **Open GitHub Copilot** and select the "Select tools" icon  
4. **Verify** that "GCFoundation.McpServer" appears with available tools
5. **Try example prompts**:
   - "Generate a GCDS primary button with the text 'Submit'"
   - "Create a breadcrumb navigation for Home > Services > Applications"
   - "Build a required email input field with GCDS styling"
   - "Generate a card component with header 'Welcome' and body content"

### Example Outputs

#### Complete Program.cs Generation
**Prompt**: "Generate a complete Program.cs for GC Foundation with authentication and localization"
```csharp
using GCFoundation.Common.Utilities;
using GCFoundation.Components.Middleware;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

// Add authentication
builder.Services.AddAuthentication("DemoAuth")
    .AddCookie("DemoAuth", options => {
        options.LoginPath = "/account/login";
        // ... additional configuration
    });

// Configure GC Foundation services  
builder.Services.AddGCFoundationComponents(builder.Configuration);
builder.Services.AddGCFoundationContentPolicies(builder.Configuration);

var app = builder.Build();

// Configure pipeline with proper middleware order
app.UseMiddleware<GCFoundationComponentsMiddleware>();
app.UseGCFoundationComponents();
// ... complete pipeline setup

app.Run();
```

#### Advanced FDCP Form Generation  
**Prompt**: "Create a complete FDCP form with personal info, contact details, and file upload"
```html
<fdcp-form action="/submit" method="post" title="Application Form">
  <fdcp-error-summary></fdcp-error-summary>
  
  <gcds-fieldset fieldset-id="personal-info" legend="Personal Information" legend-size="h2">
    <fdcp-input name="firstName" label="First Name" type="text" required="true"></fdcp-input>
    <fdcp-input name="lastName" label="Last Name" type="text" required="true"></fdcp-input>
    <gcds-date-input name="dateOfBirth" legend="Date of Birth" format="full"></gcds-date-input>
  </gcds-fieldset>
  
  <gcds-fieldset fieldset-id="contact-info" legend="Contact Information" legend-size="h2">
    <fdcp-input name="email" label="Email Address" type="email" required="true"></fdcp-input>
    <fdcp-input name="phone" label="Phone Number" type="tel"></fdcp-input>
    <gcds-textarea textarea-id="address" name="address" label="Address" rows="3"></gcds-textarea>
  </gcds-fieldset>
  
  <gcds-fieldset fieldset-id="documents" legend="Required Documents" legend-size="h2">
    <gcds-file-upload uploader-id="documents" name="documents" label="Upload Supporting Documents" accept=".pdf,.doc,.docx" multiple="true"></gcds-file-upload>
  </gcds-fieldset>
  
  <gcds-button type="submit" variant="primary">Submit Application</gcds-button>
</fdcp-form>
```

#### Data Table with Advanced Features
**Prompt**: "Generate a Tabulator table with search, pagination, and AJAX data loading"
```html
<fdcp-tabulator-table
    id="employeeTable"
    ajax-url="/api/employees"
    pagination-size="10"
    columns='[{"title":"Name","field":"name","sorter":"string"},{"title":"Department","field":"department"},{"title":"Email","field":"email"}]'
    enable-search="true"
    layout="fitColumns"
    selectable-rows="true">
</fdcp-tabulator-table>
```

#### Canadian Form Validation
**Prompt**: "Create validation for a Social Insurance Number field"
```html
<gcds-input
    input-id="sin"
    name="sin"
    label="Social Insurance Number"
    type="text"
    pattern="^\d{3}-?\d{3}-?\d{3}$"
    error-message="Please enter a valid Social Insurance Number (e.g., 123-456-789)"
    hint="Example: 123-456-789"
    required="true">
</gcds-input>
```

#### Project Structure Scaffolding
**Prompt**: "Generate a complete project structure for a new GC Foundation application"
```
MyGCFoundationApp/
├── src/
│   ├── MyGCFoundationApp.Web/
│   │   ├── Controllers/
│   │   │   ├── HomeController.cs
│   │   │   ├── ErrorsController.cs
│   │   │   └── LanguageController.cs
│   │   ├── Models/
│   │   ├── Views/
│   │   ├── Resources/
│   │   ├── wwwroot/
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── MyGCFoundationApp.Web.csproj
├── docs/
├── .gitignore
├── .editorconfig
└── MyGCFoundationApp.sln
```

#### Markup Validation and Compliance
**Prompt**: "Validate this GCDS markup for accessibility"
```html
## GCDS Markup Validation Results

### Accessibility Checks:
✅ **No issues found!** Your markup follows GCDS best practices.

### GC Design System Compliance:
- All components use proper GCDS attributes
- No custom CSS classes detected on GCDS components
- Semantic structure maintained

### Structure Validation:
- Form includes error summary component
- All inputs have proper labels
- Fieldset elements include legend attributes
```

## Publishing to NuGet (Optional)

### Build and Pack
```bash
dotnet pack GCFoundation.McpServer -c Release
```

### Publish to NuGet.org
```bash  
dotnet nuget push GCFoundation.McpServer/bin/Release/*.nupkg --api-key <your-api-key> --source https://api.nuget.org/v3/index.json
```

### Using from NuGet (After Publishing)
Update `.vscode/mcp.json` to use the published package:
```json
{
  "servers": {
    "GCFoundation.McpServer": {
      "type": "stdio",
      "command": "dnx", 
      "args": ["GCFoundation.McpServer@0.1.0-beta", "--yes"],
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

## Development and Maintenance

### Adding New Tools
1. Open `GCFoundation.McpServer/Tools/GCFoundationTools.cs`
2. Add new methods with the `[McpServerTool]` attribute
3. Include proper parameter descriptions using `[Description]` attributes
4. Build and test the changes

### Updating Component Support
The MCP tools automatically access the latest GCFoundation.Components functionality through project references. When components are updated, rebuild the MCP server to get the latest features.

### Testing Changes
```bash
# Build the project
dotnet build GCFoundation.McpServer

# Run for testing  
dotnet run --project GCFoundation.McpServer
```

## New Comprehensive Capabilities

### 🎯 **Complete Component Coverage**
- **80+ MCP Tools** covering every GCDS and FDCP component
- **Project Scaffolding**: Complete Program.cs, appsettings.json, and project structure generation
- **Form Validation**: Canadian-specific patterns (SIN, postal codes, phone numbers)
- **Markup Validation**: Compliance checking and accessibility validation
- **HTML Conversion**: Automatic conversion from standard HTML to GCDS components

### 🚀 **Advanced Features**
- **Data Tables**: Full Tabulator.js integration with search, sorting, pagination
- **Session Management**: Automatic session timeout and modal handling  
- **Multi-language Support**: Complete localization setup and configuration
- **Security**: Content Security Policy and secure cookie configuration
- **Modal Dialogs**: Advanced modal components with custom actions

### 🛠 **Development Productivity**
- **Quick Component Generator**: Fast creation of common patterns
- **Project Templates**: Complete scaffolding for new GC Foundation projects
- **Validation Helpers**: Built-in Canadian government form validation
- **Best Practices**: Automated compliance checking and guidance

## Benefits for Development Teams

### 1. **Dramatically Faster Development**
- AI assistants can generate complete project structures in seconds
- Instant creation of complex forms with full validation
- Advanced data tables with search/filter capabilities out-of-the-box
- No more manual component API lookups or documentation reading

### 2. **Complete Accessibility & Compliance**  
- All generated components automatically include proper ARIA attributes
- Built-in WCAG 2.1 AA compliance validation
- Government of Canada web standards automatically enforced
- Real-time markup validation for accessibility issues

### 3. **Enterprise-Grade Features**
- Advanced data components (Tabulator tables, advanced forms)
- Session management with automatic timeout handling
- Content Security Policy configuration
- Multi-language support with proper localization setup

### 4. **Knowledge Transfer & Training**
- New developers can immediately generate production-ready code
- Built-in guidance and best practices documentation
- Automatic conversion from standard HTML to government standards
- Comprehensive project scaffolding reduces learning curve

### 5. **Quality Assurance**
- Automated validation prevents compliance issues before deployment
- Consistent component usage across all team members
- Built-in Canadian government form validation patterns
- Error prevention through guided component generation

## Troubleshooting

### MCP Server Not Starting
1. Verify .NET 8.0+ is installed
2. Check that the project builds successfully: `dotnet build GCFoundation.McpServer`
3. Ensure all project references are restored: `dotnet restore`

### Tools Not Appearing in VS Code
1. Verify the `.vscode/mcp.json` file exists and is properly formatted
2. Restart VS Code
3. Check the VS Code output panel for MCP-related errors
4. Ensure GitHub Copilot extension is installed and enabled

### Generated Components Not Working
1. Verify you're using the latest GCDS/FDCP CSS and JavaScript files
2. Check that the component markup is placed within proper page structure
3. Ensure required dependencies are loaded (GCDS web components, styles)

## Next Steps

1. **Test with your development team** - Have developers try the example prompts
2. **Customize tools** - Add project-specific component generators if needed  
3. **Document team usage** - Create team-specific examples and best practices
4. **Consider publishing** - If useful beyond your organization, publish to NuGet for broader use

The MCP server is now ready for use and should significantly improve the developer experience when working with GC Foundation components!

