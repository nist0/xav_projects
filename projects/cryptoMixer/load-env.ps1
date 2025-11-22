# Helper script to load local environment variables for cryptoMixer projects
# Usage (in PowerShell):
#   cd C:\Users\yanis\source\repos\xav_projects\projects\cryptoMixer
#   . .\load-env.ps1

$envFile = Join-Path $PSScriptRoot ".env.local"
if (-not (Test-Path $envFile)) {
    Write-Warning ".env.local not found at $envFile. Nothing to load."
    return
}

Get-Content $envFile | ForEach-Object {
    $line = $_.Trim()
    if (-not $line -or $line.StartsWith('#')) { return }

    $parts = $line -split '=', 2
    if ($parts.Count -eq 2) {
        $name  = $parts[0].Trim()
        $value = $parts[1].Trim()
        $env:$name = $value
    }
}

Write-Host "Environment variables from .env.local have been loaded into this PowerShell session."