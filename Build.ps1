# Generatorskript für Dataline.Tax.BmfPapConverter
# Generiert aus allen PAP in .\data Projekte, buildet sie und verschiebt sie in das Rootprojekt

Param(
    [switch]$Test,
    [switch]$Pack,
    [switch]$RebuildExisting,
    [String]$Version="0.0.1",
    [String]$PackOutput="dist"
)

$ErrorActionPreference = "Stop"

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
$packDir = Join-Path -Path $thisDir -ChildPath $PackOutput

if (!(Test-Path $generatedDir)) {
    New-Item -ItemType Directory -Path $generatedDir
}

foreach ($jahr in Get-ChildItem -Directory $targetDir) {
    $name = "Dataline.Tax.LSt$jahr"
    $currentDir = Join-Path -Path $targetDir -ChildPath $jahr
    $papPath = Join-Path -Path $currentDir -ChildPath "pap.xml"
    $outDir = Join-Path -Path $generatedDir -ChildPath $name

    if (Test-Path $outDir) {
        if (!$RebuildExisting) {
            continue
        }

        Remove-Item -Recurse $outDir
    }

    $testCsvs = Get-ChildItem (Join-Path -Path $currentDir -ChildPath "test-*.csv")
    
    Write-Progress -Activity $name -Status "Konvertiere PAP"
    Convert-BmfPap -PapPath $papPath -OutputDirectory $outDir -Namespace $name -TestDataPaths $testCsvs -ProjectAuthor "DATALINE GmbH & Co. KG" -ProjectCopyright "2017 DATALINE GmbH & Co. KG" -ProjectDescription "BMF-PAP Lohnsteuerberechnung $jahr" -ProjectVersion $Version -ProjectTags "DATALINE", "$jahr"
    CheckExitCode

    if ($Test -and $jahr.Name -ne "2014") { # Der PAP 2014 unterstützt das Testprojekt nicht
        Write-Progress -Activity $name -Status "Ausführung Testprojekt"
        dotnet test (Join-Path -Path $outDir -ChildPath "$name.Test\$name.Test.csproj")
        CheckExitCode
    }

    if ($Pack) {
        Write-Progress -Activity $name -Status "Erstelle Paket"
        dotnet pack -c Release -o $packDir (Join-Path -Path $outDir -ChildPath "$name\$name.csproj")
    }
}
