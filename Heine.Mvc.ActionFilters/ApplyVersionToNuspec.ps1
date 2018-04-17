##-----------------------------------------------------------------------
## <copyright file="ApplyVersionToNuspec.ps1">(c) Microsoft Corporation. This source is subject to the Microsoft Permissive License. See http://www.microsoft.com/resources/sharedsource/licensingbasics/sharedsourcelicenses.mspx. All other rights reserved.</copyright>
##-----------------------------------------------------------------------
# Look for a 0.0.0.0 pattern in the build number. 
# If found use it to version the assemblies.
#
# For example, if the 'Build number format' build process parameter 
# $(BuildDefinitionName)_$(Year:yyyy).$(Month).$(DayOfMonth)$(Rev:.r)
# then your build numbers come out like this:
# "Build HelloWorld_2013.07.19.1"
# This script would then apply version 2013.07.19.1 to your assemblies.

# Make sure path to source code directory is available
if (-not (Test-Path $Env:BUILD_SOURCESDIRECTORY))
{
    Write-Error "Source directory does not exist: $Env:BUILD_SOURCESDIRECTORY"
    exit 1
}
Write-Verbose -Verbose "Source Directory: $Env:BUILD_SOURCESDIRECTORY"
Write-Verbose -Verbose "Version Number/Build Number: $Env:PACKAGE_VERSION"

# Apply the version to the assembly property files
$filename = "$Env:PACKAGE_NAME.nuspec"
$path = "$Env:BUILD_SOURCESDIRECTORY\$filename"
$file = Get-Item $path
$xml = [xml](Get-Content $file -Encoding UTF8)
if($xml)
{
    Write-Verbose -Verbose "Will apply '$Env:PACKAGE_VERSION' to nuspec file: '$filename'."

    # we use this format to we ignore any namespace settings at the package level
    $xml.SelectSingleNode("/*[local-name()='package']/metadata/version")
    $xml.package.metadata.version = [string]$Env:PACKAGE_VERSION    
    $xml.Save($file)
    write-verbose -Verbose "Updated the nusepc file: '$filename' with the version: '$Env:PACKAGE_VERSION'"
}
else
{
    Write-Warning "Found no nusepc file: '$filename'."
}