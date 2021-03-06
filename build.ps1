Remove-Item -r publish

dotnet publish -c Release -r ubuntu.22.04-arm64 -o publish --self-contained --framework net6.0 Pyto

Set-Location Pyto.Client

yarn build --outDir ..\publish\wwwroot

Set-Location .\..