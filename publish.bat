:::: Compile to Gateway, Watchout for Release or Debug mode

dotnet publish -r linux-arm /p:ShowLinkerSizeConmparison=true

pushd .\bin\debug\netcoreapp3.1\linux-arm\publish

:: ���w�� Gateway IP
pscp -pw pi -v -r .\* pi@192.168.29.16:/home/pi/sandbox/forahostdll

:: �ƻs��M��| config
:: pscp -pw pi -v -r .\appsettings.cq.json pi@192.168.29.16:/home/pi/hdg-app/HDGMessage/appsettings.json

popd