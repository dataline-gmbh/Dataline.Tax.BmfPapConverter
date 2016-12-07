using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public class ProjectBuilder
    {
        public List<SourceBuilder> Sources { get; } = new List<SourceBuilder>();
        
        public string Name { get; set; }

        public string Version { get; set; } = "1.0.0-*";

        public string Author { get; set; } = "Author";

        public string Copyright { get; set; } = DateTime.Now.Year.ToString();

        public string Description { get; set; } = "Generiert mit Dataline.Tax.BmpPapConverter";

        public string[] TestDataPaths { get; set; }

        public void SaveToDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(Name))
                throw new InvalidOperationException("Der Projektname muss angegeben werden.");

            string srcDirectory = Path.Combine(directoryPath, "src", Name);
            string testDirectory = Path.Combine(directoryPath, "test", $"{Name}.Test");

            // Ordner anlegen
            Directory.CreateDirectory(srcDirectory);
            Directory.CreateDirectory(testDirectory);

            // Sourcen schreiben
            foreach (var sourceBuilder in Sources)
            {
                string path = Path.Combine(srcDirectory, sourceBuilder.Name);

                var codeBuilder = new CodeBuilder();
                sourceBuilder.CodeGen(codeBuilder);

                File.WriteAllText(path, codeBuilder.ToString());
            }

            // project.json schreiben
            var projectJson = PrepareSkeleton(StaticCodeLoader.Load(StaticCodeLoader.ProjectSkeletonStaticCodeName));
            File.WriteAllText(Path.Combine(srcDirectory, "project.json"), projectJson);

            if (TestDataPaths != null && TestDataPaths.Length > 0)
            {
                // Testdaten kopieren
                int filei = 0;
                foreach (string path in TestDataPaths)
                {
                    File.Copy(path, Path.Combine(testDirectory, $"testdata-{++filei}.csv"));
                }

                // Testprojekt schreiben
                string testClass = PrepareSkeleton(StaticCodeLoader.Load(StaticCodeLoader.TestSkeletonStaticCodeName));
                File.WriteAllText(Path.Combine(testDirectory, "Test.cs"), testClass);

                // project.json schreiben
                var testProjectJson = PrepareSkeleton(StaticCodeLoader.Load(StaticCodeLoader.TestProjectSkeletonStaticCodeName));
                File.WriteAllText(Path.Combine(testDirectory, "project.json"), testProjectJson);
            }

            // global.json schreiben
            var globalJson = PrepareSkeleton(StaticCodeLoader.Load(StaticCodeLoader.GlobalSkeletonStaticCodeName));
            File.WriteAllText(Path.Combine(directoryPath, "global.json"), globalJson);
        }

        private string PrepareSkeleton(string skeleton)
        {
            return skeleton.Replace("%version%", Version.JsonEscaped())
                .Replace("%author%", Author.JsonEscaped())
                .Replace("%copyright%", Copyright.JsonEscaped())
                .Replace("%description%", Description.JsonEscaped())
                .Replace("%projectname%", Name.JsonEscaped());
        }
    }

    internal static class JsonStringExtensions
    {
        public static string JsonEscaped(this string text)
        {
            return text.Replace("\"", "\\\"");
        }
    }
}
