# BmfPapConverter
**PowerShell-Cmdlets zum Erstellen von .NET-Bibliotheken aus Programmablaufplänen des BMF-Lohnsteuerrechners**

[![TeamCity (simple build status)](https://img.shields.io/teamcity/https/build.service-dataline.de/s/OpenSource_DatalineTaxBmfPapConverter.svg)]()

Dieses Repository enthält PowerShell-Cmdlets, mit denen die [XML-Programmablaufpläne des Lohnsteuerrechners](https://www.bmf-steuerrechner.de/) in mit .NET 4.5 und .NET Standard 1.3 kompatible Projekte konvertiert werden können.

Mit ```Convert-BmfPap``` kann ein Projekt erstellt werden; optional wird gleichzeitig ein Testprojekt erstellt, welches die erstellte Bibliothek mit Testdaten aus CSV-Dateien überprüft. Mit ```New-BmfTestData``` können Testdaten aus Lohnsteuer-Prüftabellen erzeugt werden.

Das Buildskript ```Build.ps1``` erstellt alle Projekte automatisch. Mit dem ```-Test```-Parameter können gleichzeitig die Tests ausgeführt werden. Mit dem ```-Version```-Parameter wird die Versionsnummer der erzeugten Projekte festgelegt. Mit dem ```-Pack```-Parameter werden NuGet-Pakete erzeugt.

Das Erstellen und Veröffentlichen der Pakete erledigt ein Script auf dem Build-Server. Es müssen nur der XML-Pseudocode und die Testdateien vorhanden sein.

### Erweiterungen

Der Converter kann die PAP-Ausgaben über Erweiterungen (Cmdlet-Parameter ```Extensions```) mit zusätzlichen Funktionen versehen.

**Tarifliche Einkommensteuer:** Das Buildskript erweitert die PAP-Bibliotheken um eine Methode ```TariflicheEinkommensteuer(zve, kztab)``` um die tarifliche Einkommensteuer des jeweiligen Jahres zu berechnen (Erweiterung ```TariflicheEinkommensteuer```); diese Methode wird über eine gesonderte Testtabelle getestet.

## Lohnsteuer NuGet-Pakete
Mit BmfPapConverter erstellte LSt-Bibliotheken sind ebenfalls als NuGet-Paket verfügbar.

* [![NuGet](https://img.shields.io/nuget/v/Dataline.Tax.LSt2016.svg)](https://www.nuget.org/packages/Dataline.Tax.LSt2016/) **LSt 2016**
* [![NuGet](https://img.shields.io/nuget/v/Dataline.Tax.LSt2017.svg)](https://www.nuget.org/packages/Dataline.Tax.LSt2017/) **LSt 2017**
* [![NuGet](https://img.shields.io/nuget/v/Dataline.Tax.LSt2018.svg)](https://www.nuget.org/packages/Dataline.Tax.LSt2018/) **LSt 2018**
* [![NuGet](https://img.shields.io/nuget/v/Dataline.Tax.LSt2019.svg)](https://www.nuget.org/packages/Dataline.Tax.LSt2019/) **LSt 2019**


## Erstellung der Testdaten
Der erste Schritt ist die Erstellung eines neuen Ordners für das betreffende Jahr. Die alten Ordner können in 20xx-old umbenannt werden.

Die Testdaten test-maschinell.csv entstammt der zum XML-Pseudocode hinzugefügten xlsx-Datei. Aus dieser werden alle Zeilen incl. der Überschriftzeile als csv-Datei exportiert.

Die Testdaten test-allgemein.csv und test-besondere.csv basieren auf den Tabellen, die sich am Ende des maschinellen Programmablaufplans des BMF befinden. Zum Erstellen der Dateien müssen die Werte aus den Tabellen kopiert werden und in ein importierbares Format umgewandelt werden. Dazu werden zuerst alle Punkte entfernt und danach alle Leerzeichen durch Kommas ersetzt.
Danach wird jede Zeile in () gesetzt, zum Schluss wird alles von () umschlossen. Diese Datei kann dann als Parameter an das Cmdlet übergeben werden.
