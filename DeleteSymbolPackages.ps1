# Wir löschen die NuGet-Pakete mit integrierten Symbolen, um sie nicht
# automatisch zu veröffentlichen

$srcDir = Resolve-Path ".\src\"

Get-ChildItem "$srcDir\*\bin\Release\*.symbols.nupkg"