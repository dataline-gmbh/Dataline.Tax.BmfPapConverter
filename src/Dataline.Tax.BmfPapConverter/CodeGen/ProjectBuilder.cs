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

        public string Version { get; set; } = "1.0.0-*";

        public string Author { get; set; } = "Author";

        public string Copyright { get; set; } = DateTime.Now.Year.ToString();

        public string Description { get; set; } = "Generiert mit Dataline.Tax.BmpPapConverter";

        public void SaveToDirectory(string directoryPath)
        {
            // Sourcen schreiben
            foreach (var sourceBuilder in Sources)
            {
                string path = Path.Combine(directoryPath, sourceBuilder.Name);

                var codeBuilder = new CodeBuilder();
                sourceBuilder.CodeGen(codeBuilder);

                File.WriteAllText(path, codeBuilder.ToString());
            }

            // project.json schreiben
            File.WriteAllText(Path.Combine(directoryPath, "project.json"), GenerateProjectJson());
        }

        private string GenerateProjectJson()
        {
            var skel = StaticCodeLoader.Load(StaticCodeLoader.ProjectSkeletonStaticCodeName);

            skel = skel.Replace("%version%", Version.JsonEscaped())
                .Replace("%author%", Author.JsonEscaped())
                .Replace("%copyright%", Copyright.JsonEscaped())
                .Replace("%description%", Description.JsonEscaped());

            return skel;
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
