# Test-Runner-Skript für Dataline.Tax.BmfPapConverter
# Der Exit-Code bestimmt das Ergebnis des Tests (0 = Erfolg, 1 = Fehler)

function Fail([string] $message)
{
    echo "Tests fehlgeschlagen: $message"
    exit 1
}

function CheckExitCode()
{
    if (!$?) {
        Fail("Der vorausgegangene Befehl gab einen Fehler zurück.")
    }
}

echo "Kompiliere BmfPapConverter"

dotnet restore
CheckExitCode

dotnet build .\src\Dataline.Tax.BmfPapConverter.Cmdlets --Configuration Release
CheckExitCode

echo "Importiere Modul"

Import-Module .\src\Dataline.Tax.BmfPapConverter.Cmdlets\bin\Release\Dataline.Tax.BmfPapConverter.Cmdlets.dll
CheckExitCode

