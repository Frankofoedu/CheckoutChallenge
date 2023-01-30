
$ErrorActionPreference = 'Stop'

try {
    Set-PSDebug -Trace 1 # log commands before running https://stackoverflow.com/a/49647445

    
    $filepath = Join-Path $PSScriptRoot "appsettings.UnitTests.json"
    $appSettings = (Get-Content $filepath | Out-String | ConvertFrom-Json)

    $postgresConnection = $appSettings.ConnectionStrings.PostgresConnection

    Set-Location  -Path ./src/PaymentGateway.API/
    $output = .\efbundle.exe --connection $postgresConnection --verbose    #https://stackoverflow.com/a/61674353

    if ($LastExitCode -ne 0) { #catch efcore errors and log them
    
        Write-Output $output
        exit(1)
    }
}
Catch {
    $ErrorMessage = $_.Exception.Message
    Write-Output $ErrorMessage

    Write-Output $output
    exit(1)
}
Write-Output $output

