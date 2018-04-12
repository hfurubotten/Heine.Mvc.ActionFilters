:: To be able to push the nuget package to nuget.org you need to set your apikey
:: "nuget setApiKey <personal_api_key>" => "The API Key 'personal_api_key>' was saved for the NuGet gallery (https://www.nuget.org) and the symbol server (https://nuget.smbsrc.net/)."

SET packagename=Heine.Mvc.ActionFilters
SET packageversion=3.0.0-beta07

nuget push %packagename%.%packageversion%.nupkg -Source https://api.nuget.org/v3/index.json
nuget push %packagename%.%packageversion%.symbols.nupkg -Source https://nuget.smbsrc.net