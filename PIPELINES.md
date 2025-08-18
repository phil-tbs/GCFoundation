# Azure Pipelines for GCFoundation

This document describes the Azure pipelines available for the GCFoundation project.

## Pipeline Files

### 1. `foundation-azure-pipelines.yml` (Main Build Pipeline)
**Purpose**: Main CI/CD pipeline for building, packaging, and publishing the application
- **Triggers**: On push to `master` and `develop` branches
- **Actions**: 
  - Builds all projects (Common, Components, Security) in parallel
  - Creates NuGet packages with versioning
  - Performs comprehensive security scanning (OWASP, vulnerability checks, code analysis)
  - Publishes packages to Azure Artifacts feed
- **Usage**: Automatically runs on main branch pushes
- **Features**:
  - Parallel build matrix for efficiency
  - Advanced security scanning with multiple tools
  - Package versioning with beta suffix for non-master branches
  - Comprehensive vulnerability and dependency analysis

### 2. `pr-validation-pipeline.yml` (PR Validation Pipeline)
**Purpose**: Validates pull requests with comprehensive build, test, and security validation
- **Triggers**: On pull requests to `master` and `develop` branches
- **Actions**:
  - Builds all projects (Common, Components, Security, Web) in parallel
  - Runs unit tests with enhanced coverage analysis
  - Performs .NET built-in security scanning
  - Publishes test results, code coverage, and security reports
  - Provides PR-specific validation summary
- **Usage**: Automatically runs on PR creation/updates
- **Features**:
  - Parallel build matrix for all projects
  - Enhanced code coverage with multiple formats (Cobertura, OpenCover, LCOV, JSON)
  - PR-specific coverage validation with thresholds
  - .NET built-in vulnerability, deprecated, and outdated package checks
  - SARIF reporting for Azure DevOps Scans tab
  - Comprehensive PR validation summary

## Pipeline Configuration

### Prerequisites
- Azure DevOps project with self-hosted agent pool: `TBS Self Hosted`
- .NET 8.0 SDK
- Node.js 22.x (for Foundation.Components build)
- NuGet package manager

### Key Features

#### Build Validation
- ✅ Builds all project components in parallel
- ✅ Validates solution integrity across all projects
- ✅ Ensures no compilation errors
- ✅ Handles NPM dependencies for Foundation.Components

#### Test Execution (PR Pipeline)
- 🧪 Runs xUnit tests from `GCFoundation.Tests.Components`
- 📊 Publishes test results to Azure DevOps with detailed logging
- 📈 Generates comprehensive code coverage reports
- 🔍 Provides PR-specific coverage analysis and validation
- 📋 Creates PR coverage comment data for integration

#### Code Coverage (PR Pipeline)
- Uses Coverlet collector for accurate coverage measurement
- Generates multiple formats: Cobertura, OpenCover, LCOV, JSON
- Publishes coverage data to Azure DevOps
- Shows line and branch coverage metrics with PR-specific thresholds
- Provides coverage validation and recommendations

#### Security Scanning
- **Main Pipeline**: Advanced security scanning with OWASP Dependency Check
- **PR Pipeline**: .NET built-in security tools (vulnerability, deprecated, outdated checks)
- **Dependency Scanning**: Checks for vulnerable, deprecated, and outdated NuGet packages
- **Code Analysis**: Static Application Security Testing with pattern matching
- **Vulnerability Detection**: Scans for common security issues and hardcoded secrets
- **Advanced Analysis**: SQL injection, XSS, and insecure deserialization pattern detection

## Usage Instructions

### For Pull Requests
1. Create a pull request to `master` or `develop`
2. The following pipeline will automatically trigger:
   - `pr-validation-pipeline.yml` - Comprehensive build, test, and security validation

### For Main Branch Commits
1. Push directly to `master` or `develop`
2. The following pipeline will automatically trigger:
   - `foundation-azure-pipelines.yml` - Build, package, security scan, and publish

### Manual Pipeline Execution
You can manually trigger any pipeline from Azure DevOps:
1. Go to Pipelines in Azure DevOps
2. Select the desired pipeline
3. Click "Run pipeline"
4. Choose branch and parameters

