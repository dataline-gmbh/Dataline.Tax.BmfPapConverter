# Wir löschen die NuGet-Pakete mit integrierten Symbolen, um sie nicht
# automatisch zu veröffentlichen

$ErrorActionPreference = "Stop"

$srcDir = Resolve-Path ".\src\"

Get-ChildItem "$srcDir\*\bin\Release\*.symbols.nupkg" | Remove-Item