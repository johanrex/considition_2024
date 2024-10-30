# Get the container ID of the running container with the name "considition-2024-db-1"
$id = docker ps --filter "name=considition-2024-be-1" --format "{{.ID}}"

# Check if the container ID was found
if (-not $id) {
    Write-Error "Container 'considition-2024-db-1' not found."
    exit 1
}

# Copy the /app directory from the container to the local machine
docker cp ${id}:/app c:\temp

