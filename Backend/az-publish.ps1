Remove-Item -Path .\publish -Recurse -Force
Remove-Item -Path .\publish.zip -Force

# This script builds and publishes a .NET application to an Azure Web App
# Targeting a Linux environment without self-contained deployment
dotnet publish -c Release -o ./publish --self-contained false

# Compress the publish directory into a zip file
Compress-Archive -Path .\publish\* -DestinationPath .\publish.zip

# Deploy the zip file to Azure Web App
# Ensure you have the Azure CLI installed and logged in
az webapp deploy -g ArtBooking -n Artbook-1 --src-path .\publish.zip