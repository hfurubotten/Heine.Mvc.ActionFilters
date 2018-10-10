SET MS_BUILD_DIR="C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin"

SET BUILD_SOURCESDIRECTORY=%cd%

SET BUILD_BUILDVERSION=3.0.5
SET BUILD_BUILDREVISION=1
SET BUILD_BUILDNUMBER=%BUILD_BUILDVERSION%.%BUILD_BUILDREVISION%

SET PACKAGE_NAME=Heine.Mvc.ActionFilters
::SET PACKAGE_VERSION=%BUILD_BUILDVERSION%
SET PACKAGE_VERSION=%BUILD_BUILDVERSION%-beta%BUILD_BUILDREVISION%

powershell.exe -executionpolicy bypass -file ApplyVersionToAssemblies.ps1
powershell.exe -NoProfile -ExecutionPolicy Bypass -File ApplyVersionToNuspec.ps1

%MS_BUILD_DIR%\MSBuild.exe %PACKAGE_NAME%.csproj /t:rebuild /p:configuration=release

nuget pack %PACKAGE_NAME%.nuspec -symbols

:: To be able to push the nuget package to nuget.org you need to set your apikey
:: "nuget setApiKey <personal_api_key>" outputs: "The API Key '<personal_api_key>' was saved for the NuGet gallery (https://www.nuget.org) and the symbol server (https://nuget.smbsrc.net/)."

nuget push %PACKAGE_NAME%.%PACKAGE_VERSION%.nupkg -Source https://api.nuget.org/v3/index.json
nuget push %PACKAGE_NAME%.%PACKAGE_VERSION%.symbols.nupkg -Source https://nuget.smbsrc.net