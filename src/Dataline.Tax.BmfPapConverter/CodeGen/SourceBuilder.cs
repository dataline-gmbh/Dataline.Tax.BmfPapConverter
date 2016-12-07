using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public class SourceBuilder : ICodeGen
    {
        public SourceBuilder(ModuleBuilder moduleContent = null)
        {
            AddDefaultUsings();

            if (moduleContent != null)
            {
                Modules.Add(moduleContent);
                Name = $"{moduleContent.Name}.cs";
            }
        }

        public List<string> Usings { get; set; } = new List<string>();

        public string Header { get; set; }

        public string Namespace { get; set; }

        public string Name { get; set; }

        public List<ModuleBuilder> Modules { get; set; } = new List<ModuleBuilder>();

        private void AddDefaultUsings()
        {
            Usings.Add("System");
            Usings.Add("System.Collections.Generic");
            Usings.Add("System.Linq");
        }

        public void CodeGen(CodeBuilder builder)
        {
            if (string.IsNullOrEmpty(Namespace))
                throw new InvalidOperationException("Kein Namespace angegeben");

            // Header
            if (!string.IsNullOrEmpty(Header))
                builder.AppendMultiLineComment(Header);

            // Using-Direktiven
            foreach (string u in Usings)
            {
                builder.AppendToken("using");
                builder.AppendToken(u);
                builder.EndOfStatement();
            }

            builder.EndOfLineBlock();

            // Namespace
            builder.AppendToken("namespace");
            builder.AppendToken(Namespace);
            builder.BeginBlock();

            // Inhalt
            foreach (var module in Modules)
            {
                module.CodeGen(builder);
            }

            builder.EndBlock();
        }
    }
}
