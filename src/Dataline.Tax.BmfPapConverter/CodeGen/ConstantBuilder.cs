using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dataline.Tax.BmfPapConverter.CodeGen
{
    public class ConstantBuilder
    {
        public ConstantBuilder(Visibilities visibility,
                        Type type,
                        string name,
                        bool compileTimeConstant,
                        ExpressionBuilder expression)
        {
            Visibility = visibility;
            Type = type;
            Name = name;
            CompileTimeConstant = compileTimeConstant;
            Expression = expression;
        }

        public string Name { get; set; }

        public Type Type { get; set; }

        public Visibilities Visibility { get; set; }

        public bool CompileTimeConstant { get; set; }

        public ExpressionBuilder Expression { get; set; }

        public string Summary { get; set; }

        public void CodeGen(CodeBuilder builder)
        {
            if (Summary != null)
                builder.AppendSummary(Summary);

            Visibility.CodeGen(builder);

            if (CompileTimeConstant)
                builder.AppendToken("const");
            else
                builder.AppendToken("static readonly");

            Type.CodeGen(builder);

            builder.AppendToken(Name);

            builder.AppendToken("=");

            Expression.CodeGen(builder);

            builder.EndOfStatement();
        }
    }
}
