// <copyright file="ProjectBuilder.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public class ProjectBuilder
    {
        public List<SourceBuilder> Sources { get; } = new List<SourceBuilder>();

        public string Name { get; set; }

        public string Version { get; set; } = "1.0.0-*";

        public string Author { get; set; } = "Author";

        public string Copyright { get; set; } = DateTime.Now.Year.ToString();

        public List<string> Tags { get; set; } = new List<string> { "Tax", "Lohn", "Berechnung", "Lohnsteuer" };

        public string Description { get; set; } = "Generiert mit Dataline.Tax.BmfPapConverter";

        public string[] TestDataPaths { get; set; }

        public void SaveToDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(Name))
                throw new InvalidOperationException("Der Projektname muss angegeben werden.");

            string srcDirectory = Path.Combine(directoryPath, Name);
            string testDirectory = Path.Combine(directoryPath, $"{Name}.Test");

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

            // packaging-Ordner schreiben
            CreatePackaging(Path.Combine(srcDirectory, "packaging"));

            // csproj schreiben
            var csproj = PrepareSkeleton(StaticCodeLoader.Load(StaticCodeLoader.ProjectSkeletonCsprojStaticCodeName));
            File.WriteAllText(Path.Combine(srcDirectory, $"{Name}.csproj"), csproj);

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

                // csproj schreiben
                var testCsproj = PrepareSkeleton(StaticCodeLoader.Load(StaticCodeLoader.TestProjectSkeletonCsprojStaticCodeName));
                File.WriteAllText(Path.Combine(testDirectory, $"{Name}.Test.csproj"), testCsproj);
            }

            // Solution schreiben
            var solution = PrepareSkeleton(StaticCodeLoader.Load(StaticCodeLoader.SolutionSkeletonStaticCodeName));
            File.WriteAllText(Path.Combine(directoryPath, $"{Name}.sln"), solution);
        }

        private string PrepareSkeleton(string skeleton)
        {
            string tagsContent = string.Join(", ", Tags.Select(t => t.XmlEscaped()));

            return skeleton.Replace("%version%", Version.XmlEscaped())
                .Replace("%author%", Author.XmlEscaped())
                .Replace("%copyright%", Copyright.XmlEscaped())
                .Replace("%description%", Description.XmlEscaped())
                .Replace("%projectname%", Name.XmlEscaped())
                .Replace("%tags%", tagsContent)
                .Replace("%guid.1%", Guid.NewGuid().ToString().ToUpper())
                .Replace("%guid.2%", Guid.NewGuid().ToString().ToUpper())
                .Replace("%guid.3%", Guid.NewGuid().ToString().ToUpper());
        }

        private void CreatePackaging(string path)
        {
            Directory.CreateDirectory(path);

            // _._ für das NuGet-Paket
            File.Create(Path.Combine(path, "_._")).Dispose();

            // Targets-Datei
            string targets = PrepareSkeleton(StaticCodeLoader.Load(StaticCodeLoader.TargetsSkeletonStaticCodeName));
            File.WriteAllText(Path.Combine(path, $"{Name}.targets"), targets);
        }
    }

    internal static class XmlStringExtensions
    {
        public static string XmlEscaped(this string text)
        {
            return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
        }
    }
}
