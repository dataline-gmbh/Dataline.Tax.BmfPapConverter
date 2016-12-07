using System;
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
        [Parameter(Mandatory = true)]
        public FileInfo PapPath { get; set; }

        [Parameter(Mandatory = true)]
        public DirectoryInfo OutputDirectory { get; set; }

        [Parameter]
        public string InputClassName { get; set; } = "Eingabeparameter";

        [Parameter]
        public string OutputClassName { get; set; } = "Ausgabeparameter";

        [Parameter]
        public string OperationClassName { get; set; } = "Berechnung";

        [Parameter]
        public string OperationMainMethodName { get; set; } = "Lohnsteuer";

        [Parameter(Mandatory = true)]
        public string Namespace { get; set; }

        [Parameter]
        public string FileHeader { get; set; }

        [Parameter]
        public string ProjectVersion { get; set; }

        [Parameter]
        public string ProjectAuthor { get; set; }

        [Parameter]
        public string ProjectCopyright { get; set; }

        [Parameter]
        public string ProjectDescription { get; set; }

        [Parameter]
        public FileInfo[] TestDataPaths { get; set; }

        [Parameter]
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
