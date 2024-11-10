# curl -X POST http://localhost:8080/game \
# -H "x-api-key: 05ae5782-1936-4c6a-870b-f3d64089dcf5" \
# -H "Content-Type: application/json" \
# --data @GameInputBeforeSubmit.json

# $content = (Get-Content "C:\git\considition-starterkit\Considition-2024\Cs\CsharpStarterkit\bin\Debug\net8.0\GameInputBeforeSubmit.json" -Raw)
$content = (Get-Content "C:\git\considition_2024\src\optimizer\bin\Debug\net8.0\finalGameInput - Copy.json" -Raw)


$response = Invoke-WebRequest -Uri "http://localhost:8080/game" `
    -Method Post `
    -Headers @{
    "x-api-key"    = "05ae5782-1936-4c6a-870b-f3d64089dcf5"
    "Content-Type" = "application/json"
} `
    -Body $content
Write-Host "Response from curl:"
Write-Host $response.Content
Write-Host "done"


$response = Invoke-WebRequest -Uri "http://localhost:8080/game" `
    -Method Post `
    -Headers @{
    "x-api-key"    = "05ae5782-1936-4c6a-870b-f3d64089dcf5"
    "Content-Type" = "application/json"
} `
    -Body $content
Write-Host "Response from curl:"
Write-Host $response.Content
Write-Host "done"

