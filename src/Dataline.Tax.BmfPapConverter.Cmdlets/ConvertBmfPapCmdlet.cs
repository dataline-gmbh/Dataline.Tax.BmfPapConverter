﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Dataline.Tax.BmfPapConverter.Cmdlets
{
    [Cmdlet(VerbsData.Convert, "BmfPap")]
    public class ConvertBmfPapCmdlet : Cmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Pfad des XML-PAP")]
        public FileInfo PapPath { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Ausgabeverzeichnis des Projekts")]
        public DirectoryInfo OutputDirectory { get; set; }

        [Parameter(HelpMessage = "Name der Eingabeklasse")]
        public string InputClassName { get; set; } = "Eingabeparameter";

        [Parameter(HelpMessage = "Name der Ausgabeklasse")]
        public string OutputClassName { get; set; } = "Ausgabeparameter";

        [Parameter(HelpMessage = "Name der Berechnungsklasse")]
        public string OperationClassName { get; set; } = "Berechnung";

        [Parameter(HelpMessage = "Name der Berechnungsmethode")]
        public string OperationMainMethodName { get; set; } = "Lohnsteuer";

        [Parameter(Mandatory = true, HelpMessage = "Namespace des zu generierenden Projekts")]
        public string Namespace { get; set; }

        [Parameter(HelpMessage = "Headerkommentar in jeder generierten Klasse")]
        public string FileHeader { get; set; }

        [Parameter(HelpMessage = "Version des generierten Projekts")]
        public string ProjectVersion { get; set; }

        [Parameter(HelpMessage = "Autor des generierten Projekts")]
        public string ProjectAuthor { get; set; }

        [Parameter(HelpMessage = "Copyright des generierten Projekts")]
        public string ProjectCopyright { get; set; }

        [Parameter(HelpMessage = "Beschreibung des generierten Projekts")]
        public string ProjectDescription { get; set; }

        [Parameter(HelpMessage = "CSV-Testdaten für das generierte Testprojekt")]
        public FileInfo[] TestDataPaths { get; set; }

        [Parameter(HelpMessage = "Die gewünschten Erweiterungen der Berechnungsklasse")]
        public string[] Extensions { get; set; }

        protected override void ProcessRecord()
        {
            XDocument pap;

            using (var papStream = PapPath.OpenRead())
            {
                pap = XDocument.Load(papStream);
            }

            var papDocument = new PapDocument(pap);

            var converter = new PapConverter
            {
                InputClassName = InputClassName,
                OutputClassName = OutputClassName,
                OperationClassName = OperationClassName,
                OperationMainMethodName = OperationMainMethodName,
                Namespace = Namespace,
                Extensions = Extensions
            };

            if (!string.IsNullOrEmpty(FileHeader))
                converter.FileHeader = FileHeader;

            var project = converter.GenerateProject(papDocument);
            project.Name = Namespace;

            if (ProjectVersion != null)
                project.Version = ProjectVersion;
            if (ProjectAuthor != null)
                project.Author = ProjectAuthor;
            if (ProjectCopyright != null)
                project.Copyright = ProjectCopyright;
            if (ProjectDescription != null)
                project.Description = ProjectDescription;
            if (TestDataPaths != null)
                project.TestDataPaths = TestDataPaths.Select(f => f.FullName).ToArray();
            
            if (!OutputDirectory.Exists)
            {
                OutputDirectory.Create();
            }

            project.SaveToDirectory(OutputDirectory.FullName);
        }
    }
}
