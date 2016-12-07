using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.CommandLineUtils;

namespace Dataline.Tax.BmfPapConverter.CliConvbmf
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = Path.GetFileNameWithoutExtension(Environment.GetCommandLineArgs()[0]),
                Description = "Konvertierung des BMF-PAP in ein C#-Projekt"
            };
            app.HelpOption("-h|--help|-?");
            app.OptionHelp.Description = "Zeigt die Hilfe";

            var papPathOpt = app.Option("-p|--pap-path",
                                        "Pfad des XML-PAP (erforderlich)",
                                        CommandOptionType.SingleValue);
            var outputDirectoryOpt = app.Option("-o|--output-dir",
                                                "Ausgabeverzeichnis des Projekts (erforderlich)",
                                                CommandOptionType.SingleValue);
            var namespaceOpt = app.Option("-n|--namespace",
                                          "Namespace des zu generierenden Projekts (erforderlich)",
                                          CommandOptionType.SingleValue);
            var testDataPathsOpt = app.Option("-t|--test",
                                              "CSV-Testdaten für das generierte Testprojekt",
                                              CommandOptionType.MultipleValue);

            app.OnExecute(() =>
            {
                string papPath = papPathOpt.Value();
                string outputDirectory = outputDirectoryOpt.Value();
                string namespc = namespaceOpt.Value();
                var testPaths = testDataPathsOpt.Values;

                if (string.IsNullOrEmpty(papPath) ||
                    string.IsNullOrEmpty(outputDirectory) ||
                    string.IsNullOrEmpty(namespc))
                {
                    app.ShowHelp();
                    return 1;
                }

                return Run(papPath, outputDirectory, namespc, testPaths);
            });
            app.Execute(args);
        }

        private static int Run(string papPath, string outputDir, string namespc, IEnumerable<string> testPaths)
        {
            XDocument pap;

            using (var papStream = new FileStream(papPath, FileMode.Open))
            {
                pap = XDocument.Load(papStream);
            }

            var papDocument = new PapDocument(pap);

            var converter = new PapConverter
            {
                InputClassName = "Eingabeparameter",
                OutputClassName = "Ausgabeparameter",
                OperationClassName = "Berechnung",
                OperationMainMethodName = "Lohnsteuer",
                Namespace = namespc
            };

            var project = converter.GenerateProject(papDocument);
            project.Name = namespc;
            project.TestDataPaths = testPaths.ToArray();

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            project.SaveToDirectory(outputDir);

            return 0;
        }
    }
}
