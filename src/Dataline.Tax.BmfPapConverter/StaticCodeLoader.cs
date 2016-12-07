using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dataline.Tax.BmfPapConverter
{
    internal static class StaticCodeLoader
    {
        public const string PapOperationalClassStaticCodeName = "Dataline.Tax.BmfPapConverter.Resources.PapOperationalClassStaticCode.cs";
        public const string ProjectSkeletonStaticCodeName = "Dataline.Tax.BmfPapConverter.Resources.ProjectSkeleton.json";

        public static string Load(string name)
        {
            var asm = typeof(StaticCodeLoader).GetTypeInfo().Assembly;
            var stream = asm.GetManifestResourceStream(name);

            Debug.Assert(stream != null, "stream != null");

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
