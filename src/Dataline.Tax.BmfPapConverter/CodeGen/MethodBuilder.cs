using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public class MethodBuilder : ICodeGen
    {
        public MethodBuilder(Visibilities visibility, string name, StatementBuilder body, Type returnType = null)
        {
            Visibility = visibility;
            ReturnType = returnType;
            Name = name;
            Body = body;
        }

        public Visibilities Visibility { get; }

        public Type ReturnType { get; }

        public string Name { get; }

        public StatementBuilder Body { get; set; }

        public string Summary { get; set; }

        public void CodeGen(CodeBuilder builder)
        {
            if (Summary != null)
                builder.AppendSummary(Summary);

            Visibility.CodeGen(builder);

            if (ReturnType != null)
                ReturnType.CodeGen(builder);
            else
                builder.AppendToken("void");

            builder.AppendToken(Name);
            builder.ForceNoWhitespace();

            builder.AppendToken("()");
            builder.BeginBlock();

            Body.CodeGen(builder);

            builder.EndBlock();
        }
    }
}
