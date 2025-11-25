# BmfPapConverter
**PowerShell-Cmdlets zum Erstellen von .NET-Bibliotheken aus Programmablaufplänen des BMF-Lohnsteuerrechners**

[![TeamCity (simple build status)](https://img.shields.io/teamcity/https/build.service-dataline.de/s/OpenSource_DatalineTaxBmfPapConverter.svg)]()

Dieses Repository enthält PowerShell-Cmdlets, mit denen die [XML-Programmablaufpläne des Lohnsteuerrechners](https://www.bmf-steuerrechner.de/) in mit .NET 4.5 und .NET Standard 1.3 kompatible Projekte konvertiert werden können.

Mit ```Convert-BmfPap``` kann ein Projekt erstellt werden; optional wird gleichzeitig ein Testprojekt erstellt, welches die erstellte Bibliothek mit Testdaten aus CSV-Dateien überprüft. Den gesamten Quellcode findet man dann im Verzeichnis generated.
Mit ```New-BmfTestData``` können Testdaten aus Lohnsteuer-Prüftabellen erzeugt werden.

Das Buildskript ```Build.ps1``` erstellt alle Projekte automatisch. 
* Mit dem ```-Test```-Parameter können gleichzeitig die Tests ausgeführt werden. 
* Mit dem ```-Version```-Parameter wird die Versionsnummer der erzeugten Projekte festgelegt. 
* Mit dem ```-Pack```-Parameter werden NuGet-Pakete erzeugt (nur für den Build-Script interessant).

Das Erstellen und Veröffentlichen der Pakete erledigt ein Script auf dem Build-Server. Es müssen nur der XML-Pseudocode und die Testdateien vorhanden sein.

### Erweiterungen

Der Converter kann die PAP-Ausgaben über Erweiterungen (Cmdlet-Parameter ```Extensions```) mit zusätzlichen Funktionen versehen.

**Tarifliche Einkommensteuer:** Das Buildskript erweitert die PAP-Bibliotheken um eine Methode ```TariflicheEinkommensteuer(zve, kztab)``` um die tarifliche Einkommensteuer des jeweiligen Jahres zu berechnen (Erweiterung ```TariflicheEinkommensteuer```); diese Methode wird über eine gesonderte Testtabelle getestet.

Für diese Option wird allerdings der neue BMF-Lohnsteuerrechner benötigt, der i.d.R. erst recht spät zur Verfügung steht. Die Erzeugung der Testdaten erfolgt durch Aufruf desselben.

## Lohnsteuer NuGet-Pakete
Mit BmfPapConverter erstellte LSt-Bibliotheken sind ebenfalls als NuGet-Paket verfügbar.

* [![NuGet](https://img.shields.io/nuget/v/Dataline.Tax.LSt2016.svg)](https://www.nuget.org/packages/Dataline.Tax.LSt2016/) **LSt 2016**
* [![NuGet](https://img.shields.io/nuget/v/Dataline.Tax.LSt2017.svg)](https://www.nuget.org/packages/Dataline.Tax.LSt2017/) **LSt 2017**
* [![NuGet](https://img.shields.io/nuget/v/Dataline.Tax.LSt2018.svg)](https://www.nuget.org/packages/Dataline.Tax.LSt2018/) **LSt 2018**
* [![NuGet](https://img.shields.io/nuget/v/Dataline.Tax.LSt2019.svg)](https://www.nuget.org/packages/Dataline.Tax.LSt2019/) **LSt 2019**
* **ab hier wurden die Pakete nicht mehr auf Nuget.org veröffentlicht**
* ![NuGet](https://img.shields.io/badge/nuget-v2.2.4-blue) **LSt 2020** 
* ![NuGet](https://img.shields.io/badge/nuget-v2.2.5-blue) **LSt 2021** 
* ![NuGet](https://img.shields.io/badge/nuget-v2.2.6-blue) **LSt 2022** 
* ![NuGet](https://img.shields.io/badge/nuget-v2.2.7-blue) **LSt 2022/06**


## Erstellung der Testdaten
Der erste Schritt ist die Erstellung eines neuen Data-Ordners für das betreffende Jahr. In diesen Data-Ordner gehören die Dateien
* pap.xml
* test-allgemein.csv
* test-besondere.csv
* test-maschinell.csv

Die alten Data-Ordner sollten in 20xx-old umbenannt werden, damit keine neue Artefakte erstellt werden. 

Die Datei **pap.xml** ist die neue Pseudo-XML-Datei vom Bundesministerium der Finanzen, die auf der o.g. Internetseite veröffentlicht wird. Diese sollte so wie sie ist in das Verzeichnis kopiert werden. Hinweis: Seitenquelltext in eine Datei namens pap.xml kopieren!

Die Testdaten **test-maschinell.csv** entstammt der (bis 2025?) zum XML-Pseudocode hinzugefügten xlsx-Datei. Aus dieser werden alle Zeilen incl. der Überschriftzeile als csv-Datei exportiert. Dazu einfach als csv-Datei speichern. Hinweis: für 2026 wurde dies wahrscheinlich eingestellt.

Die Testdaten **test-allgemein.csv** und **test-besondere.csv** basieren auf den Tabellen, die sich am Ende des PDFs des maschinellen Programmablaufplans des BMF befinden. 
Zum Erstellen der Dateien müssen die Werte aus den Tabellen kopiert werden und in ein importierbares Format umgewandelt werden. Dazu werden zuerst alle Punkte entfernt und danach alle Leerzeichen durch Kommas ersetzt.
Danach wird jede Zeile in () gesetzt, mit einem Komma von der nächsten getrennt, zum Schluss wird alles von () umschlossen. Hinweis: hierzu kann eine KI sehr gut verwendet werden.
Diese Datei muss dann mit Hilfe des Cmdlets in dasselbe Format wie die Datei test-maschinell umgewandelt werden. Dazu wird der Inhalt als Parameter übergeben.

Zur Verwendung des Cmdlets New-BmfTestData muss das Projekt BmfPapConverter.Cmdlets ausgeführt werden, welches die Powershell öffnet. Dort muss dann **import-module -Name .\Dataline.Tax.BmfPapConverter.Cmdlets.dll** aufgerufen werden, um das Cmdlet zu installieren.
Anschließend muss das Cmdlet aufgerufen werden:

Beispiel für allgemeine Tabelle: New-BmfTestData -Type Allgemein -Table ((5000,0,0,0,0,470,616), ...)

Beispiel für besondere Tabelle: New-BmfTestData -Type Besonders -Table ((5000,0,0,0,0,470,616), ...)
  
Danach wird noch der Parameter KVZ abgefragt, dessen Wert sich ebenfalls im PDF vom maschinellen Programmablaufplan unterhalb der allgemeinen Tabelle befindet. 
Ebenso wird der Name der Ausgabedatei für die zu erstellende Datei abgefragt.

**intern**

Nachdem diese 4 Dateien vorliegen, erledigt der Build-Server den Rest; man muss "nur" noch das Projekt in den master auf Github mergen, und dann die Erstellung auf TeamCity anstoßen. Als Ergebnis erhalten wir das gewünschte neue Nuget-Paket, dass wir im Server installieren müssen (Achtung bei der Quelle: dataline-alt!)
