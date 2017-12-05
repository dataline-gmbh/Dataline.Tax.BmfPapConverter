// <copyright file="StaticCodeLoader.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

using System;
using System.IO;
using System.Reflection;

namespace Dataline.Tax.BmfPapConverter
{
    internal static class StaticCodeLoader
    {
        public const string PapOperationalClassStaticCodeName = "Dataline.Tax.BmfPapConverter.Resources.PapOperationalClassStaticCode.cs";
        public const string ProjectSkeletonCsprojStaticCodeName = "Dataline.Tax.BmfPapConverter.Resources.ProjectSkeleton.csproj";
        public const string TestSkeletonStaticCodeName = "Dataline.Tax.BmfPapConverter.Resources.TestSkeleton.cs";
        public const string TestProjectSkeletonCsprojStaticCodeName = "Dataline.Tax.BmfPapConverter.Resources.TestProjectSkeleton.csproj";
        public const string SolutionSkeletonStaticCodeName = "Dataline.Tax.BmfPapConverter.Resources.SolutionSkeleton.sln";
        public const string TargetsSkeletonStaticCodeName = "Dataline.Tax.BmfPapConverter.Resources.TargetsSkeleton.targets";

        public static string Load(string name)
        {
            var asm = typeof(StaticCodeLoader).GetTypeInfo().Assembly;
            var stream = asm.GetManifestResourceStream(name);

            if (stream == null)
                throw new InvalidOperationException($"Die statische Ressource {name} wurde nicht gefunden.");

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static string ExtensionStaticCodeName(string extension)
            => $"Dataline.Tax.BmfPapConverter.Resources.Extension_{extension}.cs";
    }
}
