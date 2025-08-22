# GC Foundation MCP Server - Automated Test Runner
param(
    [string]$TestCategory = "all",
    [switch]$Coverage = $false,
    [switch]$Verbose = $false,
    [switch]$ContinousIntegration = $false
)

Write-Host "🧪 GC Foundation MCP Server - Automated Test Suite" -ForegroundColor Cyan
Write-Host "=================================================" -ForegroundColor Cyan

# Test categories are handled via command line filter parameter

# Build the test project first
Write-Host "`n🔨 Building test project..." -ForegroundColor Yellow
dotnet build GCFoundation.Tests.McpServer --no-restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Build failed!" -ForegroundColor Red
    exit 1
}

# Determine which tests to run
$testFilter = switch ($TestCategory.ToLower()) {
    "unit" { "--filter Category=Unit" }
    "integration" { "--filter Category=Integration" }
    "validation" { "--filter Category=Validation" }
    "all" { "" }
    default { "" }
}

# Configure test arguments
$testArgs = @(
    "test"
    "GCFoundation.Tests.McpServer"
    "--logger", "console;verbosity=normal"
    "--results-directory", "TestResults"
)

if ($testFilter) {
    $testArgs += $testFilter
}

if ($Coverage) {
    Write-Host "📊 Code coverage enabled" -ForegroundColor Green
    $testArgs += "--collect", "XPlat Code Coverage"
}

if ($Verbose) {
    $testArgs += "--verbosity", "detailed"
}

if ($ContinousIntegration) {
    $testArgs += "--logger", "trx"
    $testArgs += "--logger", "junit"
}

# Run the tests
Write-Host "`n🏃 Running tests..." -ForegroundColor Yellow
Write-Host "Category: $TestCategory" -ForegroundColor Gray
Write-Host "Coverage: $Coverage" -ForegroundColor Gray

$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()

& dotnet @testArgs

$exitCode = $LASTEXITCODE
$stopwatch.Stop()

# Display results
Write-Host "`n📈 Test Results Summary" -ForegroundColor Cyan
Write-Host "======================" -ForegroundColor Cyan
Write-Host "Execution Time: $($stopwatch.Elapsed.TotalSeconds.ToString('F2')) seconds" -ForegroundColor Gray

if ($exitCode -eq 0) {
    Write-Host "✅ All tests passed!" -ForegroundColor Green
} else {
    Write-Host "❌ Some tests failed!" -ForegroundColor Red
}

# Generate coverage report if requested
if ($Coverage -and $exitCode -eq 0) {
    Write-Host "`n📊 Generating coverage report..." -ForegroundColor Yellow
    
    # Look for coverage files
    $coverageFiles = Get-ChildItem -Path "TestResults" -Filter "coverage.cobertura.xml" -Recurse -ErrorAction SilentlyContinue
    
    if ($coverageFiles.Count -gt 0) {
        Write-Host "📋 Found coverage data in:" -ForegroundColor Green
        $coverageFiles | ForEach-Object { Write-Host "  • $($_.FullName)" -ForegroundColor Gray }
        
        if (Get-Command reportgenerator -ErrorAction SilentlyContinue) {
            reportgenerator `
                "-reports:TestResults/**/coverage.cobertura.xml" `
                "-targetdir:TestResults/CoverageReport" `
                "-reporttypes:Html;Badges"
                
            Write-Host "📋 Coverage report: TestResults/CoverageReport/index.html" -ForegroundColor Green
        } else {
            Write-Host "⚠️  Install reportgenerator for HTML coverage reports: dotnet tool install -g dotnet-reportgenerator-globaltool" -ForegroundColor Yellow
        }
    } else {
        Write-Host "⚠️  No coverage data found. Make sure coverlet.collector is included in test project." -ForegroundColor Yellow
    }
}

# Performance benchmark summary
Write-Host "`n⚡ Quick Performance Check" -ForegroundColor Cyan
Write-Host "=========================" -ForegroundColor Cyan
dotnet run --project GCFoundation.McpServer -- --version 2>$null
if ($LASTEXITCODE -eq 0) {
    Write-Host "✅ MCP Server starts successfully" -ForegroundColor Green
} else {
    Write-Host "❌ MCP Server failed to start" -ForegroundColor Red
}

# Tool count verification
Write-Host "🔧 Available Tools Count:" -ForegroundColor Cyan
$toolCount = (Get-Content "GCFoundation.McpServer/Tools/*.cs" | Select-String "\[McpServerTool\]").Count
Write-Host "$toolCount MCP tools registered" -ForegroundColor Gray

Write-Host "`n🎯 Test Categories Available:" -ForegroundColor Cyan
Write-Host "  • unit        - Unit tests for individual tools" -ForegroundColor Gray
Write-Host "  • integration - MCP protocol communication tests" -ForegroundColor Gray  
Write-Host "  • validation  - Component generation validation" -ForegroundColor Gray
Write-Host "  • all         - Run all test categories" -ForegroundColor Gray

Write-Host "`n📝 Usage Examples:" -ForegroundColor Cyan
Write-Host "  .\run-mcp-tests.ps1 -TestCategory unit" -ForegroundColor Gray
Write-Host "  .\run-mcp-tests.ps1 -Coverage" -ForegroundColor Gray
Write-Host "  .\run-mcp-tests.ps1 -TestCategory integration -Verbose" -ForegroundColor Gray

exit $exitCode
