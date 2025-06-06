Remove-Item -Path .\publish -Recurse -Force
Remove-Item -Path .\publish.zip -Force
# This script builds and publishes a .NET application to an Azure Web App
# Targeting a Linux environment without self-contained deployment
dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish
# Compress the publish directory into a zip file
Compress-Archive -Path .\publish\* -DestinationPath .\publish.zip
# Deploy the zip file to Azure Web App
# Ensure you have the Azure CLI installed and logged in
az webapp deploy --resource-group ArtBooking --name artbook --src-path .\publish.zip