### Pipeline Results
- **Build Status**: Check if all projects compile successfully
- **Test Results**: View test execution results and coverage (PR pipeline)
- **Security Reports**: Review security analysis and vulnerability reports
- **Artifacts**: Download build outputs, test reports, and security analysis

## Pipeline Variables

| Variable | Description | Default Value |
|----------|-------------|---------------|
| `buildConfiguration` | Build configuration | `Release` |
| `solution` | Solution file pattern | `**/*.sln` |
| `testProject` | Test project path | `GCFoundation.Tests.Components/GCFoundation.Tests.Components.csproj` |
| `componentsProject` | Components project path | `GCFoundation.Components/CGFoundation.Components.csproj` |
| `commonProject` | Common project path | `GCFoundation.Common/GCFoundation.Common.csproj` |
| `securityProject` | Security project path | `GCFoundation.Security/GCFoundation.Security.csproj` |
| `webProject` | Web project path | `GCFoundation.Web/GCFoundation.Web.csproj` |
| `outputDir` | NuGet package output directory | `$(Build.ArtifactStagingDirectory)/nuget` |
| `nugetSource` | NuGet feed name | `TBS_Custom_Feed` |

## Pipeline Stages

### Main Pipeline (`foundation-azure-pipelines.yml`)
1. **Build Stage**: Parallel build matrix for all projects
2. **Security Stage**: Advanced security scanning and analysis
3. **Package Stage**: NuGet package publishing (non-PR builds only)

### PR Pipeline (`pr-validation-pipeline.yml`)
1. **Build Stage**: Parallel build matrix for all projects
2. **Test Stage**: Test execution, coverage analysis, and security scanning

## Troubleshooting

### Common Issues

1. **Build Failures**
   - Check if all dependencies are restored
   - Verify .NET SDK version compatibility
   - Ensure all project references are valid
   - For Components project: Verify NPM packages are installed

2. **Test Failures**
   - Review test output for specific failure reasons
   - Check if test dependencies are properly configured
   - Verify test project builds successfully
   - **Common Fix**: Ensure test project has `<IsTestProject>true</IsTestProject>` property
   - **Common Fix**: Remove `<OutputType>Exe</OutputType>` from test project (should be Library)

3. **Pipeline Not Triggering**
   - Ensure branch names match trigger conditions
   - Check if pipeline is enabled in Azure DevOps
   - Verify agent pool availability

4. **Test Discovery Issues**
   - Verify test project references the correct test framework packages
   - Check that test classes and methods have proper attributes ([Fact], [Test], etc.)
   - Ensure test project builds without errors

5. **Security Scanning Issues**
   - Check if security tools are properly installed
   - Verify network access for OWASP Dependency Check downloads
   - Review security analysis logs for specific error messages

### Local Troubleshooting

Run the included `test-troubleshoot.ps1` script to diagnose test issues locally:

```powershell
./test-troubleshoot.ps1
```

This script will:
- ✅ Check .NET SDK availability
- 📁 Verify test project and files exist
- 📦 Restore NuGet packages
- 🔨 Build the test project
- 🧪 Run tests with different verbosity levels

### Test Project Configuration

Ensure your test project has the correct configuration:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <IsTestProject>true</IsTestProject>
    <!-- Remove <OutputType>Exe</OutputType> if present -->
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.13.0" />
    <PackageReference Include="xunit" Version="2.4.2" />
    <PackageReference Include="xunit.runner.visualstudio" Version="2.4.5" />
  </ItemGroup>
</Project>
```

### Pipeline Diagnostics

The updated pipelines include diagnostic steps that will:
- 🔍 Check test project file existence
- 📋 List all test files found
- 📊 Verify test results generation
- 🎯 Provide detailed error information
- 🔒 Generate security analysis reports
- 📈 Create coverage analysis with PR-specific validation

### Support
For pipeline issues or questions, contact the development team or check Azure DevOps pipeline logs for detailed error information. 