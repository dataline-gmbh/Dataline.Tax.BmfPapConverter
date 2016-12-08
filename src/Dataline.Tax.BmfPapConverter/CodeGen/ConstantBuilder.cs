// <copyright file="ConstantBuilder.cs" company="DATALINE GmbH &amp; Co. KG">
// Copyright (c) DATALINE GmbH &amp; Co. KG. All rights reserved.
// See LICENSE in the project root for license information.
// </copyright>

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
