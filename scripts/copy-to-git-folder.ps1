$srcFodler = "C:\temp\project\LocalHost"
$dstFolder = "C:\git\considition_2024\decompiled_server_project\LocalHost"

$excludeItems = @("bin", "lib", "obj", "LocalHost.sln", "LocalHost.csproj")

Get-ChildItem -Path $srcFodler -Recurse | Where-Object {
    $excludeItems -notcontains $_.Name -and $_.Extension -ne ".dll"
} | Copy-Item -Destination $dstFolder -Recurse -Force
