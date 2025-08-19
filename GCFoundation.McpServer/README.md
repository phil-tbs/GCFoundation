# GC Foundation MCP Server

A Model Context Protocol (MCP) server that provides AI assistants with tools to generate Government of Canada Design System (GCDS) and Federal Design Component Package (FDCP) components, forms, and layouts.

## Features

### GCDS Component Generation
- **Text Components**: Generate semantic text elements with proper sizing, roles, and margins
- **Button Components**: Create accessible buttons with various styles and behaviors  
- **Card Components**: Build structured card layouts with headers, bodies, and footers
- **Input Components**: Generate form inputs with built-in validation and accessibility
- **Breadcrumb Navigation**: Create navigation breadcrumb trails
- **Layout Components**: Generate grid systems and containers

### FDCP Component Generation
- **Flexible Cards**: Create advanced card components with image support
- **Badges**: Generate status and label badges
- **Data Tables**: Build advanced tabular data displays
- **Form Builders**: Dynamic form generation tools

### Additional Tools
- **Component Information**: Get detailed information about available components
- **Usage Examples**: Access practical implementation examples
- **Accessibility Features**: Built-in WCAG compliance support

## Available MCP Tools

### Core Foundation Tools
| Tool Name | Description |
|-----------|-------------|
| `QuickGCDSComponent` | Quick generator for common GCDS components |
| `ValidateGCDSMarkup` | Validates markup for GCDS compliance and accessibility |
| `ConvertToGCDS` | Converts HTML to GCDS component equivalents |
| `GenerateFormValidation` | Creates validation patterns for Canadian forms |
| `GetGCFoundationGuidance` | Comprehensive development guidance and best practices |

### Complete GCDS Component Suite (38+ Tools)
| Category | Tools Available |
|----------|-----------------|
| **Form Components** | Input, Select, Textarea, Radios, Checkboxes, Date Input, File Upload, Fieldset |
| **Navigation** | Breadcrumbs, Top Navigation, Side Navigation, Nav Groups, Pagination |
| **Layout** | Container, Grid, Header, Footer |
| **Content** | Text, Heading, Button, Link, Card, Details, Notice, Icon |
| **Utilities** | Error Message, Error Summary, Search, Language Toggle, Signature |

### Complete FDCP Component Suite (25+ Tools)
| Category | Tools Available |
|----------|-----------------|
| **Advanced Forms** | Enhanced inputs, selects, checkboxes with validation |
| **Data Components** | Tabulator tables, filter boxes with search capabilities |
| **UI Elements** | Cards with slots, badges, modals, session management |
| **Layout** | Page headers with breadcrumbs and actions |
| **Specialized** | Stepper components, form builders, error handling |

### Project Configuration Tools (15+ Tools)
| Tool Name | Description |
|-----------|-------------|
| `GenerateGCFoundationProgramCs` | Complete Program.cs with all middleware and services |
| `GenerateAppSettingsJson` | Full appsettings.json with Foundation configuration |
| `GenerateProjectFile` | .csproj with all necessary GC Foundation dependencies |
| `GenerateBaseController` | Foundation-based controller templates |
| `GenerateFoundationLayoutView` | Complete layout views with GCDS structure |
| `GenerateSampleFormView` | Form examples with validation and components |
| `GenerateProjectStructure` | Complete directory structure for new projects |
| `GetProjectConfigurationInfo` | Setup guidance and configuration help |

## Installation

### From NuGet (Coming Soon)
```bash
dotnet tool install -g GCFoundation.McpServer
```

### From Source
1. Clone the repository
2. Build the project:
   ```bash
   dotnet build GCFoundation.McpServer
   ```

## Configuration

### VS Code with GitHub Copilot

Create a `.vscode/mcp.json` file in your project:

```json
{
  "servers": {
    "GCFoundation.McpServer": {
      "type": "stdio",
      "command": "dotnet",
      "args": [
        "run",
        "--project",
        "path/to/GCFoundation.McpServer"
      ],
      "env": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

### From NuGet Package (Coming Soon)

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

## Usage Examples

### Generate a GCDS Text Component
Ask your AI assistant:
> "Generate a large heading text using GCDS components"

This will use the `GenerateGCDSText` tool to create:
```html
<gcds-text size="h2" text-role="primary" display="block">Your Heading Text</gcds-text>
```

### Create a Form Input
Ask your AI assistant:
> "Create a required email input field with validation"

This will generate:
```html
<div class="form-group">
  <gcds-label for="email">Email Address *</gcds-label>
  <gcds-input name="email" id="email" type="email" required="true"></gcds-input>
</div>
```

### Build Breadcrumb Navigation
Ask your AI assistant:
> "Generate breadcrumbs for Home > Services > Applications"

This creates:
```html
<gcds-breadcrumbs>
  <gcds-breadcrumbs-item href="/">Home</gcds-breadcrumbs-item>
  <gcds-breadcrumbs-item href="/services">Services</gcds-breadcrumbs-item>
  <gcds-breadcrumbs-item>Applications</gcds-breadcrumbs-item>
</gcds-breadcrumbs>
```

## Component Library Information

The MCP server provides access to the full GC Foundation component library:

- **GCDS Components**: Based on the official Government of Canada Design System
- **FDCP Components**: Federal Design Component Package for enhanced functionality  
- **Form Builders**: Dynamic form generation and validation tools
- **Layout System**: Grid systems, containers, and responsive design helpers

## Development

### Requirements
- .NET 8.0 or higher
- Visual Studio Code with GitHub Copilot (for testing)

### Building
```bash
dotnet build
```

### Testing
```bash
dotnet test
```

## Testing the MCP Server

### Local Development Testing

1. Ensure the `.vscode/mcp.json` file is configured correctly
2. Open GitHub Copilot in VS Code
3. Try these example prompts:
   - "Generate a GCDS button with primary styling"
   - "Create a card component with header and body"
   - "Build a form input for email address"
   - "Generate breadcrumbs for a three-level navigation"

### Verify Tools are Available

1. In GitHub Copilot, select the "Select tools" icon
2. Verify that "GCFoundation.McpServer" appears with all available tools
3. Test individual tools by referencing them by name in your prompts

## Publishing to NuGet.org

1. Update package version in the `.csproj` file
2. Create the NuGet package:
   ```bash
   dotnet pack -c Release
   ```
3. Publish to NuGet.org:
   ```bash
   dotnet nuget push bin/Release/*.nupkg --api-key <your-api-key> --source https://api.nuget.org/v3/index.json
   ```

## Contributing

Contributions are welcome! Please follow the established coding standards and include tests for new functionality.

## License

© Government of Canada - See license file for details.

## Related Links

- [Government of Canada Design System](https://design-system.alpha.canada.ca/)
- [Model Context Protocol](https://modelcontextprotocol.io/)
- [GC Foundation Documentation](https://github.com/gc-foundation/GCFoundation)
- [MCP .NET Guide](https://aka.ms/nuget/mcp/guide)