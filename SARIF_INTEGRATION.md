# SARIF Integration with .NET CLI

This document explains how SARIF (Static Analysis Results Interchange Format) logs are generated and integrated into the Azure DevOps pipeline for the GCFoundation project.

## Overview

SARIF logs provide standardized static analysis results that can be consumed by Azure DevOps Security Scans tab and Microsoft Defender for Cloud. This implementation uses .NET CLI with Roslyn analyzers to generate comprehensive security and code quality reports.

## Implementation Details

### 1. Analyzer Configuration

All projects in the solution have been configured with the following:

```xml
<AnalysisMode>All</AnalysisMode>
<PackageReference Include="Microsoft.CodeAnalysis.Analyzers" Version="3.11.0">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
```

**Projects Updated:**
- `GCFoundation.Common`
- `GCFoundation.Components` (already had analyzers)
- `GCFoundation.Security`
- `GCFoundation.Web`

### 2. Pipeline Integration

The PR validation pipeline (`pr-validation-pipeline.yml`) includes the following SARIF generation steps:

#### Step 1: Create Directory
```yaml
- task: PowerShell@2
  displayName: 'Create CodeAnalysisLogs Directory'
  inputs:
    targetType: 'inline'
    script: |
      New-Item -ItemType Directory -Path "CodeAnalysisLogs" -Force
```

#### Step 2: Generate SARIF for Each Project
```yaml
- task: DotNetCoreCLI@2
  displayName: 'Generate SARIF Logs for [ProjectName]'
  inputs:
    command: 'build'
    projects: '$(projectPath)'
    arguments: '--configuration $(buildConfiguration) --verbosity normal --logger "BinaryLogger;LogFile=CodeAnalysisLogs/[ProjectName].sarif"'
```

#### Step 3: Run Roslyn Analyzers
```yaml
- task: DotNetCoreCLI@2
  displayName: 'Run Roslyn Analyzers with SARIF Output'
  inputs:
    command: 'custom'
    custom: 'build'
    arguments: '$(solution) --configuration $(buildConfiguration) --verbosity normal --logger "SarifLogger;LogFile=CodeAnalysisLogs/roslyn-analysis.sarif"'
```

### 3. Analysis and Reporting

The pipeline includes comprehensive SARIF analysis that:

- **Counts Issues**: Tracks critical, warning, and total issues
- **Provides Summary**: Shows detailed breakdown of findings
- **Generates Reports**: Creates human-readable summaries
- **Publishes Artifacts**: Makes SARIF files available in Azure DevOps

### 4. Artifact Publishing

SARIF files are published to the `CodeAnalysisLogs` artifact, which is specifically monitored by:

- **Azure DevOps Security Scans Tab**: Automatically displays SARIF results
- **Microsoft Defender for Cloud**: Monitors for security findings
- **SARIF SAST Scans Tab Extension**: Provides enhanced visualization

## Generated SARIF Files

The pipeline generates the following SARIF files:

1. `GCFoundation.Common.sarif` - Analysis for the Common library
2. `GCFoundation.Components.sarif` - Analysis for the Components library
3. `GCFoundation.Security.sarif` - Analysis for the Security module
4. `GCFoundation.Web.sarif` - Analysis for the Web application
5. `roslyn-analysis.sarif` - Comprehensive Roslyn analyzer results

## Usage

### Viewing Results in Azure DevOps

1. **Security Scans Tab**: Navigate to your pipeline run and click on the "Security" tab
2. **Artifacts**: Download the `CodeAnalysisLogs` artifact to view raw SARIF files
3. **Pipeline Logs**: Check the "Analyze SARIF Results" step for a summary

### Local Development

To generate SARIF logs locally:

```bash
# Generate SARIF for a specific project
dotnet build GCFoundation.Common/GCFoundation.Common.csproj --configuration Release --verbosity normal --logger "BinaryLogger;LogFile=CodeAnalysisLogs/GCFoundation.Common.sarif"

# Generate comprehensive analysis
dotnet build GCFoundation.sln --configuration Release --verbosity normal --logger "SarifLogger;LogFile=CodeAnalysisLogs/roslyn-analysis.sarif"
```

### Customizing Analysis

#### Adding Custom Analyzers

To add additional analyzers, update the project files:

```xml
<ItemGroup>
  <PackageReference Include="YourCustomAnalyzer" Version="1.0.0">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
  </PackageReference>
</ItemGroup>
```

#### Configuring Analysis Rules

Modify the `AnalysisMode` property in project files:

```xml
<PropertyGroup>
  <AnalysisMode>All</AnalysisMode>  <!-- All rules -->
  <!-- OR -->
  <AnalysisMode>Default</AnalysisMode>  <!-- Default rules only -->
  <!-- OR -->
  <AnalysisMode>None</AnalysisMode>  <!-- Disable analysis -->
</PropertyGroup>
```

## Benefits

### Security
- **Vulnerability Detection**: Identifies potential security issues
- **Code Quality**: Ensures consistent coding standards
- **Compliance**: Supports security compliance requirements

### Integration
- **Azure DevOps**: Native integration with Security Scans tab
- **Microsoft Defender**: Automatic monitoring and alerting
- **Standard Format**: SARIF is an industry standard format

### Developer Experience
- **Automated Analysis**: No manual intervention required
- **Clear Reporting**: Easy-to-understand summaries
- **PR Integration**: Automatic validation on pull requests

## Troubleshooting

### Common Issues

1. **No SARIF Files Generated**
   - Check that analyzers are properly installed
   - Verify `AnalysisMode` is set to `All`
   - Ensure build succeeds without errors

2. **Missing Security Scans Tab**
   - Verify SARIF files are in `CodeAnalysisLogs` artifact
   - Check that the SARIF SAST Scans Tab extension is installed
   - Ensure pipeline has proper permissions

3. **Analysis Not Running**
   - Check that `Microsoft.CodeAnalysis.Analyzers` package is installed
   - Verify project files have correct analyzer configuration
   - Ensure .NET SDK version is compatible

### Debugging

To debug SARIF generation:

```bash
# Enable detailed logging
dotnet build --verbosity detailed --logger "BinaryLogger;LogFile=debug.sarif"

# Check analyzer output
dotnet build --verbosity normal --logger "console;verbosity=detailed"
```

## References

- [SARIF Specification](https://docs.oasis-open.org/sarif/sarif/v2.1.0/sarif-v2.1.0.html)
- [Azure DevOps Security Scans](https://docs.microsoft.com/en-us/azure/devops/pipelines/tasks/security/security-scanning?view=azure-devops)
- [Microsoft Code Analysis](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/)
- [Roslyn Analyzers](https://github.com/dotnet/roslyn-analyzers)

## Support

For issues or questions regarding SARIF integration:

1. Check the pipeline logs for detailed error messages
2. Verify analyzer package versions are compatible
3. Ensure all projects have proper analyzer configuration
4. Review the generated SARIF files for specific issues 