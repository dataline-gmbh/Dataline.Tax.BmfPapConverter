# Wir löschen die NuGet-Pakete mit integrierten Symbolen, um sie nicht
# automatisch zu veröffentlichen

$ErrorActionPreference = "Stop"

Get-ChildItem "*.symbols.nupkg" -Recurse | Remove-Item