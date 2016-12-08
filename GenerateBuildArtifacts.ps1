# Generatorskript für Dataline.Tax.BmfPapConverter
# Generiert aus allen PAP in .\data Projekte, buildet sie und verschiebt sie in das Rootprojekt

$ErrorActionPreference = "Stop"

$targetProjectVersion = "1.0.0-*"

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

Write-Progress -Activity "Vorbereitung" -Status "Importiere Modul"

Import-Module ".\src\Dataline.Tax.BmfPapConverter.Cmdlets\bin\Release\net46\Dataline.Tax.BmfPapConverter.Cmdlets.dll"
CheckExitCode

$srcDir = Resolve-Path ".\src\"
$testDir = Resolve-Path ".\test\"
$targetDir = Resolve-Path ".\data\"

foreach ($jahr in Get-ChildItem $targetDir) {
    $name = "Dataline.Tax.LSt$jahr"
    $currentDir = Join-Path -Path $targetDir -ChildPath $jahr
    $papPath = Join-Path -Path $currentDir -ChildPath "pap.xml"
    $outDir = Join-Path -Path $currentDir -ChildPath "out"
    $resultSrc = Join-Path -Path (Join-Path -Path $outDir -ChildPath "src") -ChildPath $name
    $testSrc = Join-Path -Path (Join-Path -Path $outDir -ChildPath "test") -ChildPath "$name.Test"

    $testCsvs = Get-ChildItem (Join-Path -Path $currentDir -ChildPath "test-*.csv")
    
    Write-Progress -Activity $name -Status "Konvertiere PAP"
    Convert-BmfPap -PapPath $papPath -OutputDirectory $outDir -Namespace $name -TestDataPaths $testCsvs -ProjectAuthor "DATALINE GmbH & Co. KG" -ProjectCopyright "2016 DATALINE GmbH & Co. KG" -ProjectDescription "BMF-PAP Lohnsteuerberechnung $jahr" -ProjectVersion $targetProjectVersion
    CheckExitCode
    
    Write-Progress -Activity $name -Status "Verschiebe Artifakte in Zielverzeichnis"
    Move-Item -Path $resultSrc -Destination $srcDir
    Move-Item -Path $testSrc -Destination $testDir
}
