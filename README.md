# BmfPapConverter
**PowerShell-Cmdlets zum Erstellen von .NET-Bibliotheken aus Programmablaufplänen des BMF-Lohnsteuerrechners**

[![TeamCity (simple build status)](https://img.shields.io/teamcity/https/build.service-dataline.de:8081/s/OpenSource_DatalineTaxBmfPapConverter.svg)]()

Dieses Repository enthält PowerShell-Cmdlets, mit denen die [XML-Programmablaufpläne des Lohnsteuerrechners](https://www.bmf-steuerrechner.de/) in mit .NET 4.5 und .NET Standard 1.3 kompatible Projekte konvertiert werden können.

Mit ```Convert-BmfPap``` kann ein Projekt erstellt werden; optional wird gleichzeitig ein Testprojekt erstellt, welches die erstellte Bibliothek mit Testdaten aus CSV-Dateien überprüft. Mit ```New-BmfTestData``` können Testdaten aus Lohnsteuer-Prüftabellen erzeugt werden.

## Lohnsteuer NuGet-Pakete
Mit BmfPapConverter erstellte LSt-Bibliotheken sind ebenfalls als NuGet-Paket verfügbar.

* [![NuGet](https://img.shields.io/nuget/v/Dataline.Tax.LSt2016.svg)](https://www.nuget.org/packages/Dataline.Tax.LSt2016/) **LSt 2016**
* [![NuGet](https://img.shields.io/nuget/v/Dataline.Tax.LSt2017.svg)](https://www.nuget.org/packages/Dataline.Tax.LSt2017/) **LSt 2017**
