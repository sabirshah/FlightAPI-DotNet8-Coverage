$TestProject = "./UnitTests.Tests.csproj"
$OutputDir = "coverage-report"
$CoverageFile = "coverage.cobertura.xml"
$RunSettingsFile = "CodeCoverage.runsettings"

# Rebuild the test project
dotnet build $TestProject -c Debug

# Run tests with coverage and use runsettings
dotnet test $TestProject `
  --no-build `
  --settings $RunSettingsFile `
  --collect:"XPlat Code Coverage"

# Find the generated coverage file (adjust if needed)
$coveragePath = Get-ChildItem -Recurse -Filter $CoverageFile | Select-Object -First 1

if ($coveragePath -eq $null) {
    Write-Error "Coverage file not found!"
    exit 1
}

# Generate HTML report
reportgenerator -reports:$coveragePath.FullName -targetdir:$OutputDir -reporttypes:Html

# Open in browser
Start-Process "$OutputDir/index.html"
