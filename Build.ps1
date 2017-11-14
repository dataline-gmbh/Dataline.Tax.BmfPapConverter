﻿# Generatorskript für Dataline.Tax.BmfPapConverter
# Generiert aus allen PAP in .\data Projekte, buildet sie und verschiebt sie in das Rootprojekt

Param(
    [switch]$Test
)

$ErrorActionPreference = "Stop"

$targetProjectVersion = "1.0.0"

function Fail([string] $message)
{
    Throw "Buildskript fehlgeschlagen: $message"
}

function CheckExitCode()
{
    if (!$?) {
        Fail("Der vorausgegangene Befehl gab einen Fehler zurück.")
    }
}

Write-Progress -Activity "Vorbereitung" -Status "Build"

dotnet build -c Release ".\src\Dataline.Tax.BmfPapConverter.Cmdlets\Dataline.Tax.BmfPapConverter.Cmdlets.csproj"
CheckExitCode

Write-Progress -Activity "Vorbereitung" -Status "Importiere Modul"

Import-Module ".\src\Dataline.Tax.BmfPapConverter.Cmdlets\bin\Release\net46\Dataline.Tax.BmfPapConverter.Cmdlets.dll"
CheckExitCode

$thisDir = Resolve-Path .
$generatedDir = Join-Path -Path $thisDir -ChildPath "generated"
$targetDir = Join-Path -Path $thisDir -ChildPath "data"

if (Test-Path $generatedDir) {
    Remove-Item -Recurse $generatedDir
}
    
New-Item -ItemType Directory -Path $generatedDir

foreach ($jahr in Get-ChildItem $targetDir) {
    $name = "Dataline.Tax.LSt$jahr"
    $currentDir = Join-Path -Path $targetDir -ChildPath $jahr
    $papPath = Join-Path -Path $currentDir -ChildPath "pap.xml"
    $outDir = Join-Path -Path $generatedDir -ChildPath $name

    $testCsvs = Get-ChildItem (Join-Path -Path $currentDir -ChildPath "test-*.csv")
    
    Write-Progress -Activity $name -Status "Konvertiere PAP"
    Convert-BmfPap -PapPath $papPath -OutputDirectory $outDir -Namespace $name -TestDataPaths $testCsvs -ProjectAuthor "DATALINE GmbH & Co. KG" -ProjectCopyright "2017 DATALINE GmbH & Co. KG" -ProjectDescription "BMF-PAP Lohnsteuerberechnung $jahr" -ProjectVersion $targetProjectVersion -ProjectTags "DATALINE", "$jahr"
    CheckExitCode

    if ($Test -and $jahr.Name -ne "2014") { # Der PAP 2014 unterstützt das Testprojekt nicht
        Write-Progress -Activity $name -Status "Ausführung Testprojekt"
        dotnet test (Join-Path -Path $outDir -ChildPath "$name.Test\$name.Test.csproj")
        CheckExitCode
    }
}