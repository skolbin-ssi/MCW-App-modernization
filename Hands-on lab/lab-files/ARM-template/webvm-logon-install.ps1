function Wait-Install {
    $msiRunning = 1
    $msiMessage = ""
    while($msiRunning -ne 0)
    {
        try
        {
            $Mutex = [System.Threading.Mutex]::OpenExisting("Global\_MSIExecute");
            $Mutex.Dispose();
            $DST = Get-Date
            $msiMessage = "An installer is currently running. Please wait...$DST"
            Write-Host $msiMessage 
            $msiRunning = 1
        }
        catch
        {
            $msiRunning = 0
        }
        Start-Sleep -Seconds 1
    }
}

# Install App Service Migration Assistant
Wait-Install
Write-Host "Installing App Service Migration Assistant..."
Start-Process -file 'C:\AppServiceMigrationAssistant.msi ' -arg '/qn /l*v C:\asma_install.txt' -passthru | wait-process

# Install Edge
Wait-Install
Write-Host "Installing Edge..."
Start-Process -file 'C:\MicrosoftEdgeEnterpriseX64.msi' -arg '/qn /l*v C:\edge_install.txt' -passthru | wait-process

# Install .NET Core 3.1 SDK
Wait-Install
Write-Host "Installing .NET Core 3.1 SDK..."
$pathArgs = {C:\dotnet-sdk-3.1.406-win-x64.exe /Install /Quiet /Norestart /Logs logCore31SDK.txt}
Invoke-Command -ScriptBlock $pathArgs

Unregister-ScheduledTask -TaskName "Install Lab Requirements" -Confirm:$false
