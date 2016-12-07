# Generatorskript für Dataline.Tax.BmfPapConverter
# Generiert aus allen PAP in .\data Projekte, buildet sie und verschiebt sie in das Rootprojekt

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
    $testStatic = Join-Path -Path $currentDir -ChildPath "test-static.csv"
    $testTables = Join-Path -Path $currentDir -ChildPath "test-tables.csv"
    $resultSrc = Join-Path -Path (Join-Path -Path $outDir -ChildPath "src") -ChildPath $name
    $testSrc = Join-Path -Path (Join-Path -Path $outDir -ChildPath "test") -ChildPath "$name.Test"
    
    Write-Progress -Activity $name -Status "Konvertiere PAP"
    Convert-BmfPap -PapPath $papPath -OutputDirectory $outDir -Namespace $name -TestDataPaths $testStatic, $testTables -Extensions TariflicheEinkommensteuer -ProjectAuthor "DATALINE GmbH & Co. KG" -ProjectCopyright "2016 DATALINE GmbH & Co. KG" -ProjectDescription "BMF-PAP Lohnsteuerberechnung $jahr"
    CheckExitCode
    
    Write-Progress -Activity $name -Status "Verschiebe Artifakte in Zielverzeichnis"
    Move-Item -Path $resultSrc -Destination $srcDir
    Move-Item -Path $testSrc -Destination $testDir
}
